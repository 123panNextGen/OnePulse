# OnePulse.Features.LoginManager

Manages the credentials of 123pan user accounts on the local machine. The module
persists user login information (incl. passwords and access tokens) into an
encrypted LiteDB database, so that raw credentials are never stored in plain
text.

## Why This Module Exists

A 123pan desktop client needs to remember multiple accounts so that the user
does not have to log in again on every start. Storing the password and token
directly on disk (or behind a hardcoded database password) would let anyone who
copies the database file read every saved credential.

To mitigate that, this module applies a layered approach:

| Layer                  | Mechanism                           | Effect                                                                          |
| ---------------------- | ----------------------------------- | ------------------------------------------------------------------------------- |
| Database encryption    | LiteDB AES with a random master key | The `.db` file is unreadable without the key                                    |
| Key protection         | DPAPI (`CurrentUser` scope)         | The key file can only be decrypted by the same Windows user on the same machine |
| Field-level encryption | DPAPI on each sensitive field       | Password / token / UUID / OpenInfo are stored as ciphertext                     |

The combination means that even if `UserInfo.db` is stolen, the credentials
inside remain inaccessible.

## Architecture

```
Models/
└── StorageUser.cs                      # Storage record: display fields + encrypted UserInfo

Services/
├── LoginManager.cs                     # Singleton, owns the database and registers sub-services
├── LoginManager.UtilityService.cs     # Opens the LiteDB database with the stored key
├── LoginManager.AddService.cs         # Persists a StorageUser record (already encrypted)
├── LoginManager.GetService.cs         # Reads StorageUser records and decrypts UserInfo
├── LoginManager.DeleteService.cs      # Deletes a user by StorageUser.Uuid
├── UserInfoConverter.cs               # Maps UserInfo → StorageUser and encrypts sensitive fields
├── Interface/
│   ├── IAddService.cs                 # Contract for adding a user account
│   ├── IGetService.cs                 # Contract for reading users
│   ├── IDeleteService.cs              # Contract for deleting a user
│   ├── IUserInfoConverter.cs          # Contract for UserInfo → StorageUser conversion
│   ├── IUtilityService.cs             # Contract for database initialization
│   └── ISecureKeyStore.cs             # Contract for the database key
└── SecureCrypto/
    ├── Secure.CryptoService.cs        # DPAPI encryption/decryption + random key generation
    ├── Secure.KeyStore.cs             # Persists the random DB password to key.dat
    └── Secure.UserInfoProtector.cs    # Maps sensitive UserInfo fields to ciphertext
```

```
┌──────────────────────────────────────────────────────────────┐
│  LoginManager (singleton)                                     │
│                                                              │
│  KeyStore ── GenerateRandomKey/read key.dat ──► LiteDB password
│  Utils ───── opens LiteDB with password ───────────────────┐ │
│  Converter ─ UserInfo → StorageUser (encrypts sensitive fields)│ │
│  Add ──────── persists the converted StorageUser ─────────────►│ │
│  Get ─────── UserInfoProtector.Decrypt() ◄── StorageUser ◄─────│ │
│  Delete ──── DeleteMany(Uuid)                            │ │
└────────────────────────────────────────────────────────────┘ │
                                                               ▼
   %APPDATA%\OnePulse\Database\UserInfo.db   (AES-encrypted)
   %APPDATA%\OnePulse\Database\key.dat       (DPAPI-encrypted key)
```

## How It Works

### 1. Startup order (dependency chain)

The database password is generated lazily on first access, so the
initialization runs in two phases:

```
LoginManager()                     # ctor
 ├── KeyStore = new SecureKeyStore(AppDataPath)   # registers service
 ├── Converter = new UserInfoConverter()          # converts + encrypts UserInfo
 ├── Utils     = new UtilityService(this)
 ├── Add       = new AddService(this)
 └── Utils.Initialize()            # now KeyStore.Key exists
      └── new LiteDatabase($"Filename=...UserInfo.db;Password={KeyStore.Key}")
```

- **First run**: `SecureKeyStore.Key` is accessed for the first time. No
  `key.dat` exists yet, so a random 64-hex-digit key is generated, DPAPI-encrypted
  and written to `%APPDATA%\OnePulse\Database\key.dat`.
- **Later runs**: the key is read from `key.dat` and decrypted with DPAPI.
- The key is cached in memory so the file is only decrypted once per session.

### 2. Writing an account (encrypted at rest)

`IAddService.AddUser(user)` (the `user` must come from
`UserInfoConverter.ToStorageUser`):

- Conversion: `UserInfoConverter.ToStorageUser(info)` maps the plaintext
  `UserInfo` into a `StorageUser` record — outer display fields stay plaintext,
  and the inner `UserInfo` is encrypted via `UserInfoProtector.Encrypt`
  (`Password`, `Authorization`, `Uuid` are DPAPI-encrypted; `OpenInfo` is
  serialized to JSON and encrypted into the `OpenInfoCipher` column).
- Duplicate check on the plaintext `UserName` (`UserName` must stay plaintext —
  ciphertext cannot be matched in a database query).
- Only the encrypted record is inserted. The original `UserInfo` stays in memory
  in plaintext, so the login flow that produced it keeps working.

| Field                        | Stored as                                  | Why                                             |
| ---------------------------- | ------------------------------------------- | ----------------------------------------------- |
| `StorageUser.Uuid`           | plaintext                                  | unique record key (Guid string), set on convert |
| `StorageUser.UserId`         | plaintext                                  | account id (from `OpenInfo.Uid`)                |
| `StorageUser.UserName`       | plaintext                                  | needed for duplicate checks (query) and display |
| `StorageUser.HeadImageUrl`   | plaintext                                  | avatar url                                      |
| `UserInfo.Password`          | DPAPI ciphertext                           | login credential                                |
| `UserInfo.Authorization`     | DPAPI ciphertext                           | session token                                   |
| `UserInfo.Uuid`              | DPAPI ciphertext                           | device-bound token                              |
| `UserInfo.OpenInfo`          | JSON → DPAPI ciphertext (`OpenInfoCipher`) | open-platform user info incl. tokens            |
| `UserInfo.DeviceInfo`        | plaintext                                  | non-sensitive                                   |

### 3. Reading accounts

`IGetService.GetUsers()` reads all `StorageUser` records and decrypts the inner
`UserInfo` via `UserInfoProtector.Decrypt` before returning, so callers receive
in-memory plaintext credentials directly.

### 4. Deleting an account

`IDeleteService.DeleteUser(uuid)` removes the record whose `StorageUser.Uuid`
matches (the unique key generated by `UserInfoConverter` on write — it is distinct
from the device-bound `UserInfo.Uuid`), returning
`ApiResult.Failed` with `用户不存在` when no such user exists. Legacy records
written before the `Uuid` column existed have no key and are not deletable.

## Usage

```csharp
using OnePulse.Features.LoginManager.Services;

// 1. Get the manager (database + key are initialized lazily on first login)
var manager = LoginManager.Instance;

// 2. Convert + store an account after a successful login
var storageUser = manager.Converter.ToStorageUser(userInfo);  // encrypts sensitive fields
var result = manager.Add.AddUser(storageUser);
if (result.Result == ApiResult.Success)
    Console.WriteLine("Account saved");

// 3. List saved users (inner credentials are decrypted on read)
var users = manager.Get.GetUsers();
foreach (var user in users)
    Console.WriteLine($"{user.UserName} - {user.UserInfo.OpenInfo?.Passport}");

// 4. Delete an account (uuid comes from the StorageUser you listed earlier)
var deleteResult = manager.Delete.DeleteUser(storageUser.Uuid);
if (deleteResult.Result == ApiResult.Success)
    Console.WriteLine("Account deleted");
```

## Security Properties & Trade-offs

| Property                     | Behavior                                                                                                                          |
| ---------------------------- | --------------------------------------------------------------------------------------------------------------------------------- |
| Machine-bound                | DPAPI is bound to the current Windows user + machine. Copying `key.dat` or the DB to another machine yields nothing               |
| `key.dat` removal            | The database cannot be opened anymore. This is intended behavior: security wins over recoverability                               |
| No hardcoded secrets         | Neither the DB password nor any encryption key appears in the source code                                                         |
| Plaintext password in memory | An in-memory `UserInfo` stays plaintext while the app is running; only the on-disk copy is encrypted                              |
| Windows only                 | `ProtectedData` works on Windows only; the module is intended for the WinUI desktop client (CA1416 is suppressed for that reason) |

## Storage Layout

```
%APPDATA%\OnePulse\
└── Database\
    ├── UserInfo.db      # LiteDB, password = random key from key.dat
    └── key.dat          # DPAPI-encrypted key file (same dir as the DB)
```

`key.dat` lives next to the database file on purpose: if the directory is
migrated as a whole, the key travels with it; decrypting it on another machine is still impossible.

## Development

```bash
dotnet build Projects/OnePulse.Features.LoginManager
```

- Package references: `LiteDB` 5.0.21, `System.Security.Cryptography.ProtectedData` 10.0.10 (both declared in the csproj).
- If you ever open a database with a wrong/old password, throw away the DB — the key is authoritative.

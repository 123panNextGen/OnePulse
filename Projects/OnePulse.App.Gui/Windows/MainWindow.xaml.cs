namespace OnePulse.App.Gui.Windows
{
    public sealed partial class MainWindow
    {
        public MainWindow()
        {
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            InitializeComponent();
        }
    }
}

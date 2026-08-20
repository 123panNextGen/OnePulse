using System;
using CommunityToolkit.WinUI.Behaviors;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;

namespace OnePulse.App.Gui.Services
{
    public static class NotificationService
    {
        private static StackedNotificationsBehavior? _notificationQueue;

        // 可在此调整最大高度（像素），超过此高度将显示滚动条
        private const double MaxNotificationHeight = 160; // 可根据实际界面调整

        public static void Initialize(StackedNotificationsBehavior queue)
        {
            _notificationQueue = queue;
        }

        /// <summary>
        /// 创建带滚动支持的消息内容，返回 ScrollViewer 避免装箱
        /// </summary>
        private static ScrollViewer CreateMessageContent(string message, double bottomMargin = 0)
        {
            var textBlock = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                IsTextSelectionEnabled = true,
                Margin = new Thickness(0, 0, 0, bottomMargin)
            };

            var scrollViewer = new ScrollViewer
            {
                Content = textBlock,
                MaxHeight = MaxNotificationHeight,          // 固定最大值，超长自动滚动
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Margin = new Thickness(0)
            };

            return scrollViewer;
        }

        public static void Show(
            string title,
            string message,
            InfoBarSeverity severity = InfoBarSeverity.Informational,
            int durationMs = 5000,
            bool showCopyButton = false
        )
        {
            if (_notificationQueue == null)
                return;

            var content = CreateMessageContent(message, 12);

            var notification = new Notification
            {
                Title = title,
                Severity = severity,
                Duration = TimeSpan.FromMilliseconds(durationMs),
                Content = content
            };

            if (showCopyButton)
            {
                var copyButton = new Button
                {
                    Content = new FontIcon { Glyph = "\uE8C8", FontSize = 16 },
                    Margin = new Thickness(0, 0, 4, 0),
                    Style = (Style)Application.Current.Resources["SubtleButtonStyle"]
                };
                ToolTipService.SetToolTip(copyButton, "复制内容");
                copyButton.Click += (s, e) =>
                {
                    var dataPackage = new DataPackage();
                    dataPackage.SetText(message);
                    Clipboard.SetContent(dataPackage);
                };
                notification.ActionButton = copyButton;
            }

            _notificationQueue.Show(notification);
        }

        public static void ShowWithCopy(
            string title,
            string message,
            string contentToCopy,
            InfoBarSeverity severity = InfoBarSeverity.Informational,
            int durationMs = 5000
        )
        {
            if (_notificationQueue == null)
                return;

            var content = CreateMessageContent(message, 8); // 有复制按钮，底部留 8px

            var notification = new Notification
            {
                Title = title,
                Severity = severity,
                Duration = TimeSpan.FromMilliseconds(durationMs),
                Content = content
            };

            var copyButton = new Button
            {
                Content = new FontIcon { Glyph = "\uE8C8", FontSize = 16 },
                Margin = new Thickness(0, 0, 4, 0),
                Style = (Style)Application.Current.Resources["SubtleButtonStyle"]
            };
            ToolTipService.SetToolTip(copyButton, "复制内容");
            copyButton.Click += (s, e) =>
            {
                var dataPackage = new DataPackage();
                dataPackage.SetText(contentToCopy);
                Clipboard.SetContent(dataPackage);
            };
            notification.ActionButton = copyButton;

            _notificationQueue.Show(notification);
        }
    }
}
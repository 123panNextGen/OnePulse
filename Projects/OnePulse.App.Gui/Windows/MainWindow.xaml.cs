using Microsoft.UI.Xaml;

namespace OnePulse.App.Gui.Windows
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            InitializeComponent();
        }
    }
}

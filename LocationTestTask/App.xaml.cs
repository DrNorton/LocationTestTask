using System.Windows;
using Microsoft.Phone.Controls;

namespace LocationTestTask.UI
{
    public partial class App : Application
    {
        public PhoneApplicationFrame RootFrame { get; private set; }

        public App()
        {
            InitializeComponent();
            
        }

        void App_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            
        }

  
    }
}
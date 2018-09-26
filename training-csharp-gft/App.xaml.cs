using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TrainingCsharpGft;
using TrainingCsharpGft.ViewModel;

namespace training_csharp_gft
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void StartupHandler(object sender, StartupEventArgs e)
        {
            var vm = new ViewModel();
            var mainWindow = new MainWindow(vm);
            mainWindow.Show();
        }
    }
}

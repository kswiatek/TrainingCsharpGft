using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TrainingCsharpGft.Api;

namespace TrainingCsharpGft
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Accounts api = new Accounts();

        public MainWindow()
        {
            InitializeComponent();

        }

        private void lb_accounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btn_selectAccount_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_addNewAccount_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_topUp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cbo_accountToTransfer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btn_transferToSelectedAccount_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_deleteYourAccount_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

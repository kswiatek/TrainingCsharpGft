using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using TrainingCsharpGft.ViewModel;

namespace TrainingCsharpGft
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(ViewModel.ViewModel vm) : this()
        {
            DataContext = vm;
        }

        private void txt_topUpAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (btn_topUp == null)
                return;

            if (double.TryParse(txt_topUpAmount.Text, out double temp))
                btn_topUp.IsEnabled = true;
            else
                btn_topUp.IsEnabled = false;
        }

        private void txt_transferAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (btn_transferToSelectedAccount == null)
                return;

            if (double.TryParse(txt_transferAmount.Text, out double temp))
                btn_transferToSelectedAccount.IsEnabled = true;
            else
                btn_transferToSelectedAccount.IsEnabled = false;
        }
    }
}

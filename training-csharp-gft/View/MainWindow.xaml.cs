using System.Windows;
using System.Windows.Controls;

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

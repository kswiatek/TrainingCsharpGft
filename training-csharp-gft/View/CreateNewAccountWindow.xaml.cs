using System.Windows;
using System.Windows.Controls;

namespace TrainingCsharpGft
{
    /// <summary>
    /// Interaction logic for CreateNewAccountWindow.xaml
    /// </summary>
    public partial class CreateNewAccountWindow : Window
    {
        public CreateNewAccountWindow()
        {
            InitializeComponent();
        }

        public CreateNewAccountWindow(ViewModel.ViewModel vm) : this()
        {
            DataContext = vm;
        }

        private void txt_accountInitialAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (btn_createNewAccount == null)
                return;

            if (double.TryParse(txt_accountInitialAmount.Text, out double temp))
                btn_createNewAccount.IsEnabled = true;
            else
                btn_createNewAccount.IsEnabled = false;
        }
    }
}

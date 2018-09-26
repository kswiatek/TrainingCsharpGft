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
using System.Windows.Shapes;
using TrainingCsharpGft.Api;

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
            this.DataContext = vm;
        }


    }
}

using DataModel.Models;
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

namespace CamcoManufacturing.View
{
    /// <summary>
    /// Interaction logic for View_Operation.xaml
    /// </summary>
    public partial class View_Operation : Window
    {
        BaseDataContext db = new BaseDataContext();
        public View_Operation()
        {
            InitializeComponent();
            FillControls();
        }
        private void FillControls()
        {
            dataGridOperations.ItemsSource = null;
            dataGridOperations.ItemsSource = db.tOperations.ToList();
        }

        private void ButtonCreateNew_Click(object sender, RoutedEventArgs e)
        {
            if (!HelperClass.IsWindowOpen(typeof(View.CreateNew_Operation)))
            {
                View.CreateNew_Operation obj = new View.CreateNew_Operation();
                obj.ShowDialog();
            }
            else
            {
                HelperClass.activateWindow(typeof(View.CreateNew_Operation));
            }
        }
    }
}

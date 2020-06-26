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
    /// Interaction logic for Edit_SetupSheet.xaml
    /// </summary>
    public partial class Edit_SetupSheet : Window
    {
        public Edit_SetupSheet()
        {
            InitializeComponent();
            HelperClass.ShowWindowPath(PathLabel);
        }


        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (!HelperClass.IsWindowOpen(typeof(MainWindow)))
            {
            
                MainWindow obj = new MainWindow();
                obj.ShowDialog();
            }
            else
            {
                HelperClass.activateWindow(typeof(MainWindow));
            }
        }

        private void ButtonBump_Click(object sender, RoutedEventArgs e)
        {
            if (!HelperClass.IsWindowOpen(typeof(CreateNew_BumpToolHeader)))
            {
                CreateNew_BumpToolHeader obj = new CreateNew_BumpToolHeader();
                obj.ShowDialog();
            }
            else
            {
                HelperClass.activateWindow(typeof(CreateNew_BumpToolHeader));
            }
        }
    }
}

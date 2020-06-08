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
    /// Interaction logic for CreateNew_BumpToolHeader.xaml
    /// </summary>
    public partial class CreateNew_BumpToolHeader : Window
    {
        public CreateNew_BumpToolHeader()
        {
            InitializeComponent();
        }

        private void ButtonReturnToAddTool_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (!HelperClass.IsWindowOpen(typeof(CreateNew_TurretTool)))
            {
                CreateNew_TurretTool obj = new CreateNew_TurretTool();
                obj.ShowDialog();
            }
            else
            {
                HelperClass.activateWindow(typeof(CreateNew_TurretTool));
            }
        }
    }
}

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
    /// Interaction logic for ManufacturingSheetListView.xaml
    /// </summary>
    public partial class ManufacturingSheetListView : Window
    {
        BaseDataContext db = new BaseDataContext();
        public ManufacturingSheetListView()
        {
            InitializeComponent();
            FillControls();
        }
        private void FillControls()
        {
            dataGridMCS.ItemsSource = null;
            dataGridMCS.ItemsSource = db.tManufacturingCheckSheets.ToList();

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tblManufacturingCheckSheet obj = ((FrameworkElement)sender).DataContext as tblManufacturingCheckSheet;
            ManufacturingCheckSheetReportView obj1 = new ManufacturingCheckSheetReportView(obj.MCS_id);
            obj1.ShowDialog();
        }
    }
}

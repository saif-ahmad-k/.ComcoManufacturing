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

namespace CamcoManufacturing
{
    /// <summary>
    /// Interaction logic for ListView.xaml
    /// </summary>
    public partial class ListView : Window
    {
        BaseDataContext db = new BaseDataContext();
        public ListView()
        {
            InitializeComponent();
            FillControls();
        }
        private void FillControls()
        {
            dataGridCamcoRecord.ItemsSource = null;
            dataGridCamcoRecord.ItemsSource = db.tCamcoRecords.OrderByDescending(p => p.CreatedDate).ToList();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tblCamcoRecord obj = ((FrameworkElement)sender).DataContext as tblCamcoRecord;
            CamcoRecordReportView obj1 = new CamcoRecordReportView(obj.Id);
            obj1.ShowDialog();
        }
    }
}

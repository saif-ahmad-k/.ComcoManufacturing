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
    /// Interaction logic for View_AllCategories.xaml
    /// </summary>
    public partial class View_AllCategories : Window
    {
        BaseDataContext db = new BaseDataContext();
        public View_AllCategories()
        {
            InitializeComponent();
            HelperClass.ShowWindowPath(PathLabel);
            FillControls();
        }
        private void FillControls()
        {
            dataGridCategory.ItemsSource = null;
            dataGridCategory.ItemsSource = db.tCategories.ToList();
            
        }
        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            tblCategory dataRowView = (tblCategory)((Button)e.Source).DataContext;
            //this.Close();
            if (!HelperClass.IsWindowOpen(typeof(View.Edit_Category)))
            {
                View.Edit_Category obj = new View.Edit_Category(dataRowView.Category_ID);
                obj.ShowDialog();
            }
            else
            {
                Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "EditCategory");
                win.Close();
                View.Edit_Category obj = new View.Edit_Category(dataRowView.Category_ID);
                obj.ShowDialog();
            }
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                tblCategory dataRowView = (tblCategory)((Button)e.Source).DataContext;
                db.tCategories.Remove(dataRowView);
                db.SaveChanges();
                MessageBox.Show("Deleted SuccessFully!");
                FillControls();
            }
        }

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

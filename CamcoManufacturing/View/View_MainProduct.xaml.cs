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
    /// Interaction logic for View_MainProduct.xaml
    /// </summary>
    public partial class View_MainProduct : Window
    {
        BaseDataContext db = new BaseDataContext();
        int CategoryId = 0;
        bool fromSetupSheet = false;
        public View_MainProduct(int CatId, bool isFromCreateSetupSheet)
        {
            InitializeComponent();
            CategoryId = CatId;
            fromSetupSheet = isFromCreateSetupSheet;
            HelperClass.ShowWindowPath(PathLabel);
            FillWrapPanelProducts();
        }

        private void ButtonAddNewProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!HelperClass.IsWindowOpen(typeof(View.MainProduct)))
            {
                View.MainProduct obj = new View.MainProduct();
                obj.ShowDialog();
            }
            else
            {
                HelperClass.activateWindow(typeof(View.MainProduct));
            }
        }
        private void FillWrapPanelProducts()
        {
            //var ParentCategories = db.tProducts.Where(p => p.CategoryID == CategoryId).ToList();
            //foreach (var item in ParentCategories)
            //{
            //    Button button = new Button();
            //    button.Content = item.ProductName;
            //    button.Width = 140;
            //    button.Height = 44;
            //    button.Click += new RoutedEventHandler(buttonWrapProduct_Click);
            //    WrapPanelProducts.Children.Add(button);
            //}
        }
        void buttonWrapProduct_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            //var product = db.tProducts.Where(p => p.ProductName == btn.Content.ToString()).FirstOrDefault();
            //if (product != null)
            //{
            //    if (product.CategoryID == 26)
            //    {
            //        foreach (Window item in Application.Current.Windows)
            //        {
            //            if (item.Name == "CreateSetUpSheet")
            //            {
            //                ((SetUpSheet)item).textBoxDrillTap.Text = product.ProductName;
            //            }
            //        }
            //    }
            //    if (!HelperClass.IsWindowOpen(typeof(SetUpSheet)))
            //    {
            //        this.Close();
            //        SetUpSheet obj = new SetUpSheet();
            //        obj.ShowDialog();
            //    }
            //    else
            //    {
            //        this.Close();
            //        HelperClass.activateWindow(typeof(SetUpSheet));
            //    }
            //}
        }
    }
}

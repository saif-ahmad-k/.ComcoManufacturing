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
    /// Interaction logic for View_SubCategory.xaml
    /// </summary>
    public partial class View_SubCategory : Window
    {
        BaseDataContext db = new BaseDataContext();
        int ParentCatId = 0;
        int CategoryId = 0;
        public View_SubCategory(int ParentId)
        {
            InitializeComponent();
            ParentCatId = ParentId;
            FillWrapPanelProductCategories();
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
        private void FillWrapPanelProductCategories()
        {
            var ParentCategories = db.tCategories.Where(p => p.ParentId == ParentCatId).ToList();
            foreach (var item in ParentCategories)
            {
                Button button = new Button();
                button.Content = item.Name;
                button.Width = 140;
                button.Height = 44;
                CategoryId = item.Category_ID;
                button.Click += new RoutedEventHandler(buttonCategory_Click);
                WrapPanelProductCategories.Children.Add(button);
            }
        }
        
        void buttonCategory_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var category = db.tCategories.Where(p => p.Name == btn.Content.ToString()).FirstOrDefault();
            if (category != null)
            {
                var subCats = db.tCategories.Where(p => p.ParentId == category.Category_ID).ToList();
                if (subCats.Count > 0)
                {
                    View.View_SubCategory obj = new View.View_SubCategory(category.Category_ID);
                    obj.ShowDialog();
                }
                else
                {
                    View.View_MainProduct obj = new View.View_MainProduct(category.Category_ID, false);
                    obj.ShowDialog();
                }
            }
        }
    }
}

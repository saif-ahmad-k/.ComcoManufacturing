using DataModel.Models;
using DataModel.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for View_Product.xaml
    /// </summary>
    public partial class View_Product : Window
    {
        BaseDataContext db = new BaseDataContext();
        int CategoryId = 0;
        bool fromSetupSheet = false;
        int SeqaunceNumber = 0;
        public View_Product()
        {
            InitializeComponent();
            cmbParentProductCategory.ItemsSource = null;
            cmbParentProductCategory.ItemsSource = db.tCategories.ToList();
        }
        public View_Product(int CatId, bool isFromCreateSetupSheet, int Sequance)
        {
            InitializeComponent();
            CategoryId = CatId;
            SeqaunceNumber = Sequance;
            fromSetupSheet = isFromCreateSetupSheet;
            cmbParentProductCategory.ItemsSource = null;
            cmbParentProductCategory.ItemsSource = db.tCategories.ToList();
            cmbParentProductCategory.SelectedValue = CategoryId;
            FillWrapPanelProductParentCategories(CategoryId);

        }
        private void FillWrapPanelProductParentCategories(int CategoryId)
        {
            WrapPanelProductParentsCategories.Children.Clear();
            var ParentCategories = db.tProducts.Where(p => p.CategoryId == CategoryId).ToList();
            foreach (var item in ParentCategories)
            {
                Button button = new Button();
                button.Content = item.ProductName + Environment.NewLine + item.QRN;
                button.Width = 150;
                button.Height = 60;
                if (item.ProductImage != null)
                {
                    ImageBrush brush;
                    BitmapImage bi;
                    using (var ms = new MemoryStream(item.ProductImage))
                    {
                        brush = new ImageBrush();

                        bi = new BitmapImage();
                        bi.BeginInit();
                        bi.CreateOptions = BitmapCreateOptions.None;
                        bi.CacheOption = BitmapCacheOption.OnLoad;
                        bi.StreamSource = ms;
                        bi.EndInit();
                    }

                    brush.ImageSource = bi;
                    button.Background = brush;
                }
                
                button.Click += new RoutedEventHandler(buttonParentCategory_Click);
                WrapPanelProductParentsCategories.Children.Add(button);
            }
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
        void buttonParentCategory_Click(object sender, RoutedEventArgs e)
        {
            if (fromSetupSheet)
            {
                Button btn = (Button)sender;
                string abc = btn.Content.ToString();
                string[] multiArray = abc.Split(new Char[] { '\r', '\n' });
                string Name = multiArray[0].ToString();
                var product = db.tProducts.Where(p => p.ProductName == Name).FirstOrDefault();
                if (product != null)
                {
                    foreach (Window item in Application.Current.Windows)
                    {
                        if (item.Name == "CreateSetUpSheet")
                        {
                            if (SeqaunceNumber == 1)
                            {
                                ((SetUpSheet)item).textBoxDrillTap.Text = product.ProductName;
                                ((SetUpSheet)item).textBoxQRN1.Text = product.QRN;
                            }
                            else if (SeqaunceNumber == 2)
                            {
                                ((SetUpSheet)item).textBoxStickBore.Text = product.ProductName;
                                ((SetUpSheet)item).textBoxQRN2.Text = product.QRN;
                            }
                            else if (SeqaunceNumber == 3)
                            {
                                ((SetUpSheet)item).textBoxTurrentHolder.Text = product.ProductName;
                                ((SetUpSheet)item).textBoxQRN3.Text = product.QRN;
                            }
                        }
                    }
                    if (!HelperClass.IsWindowOpen(typeof(SetUpSheet)))
                    {
                        this.Close();
                        SetUpSheet obj = new SetUpSheet();
                        obj.ShowDialog();
                    }
                    else
                    {
                        this.Close();
                        HelperClass.activateWindow(typeof(SetUpSheet));
                    }
                }
            }
        }

        private void CmbParentProductCategory_KeyDown(object sender, KeyEventArgs e)
        {
            tblCategory selectedCategory = (tblCategory)cmbParentProductCategory.SelectedItem;
            FillWrapPanelProductParentCategories(selectedCategory.Category_ID);
        }

        private void CmbParentProductCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tblCategory selectedCategory = (tblCategory)cmbParentProductCategory.SelectedItem;
            FillWrapPanelProductParentCategories(selectedCategory.Category_ID);
        }
    }
}

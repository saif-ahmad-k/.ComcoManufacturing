using DataModel.Models;
using Microsoft.Win32;
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
    /// Interaction logic for Edit_Product.xaml
    /// </summary>
    public partial class Edit_Product : Window
    {
        BaseDataContext db = new BaseDataContext();
        bool isProductValid = true;
        private byte[] _imageBytes = null;
        string imagePath;
        int ProductId = 0;
        tblProduct ExistingProduct = new tblProduct();
        public Edit_Product(int Product)
        {
            InitializeComponent();
            if (Product > 0)
            {
                ProductId = Product;
                ExistingProduct = db.tProducts.Find(Product);
            }
            FillControls(Product);
        }
        private void CheckProductValidations()
        {
            if (cmbParentProductCategory.SelectedIndex == -1)
            {
                isProductValid = false;
                MessageBox.Show("Category is mandatory!");
            }
            else if (String.IsNullOrEmpty(textBoxProductName.Text))
            {
                isProductValid = false;
                MessageBox.Show("Product Name is mandatory!");
            }
            else if (String.IsNullOrEmpty(textBoxProductCost.Text))
            {
                isProductValid = false;
                MessageBox.Show("Cost is mandatory!");
            }
            else if (db.tProducts.Where(p => p.ProductName == textBoxProductName.Text).FirstOrDefault() != null)
            {
                isProductValid = false;
                MessageBox.Show("Name already exist!");
            }
        }
        private void FillControls(int ProdId)
        {
            var Category = db.tCategories.Find(ExistingProduct.CategoryId);
            cmbParentProductCategory.ItemsSource = null;
            cmbParentProductCategory.ItemsSource = db.tCategories.ToList();
            if (Category!=null)
            {
                int index = -1;
                foreach (tblCategory cmbItem in cmbParentProductCategory.Items)
                {
                    index++; if (cmbItem.Category_ID == Category.Category_ID)
                    { break; }
                }
                cmbParentProductCategory.SelectedIndex = index;
            }
            textBoxProductName.Text = ExistingProduct.ProductName;
            textBoxProductQRN.Text = ExistingProduct.QRN;
            textBoxProductCost.Text = ExistingProduct.Cost.ToString();
        }
        private void ButtonSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckProductValidations();
                if (isProductValid)
                {
                    ExistingProduct.ProductName = textBoxProductName.Text;
                    ExistingProduct.Cost = textBoxProductCost.Text.ToDecimal();
                    ExistingProduct.QRN = textBoxProductQRN.Text;
                    tblCategory selectedProductCategory = (tblCategory)cmbParentProductCategory.SelectedItem;
                    if (selectedProductCategory != null)
                    {
                        ExistingProduct.CategoryId = selectedProductCategory.Category_ID;
                        db.SaveChanges();
                    }
                    ExistingProduct.ProductImage = _imageBytes;
                    db.SaveChanges();
                    MessageBox.Show("Updated SuccessFully!");
                    textBoxProductName.Text = "";
                    textBoxProductCost.Text = "0";
                    textBoxProductQRN.Text = "";
                }
                //FillControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button_Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imgPhoto.Source = new BitmapImage(new Uri(op.FileName));
                imagePath = op.FileName;
                using (var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    _imageBytes = new byte[fs.Length];
                    fs.Read(_imageBytes, 0, System.Convert.ToInt32(fs.Length));
                }
            }
        }
    }
}

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
    /// Interaction logic for CreateNew_Product.xaml
    /// </summary>
    public partial class CreateNew_Product : Window
    {
        BaseDataContext db = new BaseDataContext();
        bool isProductValid = true;
        private byte[] _imageBytes = null;
        string imagePath;
        int ParentCategoryId = 0;
        public CreateNew_Product(int CategoryId)
        {
            InitializeComponent();
            if (CategoryId > 0)
            {
                ParentCategoryId = CategoryId;
            }
            FillControls(ParentCategoryId);
        }

        private void ButtonSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckProductValidations();
                if (isProductValid)
                {
                    tblProduct product = new tblProduct();
                    product.ProductName = textBoxProductName.Text;
                    product.Cost = textBoxProductCost.Text.ToDecimal();
                    product.QRN = textBoxProductQRN.Text;
                    tblCategory selectedProductCategory = (tblCategory)cmbParentProductCategory.SelectedItem;
                    if (selectedProductCategory != null)
                    {
                        product.CategoryId = selectedProductCategory.Category_ID;
                        db.SaveChanges();
                    }
                    product.ProductImage = _imageBytes;
                    db.tProducts.Add(product);
                    db.SaveChanges();
                    MessageBox.Show("Added SuccessFully!");
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
        private void FillControls(int catId)
        {
            cmbParentProductCategory.ItemsSource = null;
            cmbParentProductCategory.ItemsSource = db.tCategories.ToList();
            if (catId > 0)
            {
                var cat = db.tCategories.Find(ParentCategoryId);
                int index = -1;
                foreach (tblCategory cmbItem in cmbParentProductCategory.Items)
                {
                    index++; if (cmbItem.Category_ID == cat.Category_ID)
                    { break; }
                }
                cmbParentProductCategory.SelectedIndex = index;
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

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
        int ParentProductId = 0;
        int colletInsertVisibilty = 0;
        int SequanceNumber = 0;
        public CreateNew_Product(int CategoryId, int ParentProduct, int ColletOrInsert, int Sequance)
        {
            InitializeComponent();
            if (CategoryId > 0)
            {
                ParentCategoryId = CategoryId;
            }
            if (ParentProduct > 0)
            {
                ParentProductId = ParentProduct;
            }
            SequanceNumber = Sequance;
            colletInsertVisibilty = ColletOrInsert;
            HelperClass.ShowWindowPath(PathLabel);
            FillControls(ParentCategoryId, ParentProductId);
        }

        private void ButtonSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckProductValidations();
                if (isProductValid)
                {
                    tblProduct selectedProduct = (tblProduct)cmbParentProduct.SelectedItem;
                    if (selectedProduct != null)
                    {
                        var AllParentsWithSameAttributes = db.tProducts.Where(p => p.ProductName == selectedProduct.ProductName && p.QRN == selectedProduct.QRN).ToList();
                        foreach(var prod in AllParentsWithSameAttributes)
                        {
                            tblProduct product = new tblProduct();
                            product.ProductName = textBoxProductName.Text;
                            product.Cost = textBoxProductCost.Text.ToDecimal();
                            product.QRN = textBoxProductQRN.Text;
                            product.Code = textBoxProductCode.Text;
                            product.PartNumber = textBoxProductPartNumber.Text;
                            product.Length = textBoxProductLength.Text;
                            product.Diameter = textBoxProductDiameter.Text;
                            tblCategory selectedProductCategory = (tblCategory)cmbParentProductCategory.SelectedItem;
                            if (selectedProductCategory != null)
                            {
                                product.CategoryId = selectedProductCategory.Category_ID;
                            }
                                product.ParentId = prod.Product_ID;
                            prod.IsParent = true;
                                if (prod.ParentId != null)
                                {
                                    if (prod.HolderTypeId != null && prod.HolderTypeId != 2)
                                    {
                                        product.HolderTypeId = prod.HolderTypeId + 1;
                                    }
                                }
                                else
                                {
                                    product.HolderTypeId = 2;
                                prod.HolderTypeId = 1;
                                }
                                db.SaveChanges();
                            if (CheckBoxInsert.IsChecked == true)
                            {
                                product.HolderTypeId = 3;
                            }
                            else if (CheckBoxColletBlade.IsChecked == true)
                            {
                                product.HolderTypeId = 4;
                            }
                            product.ProductImage = _imageBytes;
                            db.tProducts.Add(product);
                            db.SaveChanges();
                        }
                        MessageBox.Show("Added SuccessFully!");
                        textBoxProductName.Text = "";
                        textBoxProductCost.Text = "0";
                        textBoxProductQRN.Text = "";
                        this.Close();
                    }
                    else
                    {
                        tblCategory selectedProductCategory = (tblCategory)cmbParentProductCategory.SelectedItem;
                        if (selectedProductCategory != null)
                        {
                            var AllParentsWithSameAttributes = db.tCategories.Where(p => p.Name == selectedProductCategory.Name).ToList();
                            foreach(var cat in AllParentsWithSameAttributes)
                            {
                                tblProduct product = new tblProduct();
                                product.ProductName = textBoxProductName.Text;
                                product.Cost = textBoxProductCost.Text.ToDecimal();
                                product.QRN = textBoxProductQRN.Text;
                                product.Code = textBoxProductCode.Text;
                                product.PartNumber = textBoxProductPartNumber.Text;
                                product.Length = textBoxProductLength.Text;
                                product.Diameter = textBoxProductDiameter.Text;
                                product.CategoryId = cat.Category_ID;
                                if (CheckBoxInsert.IsChecked == true)
                                {
                                    product.HolderTypeId = 3;
                                }
                                else if (CheckBoxColletBlade.IsChecked == true)
                                {
                                    product.HolderTypeId = 4;
                                }
                                else
                                {
                                    product.HolderTypeId = 1;
                                }
                                product.ProductImage = _imageBytes;
                                db.tProducts.Add(product);
                                db.SaveChanges();
                            }
                            MessageBox.Show("Added SuccessFully!");
                            textBoxProductName.Text = "";
                            textBoxProductCost.Text = "0";
                            textBoxProductQRN.Text = "";
                            this.Close();
                        }
                        if (selectedProductCategory.Category_ID != null)
                        {
                            View.View_Product obj = new View.View_Product(selectedProductCategory.Category_ID.ToString().ToInteger(), 0, SequanceNumber, 1, null);
                            obj.ShowDialog();
                        }
                    }


                    
                    
                    //if(product.ParentId != null && product.CategoryId != null)
                    //{
                    //    View.View_Product obj = new View.View_Product(product.CategoryId.ToString().ToInteger(), product.ParentId.ToString().ToInteger(), SequanceNumber, product.HolderTypeId.ToString().ToInteger(), null);
                    //    obj.ShowDialog();
                    //}
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
            else if (db.tProducts.Where(p => p.ProductName == textBoxProductName.Text && p.QRN == textBoxProductQRN.Text).FirstOrDefault() != null)
            {
                isProductValid = false;
                MessageBox.Show("Name already exist!");
            }
        }
        private void FillControls(int catId, int ParentProduct)
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
                if (index > -1)
                {
                    cmbParentProductCategory.IsHitTestVisible = false;
                    cmbParentProductCategory.Focusable = false;
                }
                cmbParentProduct.ItemsSource = null;
                cmbParentProduct.ItemsSource = db.tProducts.Where(p => p.CategoryId == catId).ToList();
                CheckBoxInsert.Visibility = Visibility.Visible;
                CheckBoxColletBlade.Visibility = Visibility.Collapsed;
            }
            else
            {
                cmbParentProduct.ItemsSource = null;
                cmbParentProduct.ItemsSource = db.tProducts.ToList();
                CheckBoxInsert.Visibility = Visibility.Visible;
                CheckBoxColletBlade.Visibility = Visibility.Collapsed;
            }
            if (ParentProduct > 0)
            {
                var prod = db.tProducts.Find(ParentProduct);
                var cat = db.tCategories.Find(prod.CategoryId);
                int index = -1;
                foreach (tblCategory cmbItem in cmbParentProductCategory.Items)
                {
                    index++; if (cmbItem.Category_ID == cat.Category_ID)
                    { break; }
                }
                cmbParentProductCategory.SelectedIndex = index;
                if (index > -1)
                {
                    cmbParentProductCategory.IsHitTestVisible = false;
                    cmbParentProductCategory.Focusable = false;
                }
                cmbParentProduct.ItemsSource = null;
                cmbParentProduct.ItemsSource = db.tProducts.Where(p => p.CategoryId == catId).ToList();
                if (!cmbParentProduct.HasItems)
                {
                    cmbParentProduct.ItemsSource = null;
                    cmbParentProduct.ItemsSource = db.tProducts.Where(p => p.IsParent == true || p.IsParent == null).ToList();
                }
                int index1 = -1;
                foreach (tblProduct cmbItem in cmbParentProduct.Items)
                {
                    index1++; if (cmbItem.Product_ID == ParentProduct)
                    { break; }
                }
                cmbParentProduct.SelectedIndex = index1;
                if (index1 > -1)
                {
                    cmbParentProduct.IsHitTestVisible = false;
                    cmbParentProduct.Focusable = false;
                }
                if (prod.HolderTypeId == 2)
                {
                    CheckBoxInsert.Visibility = Visibility.Visible;
                    CheckBoxColletBlade.Visibility = Visibility.Visible;
                    CheckBoxInsert.IsChecked = true;
                }
                else
                {
                    CheckBoxInsert.Visibility = Visibility.Collapsed;
                    CheckBoxColletBlade.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                cmbParentProduct.ItemsSource = null;
                cmbParentProduct.ItemsSource = db.tProducts.ToList();
            }
            if (cmbParentProduct.SelectedIndex > -1)
            {
                labelParentProduct.Content = "Select Parent";
                cmbParentProductCategory.Visibility = Visibility.Hidden;
                labelCategory.Visibility = Visibility.Hidden;
            }
            else if(cmbParentProductCategory.SelectedIndex > -1)
            {
                labelCategory.Content = "Select Parent";
                cmbParentProduct.Visibility = Visibility.Hidden;
                labelParentProduct.Visibility = Visibility.Hidden;
            }
            if (colletInsertVisibilty == 3)
            {
                CheckBoxInsert.Visibility = Visibility.Visible;
                CheckBoxColletBlade.Visibility = Visibility.Collapsed;
                CheckBoxInsert.IsChecked = true;
            }else if(colletInsertVisibilty == 4)
            {
                CheckBoxInsert.Visibility = Visibility.Collapsed;
                CheckBoxColletBlade.Visibility = Visibility.Visible;
                CheckBoxColletBlade.IsChecked = true;
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

        private void CmbParentProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tblProduct selectedProduct = (tblProduct)cmbParentProduct.SelectedItem;
            if (selectedProduct != null)
            {
                var cat = db.tCategories.Find(selectedProduct.CategoryId);
                int index = -1;
                foreach (tblCategory cmbItem in cmbParentProductCategory.Items)
                {
                    index++; if (cmbItem.Category_ID == cat.Category_ID)
                    { break; }
                }
                cmbParentProductCategory.SelectedIndex = index;
            }
        }

        private void CmbParentProduct_KeyDown(object sender, KeyEventArgs e)
        {
            tblProduct selectedProduct = (tblProduct)cmbParentProduct.SelectedItem;
            if (selectedProduct != null)
            {
                var cat = db.tCategories.Find(selectedProduct.CategoryId);
                int index = -1;
                foreach (tblCategory cmbItem in cmbParentProductCategory.Items)
                {
                    index++; if (cmbItem.Category_ID == cat.Category_ID)
                    { break; }
                }
                cmbParentProductCategory.SelectedIndex = index;
            }
        }

        private void CheckBoxColletBlade_Checked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxColletBlade.IsChecked == true)
            {
                CheckBoxInsert.IsChecked = false;
            }
            else
            {
                CheckBoxInsert.IsChecked = true;
            }
        }

        private void CheckBoxInsert_Checked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxInsert.IsChecked == true)
            {
                CheckBoxColletBlade.IsChecked = false;
            }
            else
            {
                CheckBoxColletBlade.IsChecked = true;
            }
        }

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

using DataModel.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            HelperClass.ShowWindowPath(PathLabel);
            FillControls(Product);
        }
        private void CheckProductValidations()
        {
            if (ComboBoxCategory.SelectedItems.Count == 0)
            {
                isProductValid = false;
                MessageBox.Show("Category is mandatory!");
            }
            else if (String.IsNullOrEmpty(textBoxProductName.Text))
            {
                isProductValid = false;
                MessageBox.Show("Product Name is mandatory!");
            }
        }
        private void FillControls(int ProdId)
        {
            var CategoryList = new List<tblCategory>();
            var prod = db.tProducts.Find(ProdId);
            int parentType = 1;
            var prodList = db.tProducts.Where(p => p.ProductName == prod.ProductName).ToList();
            var parentprod = db.tProducts.Find(prod.ParentId);
            if (parentprod != null)
            {
                parentType = parentprod.HolderTypeId.ToString().ToInteger();
            }
            
            foreach(var prodd in prodList)
            {
                var cat = db.tCategories.Find(prodd.CategoryId);
                if (cat != null)
                {
                    ComboBoxCategory.SelectedItems.Add(cat);
                }
                if (prodd.ParentId != null)
                {
                    var parentProd = db.tProducts.Find(prodd.ParentId);
                    if (parentProd != null)
                    {
                        ComboBoxParentProduct.SelectedItems.Add(parentProd);
                    }
                }
                

            }
            var Category = db.tCategories.Find(ExistingProduct.CategoryId);
            ComboBoxCategory.ItemsSource = null;
            ComboBoxCategory.ItemsSource = db.tCategories.ToList();
            ComboBoxParentProduct.ItemsSource = null;
            if (parentType > 0 && parentType != null)
            {
                ComboBoxParentProduct.ItemsSource = db.tProducts.Where(p => p.HolderTypeId == parentType).ToList();
            }
            else
            {
                ComboBoxParentProduct.ItemsSource = db.tProducts.Where(p => p.IsParent == true && p.HolderTypeId == null).ToList();

            }
            //if (Category!=null)
            //{
            //    int index = -1;
            //    foreach (tblCategory cmbItem in ComboBoxCategory.Items)
            //    {
            //        cmbItem.isSelected = false;
            //        index++; if (cmbItem.Category_ID == Category.Category_ID)
            //        {
            //            ComboBoxCategory.SelectedItems.Add(cmbItem);
            //            break; }
            //    }

            //}
            //ObservableCollection<SelectableObject<tblCategory>>();
            textBoxProductName.Text = ExistingProduct.ProductName;
            textBoxProductQRN.Text = ExistingProduct.QRN;
            textBoxProductCost.Text = ExistingProduct.Cost.ToString();
            if(ExistingProduct.HolderTypeId == 3)
            {
                CheckBoxInsert.IsChecked = true;
                CheckBoxColletBlade.IsChecked = false;
            }
            else if (ExistingProduct.HolderTypeId == 4)
            {
                CheckBoxColletBlade.IsChecked = true;
                CheckBoxInsert.IsChecked = false;
            }
            if (ComboBoxParentProduct.SelectedItems.Count > 0)
            {
                labelParentProduct.Content = "Select Parent";
                ComboBoxCategory.Visibility = Visibility.Hidden;
                labelCategory.Visibility = Visibility.Hidden;
            }
            else if (ComboBoxCategory.SelectedItems.Count > 0)
            {
                labelCategory.Content = "Select Parent";
                ComboBoxParentProduct.Visibility = Visibility.Hidden;
                labelParentProduct.Visibility = Visibility.Hidden;
            }
        }
        private void ButtonSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckProductValidations();
                if (isProductValid)
                {

                    foreach (tblCategory parent in ComboBoxCategory.SelectedItems)
                    {
                        ExistingProduct.ProductName = textBoxProductName.Text;
                        ExistingProduct.Cost = textBoxProductCost.Text.ToDecimal();
                        ExistingProduct.QRN = textBoxProductQRN.Text;
                        ExistingProduct.Code = textBoxProductCode.Text;
                        ExistingProduct.PartNumber = textBoxProductPartNumber.Text;
                        ExistingProduct.Length = textBoxProductLength.Text;
                        ExistingProduct.Diameter = textBoxProductDiameter.Text;
                        ExistingProduct.CategoryId = parent.Category_ID;
                        ExistingProduct.ProductImage = _imageBytes;
                        db.SaveChanges();
                        //if (ExistingProduct != null)
                        //{
                        //    foreach (tblProduct prod in db.tProducts.Where(p => p.ParentId == ExistingProduct.Product_ID))
                        //    {
                        //        var newChildProd = new tblProduct();
                        //        newChildProd = prod;
                        //        newChildProd.ParentId = newprod.Product_ID;
                        //        db.tProducts.Add(newChildProd);
                        //        db.SaveChanges();
                        //    }
                        //}
                    }
                    
                    MessageBox.Show("Updated SuccessFully!");
                    textBoxProductName.Text = "";
                    textBoxProductCost.Text = "0";
                    textBoxProductQRN.Text = "";
                    this.Close();
                }
                else
                {
                    isProductValid = true;
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

        private void ButtonAddNewProduct_Click(object sender, RoutedEventArgs e)
        {
            try
                {
                CheckProductValidations();
                if (isProductValid)
                {
                    //if (db.tProducts.Where(p => p.ProductName == textBoxProductName.Text && p.QRN == textBoxProductQRN.Text).FirstOrDefault() != null)
                    //{
                    //    MessageBox.Show("already exist!");
                    //}
                    //else
                    //{
                    foreach (tblProduct parent in ComboBoxParentProduct.SelectedItems)
                    {
                        if(db.tProducts.Where(p=>p.ParentId == parent.Product_ID && p.ProductName == textBoxProductName.Text && p.QRN== textBoxProductQRN.Text).ToList().Count == 0)
                        {
                            var cat = db.tCategories.Find(parent.CategoryId);
                            var newprod = new tblProduct();
                            newprod.ProductName = textBoxProductName.Text;
                            newprod.Cost = textBoxProductCost.Text.ToDecimal();
                            newprod.QRN = textBoxProductQRN.Text;
                            newprod.Code = textBoxProductCode.Text;
                            newprod.PartNumber = textBoxProductPartNumber.Text;
                            newprod.Length = textBoxProductLength.Text;
                            newprod.Diameter = textBoxProductDiameter.Text;
                            newprod.CategoryId = cat.Category_ID;
                            if (parent.HolderTypeId != null && parent.HolderTypeId > 0)
                            {
                                newprod.HolderTypeId = parent.HolderTypeId + 1;
                            }
                            else
                            {
                                newprod.HolderTypeId = 2;
                            }
                            if (CheckBoxInsert.IsChecked == true)
                            {
                                newprod.HolderTypeId = 3;
                            }
                            else if (CheckBoxColletBlade.IsChecked == true)
                            {
                                newprod.HolderTypeId = 4;
                            }
                            parent.IsParent = true;
                            newprod.ProductImage = _imageBytes;
                            newprod.ParentId = parent.Product_ID;
                            db.tProducts.Add(newprod);
                            db.SaveChanges();
                            if (ExistingProduct != null)
                            {
                                foreach (tblProduct prod in db.tProducts.Where(p => p.ParentId == ExistingProduct.Product_ID).ToList())
                                {
                                    var newChildProd = new tblProduct();
                                    newChildProd.Code = prod.Code;
                                    newChildProd.Cost = prod.Cost;
                                    newChildProd.Diameter = prod.Diameter;
                                    newChildProd.HolderTypeId = prod.HolderTypeId;
                                    newChildProd.IsParent = prod.IsParent;
                                    newChildProd.IsParentInsert = prod.IsParentInsert;
                                    newChildProd.Length = prod.Length;
                                    newChildProd.PartNumber = prod.PartNumber;
                                    newChildProd.ProductImage = prod.ProductImage;
                                    newChildProd.ProductName = prod.ProductName;
                                    newChildProd.QRN = prod.QRN;
                                    newChildProd.CategoryId = newprod.CategoryId;
                                    newChildProd.ParentId = newprod.Product_ID;
                                    db.Entry(prod).State = EntityState.Detached;
                                    db.tProducts.Add(newChildProd);

                                    db.SaveChanges();
                                }
                            }
                        }
                        
                    }
                    if (ComboBoxParentProduct.SelectedItems.Count == 0)
                    {
                        foreach (tblCategory parent in ComboBoxCategory.SelectedItems)
                        {
                            var newprod = new tblProduct();
                            newprod.ProductName = textBoxProductName.Text;
                            newprod.Cost = textBoxProductCost.Text.ToDecimal();
                            newprod.QRN = textBoxProductQRN.Text;
                            newprod.Code = textBoxProductCode.Text;
                            newprod.PartNumber = textBoxProductPartNumber.Text;
                            newprod.Length = textBoxProductLength.Text;
                            newprod.Diameter = textBoxProductDiameter.Text;
                            newprod.CategoryId = parent.Category_ID;
                            newprod.HolderTypeId = 1;
                            newprod.ProductImage = _imageBytes;
                            db.tProducts.Add(newprod);
                            db.SaveChanges();
                            if (ExistingProduct != null)
                            {
                                foreach (tblProduct prod in db.tProducts.Where(p => p.ParentId == ExistingProduct.Product_ID).ToList())
                                {
                                    var newChildProd = new tblProduct();
                                    newChildProd.Code = prod.Code;
                                    newChildProd.Cost = prod.Cost;
                                    newChildProd.Diameter = prod.Diameter;
                                    newChildProd.HolderTypeId = prod.HolderTypeId;
                                    newChildProd.IsParent = prod.IsParent;
                                    newChildProd.IsParentInsert = prod.IsParentInsert;
                                    newChildProd.Length = prod.Length;
                                    newChildProd.PartNumber = prod.PartNumber;
                                    newChildProd.ProductImage = prod.ProductImage;
                                    newChildProd.ProductName = prod.ProductName;
                                    newChildProd.QRN = prod.QRN;
                                    newChildProd.CategoryId = newprod.CategoryId;
                                    newChildProd.ParentId = newprod.Product_ID;
                                    db.Entry(prod).State = EntityState.Detached;
                                    db.tProducts.Add(newChildProd);

                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    

                    MessageBox.Show("Updated SuccessFully!");
                    textBoxProductName.Text = "";
                    textBoxProductCost.Text = "0";
                    textBoxProductQRN.Text = "";
                    this.Close();
                    //}

                }
                else
                {
                    isProductValid = true;
                }
                //FillControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

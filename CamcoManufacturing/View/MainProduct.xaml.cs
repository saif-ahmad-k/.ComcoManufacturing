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
    /// Interaction logic for MainProduct.xaml
    /// </summary>
    public partial class MainProduct : Window
    {
        BaseDataContext db = new BaseDataContext();
        bool isCategoryValid = true;
        bool isProductValid = true;
        private byte[] _imageBytes = null;
        string imagePath;
        private byte[] _CategoryImageBytes = null;
        string CategoryImagePath;
        public MainProduct()
        {
            InitializeComponent();
            HelperClass.ShowWindowPath(PathLabel);
            FillControls();
            textBoxProductCost.Text = "0";
        }


        private void FillControls()
        {
            cmbParentProductCategory.ItemsSource = null;
            cmbParentProductCategory.ItemsSource = db.tCategories.Where(p => p.IsParent == false || p.IsParent == null).ToList();
            cmbParentCategory.ItemsSource = null;
            cmbParentCategory.ItemsSource = db.tCategories.ToList();
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
                FillControls();
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
            }else if (db.tProducts.Where(p => p.ProductName == textBoxProductName.Text).FirstOrDefault() != null)
            {
                isProductValid = false;
                MessageBox.Show("Name already exist!");
            }
        }
        private void ChecCategoryValidations()
        {
            if (String.IsNullOrEmpty(textBoxCategoryName.Text))
            {
                isCategoryValid = false;
                MessageBox.Show("Category Name is mandatory!");
            }
        }
        private void ButtonSaveCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChecCategoryValidations();
                if (isCategoryValid)
                {
                    tblCategory category = new tblCategory();
                    category.Name = textBoxCategoryName.Text;
                    //tblMachineType selectedMachine = (tblMachineType)cmbCategoryMachineType.SelectedItem;
                    //if (selectedMachine != null)
                    //{
                    //    category.MachineId = selectedMachine.Machine_Id;
                    //}
                    tblCategory selectedCategory = (tblCategory)cmbParentCategory.SelectedItem;
                    if (selectedCategory != null)
                    {
                        category.ParentId = selectedCategory.Category_ID;
                        selectedCategory.IsParent = true;
                        db.SaveChanges();
                    }
                    else
                    {
                        category.IsParent = true;
                    }
                    category.CategoryImage = _CategoryImageBytes;
                    db.tCategories.Add(category);
                    db.SaveChanges();
                    MessageBox.Show("Added SuccessFully!");
                    textBoxCategoryName.Text = "";
                }
                FillControls();
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

        private void ButtonCategoryBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imgCategoryPhoto.Source = new BitmapImage(new Uri(op.FileName));
                CategoryImagePath = op.FileName;
                using (var fs = new FileStream(CategoryImagePath, FileMode.Open, FileAccess.Read))
                {
                    _CategoryImageBytes = new byte[fs.Length];
                    fs.Read(_CategoryImageBytes, 0, System.Convert.ToInt32(fs.Length));
                }
            }
        }

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

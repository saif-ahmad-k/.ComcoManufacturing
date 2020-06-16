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
    /// Interaction logic for CreateNew_Category.xaml
    /// </summary>
    public partial class CreateNew_Category : Window
    {
        BaseDataContext db = new BaseDataContext();
        bool isCategoryValid = true;
        int ParentCategoryId = 0;
        private byte[] _CategoryImageBytes = null;
        string CategoryImagePath;
        public CreateNew_Category(int CategoryId)
        {
            InitializeComponent();
            if (CategoryId > 0)
            {
                ParentCategoryId = CategoryId;
            }
            FillControls(ParentCategoryId);
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
                //FillControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FillControls(int catId)
        {
            cmbParentCategory.ItemsSource = null;
            cmbParentCategory.ItemsSource = db.tCategories.ToList();
            if (catId > 0)
            {
                var cat = db.tCategories.Find(ParentCategoryId);
                int index = -1;
                foreach (tblCategory cmbItem in cmbParentCategory.Items)
                {
                    index++; if (cmbItem.Category_ID == cat.Category_ID)
                    { break; }
                }
                cmbParentCategory.SelectedIndex = index;
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
    }
}

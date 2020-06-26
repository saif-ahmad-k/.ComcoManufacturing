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
    /// Interaction logic for Edit_Category.xaml
    /// </summary>
    public partial class Edit_Category : Window
    {
        BaseDataContext db = new BaseDataContext();
        bool isCategoryValid = true;
        int SelectedCategoryId = 0;
        private byte[] _CategoryImageBytes = null;
        string CategoryImagePath;
        int ParentCat = 0;
        tblCategory existingCategory = new tblCategory();
        public Edit_Category(int CategoryId)
        {
            InitializeComponent();
            if (CategoryId > 0)
            {
                SelectedCategoryId = CategoryId;
            }
            HelperClass.ShowWindowPath(PathLabel);
            FillControls(SelectedCategoryId);
        }
        private void ButtonSaveCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChecCategoryValidations();
                if (isCategoryValid)
                {
                    existingCategory.Name = textBoxCategoryName.Text;
                    //tblMachineType selectedMachine = (tblMachineType)cmbCategoryMachineType.SelectedItem;
                    //if (selectedMachine != null)
                    //{
                    //    category.MachineId = selectedMachine.Machine_Id;
                    //}
                    existingCategory.CategoryImage = _CategoryImageBytes;
                    db.SaveChanges();
                    MessageBox.Show("Updated SuccessFully!");
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
            existingCategory = db.tCategories.Find(catId);
            if (existingCategory != null)
            {
                cmbParentCategory.ItemsSource = null;
                cmbParentCategory.ItemsSource = db.tCategories.Where(p => p.Category_ID == existingCategory.ParentId).ToList();
                if (catId > 0)
                {
                    var cat = db.tCategories.Find(existingCategory.ParentId);
                    if (cat != null)
                    {
                        ParentCat = cat.Category_ID;
                        int index = -1;
                        foreach (tblCategory cmbItem in cmbParentCategory.Items)
                        {
                            index++; if (cmbItem.Category_ID == cat.Category_ID)
                            { break; }
                        }
                        cmbParentCategory.SelectedIndex = index;
                    }
                    textBoxCategoryName.Text = existingCategory.Name;
                }
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

        private void ButtonAddNewCategory_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (!HelperClass.IsWindowOpen(typeof(View.CreateNew_Category)))
            {
                View.CreateNew_Category obj = new View.CreateNew_Category(ParentCat);
                obj.ShowDialog();
            }
            else
            {
                Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "CreateCategory");
                if (win != null)
                {
                    win.Close();
                    View.CreateNew_Category obj = new View.CreateNew_Category(ParentCat);
                    obj.ShowDialog();
                }
                
            }
        }
    }
}

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
    /// Interaction logic for CreateNew_Category.xaml
    /// </summary>
    public partial class CreateNew_Category : Window
    {
        BaseDataContext db = new BaseDataContext();
        public CreateNew_Category()
        {
            InitializeComponent();
            FillControls();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tblCategory category = new tblCategory();
                category.Name = textBoxCategoryName.Text;
                tblCategory selectedCategory = (tblCategory)cmbParentCategory.SelectedItem;
                if (selectedCategory != null)
                {
                    category.ParentId = selectedCategory.Category_ID;
                    selectedCategory.IsParent = true;
                    db.SaveChanges();
                }

                db.tCategories.Add(category);
                db.SaveChanges();
                MessageBox.Show("Added SuccessFully!");
                textBoxCategoryName.Text = "";
                FillControls();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private void FillControls()
        {
            cmbParentCategory.ItemsSource = null;
            cmbParentCategory.ItemsSource = db.tCategories.ToList();
        }
    }
}

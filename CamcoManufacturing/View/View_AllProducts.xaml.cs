﻿using DataModel.Models;
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
    /// Interaction logic for View_AllProducts.xaml
    /// </summary>
    public partial class View_AllProducts : Window
    {
        BaseDataContext db = new BaseDataContext();
        int CategoryId = 0;
        int ParentProductId = 0;
        public View_AllProducts(int category,int parentProduct)
        {
            InitializeComponent();
            CategoryId = category;
            ParentProductId = parentProduct;
            HelperClass.ShowWindowPath(PathLabel);
            FillControls();
        }
        private void FillControls()
        {
            dataGridProducts.ItemsSource = null;
            if (ParentProductId > 0)
            {
                dataGridProducts.ItemsSource = db.tProducts.Where(p => p.ParentId == ParentProductId).ToList();
            }else if (CategoryId > 0)
            {
                dataGridProducts.ItemsSource = db.tProducts.Where(p => p.CategoryId == CategoryId).ToList();
            }
            else
            {
                dataGridProducts.ItemsSource = db.tProducts.ToList();
            }
            
        }
        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            tblProduct dataRowView = (tblProduct)((Button)e.Source).DataContext;
            //this.Close();
            if (!HelperClass.IsWindowOpen(typeof(View.Edit_Product)))
            {
                View.Edit_Product obj = new View.Edit_Product(dataRowView.Product_ID);
                obj.ShowDialog();
            }
            else
            {
                Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "EditProduct");
                win.Close();
                View.Edit_Product obj = new View.Edit_Product(dataRowView.Product_ID);
                obj.ShowDialog();
            }
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                tblProduct dataRowView = (tblProduct)((Button)e.Source).DataContext;
                db.tProducts.Remove(dataRowView);
                db.SaveChanges();
                MessageBox.Show("Deleted SuccessFully!");
                FillControls();
            }
        }

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

using DataModel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for View_SubCategory.xaml
    /// </summary>
    public partial class View_SubCategory : Window
    {
        BaseDataContext db = new BaseDataContext();
        int ParentCatId = 0;
        int CategoryId = 0;
        int SequanceNumber = 0;
        public View_SubCategory(int ParentId, int sequance)
        {
            InitializeComponent();
            ParentCatId = ParentId;
            SequanceNumber = sequance;
            FillWrapPanelProductCategories();
            HelperClass.ShowWindowPath(PathLabel);
            
        }

        private void AddNeWrapPanel1(tblCategory item)
        {
            Button button = new Button();
            //button.Content = item.ProductName + Environment.NewLine + item.QRN;
            button.Width = 150;
            button.Height = 80;
            button.Content = item.Name;
            if (item.CategoryImage != null)
            {
                ImageBrush brush;
                BitmapImage bi;
                using (var ms = new MemoryStream(item.CategoryImage))
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
            StackPanel sp = new StackPanel();
            Border buttonBorder = new Border();
            buttonBorder.Background = Brushes.SkyBlue;
            buttonBorder.BorderBrush = Brushes.Black;
            buttonBorder.BorderThickness = new Thickness(1);
            buttonBorder.Child = button;

            Label productNameLabel = new Label();
            productNameLabel.Content = item.Name;
            productNameLabel.FontWeight = FontWeights.Bold;
            productNameLabel.FontSize = 14;
            productNameLabel.Height = 50;
            productNameLabel.Width = 150;
            productNameLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            productNameLabel.VerticalContentAlignment = VerticalAlignment.Center;
            Border labelBorder = new Border();
            labelBorder.Background = Brushes.LightGray;
            labelBorder.BorderBrush = Brushes.Black;
            labelBorder.BorderThickness = new Thickness(1);
            labelBorder.Child = productNameLabel;
            
            Label emptyLabel = new Label();
            emptyLabel.Content = " ";
            emptyLabel.FontWeight = FontWeights.Bold;
            emptyLabel.Height = 30;
            emptyLabel.FontSize = 10;
            emptyLabel.Background = Brushes.Gray;
            
            emptyLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            emptyLabel.VerticalContentAlignment = VerticalAlignment.Center;
            Border labelBorderEmpty = new Border();
            labelBorderEmpty.Background = Brushes.White;
            labelBorderEmpty.BorderBrush = Brushes.Black;
            labelBorderEmpty.BorderThickness = new Thickness(0);
            labelBorderEmpty.Child = emptyLabel;

            sp.Children.Add(labelBorder);
            sp.Children.Add(buttonBorder);
            sp.Children.Add(labelBorderEmpty);
            button.Click += new RoutedEventHandler(buttonCategory_Click);
            
            WrapPanelProductCategories.Children.Add(sp);
        }
        private void FillWrapPanelProductCategories()
        {
            if (ParentCatId > 0)
            {
                var parentCat = db.tCategories.Find(ParentCatId);
                if (parentCat != null)
                {
                    var name= parentCat.Name.Replace(" ", String.Empty);
                    var abc = Regex.Replace(parentCat.Name, @"[^0-9a-zA-Z]+", "");
                    this.Name = Regex.Replace(abc, @"[\d-]", string.Empty);
                }
                var ParentCategories = db.tCategories.Where(p => p.ParentId == ParentCatId).ToList();
                foreach (var item in ParentCategories)
                {
                    AddNeWrapPanel1(item);
                }
            }
            else
            {
                var ParentCategories = db.tCategories.Where(p => p.ParentId == null).ToList();
                foreach (var item in ParentCategories)
                {
                    AddNeWrapPanel1(item);
                }
            }
        }
        void AddNewButton()
        {
            Button button1 = new Button();
            button1.Width = 50;
            button1.Height = 44;
            ImageBrush brush1;
            BitmapImage bi1 = new BitmapImage();
            bi1.BeginInit();
            bi1.UriSource = new Uri(@"Images\AddNewIcon.png", UriKind.Relative);
            bi1.EndInit();
            brush1 = new ImageBrush();
            brush1.ImageSource = bi1;
            button1.Background = brush1;
            button1.Click += new RoutedEventHandler(AddNewCategory_Click);
            WrapPanelProductCategories.Children.Add(button1);
        }
        void buttonCategory_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            var category = db.tCategories.Where(p => p.Name == btn.Content.ToString()).FirstOrDefault();
            if (category != null)
            {
                var subCats = db.tCategories.Where(p => p.ParentId == category.Category_ID).ToList();
                if (subCats.Count > 0)
                {
                    View.View_SubCategory obj = new View.View_SubCategory(category.Category_ID, SequanceNumber);
                    obj.ShowDialog();
                }
                else
                {
                    View.View_Product obj = new View.View_Product(category.Category_ID, 0, SequanceNumber, 0, null);
                    obj.ShowDialog();
                }
            }
        }
        void AddNewCategory_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (!HelperClass.IsWindowOpen(typeof(View.CreateNew_Category)))
            {
                View.CreateNew_Category obj = new View.CreateNew_Category(ParentCatId);
                obj.ShowDialog();
            }
            else
            {
                Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "ViewCategory");
                win.Close();
                View.CreateNew_Category obj = new View.CreateNew_Category(ParentCatId);
                obj.ShowDialog();
            }
        }

        private void ButtonAddNewCategory_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (!HelperClass.IsWindowOpen(typeof(View.CreateNew_Category)))
            {
                View.CreateNew_Category obj = new View.CreateNew_Category(ParentCatId);
                obj.ShowDialog();
            }
            else
            {
                Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "ViewCategory");
                win.Close();
                View.CreateNew_Category obj = new View.CreateNew_Category(ParentCatId);
                obj.ShowDialog();
            }
        }

        private void ButtonEditCategory_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
            if (!HelperClass.IsWindowOpen(typeof(View.View_AllCategories)))
            {
                View.View_AllCategories obj = new View.View_AllCategories();
                obj.ShowDialog();
            }
            else
            {
                Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "AllCategories");
                win.Close();
                View.View_AllCategories obj = new View.View_AllCategories();
                obj.ShowDialog();
            }
        }

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

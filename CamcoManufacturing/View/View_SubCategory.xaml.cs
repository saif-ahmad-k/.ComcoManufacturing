using DataModel.Models;
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
        }

        
        private void FillWrapPanelProductCategories()
        {
            if (ParentCatId > 0)
            {
                var ParentCategories = db.tCategories.Where(p => p.ParentId == ParentCatId).ToList();
                foreach (var item in ParentCategories)
                {
                    Button button = new Button();
                    button.Content = item.Name;
                    button.Width = 140;
                    button.Height = 80;
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
                    button.Click += new RoutedEventHandler(buttonCategory_Click);
                    WrapPanelProductCategories.Children.Add(button);
                }
            }
            else
            {
                var ParentCategories = db.tCategories.Where(p => p.ParentId == null).ToList();
                foreach (var item in ParentCategories)
                {
                    Button button = new Button();
                    button.Content = item.Name;
                    button.Width = 140;
                    button.Height = 80;
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
                    button.Click += new RoutedEventHandler(buttonCategory_Click);
                    WrapPanelProductCategories.Children.Add(button);
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
    }
}

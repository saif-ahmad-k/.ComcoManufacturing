using DataModel.Models;
using DataModel.Models.ViewModels;
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
    /// Interaction logic for View_Product.xaml
    /// </summary>
    public partial class View_Product : Window
    {
        BaseDataContext db = new BaseDataContext();
        int CategoryId = 0;
        int ParentProductId = 0;
        int SequanceNumber = 0;
        int HolderTypeEnumId = 0;
        List<int> HoldersList = new List<int>();
        public View_Product()
        {
            InitializeComponent();
            HelperClass.ShowWindowPath(PathLabel);
        }
        public View_Product(int CatId, int parentProduct, int Sequance, int HolderTypeEnum, List<int> holderList)
        {
            InitializeComponent();
            CategoryId = CatId;
            ParentProductId = parentProduct;
            SequanceNumber = Sequance;
            HolderTypeEnumId = HolderTypeEnum;
            if (holderList != null && holderList.Count > 0)
            {
                HoldersList = holderList;
            }
            
            FillWrapPanelProductParentCategories(CategoryId, ParentProductId);
            HelperClass.ShowWindowPath(PathLabel);
        }
        private void FillWrapPanelProductParentCategories(int CategoryId, int ProductId)
        {
            WrapPanelProductParentsCategories.Children.Clear();
            if (CategoryId > 0)
            {
                var ParentCategories = db.tProducts.Where(p => p.CategoryId == CategoryId && p.ParentId == null).ToList();
                if (ParentCategories.Count > 0)
                {
                    LabelHolderType.Content = "Turret Holders";
                    this.Name = "TurretHolders";
                    foreach (var item in ParentCategories)
                    {
                        Button button = new Button();
                        button.Content = item.ProductName + Environment.NewLine + item.QRN;
                        button.Width = 150;
                        button.Height = 60;
                        if (item.ProductImage != null)
                        {
                            ImageBrush brush;
                            BitmapImage bi;
                            using (var ms = new MemoryStream(item.ProductImage))
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
                        button.Click += new RoutedEventHandler(buttonParentCategory_Click);
                        WrapPanelProductParentsCategories.Children.Add(button);
                    }
                    ButtonAddNewCategory.Visibility =
        (ButtonAddNewCategory.Visibility == Visibility.Visible) ?
           Visibility.Collapsed : Visibility.Visible;
                }
            }
            else if(ProductId > 0)
            {
                var ParentCategories = db.tProducts.Where(p => p.ParentId == ProductId).ToList();
                if (HolderTypeEnumId > 0)
                {
                    var holder = db.tblHolderTypes.Find(HolderTypeEnumId);
                    if (holder != null)
                    {
                        LabelHolderType.Content = holder.HolderName;
                        this.Name = LabelHolderType.Content.ToString().Replace(" ", String.Empty);
                    }
                    ParentCategories = ParentCategories.Where(p => p.HolderTypeId == HolderTypeEnumId.ToString().ToInteger()).ToList();
                }
                else if (HoldersList.Count > 1)
                {
                    ParentCategories.Clear();
                    WrapPanelProductParentsCategories.Children.Clear();
                    WrapPanelProductParentsCategories_Copy.Children.Clear();
                    int count = 1;
                    foreach(var item1 in HoldersList)
                    {
                       
                        
                        var ParentNew = db.tProducts.Where(p => p.ParentId == ProductId && p.HolderTypeId == item1).ToList();
                        if (ParentNew.Count > 0)
                        {
                            var holder = db.tblHolderTypes.Find(ParentNew.FirstOrDefault().HolderTypeId);
                            
                            foreach (var item in ParentNew)
                            {
                                Button button = new Button();
                                button.Content = item.ProductName + Environment.NewLine + item.QRN;
                                button.Width = 150;
                                button.Height = 60;
                                if (item.ProductImage != null)
                                {
                                    ImageBrush brush;
                                    BitmapImage bi;
                                    using (var ms = new MemoryStream(item.ProductImage))
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
                                button.Click += new RoutedEventHandler(buttonParentCategory_Click);
                                if (count == 1)
                                {
                                    if (holder != null)
                                    {
                                        LabelHolderType.Content = holder.HolderName;
                                        this.Name = holder.HolderName.Replace(" ", String.Empty);
                                    }
                                    WrapPanelProductParentsCategories.Children.Add(button);
                                }
                                else
                                {
                                    if (holder != null)
                                    {
                                        LabelHolderType_Copy.Content = holder.HolderName;
                                        this.Name = holder.HolderName.Replace(" ", String.Empty);
                                    }
                                    WrapPanelProductParentsCategories_Copy.Children.Add(button);
                                }
                                
                            }
                            ButtonAddNewCategory.Visibility =
                (ButtonAddNewCategory.Visibility == Visibility.Visible) ?
                   Visibility.Collapsed : Visibility.Visible;
                        }
                        count++;
                    }
                }
                if (ParentCategories.Count > 0)
                {

                    foreach (var item in ParentCategories)
                    {
                        Button button = new Button();
                        button.Content = item.ProductName + Environment.NewLine + item.QRN;
                        button.Width = 150;
                        button.Height = 60;
                        if (item.ProductImage != null)
                        {
                            ImageBrush brush;
                            BitmapImage bi;
                            using (var ms = new MemoryStream(item.ProductImage))
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
                        button.Click += new RoutedEventHandler(buttonParentCategory_Click);
                        WrapPanelProductParentsCategories.Children.Add(button);
                    }
                    ButtonAddNewCategory.Visibility =
        (ButtonAddNewCategory.Visibility == Visibility.Visible) ?
           Visibility.Collapsed : Visibility.Visible;
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
            button1.Click += new RoutedEventHandler(AddNewProduct_Click);
            WrapPanelProductParentsCategories.Children.Add(button1);
        }
        private void ButtonAddNewProduct_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (!HelperClass.IsWindowOpen(typeof(View.CreateNew_Product)))
            {
                View.CreateNew_Product obj = new View.CreateNew_Product(CategoryId, ParentProductId);
                obj.ShowDialog();
            }
            else
            {
                Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "ViewProduct");
                win.Close();
                View.CreateNew_Product obj = new View.CreateNew_Product(CategoryId, ParentProductId);
                obj.ShowDialog();
            }
        }
        void buttonParentCategory_Click(object sender, RoutedEventArgs e)
        {
            List<int> HolderList = new List<int>();
            Button btn = (Button)sender;
            string abc = btn.Content.ToString();
            string[] multiArray = abc.Split(new Char[] { '\r', '\n' });
            string Name = multiArray[0].ToString();
            var resultDetail = db.tProducts.Where(p => p.ProductName == Name).FirstOrDefault();
            if (SequanceNumber > 0)
            {
                if (resultDetail != null)
                {
                    foreach (Window item in Application.Current.Windows)
                    {
                        if (item.Name == "CreateSetUpSheet")
                        {
                            if (SequanceNumber == 1)
                            {
                                ((SetUpSheet)item).textBoxDrillTap.Text = resultDetail.ProductName;
                                ((SetUpSheet)item).textBoxQRN1.Text = resultDetail.QRN;
                            }
                            else if (SequanceNumber == 4)
                            {
                                ((SetUpSheet)item).textBoxColletBlade.Text = resultDetail.ProductName;
                                ((SetUpSheet)item).textBoxQRN4.Text = resultDetail.QRN;
                            }
                            else if (SequanceNumber == 2)
                            {
                                ((SetUpSheet)item).textBoxStickBore.Text = resultDetail.ProductName;
                                ((SetUpSheet)item).textBoxQRN2.Text = resultDetail.QRN;
                            }
                            else if (SequanceNumber == 3)
                            {
                                ((SetUpSheet)item).textBoxTurrentHolder.Text = resultDetail.ProductName;
                                ((SetUpSheet)item).textBoxQRN3.Text = resultDetail.QRN;
                            }
                            else
                            {
                                ((SetUpSheet)item).textBoxTurrentHolder.Text = resultDetail.ProductName;
                                ((SetUpSheet)item).textBoxQRN3.Text = resultDetail.QRN;
                            }
                        }
                        if (item.Name == "ViewCategory")
                        {
                            item.Close();
                        }
                    }
                    if (!HelperClass.IsWindowOpen(typeof(SetUpSheet)))
                    {
                        this.Close();
                        SetUpSheet obj = new SetUpSheet();
                        obj.ShowDialog();
                    }
                    else
                    {
                        this.Close();
                        HelperClass.activateWindow(typeof(SetUpSheet));
                    }
                }
            }
            else
            {
                if (resultDetail != null)
                {
                    var subProds = db.tProducts.Where(p => p.ParentId == resultDetail.Product_ID).ToList();
                    if (subProds.Count > 0)
                    {
                        foreach(var item in subProds)
                        {
                            if (!HolderList.Contains(item.HolderTypeId.ToString().ToInteger()))
                            {
                                HolderList.Add(item.HolderTypeId.ToString().ToInteger());
                            }
                        }
                        var subProdFirst = subProds.FirstOrDefault();
                        if (subProdFirst.HolderTypeId != null)
                        {
                            if (HolderList.Count == 1)
                            {
                                View.View_Product obj = new View.View_Product(0, resultDetail.Product_ID, 0, subProdFirst.HolderTypeId.ToString().ToInteger(), null);
                                obj.ShowDialog();
                            }
                            else
                            {
                                View.View_Product obj = new View.View_Product(0, resultDetail.Product_ID, 0, 0, HolderList);
                                obj.ShowDialog();
                            } 
                            
                            //  else if (subProdFirst.HolderTypeId == 3)
                            //{
                            //    View.View_Product obj = new View.View_Product(0, resultDetail.Product_ID, 0, tblHolderType.HolderTypesEnum.Insert);
                            //    obj.ShowDialog();
                            //}
                            //else if (subProdFirst.HolderTypeId == 4)
                            //{
                            //    View.View_Product obj = new View.View_Product(0, resultDetail.Product_ID, 0, tblHolderType.HolderTypesEnum.ColletBlade);
                            //    obj.ShowDialog();
                            //}
                        }
                        else
                        {
                            View.View_Product obj = new View.View_Product(0, resultDetail.Product_ID, 0, 0, null);
                            obj.ShowDialog();
                        }
                    }
                    else
                    {
                        View.View_Product obj = new View.View_Product(0, resultDetail.Product_ID, 0, 0, null);
                        obj.ShowDialog();
                    }
                }
            }
            
        }
        void AddNewProduct_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ButtonAddNewCategory_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (!HelperClass.IsWindowOpen(typeof(View.CreateNew_Category)))
            {
                View.CreateNew_Category obj = new View.CreateNew_Category(CategoryId);
                obj.ShowDialog();
            }
            else
            {
                Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "ViewProduct");
                win.Close();
                View.CreateNew_Category obj = new View.CreateNew_Category(CategoryId);
                obj.ShowDialog();
            }
        }

        private void ButtonShowAllProducts_Click(object sender, RoutedEventArgs e)
        {
            if (!HelperClass.IsWindowOpen(typeof(View.View_AllProducts)))
            {
                View.View_AllProducts obj = new View.View_AllProducts();
                obj.ShowDialog();
            }
            else
            {
                Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "AllProducts");
                win.Close();
                View.View_AllProducts obj = new View.View_AllProducts();
                obj.ShowDialog();
            }
        }
    }
}

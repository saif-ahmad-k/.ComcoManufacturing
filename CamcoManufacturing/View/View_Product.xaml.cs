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
            hideButtons();
            HelperClass.ShowWindowPath(PathLabel);
        }
        public View_Product(int CatId, int parentProduct, int Sequance, int HolderTypeEnum, List<int> holderList)
        {
            InitializeComponent();
            hideButtons();
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
        private void hideButtons()
        {
            buttonInsert.Visibility = Visibility.Collapsed;
            buttonColletBlade.Visibility = Visibility.Collapsed;
            buttonStickHolder.Visibility = Visibility.Collapsed;
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
                        AddNeWrapPanel1(item);
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
                        if (HolderTypeEnumId == 3 || HolderTypeEnumId == 4)
                        {
                            buttonInsert.Visibility = Visibility.Visible;
                            buttonColletBlade.Visibility = Visibility.Visible;
                        }else if (HolderTypeEnumId == 2)
                        {
                            buttonStickHolder.Visibility = Visibility.Visible;
                            buttonInsert.Visibility = Visibility.Visible;
                            buttonColletBlade.Visibility = Visibility.Visible;
                        }
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
                                
                                if (count == 1)
                                {
                                    if (holder != null)
                                    {
                                        LabelHolderType.Content = holder.HolderName;
                                        this.Name = holder.HolderName.Replace(" ", String.Empty);
                                    }
                                    AddNeWrapPanel1(item);
                                }
                                else
                                {
                                    if (holder != null)
                                    {
                                        LabelHolderType_Copy.Content = holder.HolderName;
                                        this.Name = holder.HolderName.Replace(" ", String.Empty);
                                    }
                                    AddNeWrapPanel2(item);
                                }
                                if (holder.Holder_Id == 3 || holder.Holder_Id == 4)
                                {
                                    buttonInsert.Visibility = Visibility.Visible;
                                    buttonColletBlade.Visibility = Visibility.Visible;
                                }
                                else if (holder.Holder_Id == 2)
                                {
                                    buttonStickHolder.Visibility = Visibility.Visible;
                                    buttonInsert.Visibility = Visibility.Visible;
                                    buttonColletBlade.Visibility = Visibility.Visible;
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
                        AddNeWrapPanel1(item);
                    }
                    ButtonAddNewCategory.Visibility =
        (ButtonAddNewCategory.Visibility == Visibility.Visible) ?
           Visibility.Collapsed : Visibility.Visible;
                }else if (ParentCategories.Count == 0)
                {
                    buttonStickHolder.Visibility = Visibility.Visible;
                    buttonInsert.Visibility = Visibility.Visible;
                    buttonColletBlade.Visibility = Visibility.Visible;
                }
            }
            if (WrapPanelProductParentsCategories_Copy.Children.Count == 0)
            {
                WrapPanelProductParentsCategories.Width = 610;
                ScrollViewerProductParentsCategories.Width = 640;
                ScrollViewerProductParentsCategories_Copy.Visibility = Visibility.Collapsed;
                WrapPanelProductParentsCategories_Copy.Visibility = Visibility.Collapsed;
            }
        }
        private void AddNeWrapPanel1(tblProduct item)
        {
            Button button = new Button();
            //button.Content = item.ProductName + Environment.NewLine + item.QRN;
            button.Width = 150;
            button.Height = 80;
            button.Content = item.ProductName;
            button.DataContext = item;
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
            StackPanel sp = new StackPanel();
            Border buttonBorder = new Border();
            buttonBorder.Background = Brushes.SkyBlue;
            buttonBorder.BorderBrush = Brushes.Black;
            buttonBorder.BorderThickness = new Thickness(1);
            buttonBorder.Child = button;

            Label productNameLabel = new Label();
            productNameLabel.Content = item.ProductName;
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

            Label QRNLabel = new Label();
            QRNLabel.Content = "QRN: " + item.QRN;
            QRNLabel.FontWeight = FontWeights.Bold;
            QRNLabel.Height = 30;
            QRNLabel.FontSize = 10;
            QRNLabel.Width = 150;
            QRNLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            QRNLabel.VerticalContentAlignment = VerticalAlignment.Center;
            Border labelBorderQRN = new Border();
            labelBorderQRN.Background = Brushes.White;
            labelBorderQRN.BorderBrush = Brushes.Black;
            labelBorderQRN.BorderThickness = new Thickness(1);
            labelBorderQRN.Child = QRNLabel;

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
            sp.Children.Add(labelBorderQRN);
            sp.Children.Add(buttonBorder);
            sp.Children.Add(labelBorderEmpty);
            button.Click += new RoutedEventHandler(buttonParentCategory_Click);
            WrapPanelProductParentsCategories.Children.Add(sp);
        }
        private void AddNeWrapPanel2(tblProduct item)
        {
            Button button = new Button();
            //button.Content = item.ProductName + Environment.NewLine + item.QRN;
            button.Width = 150;
            button.Height = 80;
            button.Content = item.ProductName;
            button.DataContext = item;
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
            StackPanel sp = new StackPanel();
            Border buttonBorder = new Border();
            buttonBorder.Background = Brushes.SkyBlue;
            buttonBorder.BorderBrush = Brushes.Black;
            buttonBorder.BorderThickness = new Thickness(1);
            buttonBorder.Child = button;

            Label productNameLabel = new Label();
            productNameLabel.Content = item.ProductName;
            productNameLabel.FontWeight = FontWeights.Bold;
            productNameLabel.FontSize = 14;
            productNameLabel.Height = 50;
            productNameLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            productNameLabel.VerticalContentAlignment = VerticalAlignment.Center;
            Border labelBorder = new Border();
            labelBorder.Background = Brushes.LightGray;
            labelBorder.BorderBrush = Brushes.Black;
            labelBorder.BorderThickness = new Thickness(1);
            labelBorder.Child = productNameLabel;

            Label QRNLabel = new Label();
            QRNLabel.Content = "QRN: " + item.QRN;
            QRNLabel.FontWeight = FontWeights.Bold;
            QRNLabel.Height = 30;
            QRNLabel.FontSize = 10;
            QRNLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            QRNLabel.VerticalContentAlignment = VerticalAlignment.Center;
            Border labelBorderQRN = new Border();
            labelBorderQRN.Background = Brushes.White;
            labelBorderQRN.BorderBrush = Brushes.Black;
            labelBorderQRN.BorderThickness = new Thickness(1);
            labelBorderQRN.Child = QRNLabel;

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
            sp.Children.Add(labelBorderQRN);
            sp.Children.Add(buttonBorder);
            sp.Children.Add(labelBorderEmpty);
            button.Click += new RoutedEventHandler(buttonParentCategory_Click);
            WrapPanelProductParentsCategories_Copy.Children.Add(sp);
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
                View.CreateNew_Product obj = new View.CreateNew_Product(CategoryId, ParentProductId, 0);
                obj.ShowDialog();
            }
            else
            {
                Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "ViewProduct");
                win.Close();
                View.CreateNew_Product obj = new View.CreateNew_Product(CategoryId, ParentProductId, 0);
                obj.ShowDialog();
            }
        }
        void buttonParentCategory_Click(object sender, RoutedEventArgs e)
        {
            List<int> HolderList = new List<int>();
            tblProduct resultDetail = ((Button)sender).DataContext as tblProduct;
            //tblProduct btn = (tblProduct)sender;
            //string Name = btn.ProductName.ToString();
            //var resultDetail = db.tProducts.Where(p => p.ProductName == Name).FirstOrDefault();
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
                                ((SetUpSheet)item).textBoxDrillTap.DataContext = resultDetail;
                                ((SetUpSheet)item).textBoxDrillTap.Text = resultDetail.ProductName;
                                ((SetUpSheet)item).textBoxQRN1.Text = resultDetail.QRN;
                            }
                            else if (SequanceNumber == 4)
                            {
                                ((SetUpSheet)item).textBoxColletBlade.DataContext = resultDetail;
                                ((SetUpSheet)item).textBoxColletBlade.Text = resultDetail.ProductName;
                                ((SetUpSheet)item).textBoxQRN4.Text = resultDetail.QRN;
                            }
                            else if (SequanceNumber == 2)
                            {
                                ((SetUpSheet)item).textBoxStickBore.DataContext = resultDetail;
                                ((SetUpSheet)item).textBoxStickBore.Text = resultDetail.ProductName;
                                ((SetUpSheet)item).textBoxQRN2.Text = resultDetail.QRN;
                            }
                            else if (SequanceNumber == 3)
                            {
                                ((SetUpSheet)item).textBoxTurrentHolder.DataContext = resultDetail;
                                ((SetUpSheet)item).textBoxTurrentHolder.Text = resultDetail.ProductName;
                                ((SetUpSheet)item).textBoxQRN3.Text = resultDetail.QRN;
                            }
                            else
                            {
                                ((SetUpSheet)item).textBoxTurrentHolder.DataContext = resultDetail;
                                ((SetUpSheet)item).textBoxTurrentHolder.Text = resultDetail.ProductName;
                                ((SetUpSheet)item).textBoxQRN3.Text = resultDetail.QRN;
                            }
                        }
                        if (item.Name != "CreateSetUpSheet" && item.Name != "Main" && item.Name != "")
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
                View.View_AllProducts obj = new View.View_AllProducts(CategoryId, ParentProductId);
                obj.ShowDialog();
            }
            else
            {
                Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "AllProducts");
                win.Close();
                View.View_AllProducts obj = new View.View_AllProducts(CategoryId, ParentProductId);
                obj.ShowDialog();
            }
        }

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonStickHolder_Click(object sender, RoutedEventArgs e)
        {
            if (!HelperClass.IsWindowOpen(typeof(View.CreateNew_Product)))
            {
                View.CreateNew_Product obj = new View.CreateNew_Product(CategoryId, ParentProductId, 0);
                obj.ShowDialog();
            }
            else
            {
                Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "ViewProduct");
                win.Close();
                View.CreateNew_Product obj = new View.CreateNew_Product(CategoryId, ParentProductId, 0);
                obj.ShowDialog();
            }
        }

        private void ButtonColletBlade_Click(object sender, RoutedEventArgs e)
        {
            if (!HelperClass.IsWindowOpen(typeof(View.CreateNew_Product)))
            {
                View.CreateNew_Product obj = new View.CreateNew_Product(CategoryId, ParentProductId, 4);
                obj.ShowDialog();
            }
            else
            {
                Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "ViewProduct");
                win.Close();
                View.CreateNew_Product obj = new View.CreateNew_Product(CategoryId, ParentProductId, 4);
                obj.ShowDialog();
            }
        }

        private void ButtonInsert_Click(object sender, RoutedEventArgs e)
        {
            if (!HelperClass.IsWindowOpen(typeof(View.CreateNew_Product)))
            {
                View.CreateNew_Product obj = new View.CreateNew_Product(CategoryId, ParentProductId, 3);
                obj.ShowDialog();
            }
            else
            {
                Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "ViewProduct");
                win.Close();
                View.CreateNew_Product obj = new View.CreateNew_Product(CategoryId, ParentProductId, 3);
                obj.ShowDialog();
            }
        }
    }
}

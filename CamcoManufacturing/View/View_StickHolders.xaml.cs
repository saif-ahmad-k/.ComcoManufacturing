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
    /// Interaction logic for View_StickHolders.xaml
    /// </summary>
    public partial class View_StickHolders : Window
    {
        BaseDataContext db = new BaseDataContext();
        int TurretTypeId = 0;
        int TurretHolderId = 0;
        int SeqaunceNumber = 0;
        public View_StickHolders()
        {
            InitializeComponent();
        }
        public View_StickHolders(int TurretType,int TurretHolder, int sequance)
        {
            InitializeComponent();
            TurretTypeId = TurretType;
            TurretHolderId = TurretHolder;
            SeqaunceNumber = sequance;
            FillWrapPanel(TurretTypeId, TurretHolderId);
        }
        private void FillWrapPanel(int TurretType, int TurretHolder)
        {
            WrapPanelStickHolders.Children.Clear();
            var result = new List<tStickHolder>();
            if (TurretHolderId > 0)
            {
                result = db.tStickHolders.Where(p => p.TurretTypeId == TurretType && p.TurretHolderId == TurretHolder).ToList();
            }
            else
            {
                result = db.tStickHolders.Where(p => p.TurretTypeId == TurretType).ToList();
            }
             
            foreach (var item in result)
            {
                Button button = new Button();
                button.Content = item.StickHolderName + Environment.NewLine + item.StickHolderQRN;
                button.Width = 150;
                button.Height = 60;
                if (item.StickHolderImage != null)
                {
                    ImageBrush brush;
                    BitmapImage bi;
                    using (var ms = new MemoryStream(item.StickHolderImage))
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
                button.Click += new RoutedEventHandler(buttonSelectedItem_Click);
                WrapPanelStickHolders.Children.Add(button);
            }
            void buttonSelectedItem_Click(object sender, RoutedEventArgs e)
            {
                Button btn = (Button)sender;
                string abc = btn.Content.ToString();
                string[] multiArray = abc.Split(new Char[] { '\r', '\n' });
                string Name = multiArray[0].ToString();
                var resultDetail = db.tStickHolders.Where(p => p.StickHolderName == Name).FirstOrDefault();
                if (resultDetail != null)
                {
                    foreach (Window item in Application.Current.Windows)
                    {
                        if (item.Name == "CreateSetUpSheet")
                        {
                            if (SeqaunceNumber == 2)
                            {
                                ((SetUpSheet)item).textBoxStickBore.Text = resultDetail.StickHolderName;
                                ((SetUpSheet)item).textBoxQRN2.Text = resultDetail.StickHolderQRN;
                            }
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
        }
    }
}

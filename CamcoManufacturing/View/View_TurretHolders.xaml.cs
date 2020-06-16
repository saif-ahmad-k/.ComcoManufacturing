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
    /// Interaction logic for View_TurretHolders.xaml
    /// </summary>
    public partial class View_TurretHolders : Window
    {
        BaseDataContext db = new BaseDataContext();
        int TurretTypeId = 0;
        int SeqaunceNumber = 0;
        public View_TurretHolders()
        {
            InitializeComponent();
        }
        public View_TurretHolders(int TurretType,int sequance)
        {
            InitializeComponent();
            TurretTypeId = TurretType;
            SeqaunceNumber = sequance;
            FillWrapPanel(TurretTypeId);

        }
        private void FillWrapPanel(int TurretType)
        {
            WrapPanelTurretHolders.Children.Clear();
            var result = db.tTurretHolders.Where(p => p.TurretTypeId == TurretType).ToList();
            foreach (var item in result)
            {
                Button button = new Button();
                button.Content = item.TurretHolderName + Environment.NewLine + item.TurretHolderQRN;
                button.Width = 150;
                button.Height = 60;
                if (item.TurretHolderImage != null)
                {
                    ImageBrush brush;
                    BitmapImage bi;
                    using (var ms = new MemoryStream(item.TurretHolderImage))
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
                WrapPanelTurretHolders.Children.Add(button);
            }
            void buttonSelectedItem_Click(object sender, RoutedEventArgs e)
            {
                    Button btn = (Button)sender;
                    string abc = btn.Content.ToString();
                    string[] multiArray = abc.Split(new Char[] { '\r', '\n' });
                    string Name = multiArray[0].ToString();
                    var resultDetail = db.tTurretHolders.Where(p => p.TurretHolderName == Name).FirstOrDefault();
                    if (resultDetail != null)
                    {
                        foreach (Window item in Application.Current.Windows)
                        {
                            if (item.Name == "CreateSetUpSheet")
                            {
                                if (SeqaunceNumber == 3)
                                {
                                    ((SetUpSheet)item).textBoxTurrentHolder.Text = resultDetail.TurretHolderName;
                                    ((SetUpSheet)item).textBoxQRN3.Text = resultDetail.TurretHolderQRN;
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

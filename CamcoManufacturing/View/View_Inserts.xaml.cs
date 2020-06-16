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
    /// Interaction logic for View_Inserts.xaml
    /// </summary>
    public partial class View_Inserts : Window
    {
        BaseDataContext db = new BaseDataContext();
        int TurretTypeId = 0;
        int StickHolderId = 0;
        int SeqaunceNumber = 0;
        public View_Inserts()
        {
            InitializeComponent();
        }
        public View_Inserts(int TurretType, int StickHolder, int sequance)
        {
            InitializeComponent();
            TurretTypeId = TurretType;
            StickHolderId = StickHolder;
            SeqaunceNumber = sequance;
            FillWrapPanel(TurretTypeId, StickHolderId);
        }
        private void FillWrapPanel(int TurretType, int StickHolder)
        {
            WrapPanelInserts.Children.Clear();
            var result = new List<tTurretInsert>();
            if (StickHolder > 0)
            {
                result = db.tTurretInserts.Where(p => p.TurretTypeId == TurretType && p.StickHolderId == StickHolder).ToList();
            }
            else
            {
                result = db.tTurretInserts.Where(p => p.TurretTypeId == TurretType).ToList();
            }

            foreach (var item in result)
            {
                Button button = new Button();
                button.Content = item.InsertName + Environment.NewLine + item.InsertQRN;
                button.Width = 150;
                button.Height = 60;
                if (item.InsertImage != null)
                {
                    ImageBrush brush;
                    BitmapImage bi;
                    using (var ms = new MemoryStream(item.InsertImage))
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
                WrapPanelInserts.Children.Add(button);
            }
            void buttonSelectedItem_Click(object sender, RoutedEventArgs e)
            {
                Button btn = (Button)sender;
                string abc = btn.Content.ToString();
                string[] multiArray = abc.Split(new Char[] { '\r', '\n' });
                string Name = multiArray[0].ToString();
                var resultDetail = db.tTurretInserts.Where(p => p.InsertName == Name).FirstOrDefault();
                if (resultDetail != null)
                {
                    foreach (Window item in Application.Current.Windows)
                    {
                        if (item.Name == "CreateSetUpSheet")
                        {
                            if (SeqaunceNumber == 1)
                            {
                                ((SetUpSheet)item).textBoxDrillTap.Text = resultDetail.InsertName;
                                ((SetUpSheet)item).textBoxQRN1.Text = resultDetail.InsertQRN;
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

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
    /// Interaction logic for CreateNew_Operation.xaml
    /// </summary>
    public partial class CreateNew_Operation : Window
    {
        BaseDataContext db = new BaseDataContext();
        bool isValidte = true;
        public CreateNew_Operation()
        {
            InitializeComponent();
            FillControls();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckValidations();
                if (isValidte)
                {
                    var tOperation = new tblOperation();
                    tblParts selectedPart = (tblParts)cmbPart.SelectedItem;
                    tOperation.RM_DESC = textBoxRM_DESC.Text;
                    tOperation.RM_WKCTR = textBoxRM_WKCTR.Text;
                    tOperation.RM_CT = textBoxRM_CT.Text;
                    tOperation.CT_MINUTES = textBoxCT_MINUTES.Text;
                    tOperation.RM_OP = textBoxRM_OP.Text;
                    tOperation.PartId = selectedPart.PartId;
                    tOperation.RM_SWITCH = checkBoxRM_SWITCH.IsChecked;
                    tOperation.VERIFIED_CYCLETIME = checkBoxVERIFIED_CYCLETIME.IsChecked;
                    db.tOperations.Add(tOperation);
                    db.SaveChanges();
                    MessageBox.Show("Saved Successfully!");
                }
            }catch(Exception ex)
            {
                MessageBox.Show("Exception! " + ex.Message);
            }
            
        }
        private void FillControls()
        {
            cmbPart.ItemsSource = null;
            cmbPart.ItemsSource = db.tParts.ToList();
        }
        private void CheckValidations()
        {
            if (cmbPart.SelectedIndex <= -1)
            {
                isValidte = false;
                MessageBox.Show("Part# is mandatory!");
            }
            else if (String.IsNullOrEmpty(textBoxRM_DESC.Text))
            {
                isValidte = false;
                MessageBox.Show("RM_DESC is mandatory!");
            }
            
        }

        private void ButtonList_Click(object sender, RoutedEventArgs e)
        {
            if (!HelperClass.IsWindowOpen(typeof(View.View_Operation)))
            {
                View.View_Operation obj = new View.View_Operation();
                obj.ShowDialog();
            }
            else
            {
                HelperClass.activateWindow(typeof(View.View_Operation));
            }
        }
    }
}

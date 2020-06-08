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
using CamcoManufacturing.View;
using DataModel.Models;

namespace CamcoManufacturing.Views
{
    /// <summary>
    /// Interaction logic for ManufacturingCheckSheet.xaml
    /// </summary>
    public partial class ManufacturingCheckSheet : Window
    {
        BaseDataContext db = new BaseDataContext();
        public ManufacturingCheckSheet()
        {
            InitializeComponent();
            FillControls();
        }

        private void FillControls()
        {
            PartNumberComboBox.ItemsSource = null;
            PartNumberComboBox.ItemsSource = db.tParts.ToList();
            CreatedByComboBox.ItemsSource = null;
            CreatedByComboBox.ItemsSource = db.tEmployees.ToList();
            CustomerComboBox.ItemsSource = null;
            CustomerComboBox.ItemsSource = db.tCustomers.ToList();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var mcs = new tblManufacturingCheckSheet();
            
            tblParts selectedPart = (tblParts)PartNumberComboBox.SelectedItem;
            tblCustomer selectedCustomer = (tblCustomer)CustomerComboBox.SelectedItem;
            double getCustomerID = selectedCustomer.CustomerID;
            string PartNumber = selectedPart.Part.ToString();
            tblEmployee selectedEmployee = (tblEmployee)CreatedByComboBox.SelectedItem;
            string custAbb = db.tCustomers.Where(x => x.CustomerID == getCustomerID).FirstOrDefault()?.CustomerAbbreviation;
            mcs.DateCreated = DateTime.UtcNow.ToString("MM-dd-yyyy");
            mcs.RevisionNumber = "00";
            mcs.IsRevised = false;
            mcs.OperationNumber = OperationNumberTxt.Text;
            mcs.PartId = selectedPart.PartId.ToString();
            mcs.CustomerId = selectedCustomer.CustomerID.ToString();
            mcs.EmployeeId = selectedEmployee.EmployeeId.ToString();
            string[] DN = new string[] { custAbb, PartNumber, "CS-" + mcs.OperationNumber, "REV-00" };
            mcs.DrawingNumber = String.Join("_", DN);
            db.tManufacturingCheckSheets.Add(mcs);
            db.SaveChanges();
            MessageBox.Show("Saved Successfully!");
            ManufacturingSheetListView view = new ManufacturingSheetListView();
            view.ShowDialog();
        }

        private void ButtonList_Click(object sender, RoutedEventArgs e)
        {
            ManufacturingSheetListView view = new ManufacturingSheetListView();
            view.ShowDialog();
        }
    }
}

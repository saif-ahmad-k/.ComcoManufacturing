﻿using DataModel.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
    /// Interaction logic for SetUpSheet.xaml
    /// </summary>
    public partial class SetUpSheet : Window
    {
        bool isValidte = true;
        private byte[] _imageBytes = null;
        string imagePath;
        BaseDataContext db = new BaseDataContext();
        public SetUpSheet()
        {
            InitializeComponent();
            HelperClass.ShowWindowPath(PathLabel);
            FillControls();
        }
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckValidations();
                if (isValidte)
                {
                    var camcorecord = new tblCamcoRecord();
                    tblParts selectedPart = (tblParts)cmbPart.SelectedItem;
                    tblCustomer selectedCustomer = (tblCustomer)cmbCustomer.SelectedItem;
                    tblChuckCollet selectedChuckCollet = (tblChuckCollet)cmbChuckCollet.SelectedItem;
                    tblMachineType selectedMachine = (tblMachineType)cmbMachine.SelectedItem;
                    tblCategory selectedTurret = (tblCategory)cmbProductCategory.SelectedItem;
                    tblOperation selectedOperation = (tblOperation)cmbOperation.SelectedItem;
                    //tblLathe selectedLathe = (tblLathe)cmbLathe.SelectedItem;
                    tblEmployee selectedEmployee = (tblEmployee)cmbEmployee.SelectedItem;
                    camcorecord.CNCProgramNumber = txtCNCProgram.Text;
                    camcorecord.CreatedDate = Convert.ToDateTime(DateTime.UtcNow.ToString("MM-dd-yyyy"));
                    camcorecord.CustomerId = selectedCustomer.CustomerID;
                    camcorecord.Cycletime = txtCycletime.Text;
                    double getCustomerID = selectedCustomer.CustomerID;
                    string PartNumber = selectedPart.Part.ToString();
                    camcorecord.EmployeeId = selectedEmployee.EmployeeId;
                    //camcorecord.Fixturing = txtFixturing.Text;
                    //camcorecord.ITTPartShift = txtITTShift.Text;
                    //camcorecord.LatheId = selectedLathe.LatheId;
                    //camcorecord.OperationNumber = txtOperation.Text;
                    camcorecord.PartId = selectedPart.PartId;
                    camcorecord.Chuck_Collet_Id = selectedChuckCollet.Id;
                    camcorecord.MachineId = selectedMachine.Machine_Id;
                    camcorecord.TurrentTypeId = selectedTurret.Category_ID;
                    camcorecord.OperationNumber = selectedOperation.RM_DESC;
                    camcorecord.OperationId = selectedOperation.Operation_Id;
                    camcorecord.ChuckPresure = textBoxChuckPressure.Text;
                    camcorecord.ColletQRN = textBoxColletJawsQRN.Text;
                    camcorecord.CamcoRecordImage = _imageBytes;
                    string custAbb = db.tCustomers.Where(x => x.CustomerID == getCustomerID).FirstOrDefault()?.CustomerAbbreviation;
                    string[] DN = new string[] { custAbb, PartNumber, "SU-" + camcorecord.OperationNumber, "REV-00" };
                    camcorecord.DrawingNumber = String.Join("_", DN);
                    db.tCamcoRecords.Add(camcorecord);
                    db.SaveChanges();
                    for (int i = 0; i < dataGridCamcoRecordDetails.Items.Count; i++)
                    {
                        var camcorecordDetail = new ComcoRecordDetail();
                        DataGridRow row = (DataGridRow)dataGridCamcoRecordDetails.ItemContainerGenerator.ContainerFromIndex(i);
                        camcorecordDetail.lineNumber = (dataGridCamcoRecordDetails.Columns[0].GetCellContent(row) as TextBlock).Text.ToInteger();
                        camcorecordDetail.toolNumber = (dataGridCamcoRecordDetails.Columns[1].GetCellContent(row) as TextBlock).Text;
                        camcorecordDetail.OffSetNumber = (dataGridCamcoRecordDetails.Columns[2].GetCellContent(row) as TextBlock).Text;
                        camcorecordDetail.ToolDescription = (dataGridCamcoRecordDetails.Columns[3].GetCellContent(row) as TextBlock).Text;
                        camcorecordDetail.SFM_RPM = (dataGridCamcoRecordDetails.Columns[4].GetCellContent(row) as TextBlock).Text;
                        camcorecordDetail.FEEDiN_REV = (dataGridCamcoRecordDetails.Columns[5].GetCellContent(row) as TextBlock).Text;
                        camcorecordDetail.Projection = (dataGridCamcoRecordDetails.Columns[6].GetCellContent(row) as TextBlock).Text;
                        camcorecordDetail.InsertDrillTap = (dataGridCamcoRecordDetails.Columns[7].GetCellContent(row) as TextBlock).Text;
                        camcorecordDetail.QRN1 = (dataGridCamcoRecordDetails.Columns[8].GetCellContent(row) as TextBlock).Text;
                        camcorecordDetail.StickHolderBore = (dataGridCamcoRecordDetails.Columns[9].GetCellContent(row) as TextBlock).Text;
                        camcorecordDetail.QRN2 = (dataGridCamcoRecordDetails.Columns[10].GetCellContent(row) as TextBlock).Text;
                        camcorecordDetail.HolderOnTurret = (dataGridCamcoRecordDetails.Columns[11].GetCellContent(row) as TextBlock).Text;
                        camcorecordDetail.QRN3 = (dataGridCamcoRecordDetails.Columns[12].GetCellContent(row) as TextBlock).Text;
                        camcorecordDetail.ColletBlade = (dataGridCamcoRecordDetails.Columns[13].GetCellContent(row) as TextBlock).Text;
                        camcorecordDetail.QRN4 = (dataGridCamcoRecordDetails.Columns[14].GetCellContent(row) as TextBlock).Text;
                        camcorecordDetail.CamcoRecordId = camcorecord.Id;
                        db.tComcoRecordDetails.Add(camcorecordDetail);
                        db.SaveChanges();
                    }

                    MessageBox.Show("Saved Successfully!");
                    CamcoRecordReportView obj1 = new CamcoRecordReportView(camcorecord.Id);
                    obj1.ShowDialog();
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GotoMCSPage(object sender, RoutedEventArgs e)
        {

        }

        private void buttonList_Click(object sender, RoutedEventArgs e)
        {
            ListView view = new ListView();
            view.ShowDialog();
        }
        private void FillControls()
        {
            cmbPart.ItemsSource = null;
            cmbPart.ItemsSource = db.tParts.ToList();
            cmbEmployee.ItemsSource = null;
            cmbEmployee.ItemsSource = db.tEmployees.ToList();
            cmbCustomer.ItemsSource = null;
            cmbCustomer.ItemsSource = db.tCustomers.ToList();
            cmbChuckCollet.ItemsSource = null;
            cmbChuckCollet.ItemsSource = db.tChuckCollets.ToList();
            cmbMachine.ItemsSource = null;
            cmbMachine.ItemsSource = db.tMachineTypes.ToList();
            cmbProductCategory.ItemsSource = null;
            cmbProductCategory.ItemsSource = db.tTurrentTypes.ToList();
            cmbCustomer.ItemsSource = null;
            cmbCustomer.ItemsSource = db.tCustomers.ToList();
            cmbOperation.ItemsSource = null;
            cmbOperation.ItemsSource = db.tOperations.OrderBy(p => p.RM_DESC + p.RM_OP).ToList();
            //cmbLathe.ItemsSource = null;
            //cmbLathe.ItemsSource = db.tLathes.ToList();
        }
        private void CheckValidations()
        {
            if (cmbOperation.SelectedIndex <= -1)
            {
                isValidte = false;
                MessageBox.Show("Operation# is mandatory!");
            }
            else if (String.IsNullOrEmpty(txtCycletime.Text))
            {
                isValidte = false;
                MessageBox.Show("CycleTime is mandatory!");
            }
            else if (String.IsNullOrEmpty(txtCNCProgram.Text))
            {
                isValidte = false;
                MessageBox.Show("Program # is mandatory!");
            }
            else if (cmbPart.SelectedIndex <= -1)
            {
                isValidte = false;
                MessageBox.Show("Part# is mandatory!");
            }
            else if (cmbEmployee.SelectedIndex <= -1)
            {
                isValidte = false;
                MessageBox.Show("CreatedBy is mandatory!");
            }
            //else if (cmbMaterial.SelectedIndex <= -1)
            //{
            //    isValidte = false;
            //    MessageBox.Show("Material is mandatory!");
            //}
        }

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (!HelperClass.IsWindowOpen(typeof(MainWindow)))
            {
                MainWindow obj = new MainWindow();
                obj.ShowDialog();
            }
            else
            {
                HelperClass.activateWindow(typeof(MainWindow));
            }
        }

        private void ButtonAddTurretTool_Click(object sender, RoutedEventArgs e)
        {
            if (!HelperClass.IsWindowOpen(typeof(CreateNew_TurretTool)))
            {
                CreateNew_TurretTool obj = new CreateNew_TurretTool();
                obj.ShowDialog();
            }
            else
            {
                HelperClass.activateWindow(typeof(CreateNew_TurretTool));
            }
        }

        private void CmbPart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tblParts selectedPart = (tblParts)cmbPart.SelectedItem;
            if (selectedPart != null)
            {
                MaterialTxt.Text = selectedPart.Material;
                if (selectedPart.CustomerId != null)
                {
                    cmbCustomer.SelectedValue = selectedPart.CustomerId;
                }
                cmbOperation.ItemsSource = null;
                cmbOperation.ItemsSource = db.tOperations.Where(p => p.PartId == selectedPart.PartId).OrderBy(p => p.RM_DESC + p.RM_OP).ToList();
            }
            
        }

        private void CmbPart_SelectionChanged(object sender, KeyEventArgs e)
        {
            tblParts selectedPart = (tblParts)cmbPart.SelectedItem;
            if (selectedPart != null)
            {
                MaterialTxt.Text = selectedPart.Material;
                if (selectedPart.CustomerId != null)
                {
                    cmbCustomer.SelectedValue = selectedPart.CustomerId;
                }
                cmbOperation.ItemsSource = null;
                cmbOperation.ItemsSource = db.tOperations.Where(p => p.PartId == selectedPart.PartId).OrderBy(p => p.RM_DESC + p.RM_OP).ToList();
            }
        }

        private void ButtonAddDetailsRow_Click(object sender, RoutedEventArgs e)
        {
            if(buttonAddDetailsRow.Content.Equals("Update Line"))
            {
                var data = dataGridCamcoRecordDetails.SelectedItem;
                var abc = dataGridCamcoRecordDetails.Items.Contains(data);
                if ((dataGridCamcoRecordDetails.SelectedCells[0].Column.GetCellContent(data) as TextBlock) != null)
                {
                    (dataGridCamcoRecordDetails.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text = textBoxLineNumber.Text;
                }

                (dataGridCamcoRecordDetails.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text = textBoxToolNumber.Text;
                (dataGridCamcoRecordDetails.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text = textBoxOffSetNumber.Text;
                (dataGridCamcoRecordDetails.SelectedCells[3].Column.GetCellContent(data) as TextBlock).Text = textBoxToolDescription.Text;
                (dataGridCamcoRecordDetails.SelectedCells[4].Column.GetCellContent(data) as TextBlock).Text = textBoxSFMRPM.Text;
                (dataGridCamcoRecordDetails.SelectedCells[5].Column.GetCellContent(data) as TextBlock).Text = textBoxFeed.Text;
                (dataGridCamcoRecordDetails.SelectedCells[6].Column.GetCellContent(data) as TextBlock).Text = textBoxProjection.Text;
                (dataGridCamcoRecordDetails.SelectedCells[7].Column.GetCellContent(data) as TextBlock).Text = textBoxDrillTap.Text;
                (dataGridCamcoRecordDetails.SelectedCells[8].Column.GetCellContent(data) as TextBlock).Text = textBoxQRN1.Text;
                (dataGridCamcoRecordDetails.SelectedCells[9].Column.GetCellContent(data) as TextBlock).Text = textBoxStickBore.Text;
                (dataGridCamcoRecordDetails.SelectedCells[10].Column.GetCellContent(data) as TextBlock).Text = textBoxQRN2.Text;
                (dataGridCamcoRecordDetails.SelectedCells[11].Column.GetCellContent(data) as TextBlock).Text = textBoxTurrentHolder.Text;
                (dataGridCamcoRecordDetails.SelectedCells[12].Column.GetCellContent(data) as TextBlock).Text = textBoxQRN3.Text;
                (dataGridCamcoRecordDetails.SelectedCells[13].Column.GetCellContent(data) as TextBlock).Text = textBoxColletBlade.Text;
                (dataGridCamcoRecordDetails.SelectedCells[14].Column.GetCellContent(data) as TextBlock).Text = textBoxQRN4.Text;
                buttonAddDetailsRow.Content = "Add Line";
                
            }
            else
            {
                dataGridCamcoRecordDetails.Items.Add(new
                {
                    lineNumber = textBoxLineNumber.Text,
                    toolNumber = textBoxToolNumber.Text,
                    OffSetNumber = textBoxOffSetNumber.Text,
                    ToolDescription = textBoxToolDescription.Text,
                    SFM_RPM = textBoxSFMRPM.Text,
                    FEEDiN_REV = textBoxFeed.Text,
                    Projection = textBoxProjection.Text,
                    InsertDrillTap = textBoxDrillTap.Text,
                    StickHolderBore = textBoxStickBore.Text,
                    HolderOnTurret = textBoxTurrentHolder.Text,
                    QRN1 = textBoxQRN1.Text,
                    QRN2 = textBoxQRN2.Text,
                    QRN3 = textBoxQRN3.Text,
                    ColletBlade = textBoxColletBlade.Text,
                    QRN4 = textBoxQRN4.Text,
                });
            }
            
            if (dataGridCamcoRecordDetails.HasItems)
            {
                ClearCamcoDetailControls();
            }
            
        }
        private void ClearCamcoDetailControls()
        {
            textBoxLineNumber.Text = "";
            textBoxToolNumber.Text = "";
            textBoxOffSetNumber.Text = "";
            textBoxToolDescription.Text = "";
            textBoxSFMRPM.Text = "";
            textBoxFeed.Text = "";
            textBoxProjection.Text = "";
            textBoxDrillTap.Text = "";
            textBoxStickBore.Text = "";
            textBoxTurrentHolder.Text = "";
            textBoxQRN1.Text = "";
            textBoxQRN2.Text = "";
            textBoxQRN3.Text = "";
            textBoxColletBlade.Text = "";
            textBoxQRN4.Text = "";
            //cmbCategory1.SelectedValue = 0;
            //cmbCategory2.SelectedValue = 0;
            //cmbCategory3.SelectedValue = 0;
        }

        private void CmbMachine_KeyDown(object sender, KeyEventArgs e)
        {
            tblMachineType selectedMachine = (tblMachineType)cmbMachine.SelectedItem;
            if (selectedMachine != null)
            {
                cmbProductCategory.ItemsSource = null;
                cmbProductCategory.ItemsSource = db.tCategories.Where(p => p.ParentId == null && p.MachineId == selectedMachine.Machine_Id).ToList();
                fillCategoriesComboBoxes(selectedMachine.Machine_Id);
            }
            
        }

        private void CmbMachine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tblMachineType selectedMachine = (tblMachineType)cmbMachine.SelectedItem;
            if (selectedMachine != null)
            {
                cmbProductCategory.ItemsSource = null;
                cmbProductCategory.ItemsSource = db.tCategories.Where(p => p.ParentId == null && p.MachineId == selectedMachine.Machine_Id).ToList();
                fillCategoriesComboBoxes(selectedMachine.Machine_Id);
            }

        }
        private void fillCategoriesComboBoxes(int MachineId)
        {
            //cmbCategory3.ItemsSource = null;
            //cmbCategory3.ItemsSource = db.tCategories.Where(p => p.MachineId == MachineId && p.ParentId == null).ToList();
            //var result = db.tCategories.Where(p => p.MachineId == MachineId).ToList();
            //if (result.Count > 1)
            //{
            //    cmbCategory1.ItemsSource = null;
            //    cmbCategory1.ItemsSource = db.tCategories.Where(p => p.MachineId == MachineId).ToList();
            //    cmbCategory2.ItemsSource = null;
            //    cmbCategory2.ItemsSource = db.tCategories.Where(p => p.MachineId == MachineId).ToList();
            //    cmbCategory3.ItemsSource = null;
            //    cmbCategory3.ItemsSource = db.tCategories.Where(p => p.MachineId == MachineId).ToList();
            //}
            //else
            //{
            //    cmbCategory3.ItemsSource = null;
            //    cmbCategory3.ItemsSource = db.tCategories.Where(p => p.MachineId == MachineId).ToList();
            //}
            
        }

        private void CmbCategory1_KeyDown(object sender, KeyEventArgs e)
        {
            //tblCategory selectedCategory = (tblCategory)cmbCategory1.SelectedItem;
            //if (selectedCategory != null)
            //{
            //    View.View_Product obj = new View.View_Product(selectedCategory.Category_ID, true, 1);
            //    obj.ShowDialog();
            //}
        }

        private void CmbCategory1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //tblCategory selectedCategory = (tblCategory)cmbCategory1.SelectedItem;
            //if (selectedCategory != null)
            //{
            //    View.View_Product obj = new View.View_Product(selectedCategory.Category_ID, true, 1);
            //    obj.ShowDialog();
            //}
                
        }

        private void CmbCategory2_KeyDown(object sender, KeyEventArgs e)
        {
            //tblCategory selectedCategory = (tblCategory)cmbCategory2.SelectedItem;
            //if (selectedCategory != null)
            //{
            //    cmbCategory1.ItemsSource = null;
            //    cmbCategory1.ItemsSource = db.tCategories.Where(p => p.ParentId == selectedCategory.Category_ID).ToList();
            //    View.View_Product obj = new View.View_Product(selectedCategory.Category_ID, true, 2);
            //    obj.ShowDialog();
            //}
        }

        private void CmbCategory2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //tblCategory selectedCategory = (tblCategory)cmbCategory2.SelectedItem;
            //if (selectedCategory != null)
            //{
            //    cmbCategory1.ItemsSource = null;
            //    cmbCategory1.ItemsSource = db.tCategories.Where(p => p.ParentId == selectedCategory.Category_ID).ToList();
            //    View.View_Product obj = new View.View_Product(selectedCategory.Category_ID, true, 2);
            //    obj.ShowDialog();
            //}
                
        }

        private void CmbCategory3_KeyDown(object sender, KeyEventArgs e)
        {
            //tblCategory selectedCategory = (tblCategory)cmbCategory3.SelectedItem;
            //if (selectedCategory != null)
            //{
            //    cmbCategory2.ItemsSource = null;
            //    cmbCategory2.ItemsSource = db.tCategories.Where(p => p.ParentId == selectedCategory.Category_ID).ToList();
            //    View.View_Product obj = new View.View_Product(selectedCategory.Category_ID, true, 3);
            //    obj.ShowDialog();
            //}
                
        }

        private void CmbCategory3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //tblCategory selectedCategory = (tblCategory)cmbCategory3.SelectedItem;
            //if (selectedCategory != null)
            //{
            //    cmbCategory2.ItemsSource = null;
            //    cmbCategory2.ItemsSource = db.tCategories.Where(p => p.ParentId == selectedCategory.Category_ID).ToList();
            //    View.View_Product obj = new View.View_Product(selectedCategory.Category_ID, true, 3);
            //    obj.ShowDialog();
            //}
        }
        private void button_Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imgPhoto.Source = new BitmapImage(new Uri(op.FileName));
                imagePath = op.FileName;
                using (var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    _imageBytes = new byte[fs.Length];
                    fs.Read(_imageBytes, 0, System.Convert.ToInt32(fs.Length));
                }
            }
        }

        private void ButtonAddExistingTurretHolder_Click(object sender, RoutedEventArgs e)
        {
            tblCategory selectedTurret = (tblCategory)cmbProductCategory.SelectedItem;
            if (selectedTurret != null)
            {
                var cats = db.tCategories.Where(p => p.ParentId == selectedTurret.Category_ID).ToList();
                if (cats.Count > 0)
                {
                    View.View_SubCategory obj = new View.View_SubCategory(selectedTurret.Category_ID, 3);
                    obj.ShowDialog();
                }
                else
                {
                    View.View_Product obj = new View.View_Product(selectedTurret.Category_ID, 0, 3, 1, null);
                    obj.ShowDialog();
                }

            }
        }

        private void ButtonAddExistingStickHolder_Click(object sender, RoutedEventArgs e)
        {
            tblProduct resultDetail = ((TextBox)textBoxTurrentHolder).DataContext as tblProduct;
            //var resultDetail = db.tProducts.Where(p => p.ProductName == textBoxTurrentHolder.Text).FirstOrDefault();
            if (resultDetail != null)
            {
                if (!HelperClass.IsWindowOpen(typeof(View.View_Product)))
                {
                    View.View_Product obj = new View.View_Product(0, resultDetail.Product_ID, 2, 2, null);
                    obj.ShowDialog();
                }
                else
                {
                    Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "ViewProduct");
                    win.Close();
                    View.View_Product obj = new View.View_Product(0, resultDetail.Product_ID, 2, 2, null);
                    obj.ShowDialog();
                }
            }
            
        }

        private void ButtonAddExistingInsert_Click(object sender, RoutedEventArgs e)
        {
            if(textBoxStickBore.Text!=null && textBoxStickBore.Text != "".Trim())
            {
                tblProduct resultDetail = ((TextBox)textBoxStickBore).DataContext as tblProduct;

                //var resultDetail = db.tProducts.Where(p => p.ProductName == textBoxStickBore.Text).FirstOrDefault();
                if (resultDetail != null)
                {
                    if (!HelperClass.IsWindowOpen(typeof(View.View_Product)))
                    {
                        View.View_Product obj = new View.View_Product(0, resultDetail.Product_ID, 1, 3, null);
                        obj.ShowDialog();
                    }
                    else
                    {
                        Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "ViewProduct");
                        win.Close();
                        View.View_Product obj = new View.View_Product(0, resultDetail.Product_ID, 1, 3, null);
                        obj.ShowDialog();
                    }
                }
            }
            else
            {
                tblCategory selectedTurret = (tblCategory)cmbProductCategory.SelectedItem;
                if (selectedTurret != null)
                {
                    View.View_SubCategory obj = new View.View_SubCategory(selectedTurret.Category_ID, 1);
                    obj.ShowDialog();
                }
            }
            
        }

        private void ButtonAddStickHolder_Click(object sender, RoutedEventArgs e)
        {
            tblProduct resultDetail = ((TextBox)textBoxTurrentHolder).DataContext as tblProduct;
            if (resultDetail != null)
            {
                if (!HelperClass.IsWindowOpen(typeof(View.CreateNew_Product)))
                {
                    View.CreateNew_Product obj = new View.CreateNew_Product(0, resultDetail.Product_ID, 0, 2);
                    obj.ShowDialog();
                }
                else
                {
                    Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "CreateProduct");
                    win.Close();
                    View.CreateNew_Product obj = new View.CreateNew_Product(0, resultDetail.Product_ID, 0, 2);
                    obj.ShowDialog();
                }
            }
            else
            {
                if (!HelperClass.IsWindowOpen(typeof(View.CreateNew_Product)))
                {
                    View.CreateNew_Product obj = new View.CreateNew_Product(0, 0, 0, 2);
                    obj.ShowDialog();
                }
                else
                {
                    Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "CreateProduct");
                    win.Close();
                    View.CreateNew_Product obj = new View.CreateNew_Product(0, 0, 0, 2);
                    obj.ShowDialog();
                }
            }
            
        }

        private void ButtonAddNewInsert_Click(object sender, RoutedEventArgs e)
        {
            tblProduct resultDetail = ((TextBox)textBoxStickBore).DataContext as tblProduct;
            if (resultDetail != null)
            {
                if (!HelperClass.IsWindowOpen(typeof(View.CreateNew_Product)))
                {
                    View.CreateNew_Product obj = new View.CreateNew_Product(0, resultDetail.Product_ID, 3, 1);
                    obj.ShowDialog();
                }
                else
                {
                    Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "CreateProduct");
                    win.Close();
                    View.CreateNew_Product obj = new View.CreateNew_Product(0, resultDetail.Product_ID, 3, 1);
                    obj.ShowDialog();
                }
            }
            else
            {
                if (!HelperClass.IsWindowOpen(typeof(View.CreateNew_Product)))
                {
                    View.CreateNew_Product obj = new View.CreateNew_Product(0, 0, 3, 1);
                    obj.ShowDialog();
                }
                else
                {
                    Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "CreateProduct");
                    win.Close();
                    View.CreateNew_Product obj = new View.CreateNew_Product(0, 0, 3, 1);
                    obj.ShowDialog();
                }
            }
        }

        private void ButtonAddExistingColletBlade_Click(object sender, RoutedEventArgs e)
        {
            tblProduct resultDetail = ((TextBox)textBoxStickBore).DataContext as tblProduct;
            if (resultDetail != null)
            {
                if (!HelperClass.IsWindowOpen(typeof(View.View_Product)))
                {
                    View.View_Product obj = new View.View_Product(0, resultDetail.Product_ID, 4, 4, null);
                    obj.ShowDialog();
                }
                else
                {
                    Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "ViewProduct");
                    win.Close();
                    View.View_Product obj = new View.View_Product(0, resultDetail.Product_ID, 4, 4, null);
                    obj.ShowDialog();
                }
            }
        }

        private void ButtonAddColletBlade_Click(object sender, RoutedEventArgs e)
        {
            tblProduct resultDetail = ((TextBox)textBoxStickBore).DataContext as tblProduct;
            if (resultDetail != null)
            {
                if (!HelperClass.IsWindowOpen(typeof(View.CreateNew_Product)))
                {
                    View.CreateNew_Product obj = new View.CreateNew_Product(0, resultDetail.Product_ID, 4, 4);
                    obj.ShowDialog();
                }
                else
                {
                    Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "CreateProduct");
                    win.Close();
                    View.CreateNew_Product obj = new View.CreateNew_Product(0, resultDetail.Product_ID, 4, 4);
                    obj.ShowDialog();
                }
            }
            else
            {
                if (!HelperClass.IsWindowOpen(typeof(View.CreateNew_Product)))
                {
                    View.CreateNew_Product obj = new View.CreateNew_Product(0, 0, 4, 4);
                    obj.ShowDialog();
                }
                else
                {
                    Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "CreateProduct");
                    win.Close();
                    View.CreateNew_Product obj = new View.CreateNew_Product(0, 0, 4, 4);
                    obj.ShowDialog();
                }
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            ClearHeaderControls();
            ClearRowControls();
            dataGridCamcoRecordDetails.ItemsSource = null;
            dataGridCamcoRecordDetails.Items.Clear();
            FillControls();
            isValidte = true;
        }
        private void ClearHeaderControls()
        {
            cmbPart.ItemsSource = null;
            txtCNCProgram.Text = "";
            cmbChuckCollet.ItemsSource = null;
            textBoxColletJawsQRN.Text = "";
            cmbMachine.ItemsSource = null;
            cmbProductCategory.ItemsSource = null;
            cmbEmployee.ItemsSource = null;
            txtCycletime.Text = "";
            textBoxChuckPressure.Text = "";
            MaterialTxt.Text = "";
            cmbCustomer.ItemsSource = null;
            cmbOperation.ItemsSource = null;
        }
        private void ClearRowControls()
        {
            cmbPart.ItemsSource = null;
            textBoxLineNumber.Text = "";
            textBoxToolNumber.Text = "";
            textBoxOffSetNumber.Text = "";
            textBoxToolDescription.Text = "";
            textBoxSFMRPM.Text = "";
            textBoxFeed.Text = "";
            textBoxProjection.Text = "";
            textBoxFeed.Text = "";
            textBoxDrillTap.Text = "";
            textBoxQRN1.Text = "";
            textBoxColletBlade.Text = "";
            textBoxQRN4.Text = "";
            textBoxStickBore.Text = "";
            textBoxQRN2.Text = "";
            textBoxQRN3.Text = "";
            textBoxTurrentHolder.Text = "";
            
        }

        private void DataGridCamcoRecordDetails_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //DataGridRow row = sender as DataGridRow;
            //if (dataGridCamcoRecordDetails.SelectedItem == null) return;
            //var selectedPerson = dataGridCamcoRecordDetails.SelectedItem as ComcoRecordDetail;
            var data = dataGridCamcoRecordDetails.SelectedItem;
            textBoxLineNumber.Text = (dataGridCamcoRecordDetails.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
            textBoxToolNumber.Text = (dataGridCamcoRecordDetails.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text;
            textBoxOffSetNumber.Text = (dataGridCamcoRecordDetails.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text;
            textBoxToolDescription.Text = (dataGridCamcoRecordDetails.SelectedCells[3].Column.GetCellContent(data) as TextBlock).Text;
            textBoxSFMRPM.Text = (dataGridCamcoRecordDetails.SelectedCells[4].Column.GetCellContent(data) as TextBlock).Text;
            textBoxFeed.Text = (dataGridCamcoRecordDetails.SelectedCells[5].Column.GetCellContent(data) as TextBlock).Text;
            textBoxProjection.Text = (dataGridCamcoRecordDetails.SelectedCells[6].Column.GetCellContent(data) as TextBlock).Text;
            textBoxDrillTap.Text = (dataGridCamcoRecordDetails.SelectedCells[7].Column.GetCellContent(data) as TextBlock).Text;
            textBoxQRN1.Text = (dataGridCamcoRecordDetails.SelectedCells[8].Column.GetCellContent(data) as TextBlock).Text;
            textBoxStickBore.Text = (dataGridCamcoRecordDetails.SelectedCells[9].Column.GetCellContent(data) as TextBlock).Text;
            textBoxQRN2.Text = (dataGridCamcoRecordDetails.SelectedCells[10].Column.GetCellContent(data) as TextBlock).Text;
            textBoxTurrentHolder.Text = (dataGridCamcoRecordDetails.SelectedCells[11].Column.GetCellContent(data) as TextBlock).Text;
            textBoxQRN3.Text = (dataGridCamcoRecordDetails.SelectedCells[12].Column.GetCellContent(data) as TextBlock).Text;
            textBoxColletBlade.Text = (dataGridCamcoRecordDetails.SelectedCells[13].Column.GetCellContent(data) as TextBlock).Text;
            textBoxQRN4.Text = (dataGridCamcoRecordDetails.SelectedCells[14].Column.GetCellContent(data) as TextBlock).Text;
            buttonAddDetailsRow.Content = "Update Line";
        }

        private void DeleteDetailRow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = dataGridCamcoRecordDetails.SelectedItem;
                if (selectedItem != null)
                {
                    dataGridCamcoRecordDetails.Items.Remove(selectedItem);
                }
                //tEmployeeJob dataRowView = (tEmployeeJob)((Button)e.Source).DataContext;
                ////String employeeName = dataRowView[0].ToString();
                ////var employee = db.tEmployees.Where(p => p.FullName == employeeName).FirstOrDefault();
                ////String departmentName = dataRowView[1].ToString();
                ////var department = db.tDepartments.Where(p => p.DepartmentName == departmentName).FirstOrDefault();
                ////String StartDate = dataRowView[2].ToString();
                ////String EndDate = dataRowView[3].ToString();
                //cmbEmployee.SelectedValue = dataRowView.EmployeeId;
                //cmbEmployeeDepartment.SelectedValue = dataRowView.DepartmentId;
                //dateTimePickerStartDateTime.Value = dataRowView.StartDateTime;
                //dateTimePickerEndDateTime.Value = dataRowView.EndDateTime;
                //TextBoxJobId.Text = dataRowView.Job_Id.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void EditDetailRow_Click(object sender, RoutedEventArgs e)
        {
            var data = dataGridCamcoRecordDetails.SelectedItem;
            if ((dataGridCamcoRecordDetails.SelectedCells[0].Column.GetCellContent(data) as TextBlock) != null)
            {
                textBoxLineNumber.Text = (dataGridCamcoRecordDetails.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
            }
            if ((dataGridCamcoRecordDetails.SelectedCells[1].Column.GetCellContent(data) as TextBlock) != null)
            {
                textBoxToolNumber.Text = (dataGridCamcoRecordDetails.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text;
            }
            if ((dataGridCamcoRecordDetails.SelectedCells[2].Column.GetCellContent(data) as TextBlock) != null)
            {
                textBoxOffSetNumber.Text = (dataGridCamcoRecordDetails.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text;
            }
            if ((dataGridCamcoRecordDetails.SelectedCells[3].Column.GetCellContent(data) as TextBlock) != null)
            {
                textBoxToolDescription.Text = (dataGridCamcoRecordDetails.SelectedCells[3].Column.GetCellContent(data) as TextBlock).Text;
            }
            if ((dataGridCamcoRecordDetails.SelectedCells[4].Column.GetCellContent(data) as TextBlock) != null)
            {
                textBoxSFMRPM.Text = (dataGridCamcoRecordDetails.SelectedCells[4].Column.GetCellContent(data) as TextBlock).Text;
            }
            if ((dataGridCamcoRecordDetails.SelectedCells[5].Column.GetCellContent(data) as TextBlock) != null)
            {
                textBoxFeed.Text = (dataGridCamcoRecordDetails.SelectedCells[5].Column.GetCellContent(data) as TextBlock).Text;
            }
            if ((dataGridCamcoRecordDetails.SelectedCells[6].Column.GetCellContent(data) as TextBlock) != null)
            {
                textBoxProjection.Text = (dataGridCamcoRecordDetails.SelectedCells[6].Column.GetCellContent(data) as TextBlock).Text;
            }
            if ((dataGridCamcoRecordDetails.SelectedCells[7].Column.GetCellContent(data) as TextBlock) != null)
            {
                textBoxDrillTap.Text = (dataGridCamcoRecordDetails.SelectedCells[7].Column.GetCellContent(data) as TextBlock).Text;
            }
            if ((dataGridCamcoRecordDetails.SelectedCells[8].Column.GetCellContent(data) as TextBlock) != null)
            {
                textBoxQRN1.Text = (dataGridCamcoRecordDetails.SelectedCells[8].Column.GetCellContent(data) as TextBlock).Text;
            }
            if ((dataGridCamcoRecordDetails.SelectedCells[9].Column.GetCellContent(data) as TextBlock) != null)
            {
                textBoxStickBore.Text = (dataGridCamcoRecordDetails.SelectedCells[9].Column.GetCellContent(data) as TextBlock).Text;
            }
            if ((dataGridCamcoRecordDetails.SelectedCells[10].Column.GetCellContent(data) as TextBlock) != null)
            {
                textBoxQRN2.Text = (dataGridCamcoRecordDetails.SelectedCells[10].Column.GetCellContent(data) as TextBlock).Text;
            }
            if ((dataGridCamcoRecordDetails.SelectedCells[11].Column.GetCellContent(data) as TextBlock) != null)
            {
                textBoxTurrentHolder.Text = (dataGridCamcoRecordDetails.SelectedCells[11].Column.GetCellContent(data) as TextBlock).Text;
            }
            if ((dataGridCamcoRecordDetails.SelectedCells[12].Column.GetCellContent(data) as TextBlock) != null)
            {
                textBoxQRN3.Text = (dataGridCamcoRecordDetails.SelectedCells[12].Column.GetCellContent(data) as TextBlock).Text;
            }
            if ((dataGridCamcoRecordDetails.SelectedCells[13].Column.GetCellContent(data) as TextBlock) != null)
            {
                textBoxColletBlade.Text = (dataGridCamcoRecordDetails.SelectedCells[13].Column.GetCellContent(data) as TextBlock).Text;
            }
            if ((dataGridCamcoRecordDetails.SelectedCells[14].Column.GetCellContent(data) as TextBlock) != null)
            {
                textBoxQRN4.Text = (dataGridCamcoRecordDetails.SelectedCells[14].Column.GetCellContent(data) as TextBlock).Text;
            }
            buttonAddDetailsRow.Content = "Update Line";
        }
    }
}

using Microsoft.Reporting.WinForms;
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

namespace CamcoManufacturing
{
    /// <summary>
    /// Interaction logic for CamcoRecordReportView.xaml
    /// </summary>
    public partial class CamcoRecordReportView : Window
    {
        public CamcoRecordReportView(int invoiceID)
        {
            InitializeComponent();
            CamcoId = invoiceID;
            _reportViewer.Load += ReportViewer_Load;


        }
        protected override void OnKeyDown(KeyEventArgs e)
        {

            {
                try
                {

                    base.OnKeyDown(e);
                    if ((e.Key == Key.P) && (Keyboard.IsKeyDown(Key.LeftCtrl) ||
                          Keyboard.IsKeyDown(Key.RightCtrl)))

                        _reportViewer.PrintDialog();

                    //PrintDialog printDialog = new PrintDialog();

                    //_reportViewer.Clear();
                    //  _reportViewer.LocalReport.ReleaseSandboxAppDomain();
                    // this.Close();

                    // Report r = this._reportViewer.LocalReport;

                    //    Run();
                    //  this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("please wait for report to load ");
                }
            }
        }

        public bool _isReportViewerLoaded { get; set; }
        public int CamcoId { get; set; }
        private void ReportViewer_Load(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show("insdie report viewer");
                if (!_isReportViewerLoaded)
                {
                    Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
                    //  Reports dataset = new Reports();
                    Reports datasets = new Reports();
                    datasets.BeginInit();

                    reportDataSource1.Name = "DataSet1"; //Name of the report dataset in our .RDLC file
                    reportDataSource1.Value = datasets.CamcoSetUpSheetTable;
                    this._reportViewer.LocalReport.DataSources.Add(reportDataSource1);
                    this._reportViewer.LocalReport.ReportPath = @"Reports\CamcoRecordReport.rdlc";

                    datasets.EndInit();

                    //fill data into ProductListing
                    ReportsTableAdapters.CamcoSetUpSheetTableTableAdapter saleInvoiceTableAdapter = new ReportsTableAdapters.CamcoSetUpSheetTableTableAdapter();
                    saleInvoiceTableAdapter.ClearBeforeFill = true;
                    saleInvoiceTableAdapter.Fill(datasets.CamcoSetUpSheetTable, CamcoId);
                    _reportViewer.RefreshReport();
                    _isReportViewerLoaded = true;
                    _reportViewer.RefreshReport();
                    _isReportViewerLoaded = true;
                    //  _reportViewer.PrintDialog();
                }
            }
            catch (Exception ex)
            {
                var abc = ex.Message;
                MessageBox.Show(ex.Message);
            }
        }

        private void _reportViewer_RenderingComplete(object sender, RenderingCompleteEventArgs e)
        {
            //_reportViewer.PrintDialog();
            //PrintDialog printDialog = new PrintDialog();

            //_reportViewer.Clear();
            //_reportViewer.LocalReport.ReleaseSandboxAppDomain();
            //this.Close();

            Report r = this._reportViewer.LocalReport;

            //Run();
            //this.Close();
        }

        private void WindowsFormsHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }
    }
}

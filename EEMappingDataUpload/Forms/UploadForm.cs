using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EEMappingDataUpload.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace EEMappingDataUpload
{
    public partial class UploadForm : Form
    {
        public UploadForm()
        {
            InitializeComponent();
            backgroundRegionMapUpload.WorkerReportsProgress = true;
            backgroundRegionMapUpload.WorkerSupportsCancellation = true;
            backgroundIrradianceUpload.WorkerReportsProgress = true;
            backgroundIrradianceUpload.WorkerSupportsCancellation = true;
        }

        // Postcode Region Upload 
        private void Browse_RegionMap_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog RegionMapFileSelector = new OpenFileDialog())
            {
                RegionMapFileSelector.Title = "";
                RegionMapFileSelector.Multiselect = false;
                RegionMapFileSelector.Filter = "Excel Files|*.xls;*.xlsx";
                if (RegionMapFileSelector.ShowDialog() == DialogResult.OK)
                {
                    RegionMap_File_Input.Text = RegionMapFileSelector.FileName;
                    RegionMap_File_Input.Enabled = false;
                }
            }
           
           
        }

        private void Upload_RegionMap_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(RegionMap_File_Input.Text))
            {
                MessageBox.Show("Please selece the Region Postcode Mapping excel.");
                return;
            }

            if (!backgroundRegionMapUpload.IsBusy)
            {
                LoadingPicture.Visible = true;
                progressLabel.Text = "connecting to excel...";
                progressLabel.Visible = true;
                backgroundRegionMapUpload.RunWorkerAsync();
            }
           

        }

        private void backgroundRegionMapUpload_DoWork(object sender, DoWorkEventArgs e)
        {

            List<RegionMap> RegionMappings = new List<RegionMap>();
            Excel.Application OExcel = new Excel.Application();
            Excel.Workbook OWorkbook = OExcel.Workbooks.Open(RegionMap_File_Input.Text);
            try
            {
                Excel.Worksheet OWorksheet = OWorkbook.Worksheets.Item[1];

                int rows = OWorksheet.UsedRange.Rows.Count;

                backgroundRegionMapUpload.ReportProgress(0, "Fetching data from excel...");

                for (int i = 2; i <= rows; i++)
                {
                    RegionMap map = new RegionMap();
                    map.Postcode = OWorksheet.UsedRange.Cells[i, 1].Value.ToString();
                    map.Region = OWorksheet.UsedRange.Cells[i, 2].Value.ToString();

                    RegionMappings.Add(map);
                }

                OWorkbook.Close();
                OExcel.Quit();

                backgroundRegionMapUpload.ReportProgress(50, "Uploading into database...");

                try
                {
                    using (var _context = new EEDbContext())
                    {
                        _context.RegionMappings.AddRange(RegionMappings);
                        _context.SaveChanges();
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Exception while uploading to database: " + exception.Message);
                    if (backgroundRegionMapUpload.WorkerSupportsCancellation == true)
                    {
                        e.Cancel = true;
                        backgroundRegionMapUpload.CancelAsync();
                    }
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show("Exception while fetching data from excel: " + exception.Message);
                OWorkbook.Close();
                OExcel.Quit();
                if (backgroundRegionMapUpload.WorkerSupportsCancellation == true)
                {
                    e.Cancel = true;
                    backgroundRegionMapUpload.CancelAsync();
                }
            }



        }

        private void backgroundRegionMapUpload_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressLabel.Text = e.UserState.ToString();
        }

        private void backgroundRegionMapUpload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressLabel.Visible = false;
            LoadingPicture.Visible = false;
            if (!e.Cancelled)
            {
                MessageBox.Show("Data uploaded successfully");
            }
            
        }

        //
        // Irradinace Upload 
        //
        //
        private void Browse_Irradiance_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog IrradianceMapFileSelector = new OpenFileDialog())
            {
                IrradianceMapFileSelector.Title = "";
                IrradianceMapFileSelector.Multiselect = false;
                IrradianceMapFileSelector.Filter = "Excel Files|*.xls;*.xlsx";
                if (IrradianceMapFileSelector.ShowDialog() == DialogResult.OK)
                {
                    Irradiance_File_Input.Text = IrradianceMapFileSelector.FileName;
                    Irradiance_File_Input.Enabled = false;
                }
            }
        }

        private void Upload_Irradiance_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(Irradiance_File_Input.Text))
            {
                MessageBox.Show("Please selece the Region Postcode Mapping excel.");
                return;
            }

            if (!backgroundIrradianceUpload.IsBusy)
            {
                LoadingPicture.Visible = true;
                progressLabel.Text = "connecting to excel...";
                progressLabel.Visible = true;
                backgroundIrradianceUpload.RunWorkerAsync();
            }
        }

        private void backgroundIrradianceUpload_DoWork(object sender, DoWorkEventArgs e)
        {
            Excel.Application OExcel = new Excel.Application();
            Excel.Workbook OWorkbook = OExcel.Workbooks.Open(Irradiance_File_Input.Text);
            try
            {
                int SheetsCount = OWorkbook.Worksheets.Count;

                for (int sheet = 1; sheet <= SheetsCount; sheet++)
                {
                    List<Irradiance> IrradianceDatasets = new List<Irradiance>();
                    Excel.Worksheet OWorksheet = OWorkbook.Worksheets.Item[sheet];
                    
                    // Get Zone and Zone Name
                    string sheetName = OWorksheet.Name;
                    string[] names = sheetName.Split("-");
                    string Zone = names[0].Replace("Zone", "").Trim();
                    string ZoneName = names[1].Trim();

                    int rows = OWorksheet.UsedRange.Rows.Count;
                    int columns = OWorksheet.UsedRange.Columns.Count;

                    backgroundIrradianceUpload.ReportProgress(0, "fetching data from sheet " + sheetName + "...");

                    for (int i = 3; i <= rows; i++)
                    {
                        for (int j = 3; j <= columns; j++)
                        {
                            Irradiance irradianceData = new Irradiance();
                            irradianceData.Zone = Zone;
                            irradianceData.ZoneName = ZoneName;
                            irradianceData.Inclination = Convert.ToInt32(OWorksheet.UsedRange.Cells[i, 2].Value);
                            irradianceData.Orientation = Convert.ToInt32(OWorksheet.UsedRange.Cells[2, j].Value);
                            irradianceData.AnnualGenValue = Convert.ToInt32(OWorksheet.UsedRange.Cells[i, j].Value);
                            IrradianceDatasets.Add(irradianceData);
                        }
                        
                    }

                    backgroundIrradianceUpload.ReportProgress(50, "uploading " + sheetName + "data into database...");

                    try
                    {
                        using (var _context = new EEDbContext())
                        {
                            _context.IrradianceDatasets.AddRange(IrradianceDatasets);
                            _context.SaveChanges();
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Exception while uploading " + sheetName + " to database: " + exception.Message);
                    }
                }

                OWorkbook.Close();
                OExcel.Quit();

            }
            catch (Exception exception)
            {
                MessageBox.Show("Exception while fetching data from excel: " + exception.Message);
                OWorkbook.Close();
                OExcel.Quit();
                if (backgroundIrradianceUpload.WorkerSupportsCancellation == true)
                {
                    e.Cancel = true;
                    backgroundIrradianceUpload.CancelAsync();
                }
            }

        }

        private void backgroundIrradianceUpload_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressLabel.Text = e.UserState.ToString();
        }

        private void backgroundIrradianceUpload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressLabel.Visible = false;
            LoadingPicture.Visible = false;
            if (!e.Cancelled)
            {
                MessageBox.Show("Data uploaded successfully");
            }
            
        }
    }
}

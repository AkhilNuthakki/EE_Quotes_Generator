
namespace EEMappingDataUpload
{
    partial class UploadForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UploadForm));
            this.Browse_RegionMap = new System.Windows.Forms.Button();
            this.Browse_Irradiance = new System.Windows.Forms.Button();
            this.Upload_RegionMap = new System.Windows.Forms.Button();
            this.Upload_Irradiance = new System.Windows.Forms.Button();
            this.RegionMap_File_Input = new System.Windows.Forms.TextBox();
            this.Irradiance_File_Input = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.headerImage = new System.Windows.Forms.PictureBox();
            this.Header = new System.Windows.Forms.Label();
            this.LoadingPicture = new System.Windows.Forms.PictureBox();
            this.progressLabel = new System.Windows.Forms.Label();
            this.backgroundRegionMapUpload = new System.ComponentModel.BackgroundWorker();
            this.backgroundIrradianceUpload = new System.ComponentModel.BackgroundWorker();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headerImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // Browse_RegionMap
            // 
            this.Browse_RegionMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(187)))), ((int)(((byte)(34)))));
            this.Browse_RegionMap.ForeColor = System.Drawing.Color.White;
            this.Browse_RegionMap.Location = new System.Drawing.Point(601, 133);
            this.Browse_RegionMap.Name = "Browse_RegionMap";
            this.Browse_RegionMap.Size = new System.Drawing.Size(120, 40);
            this.Browse_RegionMap.TabIndex = 0;
            this.Browse_RegionMap.Text = "Browse";
            this.Browse_RegionMap.UseVisualStyleBackColor = false;
            this.Browse_RegionMap.Click += new System.EventHandler(this.Browse_RegionMap_Click);
            // 
            // Browse_Irradiance
            // 
            this.Browse_Irradiance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(187)))), ((int)(((byte)(34)))));
            this.Browse_Irradiance.ForeColor = System.Drawing.Color.White;
            this.Browse_Irradiance.Location = new System.Drawing.Point(601, 249);
            this.Browse_Irradiance.Name = "Browse_Irradiance";
            this.Browse_Irradiance.Size = new System.Drawing.Size(120, 40);
            this.Browse_Irradiance.TabIndex = 1;
            this.Browse_Irradiance.Text = "Browse";
            this.Browse_Irradiance.UseVisualStyleBackColor = false;
            this.Browse_Irradiance.Click += new System.EventHandler(this.Browse_Irradiance_Click);
            // 
            // Upload_RegionMap
            // 
            this.Upload_RegionMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(187)))), ((int)(((byte)(34)))));
            this.Upload_RegionMap.ForeColor = System.Drawing.Color.White;
            this.Upload_RegionMap.Location = new System.Drawing.Point(761, 133);
            this.Upload_RegionMap.Name = "Upload_RegionMap";
            this.Upload_RegionMap.Size = new System.Drawing.Size(120, 40);
            this.Upload_RegionMap.TabIndex = 2;
            this.Upload_RegionMap.Text = "Upload";
            this.Upload_RegionMap.UseVisualStyleBackColor = false;
            this.Upload_RegionMap.Click += new System.EventHandler(this.Upload_RegionMap_Click);
            // 
            // Upload_Irradiance
            // 
            this.Upload_Irradiance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(187)))), ((int)(((byte)(34)))));
            this.Upload_Irradiance.ForeColor = System.Drawing.Color.White;
            this.Upload_Irradiance.Location = new System.Drawing.Point(761, 249);
            this.Upload_Irradiance.Name = "Upload_Irradiance";
            this.Upload_Irradiance.Size = new System.Drawing.Size(120, 40);
            this.Upload_Irradiance.TabIndex = 3;
            this.Upload_Irradiance.Text = "Upload";
            this.Upload_Irradiance.UseVisualStyleBackColor = false;
            this.Upload_Irradiance.Click += new System.EventHandler(this.Upload_Irradiance_Click);
            // 
            // RegionMap_File_Input
            // 
            this.RegionMap_File_Input.Location = new System.Drawing.Point(54, 133);
            this.RegionMap_File_Input.Name = "RegionMap_File_Input";
            this.RegionMap_File_Input.PlaceholderText = "Browse and upload Postcode Region Mapping Excel";
            this.RegionMap_File_Input.Size = new System.Drawing.Size(500, 27);
            this.RegionMap_File_Input.TabIndex = 4;
            // 
            // Irradiance_File_Input
            // 
            this.Irradiance_File_Input.Location = new System.Drawing.Point(54, 256);
            this.Irradiance_File_Input.Name = "Irradiance_File_Input";
            this.Irradiance_File_Input.PlaceholderText = "Browse and upload Irradiance Datasets Excel";
            this.Irradiance_File_Input.Size = new System.Drawing.Size(500, 27);
            this.Irradiance_File_Input.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.headerImage);
            this.panel1.Controls.Add(this.Header);
            this.panel1.Location = new System.Drawing.Point(2, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(941, 71);
            this.panel1.TabIndex = 6;
            // 
            // headerImage
            // 
            this.headerImage.Image = global::EEMappingDataUpload.Properties.Resources.ee_logo;
            this.headerImage.Location = new System.Drawing.Point(4, 15);
            this.headerImage.Name = "headerImage";
            this.headerImage.Size = new System.Drawing.Size(413, 44);
            this.headerImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.headerImage.TabIndex = 1;
            this.headerImage.TabStop = false;
            // 
            // Header
            // 
            this.Header.AutoSize = true;
            this.Header.Font = new System.Drawing.Font("Segoe UI Semibold", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Header.Location = new System.Drawing.Point(421, 17);
            this.Header.Name = "Header";
            this.Header.Size = new System.Drawing.Size(454, 38);
            this.Header.TabIndex = 0;
            this.Header.Text = "Mapping Data Upload Application";
            // 
            // LoadingPicture
            // 
            this.LoadingPicture.Image = global::EEMappingDataUpload.Properties.Resources.Loading;
            this.LoadingPicture.Location = new System.Drawing.Point(54, 334);
            this.LoadingPicture.Name = "LoadingPicture";
            this.LoadingPicture.Size = new System.Drawing.Size(49, 49);
            this.LoadingPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.LoadingPicture.TabIndex = 7;
            this.LoadingPicture.TabStop = false;
            this.LoadingPicture.Visible = false;
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = true;
            this.progressLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.progressLabel.Location = new System.Drawing.Point(104, 354);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(84, 20);
            this.progressLabel.TabIndex = 8;
            this.progressLabel.Text = "progress....";
            this.progressLabel.Visible = false;
            // 
            // backgroundRegionMapUpload
            // 
            this.backgroundRegionMapUpload.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundRegionMapUpload_DoWork);
            this.backgroundRegionMapUpload.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundRegionMapUpload_ProgressChanged);
            this.backgroundRegionMapUpload.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundRegionMapUpload_RunWorkerCompleted);
            // 
            // backgroundIrradianceUpload
            // 
            this.backgroundIrradianceUpload.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundIrradianceUpload_DoWork);
            this.backgroundIrradianceUpload.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundIrradianceUpload_ProgressChanged);
            this.backgroundIrradianceUpload.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundIrradianceUpload_RunWorkerCompleted);
            // 
            // UploadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(944, 438);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.LoadingPicture);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Irradiance_File_Input);
            this.Controls.Add(this.RegionMap_File_Input);
            this.Controls.Add(this.Upload_Irradiance);
            this.Controls.Add(this.Upload_RegionMap);
            this.Controls.Add(this.Browse_Irradiance);
            this.Controls.Add(this.Browse_RegionMap);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UploadForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EE Data Upload Form";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headerImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Browse_RegionMap;
        private System.Windows.Forms.Button Browse_Irradiance;
        private System.Windows.Forms.Button Upload_RegionMap;
        private System.Windows.Forms.Button Upload_Irradiance;
        private System.Windows.Forms.TextBox RegionMap_File_Input;
        private System.Windows.Forms.TextBox Irradiance_File_Input;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label Header;
        private System.Windows.Forms.PictureBox LoadingPicture;
        private System.Windows.Forms.Label progressLabel;
        private System.ComponentModel.BackgroundWorker backgroundRegionMapUpload;
        private System.Windows.Forms.PictureBox headerImage;
        private System.ComponentModel.BackgroundWorker backgroundIrradianceUpload;
    }
}


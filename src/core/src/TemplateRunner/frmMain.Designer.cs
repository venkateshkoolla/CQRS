namespace TemplateRunner
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbTemplate = new System.Windows.Forms.ComboBox();
            this.lblTemplate = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.grpLocale = new System.Windows.Forms.GroupBox();
            this.radFrCa = new System.Windows.Forms.RadioButton();
            this.radEnCa = new System.Windows.Forms.RadioButton();
            this.grpVersion = new System.Windows.Forms.GroupBox();
            this.radX12 = new System.Windows.Forms.RadioButton();
            this.radPesc = new System.Windows.Forms.RadioButton();
            this.grpTestType = new System.Windows.Forms.GroupBox();
            this.cmbTestType = new System.Windows.Forms.ComboBox();
            this.grpLocale.SuspendLayout();
            this.grpVersion.SuspendLayout();
            this.grpTestType.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbTemplate
            // 
            this.cmbTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTemplate.FormattingEnabled = true;
            this.cmbTemplate.Items.AddRange(new object[] {
            "Post-Secondary Transcript",
            "High School Grades",
            "Standardized Test"});
            this.cmbTemplate.Location = new System.Drawing.Point(12, 49);
            this.cmbTemplate.Name = "cmbTemplate";
            this.cmbTemplate.Size = new System.Drawing.Size(346, 21);
            this.cmbTemplate.TabIndex = 0;
            this.cmbTemplate.SelectedIndexChanged += new System.EventHandler(this.CmbTemplate_SelectedIndexChanged);
            // 
            // lblTemplate
            // 
            this.lblTemplate.AutoSize = true;
            this.lblTemplate.Location = new System.Drawing.Point(13, 30);
            this.lblTemplate.Name = "lblTemplate";
            this.lblTemplate.Size = new System.Drawing.Size(83, 13);
            this.lblTemplate.TabIndex = 1;
            this.lblTemplate.Text = "Document Type";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(12, 265);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 2;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // grpLocale
            // 
            this.grpLocale.Controls.Add(this.radFrCa);
            this.grpLocale.Controls.Add(this.radEnCa);
            this.grpLocale.Location = new System.Drawing.Point(12, 76);
            this.grpLocale.Name = "grpLocale";
            this.grpLocale.Size = new System.Drawing.Size(346, 53);
            this.grpLocale.TabIndex = 4;
            this.grpLocale.TabStop = false;
            this.grpLocale.Text = "Locale";
            // 
            // radFrCa
            // 
            this.radFrCa.AutoSize = true;
            this.radFrCa.Location = new System.Drawing.Point(67, 19);
            this.radFrCa.Name = "radFrCa";
            this.radFrCa.Size = new System.Drawing.Size(48, 17);
            this.radFrCa.TabIndex = 1;
            this.radFrCa.Text = "fr-CA";
            this.radFrCa.UseVisualStyleBackColor = true;
            // 
            // radEnCa
            // 
            this.radEnCa.AutoSize = true;
            this.radEnCa.Checked = true;
            this.radEnCa.Location = new System.Drawing.Point(7, 20);
            this.radEnCa.Name = "radEnCa";
            this.radEnCa.Size = new System.Drawing.Size(54, 17);
            this.radEnCa.TabIndex = 0;
            this.radEnCa.TabStop = true;
            this.radEnCa.Text = "en-CA";
            this.radEnCa.UseVisualStyleBackColor = true;
            // 
            // grpVersion
            // 
            this.grpVersion.Controls.Add(this.radX12);
            this.grpVersion.Controls.Add(this.radPesc);
            this.grpVersion.Location = new System.Drawing.Point(12, 135);
            this.grpVersion.Margin = new System.Windows.Forms.Padding(2);
            this.grpVersion.Name = "grpVersion";
            this.grpVersion.Padding = new System.Windows.Forms.Padding(2);
            this.grpVersion.Size = new System.Drawing.Size(345, 53);
            this.grpVersion.TabIndex = 5;
            this.grpVersion.TabStop = false;
            this.grpVersion.Text = "Schema Version";
            // 
            // radX12
            // 
            this.radX12.AutoSize = true;
            this.radX12.Checked = true;
            this.radX12.Location = new System.Drawing.Point(67, 16);
            this.radX12.Margin = new System.Windows.Forms.Padding(2);
            this.radX12.Name = "radX12";
            this.radX12.Size = new System.Drawing.Size(44, 17);
            this.radX12.TabIndex = 1;
            this.radX12.TabStop = true;
            this.radX12.Text = "X12";
            this.radX12.UseVisualStyleBackColor = true;
            // 
            // radPesc
            // 
            this.radPesc.AutoSize = true;
            this.radPesc.Location = new System.Drawing.Point(7, 16);
            this.radPesc.Margin = new System.Windows.Forms.Padding(2);
            this.radPesc.Name = "radPesc";
            this.radPesc.Size = new System.Drawing.Size(53, 17);
            this.radPesc.TabIndex = 0;
            this.radPesc.Text = "PESC";
            this.radPesc.UseVisualStyleBackColor = true;
            // 
            // grpTestType
            // 
            this.grpTestType.Controls.Add(this.cmbTestType);
            this.grpTestType.Location = new System.Drawing.Point(12, 194);
            this.grpTestType.Name = "grpTestType";
            this.grpTestType.Size = new System.Drawing.Size(346, 55);
            this.grpTestType.TabIndex = 6;
            this.grpTestType.TabStop = false;
            this.grpTestType.Text = "Test Type";
            // 
            // cmbTestType
            // 
            this.cmbTestType.FormattingEnabled = true;
            this.cmbTestType.Items.AddRange(new object[] {
            "CAC",
            "GED",
            "HOA",
            "IBT",
            "TFL"});
            this.cmbTestType.Location = new System.Drawing.Point(6, 19);
            this.cmbTestType.Name = "cmbTestType";
            this.cmbTestType.Size = new System.Drawing.Size(121, 21);
            this.cmbTestType.TabIndex = 0;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 300);
            this.Controls.Add(this.grpTestType);
            this.Controls.Add(this.grpVersion);
            this.Controls.Add(this.grpLocale);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.lblTemplate);
            this.Controls.Add(this.cmbTemplate);
            this.Name = "frmMain";
            this.Text = "Supporting Doc Template Runner";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.grpLocale.ResumeLayout(false);
            this.grpLocale.PerformLayout();
            this.grpVersion.ResumeLayout(false);
            this.grpVersion.PerformLayout();
            this.grpTestType.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbTemplate;
        private System.Windows.Forms.Label lblTemplate;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.GroupBox grpLocale;
        private System.Windows.Forms.RadioButton radFrCa;
        private System.Windows.Forms.RadioButton radEnCa;
        private System.Windows.Forms.GroupBox grpVersion;
        private System.Windows.Forms.RadioButton radX12;
        private System.Windows.Forms.RadioButton radPesc;
        private System.Windows.Forms.GroupBox grpTestType;
        private System.Windows.Forms.ComboBox cmbTestType;
    }
}


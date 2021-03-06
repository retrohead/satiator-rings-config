namespace SatiatorRingsConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.txtDir = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtImageID = new System.Windows.Forms.TextBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnBuild = new System.Windows.Forms.Button();
            this.lstDir = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRemoveImage = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnExistingImage = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lblMenuVer = new System.Windows.Forms.Label();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applicationUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDir
            // 
            this.txtDir.Location = new System.Drawing.Point(12, 29);
            this.txtDir.Name = "txtDir";
            this.txtDir.ReadOnly = true;
            this.txtDir.Size = new System.Drawing.Size(507, 20);
            this.txtDir.TabIndex = 0;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowse.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.Location = new System.Drawing.Point(526, 28);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(100, 23);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Open Folder";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            // 
            // txtImageID
            // 
            this.txtImageID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtImageID.Location = new System.Drawing.Point(526, 57);
            this.txtImageID.Name = "txtImageID";
            this.txtImageID.ReadOnly = true;
            this.txtImageID.Size = new System.Drawing.Size(100, 20);
            this.txtImageID.TabIndex = 3;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApply.Enabled = false;
            this.btnApply.Location = new System.Drawing.Point(526, 199);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(100, 29);
            this.btnApply.TabIndex = 7;
            this.btnApply.Text = "New Image";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // btnBuild
            // 
            this.btnBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuild.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuild.Enabled = false;
            this.btnBuild.Location = new System.Drawing.Point(526, 513);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(100, 63);
            this.btnBuild.TabIndex = 8;
            this.btnBuild.Text = "Build ISO";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.BtnBuild_Click);
            // 
            // lstDir
            // 
            this.lstDir.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lstDir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lstDir.FullRowSelect = true;
            this.lstDir.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstDir.HideSelection = false;
            this.lstDir.Location = new System.Drawing.Point(12, 57);
            this.lstDir.MinimumSize = new System.Drawing.Size(454, 493);
            this.lstDir.Name = "lstDir";
            this.lstDir.Size = new System.Drawing.Size(507, 519);
            this.lstDir.TabIndex = 10;
            this.lstDir.UseCompatibleStateImageBehavior = false;
            this.lstDir.View = System.Windows.Forms.View.Details;
            this.lstDir.SelectedIndexChanged += new System.EventHandler(this.lstDir_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 479;
            // 
            // btnRemoveImage
            // 
            this.btnRemoveImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRemoveImage.Enabled = false;
            this.btnRemoveImage.Location = new System.Drawing.Point(526, 269);
            this.btnRemoveImage.Name = "btnRemoveImage";
            this.btnRemoveImage.Size = new System.Drawing.Size(100, 29);
            this.btnRemoveImage.TabIndex = 11;
            this.btnRemoveImage.Text = "Remove Image";
            this.btnRemoveImage.UseVisualStyleBackColor = true;
            this.btnRemoveImage.Click += new System.EventHandler(this.BtnRemoveImage_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 586);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(634, 25);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 19);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(45, 20);
            this.toolStripStatusLabel1.Text = "ready...";
            // 
            // btnExistingImage
            // 
            this.btnExistingImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExistingImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExistingImage.Enabled = false;
            this.btnExistingImage.Location = new System.Drawing.Point(526, 234);
            this.btnExistingImage.Name = "btnExistingImage";
            this.btnExistingImage.Size = new System.Drawing.Size(100, 29);
            this.btnExistingImage.TabIndex = 13;
            this.btnExistingImage.Text = "Exisitng Image";
            this.btnExistingImage.UseVisualStyleBackColor = true;
            this.btnExistingImage.Click += new System.EventHandler(this.BtnExisitngImage_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(634, 25);
            this.toolStrip1.TabIndex = 14;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // lblMenuVer
            // 
            this.lblMenuVer.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenuVer.ForeColor = System.Drawing.Color.Red;
            this.lblMenuVer.Location = new System.Drawing.Point(526, 477);
            this.lblMenuVer.Name = "lblMenuVer";
            this.lblMenuVer.Size = new System.Drawing.Size(100, 15);
            this.lblMenuVer.TabIndex = 15;
            this.lblMenuVer.Text = "Menu v";
            this.lblMenuVer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.menuFileUpdateToolStripMenuItem,
            this.applicationUpdateToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.toolStripDropDownButton1.Image = global::SatiatorRingsConfig.Properties.Resources.open1;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(54, 22);
            this.toolStripDropDownButton1.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(173, 6);
            // 
            // menuFileUpdateToolStripMenuItem
            // 
            this.menuFileUpdateToolStripMenuItem.Name = "menuFileUpdateToolStripMenuItem";
            this.menuFileUpdateToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.menuFileUpdateToolStripMenuItem.Text = "Menu File Update";
            this.menuFileUpdateToolStripMenuItem.Click += new System.EventHandler(this.MenuFileUpdateToolStripMenuItem_Click);
            // 
            // applicationUpdateToolStripMenuItem
            // 
            this.applicationUpdateToolStripMenuItem.Name = "applicationUpdateToolStripMenuItem";
            this.applicationUpdateToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.applicationUpdateToolStripMenuItem.Text = "Application Update";
            this.applicationUpdateToolStripMenuItem.Click += new System.EventHandler(this.ApplicationUpdateToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(173, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(528, 396);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(98, 78);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(525, 85);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 611);
            this.Controls.Add(this.lblMenuVer);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnExistingImage);
            this.Controls.Add(this.txtDir);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnRemoveImage);
            this.Controls.Add(this.lstDir);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.btnBuild);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtImageID);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(650, 650);
            this.MinimumSize = new System.Drawing.Size(650, 650);
            this.Name = "frmMain";
            this.Text = "Satiator Rings Configuration";
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDir;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtImageID;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ListView lstDir;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button btnRemoveImage;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button btnExistingImage;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuFileUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applicationUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        public System.Windows.Forms.Label lblMenuVer;
    }
}


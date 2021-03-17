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
            this.btnApply = new System.Windows.Forms.Button();
            this.btnBuild = new System.Windows.Forms.Button();
            this.lstDir = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRemoveIDTag = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applicationUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblMenuVer = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pnlTheme = new System.Windows.Forms.Panel();
            this.btnFont = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnBg = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSelection = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.picGame = new System.Windows.Forms.PictureBox();
            this.picTheme = new System.Windows.Forms.PictureBox();
            this.picMenu = new System.Windows.Forms.PictureBox();
            this.picSatiator = new System.Windows.Forms.PictureBox();
            this.picOptions = new System.Windows.Forms.PictureBox();
            this.picConsole = new System.Windows.Forms.PictureBox();
            this.lstThemes = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.label11 = new System.Windows.Forms.Label();
            this.txtThemeName = new System.Windows.Forms.TextBox();
            this.btnDeleteTheme = new System.Windows.Forms.Button();
            this.btnNewTheme = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.pnlTheme.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTheme)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSatiator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picConsole)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDir
            // 
            this.txtDir.Location = new System.Drawing.Point(6, 528);
            this.txtDir.Name = "txtDir";
            this.txtDir.ReadOnly = true;
            this.txtDir.Size = new System.Drawing.Size(513, 20);
            this.txtDir.TabIndex = 0;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowse.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.Location = new System.Drawing.Point(118, 26);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(165, 76);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Open SD Game Folder";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApply.Enabled = false;
            this.btnApply.Location = new System.Drawing.Point(525, 112);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(100, 29);
            this.btnApply.TabIndex = 7;
            this.btnApply.Text = "Add Image";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // btnBuild
            // 
            this.btnBuild.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuild.Enabled = false;
            this.btnBuild.Location = new System.Drawing.Point(289, 26);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(165, 76);
            this.btnBuild.TabIndex = 8;
            this.btnBuild.Text = "Install Satiator Rings";
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
            this.lstDir.Location = new System.Drawing.Point(6, 6);
            this.lstDir.MinimumSize = new System.Drawing.Size(454, 493);
            this.lstDir.Name = "lstDir";
            this.lstDir.Size = new System.Drawing.Size(513, 516);
            this.lstDir.TabIndex = 10;
            this.lstDir.UseCompatibleStateImageBehavior = false;
            this.lstDir.View = System.Windows.Forms.View.Details;
            this.lstDir.SelectedIndexChanged += new System.EventHandler(this.lstDir_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 479;
            // 
            // btnRemoveIDTag
            // 
            this.btnRemoveIDTag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveIDTag.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRemoveIDTag.Location = new System.Drawing.Point(525, 147);
            this.btnRemoveIDTag.Name = "btnRemoveIDTag";
            this.btnRemoveIDTag.Size = new System.Drawing.Size(100, 29);
            this.btnRemoveIDTag.TabIndex = 11;
            this.btnRemoveIDTag.Text = "Remove ID Tag";
            this.btnRemoveIDTag.UseVisualStyleBackColor = true;
            this.btnRemoveIDTag.Visible = false;
            this.btnRemoveIDTag.Click += new System.EventHandler(this.BtnRemoveImage_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 692);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(655, 25);
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
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(655, 25);
            this.toolStrip1.TabIndex = 14;
            this.toolStrip1.Text = "toolStrip1";
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
            // lblMenuVer
            // 
            this.lblMenuVer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(30)))), ((int)(((byte)(39)))));
            this.lblMenuVer.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenuVer.ForeColor = System.Drawing.Color.White;
            this.lblMenuVer.Location = new System.Drawing.Point(13, 83);
            this.lblMenuVer.Name = "lblMenuVer";
            this.lblMenuVer.Size = new System.Drawing.Size(99, 19);
            this.lblMenuVer.TabIndex = 15;
            this.lblMenuVer.Text = "Menu v";
            this.lblMenuVer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(12, 26);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(100, 76);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(525, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Enabled = false;
            this.tabControl1.Location = new System.Drawing.Point(12, 108);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(639, 581);
            this.tabControl1.TabIndex = 16;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.TabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lstDir);
            this.tabPage1.Controls.Add(this.btnApply);
            this.tabPage1.Controls.Add(this.btnRemoveIDTag);
            this.tabPage1.Controls.Add(this.txtDir);
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(631, 554);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Game List";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnNewTheme);
            this.tabPage2.Controls.Add(this.pnlTheme);
            this.tabPage2.Controls.Add(this.lstThemes);
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(631, 554);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Themes";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // pnlTheme
            // 
            this.pnlTheme.Controls.Add(this.btnDeleteTheme);
            this.pnlTheme.Controls.Add(this.txtThemeName);
            this.pnlTheme.Controls.Add(this.label11);
            this.pnlTheme.Controls.Add(this.btnFont);
            this.pnlTheme.Controls.Add(this.label10);
            this.pnlTheme.Controls.Add(this.label1);
            this.pnlTheme.Controls.Add(this.label9);
            this.pnlTheme.Controls.Add(this.label2);
            this.pnlTheme.Controls.Add(this.label8);
            this.pnlTheme.Controls.Add(this.btnBg);
            this.pnlTheme.Controls.Add(this.label7);
            this.pnlTheme.Controls.Add(this.label3);
            this.pnlTheme.Controls.Add(this.label6);
            this.pnlTheme.Controls.Add(this.btnSelection);
            this.pnlTheme.Controls.Add(this.label5);
            this.pnlTheme.Controls.Add(this.picLogo);
            this.pnlTheme.Controls.Add(this.label4);
            this.pnlTheme.Controls.Add(this.picGame);
            this.pnlTheme.Controls.Add(this.picTheme);
            this.pnlTheme.Controls.Add(this.picMenu);
            this.pnlTheme.Controls.Add(this.picSatiator);
            this.pnlTheme.Controls.Add(this.picOptions);
            this.pnlTheme.Controls.Add(this.picConsole);
            this.pnlTheme.Location = new System.Drawing.Point(369, 6);
            this.pnlTheme.Name = "pnlTheme";
            this.pnlTheme.Size = new System.Drawing.Size(259, 542);
            this.pnlTheme.TabIndex = 32;
            // 
            // btnFont
            // 
            this.btnFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFont.BackColor = System.Drawing.Color.Maroon;
            this.btnFont.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFont.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFont.Location = new System.Drawing.Point(125, 45);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(21, 22);
            this.btnFont.TabIndex = 13;
            this.btnFont.UseVisualStyleBackColor = false;
            this.btnFont.Click += new System.EventHandler(this.BtnThemeColour_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 261);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(113, 14);
            this.label10.TabIndex = 31;
            this.label10.Text = "Theme Selector Sprite";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 14);
            this.label1.TabIndex = 12;
            this.label1.Text = "Font Colour";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(24, 241);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(96, 14);
            this.label9.TabIndex = 30;
            this.label9.Text = "Satiator Info Sprite";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 14);
            this.label2.TabIndex = 14;
            this.label2.Text = "Background Colour";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 219);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 14);
            this.label8.TabIndex = 29;
            this.label8.Text = "Console Info Sprite";
            // 
            // btnBg
            // 
            this.btnBg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBg.BackColor = System.Drawing.Color.Maroon;
            this.btnBg.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBg.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBg.Location = new System.Drawing.Point(125, 73);
            this.btnBg.Name = "btnBg";
            this.btnBg.Size = new System.Drawing.Size(21, 22);
            this.btnBg.TabIndex = 15;
            this.btnBg.UseVisualStyleBackColor = false;
            this.btnBg.Click += new System.EventHandler(this.BtnThemeColour_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(45, 197);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 14);
            this.label7.TabIndex = 28;
            this.label7.Text = "Options Sprite";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 14);
            this.label3.TabIndex = 16;
            this.label3.Text = "Selection Colour";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(55, 175);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 14);
            this.label6.TabIndex = 27;
            this.label6.Text = "Menu Sprite";
            // 
            // btnSelection
            // 
            this.btnSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelection.BackColor = System.Drawing.Color.Maroon;
            this.btnSelection.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelection.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelection.Location = new System.Drawing.Point(125, 101);
            this.btnSelection.Name = "btnSelection";
            this.btnSelection.Size = new System.Drawing.Size(21, 22);
            this.btnSelection.TabIndex = 17;
            this.btnSelection.UseVisualStyleBackColor = false;
            this.btnSelection.Click += new System.EventHandler(this.BtnThemeColour_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 153);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 14);
            this.label5.TabIndex = 26;
            this.label5.Text = "Game List Sprite";
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.Black;
            this.picLogo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(125, 129);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(128, 16);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 18;
            this.picLogo.TabStop = false;
            this.picLogo.Click += new System.EventHandler(this.PicLogo_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(57, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 14);
            this.label4.TabIndex = 25;
            this.label4.Text = "Logo Sprite";
            // 
            // picGame
            // 
            this.picGame.BackColor = System.Drawing.Color.Black;
            this.picGame.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picGame.Image = ((System.Drawing.Image)(resources.GetObject("picGame.Image")));
            this.picGame.Location = new System.Drawing.Point(125, 151);
            this.picGame.Name = "picGame";
            this.picGame.Size = new System.Drawing.Size(128, 16);
            this.picGame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picGame.TabIndex = 19;
            this.picGame.TabStop = false;
            this.picGame.Click += new System.EventHandler(this.PicGame_Click);
            // 
            // picTheme
            // 
            this.picTheme.BackColor = System.Drawing.Color.Black;
            this.picTheme.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picTheme.Image = ((System.Drawing.Image)(resources.GetObject("picTheme.Image")));
            this.picTheme.Location = new System.Drawing.Point(125, 261);
            this.picTheme.Name = "picTheme";
            this.picTheme.Size = new System.Drawing.Size(128, 16);
            this.picTheme.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picTheme.TabIndex = 24;
            this.picTheme.TabStop = false;
            this.picTheme.Click += new System.EventHandler(this.PicTheme_Click);
            // 
            // picMenu
            // 
            this.picMenu.BackColor = System.Drawing.Color.Black;
            this.picMenu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMenu.Image = ((System.Drawing.Image)(resources.GetObject("picMenu.Image")));
            this.picMenu.Location = new System.Drawing.Point(125, 173);
            this.picMenu.Name = "picMenu";
            this.picMenu.Size = new System.Drawing.Size(128, 16);
            this.picMenu.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picMenu.TabIndex = 20;
            this.picMenu.TabStop = false;
            this.picMenu.Click += new System.EventHandler(this.PicMenu_Click);
            // 
            // picSatiator
            // 
            this.picSatiator.BackColor = System.Drawing.Color.Black;
            this.picSatiator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSatiator.Image = ((System.Drawing.Image)(resources.GetObject("picSatiator.Image")));
            this.picSatiator.Location = new System.Drawing.Point(125, 239);
            this.picSatiator.Name = "picSatiator";
            this.picSatiator.Size = new System.Drawing.Size(128, 16);
            this.picSatiator.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSatiator.TabIndex = 23;
            this.picSatiator.TabStop = false;
            this.picSatiator.Click += new System.EventHandler(this.PicSatiator_Click);
            // 
            // picOptions
            // 
            this.picOptions.BackColor = System.Drawing.Color.Black;
            this.picOptions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picOptions.Image = ((System.Drawing.Image)(resources.GetObject("picOptions.Image")));
            this.picOptions.Location = new System.Drawing.Point(125, 195);
            this.picOptions.Name = "picOptions";
            this.picOptions.Size = new System.Drawing.Size(128, 16);
            this.picOptions.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOptions.TabIndex = 21;
            this.picOptions.TabStop = false;
            this.picOptions.Click += new System.EventHandler(this.PicOptions_Click);
            // 
            // picConsole
            // 
            this.picConsole.BackColor = System.Drawing.Color.Black;
            this.picConsole.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picConsole.Image = ((System.Drawing.Image)(resources.GetObject("picConsole.Image")));
            this.picConsole.Location = new System.Drawing.Point(125, 217);
            this.picConsole.Name = "picConsole";
            this.picConsole.Size = new System.Drawing.Size(128, 16);
            this.picConsole.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picConsole.TabIndex = 22;
            this.picConsole.TabStop = false;
            this.picConsole.Click += new System.EventHandler(this.PicConsole_Click);
            // 
            // lstThemes
            // 
            this.lstThemes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.lstThemes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lstThemes.FullRowSelect = true;
            this.lstThemes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstThemes.HideSelection = false;
            this.lstThemes.Location = new System.Drawing.Point(6, 6);
            this.lstThemes.Name = "lstThemes";
            this.lstThemes.Size = new System.Drawing.Size(361, 504);
            this.lstThemes.TabIndex = 11;
            this.lstThemes.UseCompatibleStateImageBehavior = false;
            this.lstThemes.View = System.Windows.Forms.View.Details;
            this.lstThemes.SelectedIndexChanged += new System.EventHandler(this.LstThemes_SelectedIndexChanged);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 200;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(50, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(69, 14);
            this.label11.TabIndex = 32;
            this.label11.Text = "Theme Name";
            // 
            // txtThemeName
            // 
            this.txtThemeName.Location = new System.Drawing.Point(125, 19);
            this.txtThemeName.Name = "txtThemeName";
            this.txtThemeName.Size = new System.Drawing.Size(127, 20);
            this.txtThemeName.TabIndex = 33;
            this.txtThemeName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBox1_KeyUp);
            this.txtThemeName.Leave += new System.EventHandler(this.TextBox1_Leave);
            // 
            // btnDeleteTheme
            // 
            this.btnDeleteTheme.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteTheme.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteTheme.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteTheme.Location = new System.Drawing.Point(124, 283);
            this.btnDeleteTheme.Name = "btnDeleteTheme";
            this.btnDeleteTheme.Size = new System.Drawing.Size(128, 35);
            this.btnDeleteTheme.TabIndex = 17;
            this.btnDeleteTheme.Text = "Delete Theme";
            this.btnDeleteTheme.UseVisualStyleBackColor = true;
            this.btnDeleteTheme.Click += new System.EventHandler(this.BtnDeleteTheme_Click);
            // 
            // btnNewTheme
            // 
            this.btnNewTheme.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNewTheme.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNewTheme.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewTheme.Location = new System.Drawing.Point(6, 513);
            this.btnNewTheme.Name = "btnNewTheme";
            this.btnNewTheme.Size = new System.Drawing.Size(127, 35);
            this.btnNewTheme.TabIndex = 34;
            this.btnNewTheme.Text = "Create New Theme";
            this.btnNewTheme.UseVisualStyleBackColor = true;
            this.btnNewTheme.Click += new System.EventHandler(this.BtnNewTheme_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 717);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lblMenuVer);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.btnBuild);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(671, 756);
            this.MinimumSize = new System.Drawing.Size(671, 756);
            this.Name = "frmMain";
            this.Text = "Satiator Rings Configuration";
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.pnlTheme.ResumeLayout(false);
            this.pnlTheme.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTheme)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSatiator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picConsole)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDir;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ListView lstDir;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button btnRemoveIDTag;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuFileUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applicationUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        public System.Windows.Forms.Label lblMenuVer;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox picTheme;
        private System.Windows.Forms.PictureBox picSatiator;
        private System.Windows.Forms.PictureBox picConsole;
        private System.Windows.Forms.PictureBox picOptions;
        private System.Windows.Forms.PictureBox picMenu;
        private System.Windows.Forms.PictureBox picGame;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Button btnSelection;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnBg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnFont;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lstThemes;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Panel pnlTheme;
        private System.Windows.Forms.TextBox txtThemeName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnNewTheme;
        private System.Windows.Forms.Button btnDeleteTheme;
    }
}


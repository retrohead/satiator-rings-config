using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SatiatorRingsConfig
{
    public partial class updateInfoForm : Form
    {
        private IContainer components = null;
        private Label txtApplicationNameNew;
        private RichTextBox txtChangelog;
        private Label txtApplicationName;
        private Label label3;
        private Button btnIgnore;
        private Button btnDownload;

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(updateInfoForm));
            this.txtApplicationNameNew = new System.Windows.Forms.Label();
            this.txtChangelog = new System.Windows.Forms.RichTextBox();
            this.txtApplicationName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnIgnore = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtApplicationNameNew
            // 
            this.txtApplicationNameNew.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApplicationNameNew.Location = new System.Drawing.Point(10, 87);
            this.txtApplicationNameNew.Name = "txtApplicationNameNew";
            this.txtApplicationNameNew.Size = new System.Drawing.Size(513, 23);
            this.txtApplicationNameNew.TabIndex = 3;
            this.txtApplicationNameNew.Text = "XXXXXXXXXX vX.X build xxxx";
            // 
            // txtChangelog
            // 
            this.txtChangelog.Location = new System.Drawing.Point(14, 110);
            this.txtChangelog.Name = "txtChangelog";
            this.txtChangelog.ReadOnly = true;
            this.txtChangelog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtChangelog.Size = new System.Drawing.Size(510, 233);
            this.txtChangelog.TabIndex = 2;
            this.txtChangelog.Text = "";
            // 
            // txtApplicationName
            // 
            this.txtApplicationName.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApplicationName.Location = new System.Drawing.Point(11, 23);
            this.txtApplicationName.Name = "txtApplicationName";
            this.txtApplicationName.Size = new System.Drawing.Size(513, 23);
            this.txtApplicationName.TabIndex = 4;
            this.txtApplicationName.Text = "XXXXXXXXXX vX.X build xxxx";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(513, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Update Available";
            // 
            // btnIgnore
            // 
            this.btnIgnore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIgnore.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnIgnore.Location = new System.Drawing.Point(337, 349);
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.Size = new System.Drawing.Size(75, 23);
            this.btnIgnore.TabIndex = 7;
            this.btnIgnore.Text = "Ignore";
            this.btnIgnore.UseVisualStyleBackColor = true;
            this.btnIgnore.Click += new System.EventHandler(this.btnIgnore_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDownload.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnDownload.Location = new System.Drawing.Point(418, 349);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(105, 23);
            this.btnDownload.TabIndex = 8;
            this.btnDownload.Text = "Download Update";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // updateInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 378);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnIgnore);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtApplicationName);
            this.Controls.Add(this.txtApplicationNameNew);
            this.Controls.Add(this.txtChangelog);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "updateInfoForm";
            this.Text = "Satiator Rings Config Update Information";
            this.Shown += new System.EventHandler(this.UpdateInfoForm_Shown);
            this.ResumeLayout(false);

        }
    }
}

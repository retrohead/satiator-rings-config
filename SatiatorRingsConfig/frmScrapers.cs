using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatiatorRingsConfig
{
    public partial class frmScrapers : Form
    {
        public frmScrapers()
        {
            InitializeComponent();

            string[] scrapers = Properties.Settings.Default.boxartUrls.Split('|');
            string[] formats = Properties.Settings.Default.boxartFormats.Split('|');

            for (int i = 0; i < scrapers.Length; i++)
            {
                ListViewItem item = new ListViewItem(scrapers[i]);
                if (i < formats.Count())
                    item.SubItems.Add(formats[i]);
                else
                    item.SubItems.Add(".jpg");
                lstScrapers.Items.Add(item);
            }
        }

        private void FrmScrapers_Load(object sender, EventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (frmAddScraper frm = new frmAddScraper(""))
            {
                frm.ShowDialog();
                if(frm.DialogResult != DialogResult.Cancel)
                {
                    ListViewItem item = new ListViewItem(frm.newname.Split('|')[0]);
                    item.SubItems.Add(frm.newname.Split('|')[1]);
                    lstScrapers.Items.Add(item);
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.boxartUrls = "";
            Properties.Settings.Default.boxartFormats = "";
            for (int i =0;i< lstScrapers.Items.Count;i++)
            {
                if (Properties.Settings.Default.boxartUrls != "")
                {
                    Properties.Settings.Default.boxartUrls = Properties.Settings.Default.boxartUrls + "|";
                    Properties.Settings.Default.boxartFormats = Properties.Settings.Default.boxartFormats + "|";
                }
                Properties.Settings.Default.boxartUrls = Properties.Settings.Default.boxartUrls + lstScrapers.Items[i].Text;
                Properties.Settings.Default.boxartFormats = Properties.Settings.Default.boxartFormats + lstScrapers.Items[i].SubItems[1].Text;
            }
            Properties.Settings.Default.Save();
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the selected scraper?", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;
            lstScrapers.Items.Remove(lstScrapers.SelectedItems[0]);
        }

        private void LstScrapers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            using (frmAddScraper frm = new frmAddScraper(lstScrapers.SelectedItems[0].Text + "|" + lstScrapers.SelectedItems[0].SubItems[1].Text))
            {
                frm.ShowDialog();
                if (frm.DialogResult != DialogResult.Cancel)
                {
                    lstScrapers.SelectedItems[0].Text = frm.newname.Split('|')[0];
                    lstScrapers.SelectedItems[0].SubItems[1].Text = frm.newname.Split('|')[1];
                }
            }
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            int id = lstScrapers.SelectedItems[0].Index;
            ListViewItem item = lstScrapers.SelectedItems[0];
            lstScrapers.Items.Remove(item);
            lstScrapers.Items.Insert(id - 1, item);
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            int id = lstScrapers.SelectedItems[0].Index;
            ListViewItem item = lstScrapers.SelectedItems[0];
            lstScrapers.Items.Remove(item);
            lstScrapers.Items.Insert(id + 1, item);
            lstScrapers.Focus();
        }

        private void LstScrapers_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnUp.Enabled = false;
            btnDown.Enabled = false;
            if (lstScrapers.SelectedItems.Count == 0)
                return;
            if (lstScrapers.Items.Count < 2)
                return;
            if (lstScrapers.SelectedItems[0].Index > 0)
                btnUp.Enabled = true;
            if (lstScrapers.SelectedItems[0].Index < lstScrapers.Items.Count - 1)
                btnDown.Enabled = true;
            lstScrapers.Focus();
        }
    }
}

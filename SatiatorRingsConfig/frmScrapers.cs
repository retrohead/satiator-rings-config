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

            string[] scrapers = Properties.Settings.Default.scrapers.Split('|');
            lstScrapers.Items.AddRange(scrapers);
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
                    lstScrapers.Items.Add(frm.newname);
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.scrapers = "";
            for(int i =0;i< lstScrapers.Items.Count;i++)
            {
                if (Properties.Settings.Default.scrapers != "")
                    Properties.Settings.Default.scrapers = Properties.Settings.Default.scrapers + "|";
                Properties.Settings.Default.scrapers = Properties.Settings.Default.scrapers + lstScrapers.Items[i].ToString();
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
            lstScrapers.Items.RemoveAt(lstScrapers.SelectedIndex);
        }
    }
}

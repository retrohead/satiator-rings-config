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
    public partial class frmAddScraper : Form
    {
        public string newname = "";
        public frmAddScraper(string name)
        {
            InitializeComponent();
            string format = ".jpg";
            if(name.Contains("|"))
            {
                format = name.Split('|')[1];
                name = name.Split('|')[0];
            }
            txtName.Text = name;
            comboBox1.SelectedIndex = 0;
            for (int i=0;i<comboBox1.Items.Count;i++)
            {
                if(comboBox1.Items[i].ToString() == format)
                {
                    comboBox1.SelectedIndex = i;
                    break;
                }
            }
            txtName.Focus();
        }
        private void TxtName_Enter(object sender, EventArgs e)
        {
            txtName.SelectAll();
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            newname = txtName.Text + "|" + comboBox1.SelectedItem.ToString();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            newname = "";
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

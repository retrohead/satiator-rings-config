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
            txtName.Text = name;
            txtName.Focus();
        }

        private void TxtName_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                newname = txtName.Text;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void TxtName_Enter(object sender, EventArgs e)
        {
            txtName.SelectAll();
        }

        private void TxtName_Leave(object sender, EventArgs e)
        {
            txtName.Focus();
        }
    }
}

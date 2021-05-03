using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatiatorRingsConfig
{
    public partial class frmBoxartExport : Form
    {
        int currentItem;
        public int exported = 0;
        bool cancelled = false;
        string dir;

        List<frmMain.itemData> exportBoxData;
        public frmBoxartExport(List<frmMain.itemData> itemData, string path)
        {
            InitializeComponent();
            exportBoxData = itemData;
            dir = path;
        }

        public void updateProgressLabel(string txt)
        {
            BeginInvoke(new voidDelegate(() =>
            {
                textBox1.Text = textBox1.Text + txt + "\r\n";
                textBox1.SelectionStart = textBox1.Text.Length;
                textBox1.SelectionLength = 0;
                textBox1.ScrollToCaret();
            }));
        }
        public void updateProgress(double val, double max)
        {
            double percent = (val / max) * 100;

            BeginInvoke(new voidDelegate(() =>
            {
                progressBar1.Value = (int)percent;
                if (percent >= 100)
                {
                    updateProgressLabel("ready...");
                }
            }));
        }

        private void bgWorkBoxartUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            updateProgress(currentItem, exportBoxData.Count);
            updateProgressLabel("checking item " + (currentItem + 1) + " of " + exportBoxData.Count);

            frmMain.ipBinData ipBin = frmMain.loadGameIpBin(exportBoxData[currentItem].fn);
            if (ipBin.gameId == "")
                return;
            string dest = Path.Combine(dir, ipBin.gameId + ".TGA");
            if (!File.Exists(dest))
            {
                updateProgressLabel("exporting " + ipBin.gameId + ".TGA");
                File.Copy(Path.Combine(exportBoxData[currentItem].fn, "BOX.TGA"), dest);
                exported++;
            } else
            {
                updateProgressLabel("skipped, already exists");
            }
        }

        private void bgWorkBoxartUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            currentItem++;
            if ((currentItem >= exportBoxData.Count) || (e.Cancelled) || cancelled)
            {
                Close();
                if ((e.Cancelled) || cancelled)
                {
                    DialogResult = DialogResult.Cancel;
                }
                else
                {
                    DialogResult = DialogResult.OK;
                }
            }
            else
            {
                bgWorkBoxartUpdate.RunWorkerAsync();
            }
        }

        private void frmBoxartExport_Shown(object sender, EventArgs e)
        {
            currentItem = 0;
            bgWorkBoxartUpdate.RunWorkerAsync();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel the process?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            cancelled = true;
            bgWorkBoxartUpdate.CancelAsync();
        }
    }
}

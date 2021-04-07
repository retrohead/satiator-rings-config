using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatiatorRingsConfig
{
    public partial class frmBoxartUpdate : Form
    {
        int currentItem;
        int converted = 0;
        bool cancelled = false;
        TGA T;
        List<frmMain.itemData> downloadBoxData;
        public frmBoxartUpdate(List<frmMain.itemData> itemData)
        {
            InitializeComponent();
            downloadBoxData = itemData;
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

        private void processDownloadedImage(string fn, string dir)
        {
            updateProgressLabel("converting " + System.IO.Path.GetFileName(fn));

            frmMain.addTGAtoDir("BOX.TGA", dir, -1, -1, T, null, fn);
            System.IO.File.Delete(fn);
            converted++;
        }

        private void bgWorkBoxartUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            updateProgress(currentItem, downloadBoxData.Count);
            updateProgressLabel("checking item " + (currentItem + 1) + " of " + downloadBoxData.Count);

            string[] scraperUrls = Properties.Settings.Default.scrapers.Split('|');

            for (int i = 0; i < scraperUrls.Length; i++)
            {
                frmMain.ipBinData ipBin = frmMain.loadGameIpBin(downloadBoxData[currentItem].fn);
                if (ipBin.gameId == "")
                    break;
                updateProgressLabel("trying JPG from " + scraperUrls[i]);
                if (update.downloadFile(scraperUrls[i] + ipBin.gameId + ".jpg", "data/temp/", scraperUrls[i] + ipBin.gameId + ".jpg", "", true))
                {
                    processDownloadedImage("data/temp/" + ipBin.gameId + ".jpg", downloadBoxData[currentItem].fn);
                    break;
                }
                else
                {
                    updateProgressLabel("trying PNG from " + scraperUrls[i]);
                    if (update.downloadFile(scraperUrls[i] + ipBin.gameId + ".png", "data/temp/", scraperUrls[i] + ipBin.gameId + ".png", "", true))
                    {
                        processDownloadedImage("data/temp/" + ipBin.gameId + ".png", downloadBoxData[currentItem].fn);
                        break;
                    }
                }
            }
        }

        private void bgWorkBoxartUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            currentItem++;
            if ((currentItem >= downloadBoxData.Count) || (e.Cancelled) || cancelled)
            {
                Close();
                if((e.Cancelled) || cancelled)
                    MessageBox.Show("Process Cancelled\n\n" + converted + " images installed out of " + downloadBoxData.Count, "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    MessageBox.Show("Process Completed\n\n" + converted + " images installed out of " + downloadBoxData.Count, "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                bgWorkBoxartUpdate.RunWorkerAsync();
            }
        }

        private void FrmBoxartUpdate_Shown(object sender, EventArgs e)
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

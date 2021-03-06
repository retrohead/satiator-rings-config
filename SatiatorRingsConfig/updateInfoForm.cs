using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SatiatorRingsConfig   
{
    public partial class updateInfoForm : Form
    {
        string changeLogTxt = "";
        private void showChangeLogInfo()
        {
            string str1 = "";
            changeLogTxt = "";
            try
            {
                string str2 = "changelog.rtf";
                FileStream fs = new FileStream("data//temp/" + str2, FileMode.Open, FileAccess.Read);
                using (StreamReader streamReader = new StreamReader(fs))
                {
                    string str3;
                    while ((str3 = streamReader.ReadLine()) != null)
                        str1 = str1 + str3 + "\r\n";
                    streamReader.Close();
                }
                fs.Close();
            }
            catch
            {
                str1 = str1 + "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang2057{\\fonttbl{\\f0\\fnil\\fcharset0 Verdana;}}\\r\\n" + "{\\*\\generator Msftedit 5.41.21.2509;}\\viewkind4\\uc1\\pard\\sa200\\sl276\\slmult1\\lang9\\b\\f0\\fs28 Satiator Rings Configuaration Change Log\\par\\r\\n";
            }
            changeLogTxt = str1;
        }

        public bool formSetup(string newVersion)
        {
            string str1 = "Satiator Rings Configuration";
            string str2 = "changelog.rtf";
            if (update.downloadFile("http://files-ds-scene.net/retrohead/satiator/releases/" + str2, "data/temp/", "Change Log"))
            {
                this.txtApplicationName.Text = str1 + " v" + Program.form.appVer;
                this.txtApplicationNameNew.Text = str1 + " v" + newVersion;
                this.showChangeLogInfo();
                return true;
            }
            else
            {
                MessageBox.Show("Failed to download the latest changelog, please check your internet connection\r\nor the site may be down!", "Change Log Download Failure", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }
        }

        public updateInfoForm() => this.InitializeComponent();

        private void btnDownload_Click(object sender, EventArgs e)
        {
            string str = "changelog.rtf";
            File.Delete("data/" + str);
            File.Move("data/temp/" + str, "data/" + str);
        }

        private void btnIgnore_Click(object sender, EventArgs e)
        {
            string str = "changelog.rtf";
            File.Delete("data/temp/" + str);
        }

        private void UpdateInfoForm_Shown(object sender, EventArgs e)
        {
            txtChangelog.Rtf = changeLogTxt;
        }
    }
}

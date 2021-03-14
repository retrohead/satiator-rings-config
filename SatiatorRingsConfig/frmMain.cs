using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatiatorRingsConfig
{
    public partial class frmMain : Form
    {
        private class itemData
        {
            public string fn;
            public int imageId;
        }
        struct dir
        {
            public string path;
        }
        public int selectedId;
        public bool firstInstall = false;
        public string appVer = "1.0";
        public bool firstboot = true;

        TGA T;

        public frmMain()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string resourceName = new AssemblyName(args.Name).Name + ".dll";
                string resource = Array.Find(this.GetType().Assembly.GetManifestResourceNames(), element => element.EndsWith(resourceName));

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
                {
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };
            InitializeComponent();
            Text = Text + " v" + appVer;
        }

        private void updatesCompleted(bool success)
        {
            if(!success)
            {
                MessageBox.Show("There was a problem checking for updates", "Update Problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Enabled = true;
            toolStripStatusLabel1.Text = "ready...";
            firstboot = false;
        }
        private void updatesCompletedDownload(bool success)
        {
            if (!success)
            {
                MessageBox.Show("There was a problem checking for updates", "Update Problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Enabled = true;
            toolStripStatusLabel1.Text = "ready...";
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            btnRemoveIDTag.Visible = false;
            btnApply.Enabled = false;
            txtDir.Text = "";
            lstDir.Items.Clear();
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    txtDir.Text = fbd.SelectedPath;
                    string[] dirs = Directory.GetDirectories(fbd.SelectedPath);
                    dir[] objs = new dir[dirs.Count()];
                    int i = 0;
                    foreach (string dir in dirs)
                    {
                        objs[i] = new dir();
                        objs[i].path = dir;
                        i++;
                    }
                    Array.Sort(objs, (x, y) => String.Compare(x.path, y.path));

                    for (int j=0;j<i;j++)
                    {
                        ListViewItem item = new ListViewItem();
                        itemData data = new itemData();
                        data.fn = objs[j].path;
                        string fn = objs[j].path.Replace(txtDir.Text, "");
                        if(fn.StartsWith("\\"))
                            fn = fn.Substring(1, fn.Length - 1);
                        data.imageId = -1;
                        if (fn.EndsWith("]"))
                        {
                            string idStr = fn.Substring(fn.LastIndexOf(" [") + 2, fn.Length - (fn.LastIndexOf(" [") + 2) - 1);
                            if (int.TryParse(idStr, out data.imageId))
                                fn = fn.Substring(0, fn.LastIndexOf(" ["));
                            else
                                data.imageId = -1;
                        }
                        fn = objs[j].path.Replace(txtDir.Text, "");
                        if (fn.StartsWith("\\"))
                            fn = fn.Substring(1, fn.Length - 1);
                        item.Text = fn;
                        item.Tag = data;
                        lstDir.Items.Add(item);
                    }
                    btnBuild.Enabled = true;
                } else
                {
                    btnBuild.Enabled = false;
                }
            }
        }

        private void BtnBuild_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "ISO (*.iso)|*.iso";
            fd.FileName = "satiator-rings.iso";
            fd.InitialDirectory = txtDir.Text.Substring(0, txtDir.Text.IndexOf(@"\"));
            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(fd.FileName))
                    File.Delete(fd.FileName);
                if (!File.Exists(Path.Combine("data\\satiator-rings.iso")))
                {
                    MessageBox.Show("Could not find the menu iso file, try downloading it again via a menu update", "Menu ISO Mising", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                File.Copy(Path.Combine("data\\satiator-rings.iso"), fd.FileName);
                MessageBox.Show("Completed, Enjoy :)", "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstDir.SelectedItems[0].Tag;
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Image Files (*.png;*.jpg;*.bmp;*.gif)|*.png;*.jpg;*.bmp;*.gif";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(Path.Combine(data.fn, "BOX.TGA")))
                {
                    if (MessageBox.Show("An image already exists with this ID, do you want to overwrite it?", "Confirm Replace", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        File.Delete(Path.Combine(data.fn, "BOX.TGA"));
                }
                using (Image img = Image.FromFile(fd.FileName))
                {
                    Image newimg;
                    double imgratio = (double)img.Width / (double)img.Height;
                    if (imgratio > 0.8)
                    {
                        // presume japanese
                        newimg = ResizeImage(img, 80, 80);
                    }
                    else
                    {
                        // presume eur / usa
                        newimg = ResizeImage(img, 64, 100);
                    }
                    newimg.Save("tmp.png");
                    newimg.Dispose();

                    using (Bitmap original = new Bitmap("tmp.png"))
                    using (Bitmap clone = new Bitmap(original))
                    using (Bitmap newbmp = clone.Clone(new Rectangle(0, 0, clone.Width, clone.Height), PixelFormat.Format24bppRgb))
                        T = (TGA)newbmp;
                    T.Save(Path.Combine(data.fn, "BOX.TGA"));
                    pictureBox1.Image = (Bitmap)T;
                    pictureBox1.Visible = true;
                }
            }
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void lstDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstDir.SelectedItems.Count == 0)
            {
                btnApply.Enabled = false;
                btnRemoveIDTag.Visible = false;
                pictureBox1.Image = null;
                return;
            }
            btnApply.Enabled = true;
            itemData data = (itemData)lstDir.SelectedItems[0].Tag;

            if (data.imageId != -1)
                btnRemoveIDTag.Visible = true;
            else
                btnRemoveIDTag.Visible = false;

            string path = Path.Combine(data.fn, "BOX.TGA");
            if (!File.Exists(path))
            {
                pictureBox1.Image = null;
                return;
            }
            T = new TGA(path);
            pictureBox1.Image = (Bitmap)T;
        }

        private void BtnRemoveImage_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstDir.SelectedItems[0].Tag;
            if (data.imageId != -1)
            {
                // rename the folder taking the ID off
                string newDirName = data.fn.Substring(0, data.fn.LastIndexOf(" ["));
                Directory.Move(data.fn, newDirName);
                data.fn = newDirName;
                lstDir.SelectedItems[0].Tag = data;
                lstDir.SelectedItems[0].Text = Path.GetFileName(data.fn);
                MessageBox.Show("All fixed :) Sorry about that", "Sorry!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void enableForm(bool enable)
        {
            BeginInvoke(new voidDelegate(() =>
            {
                Enabled = enable;
            }));
        }
        public void updateProgressLabel(string txt)
        {
            txt = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txt);
            txt = txt.Replace(" And ", " and ");
            txt = txt.Replace(" Of ", " of ");
            txt = txt.Replace(" For ", " for ");

            if ((txt != "") & (txt.Trim().ToLower() != "completed"))
            {
                BeginInvoke(new voidDelegate(() =>
                {
                    toolStripStatusLabel1.Text = txt;
                }));
            }
            else if ((txt == "") | (txt.Trim().ToLower() == "completed"))
            {
                BeginInvoke(new voidDelegate(() =>
                {
                    toolStripStatusLabel1.Text = "ready...";
                }));
            }
        }
        public void updateProgress(double val, double max)
        {
            double percent = (val / max) * 100;

            BeginInvoke(new voidDelegate(() =>
            {
                toolStripProgressBar1.Value = (int)percent;
                if (percent >= 100)
                {
                    toolStripStatusLabel1.Text = "ready...";
                }
            }));
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            update.checkForUpdates(this, this, updatesCompleted);
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BtnBrowse_Click(sender, e);
        }

        private void MenuFileUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            update.checkForUpdate(update.updateTypes.menu, true, this, updatesCompletedDownload);
        }

        private void ApplicationUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            enableForm(false);
            update.checkForUpdate(update.updateTypes.application, true, this, updatesCompleted);
        }
    }
    public delegate void voidDelegate();
}
    
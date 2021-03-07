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
        public string appVer = "0.6";
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
            btnRemoveImage.Enabled = false;
            btnApply.Enabled = false;
            btnExistingImage.Enabled = false;
            txtDir.Text = "";
            txtImageID.Text = "";
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
            clearUnusedBoxarts();
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "ISO (*.iso)|*.iso";
            fd.FileName = "satiator-rings.iso";
            fd.InitialDirectory = txtDir.Text.Substring(0, txtDir.Text.IndexOf(@"\"));
            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(fd.FileName))
                    File.Delete(fd.FileName);

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                if (File.Exists("satiator-rings.iso"))
                    File.Delete("satiator-rings.iso");
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = @"iso\mkisofs.exe";
                startInfo.Arguments = "-quiet -sysid \"SEGA SATURN\" -volid \"SaturnApp\" -volset \"SaturnApp\" -sectype 2352 -publisher \"SEGA ENTERPRISES, LTD.\" -preparer \"SEGA ENTERPRISES, LTD.\" -appid \"SaturnApp\" -abstract \"./iso/cd/ABS.TXT\" -copyright \"./iso/cd/CPY.TXT\" -biblio \"./iso/cd/BIB.TXT\" -generic-boot \"./iso/IP.BIN\" -full-iso9660-filenames -o satiator-rings.iso ./iso/cd";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                File.Copy("satiator-rings.iso", fd.FileName);
                MessageBox.Show("Completed, Enjoy :)", "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        int freeBoxartID()
        {
            for (int j = 0; j < 100; j++)
            {
                for (int i = 0; i < 100; i++)
                {
                    string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "iso", "cd");
                    if(j!=0)
                        path = Path.Combine(path, "BOX" + j);
                    else
                        path = Path.Combine(path, "BOX");
                    path = Path.Combine(path, i + "S.TGA");
                    if (!File.Exists(path))
                    {
                        return i + (j * 100);
                    }
                }
            }
            return -1;
        }

        void clearUnusedBoxarts()
        {
            List<string> boxes = new List<string>();
            for (int j = 0; j < 100; j++)
            {
                for (int i = 0; i < 100; i++)
                {
                    string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "iso", "cd");
                    if (j != 0)
                        path = Path.Combine(path, "BOX" + j);
                    else
                        path = Path.Combine(path, "BOX");
                    path = Path.Combine(path, i + "S.TGA");
                    if (File.Exists(path))
                    {
                        boxes.Add(path);
                        for(int k = 0; k< lstDir.Items.Count; k++)
                        {
                            itemData data = (itemData)lstDir.Items[k].Tag;
                            if (data.imageId == i + (j * 100))
                            {
                                boxes.Remove(path);
                                break;
                            }
                        }
                    }
                }
            }
            if(boxes.Count > 0)
            {
                if(MessageBox.Show("There are " + boxes.Count + " unused boxarts in the ISO folder. Do you want to remove them?", "Unused Images", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for(int i=0;i< boxes.Count;i++)
                    {
                        File.Delete(boxes[i]);
                    }
                }
            }
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstDir.SelectedItems[0].Tag;
            int boxFolderId = 0;
            int id = freeBoxartID();
            if(id < 0)
            {
                MessageBox.Show("Maximum boxarts reached", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Image FIles (*.png;*.jpg;*.bmp;*.gif)|*.png;*.jpg;*.bmp;*.gif";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (data.imageId != -1)
                {
                    if (MessageBox.Show("An image already exists with this ID, do you want to overwrite it?", "Confirm Replace", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        id = data.imageId;
                }
                while (id >= 100)
                {
                    id -= 100;
                    boxFolderId++;
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
                    string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "iso", "cd");
                    if (boxFolderId > 0)
                        path = Path.Combine(path, "BOX" + boxFolderId);
                    else
                        path = Path.Combine(path, "BOX");

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string newDirName = lstDir.SelectedItems[0].Text;
                    path = Path.Combine(path, id + "S.TGA");
                    if (File.Exists(path))
                        File.Delete(path);
                    T.Save(path);
                    File.Delete("tmp.png");
                    pictureBox1.Image = (Bitmap)T;
                    pictureBox1.Visible = true;

                    newDirName  = newDirName + " [" + id + "]";
                    newDirName = Path.Combine(Path.GetDirectoryName(data.fn), newDirName);
                    lstDir.SelectedItems[0].Tag = data;
                    if(data.fn != newDirName)
                        Directory.Move(data.fn, newDirName);
                    data.fn = newDirName;
                    data.imageId = id;
                    txtImageID.Text = data.imageId.ToString();
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
                btnRemoveImage.Enabled = false;
                btnExistingImage.Enabled = false;
                txtImageID.Text = "-1";
                pictureBox1.Image = null;
                return;
            }
            btnApply.Enabled = true;
            btnExistingImage.Enabled = true;
            itemData data = (itemData)lstDir.SelectedItems[0].Tag;
            txtImageID.Text = data.imageId.ToString();

            if (txtImageID.Text == "-1")
            {
                pictureBox1.Image = null;
                btnRemoveImage.Enabled = false;
                return;
            }
            btnRemoveImage.Enabled = true;
            int id = int.Parse(txtImageID.Text);
            int boxId = 0;
            while (id >= 100)
            {
                id--;
                boxId++;
            }
            string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "iso", "cd");
            if (boxId > 0)
                path = Path.Combine(path, "BOX" + boxId);
            else
                path = Path.Combine(path, "BOX");

            path = Path.Combine(path, id + "S.TGA");
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

                int boxFolderId = 0;
                int id = data.imageId;
                while (id >= 100)
                {
                    id -= 100;
                    boxFolderId++;
                }
                string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "iso", "cd");
                if (boxFolderId > 0)
                    path = Path.Combine(path, "BOX" + boxFolderId);
                else
                    path = Path.Combine(path, "BOX");
                if (File.Exists(Path.Combine(path, id + "S.TGA")))
                {
                    if(MessageBox.Show("Do you want to delete the image from the CD?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        File.Delete(Path.Combine(path, id + "S.TGA"));
                }
                data.fn = newDirName;
                data.imageId = -1;
                txtImageID.Text = data.imageId.ToString();
                pictureBox1.Image = null;
            }
        }

        private void BtnExisitngImage_Click(object sender, EventArgs e)
        {
            frmExistingImages f = new frmExistingImages(this);
            f.ShowDialog();

            itemData data = (itemData)lstDir.SelectedItems[0].Tag;
            if (selectedId != -1)
            {
                int boxFolderId = 0;
                int id = data.imageId;
                string newDirName = data.fn;
                string path = "";
                if (data.imageId >= 0)
                    newDirName = data.fn.Substring(0, data.fn.LastIndexOf(" ["));
                if (id != -1)
                {
                    // remove the existing image
                    // rename the folder taking the ID off
                    Directory.Move(data.fn, newDirName);

                    while (id >= 100)
                    {
                        id -= 100;
                        boxFolderId++;
                    }
                    path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "iso", "cd");
                    if (boxFolderId > 0)
                        path = Path.Combine(path, "BOX" + boxFolderId);
                    else
                        path = Path.Combine(path, "BOX");
                    if (File.Exists(Path.Combine(path, id + "S.TGA")))
                    {
                        if (MessageBox.Show("Do you want to delete the old image from the CD?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            File.Delete(Path.Combine(path, id + "S.TGA"));
                    }
                    data.fn = newDirName;
                    data.imageId = -1;
                    txtImageID.Text = data.imageId.ToString();
                    pictureBox1.Image = null;
                }

                // attach the image to the directory by renaming it
                newDirName = data.fn + " [" + selectedId + "]";
                txtImageID.Text = data.imageId.ToString();

                id = selectedId;
                while (id >= 100)
                {
                    id -= 100;
                    boxFolderId++;
                }
                path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "iso", "cd");
                if (boxFolderId > 0)
                    path = Path.Combine(path, "BOX" + boxFolderId);
                else
                    path = Path.Combine(path, "BOX");
                path = Path.Combine(path, id + "S.TGA");
                T = new TGA(path);
                pictureBox1.Image = (Bitmap)T;
                Directory.Move(data.fn, newDirName);
                data.fn = newDirName;
                data.imageId = selectedId;
                txtImageID.Text = data.imageId.ToString();
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
    
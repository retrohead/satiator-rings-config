using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatiatorRingsConfig
{
    public partial class frmMain : Form
    {
        private class nodeData
        {
            public string fn;
            public int imageId;
        }
        struct dir
        {
            public string path;
        }
        TGA T;
        public frmMain()
        {
            InitializeComponent();
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            btnApply.Enabled = false;
            treeView1.Nodes.Clear();
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
                        TreeNode node = new TreeNode();
                        nodeData data = new nodeData();
                        data.fn = objs[j].path;
                        string fn = objs[j].path.Replace(txtDir.Text, "");
                        fn = fn.Substring(1, fn.Length - 1);
                        data.imageId = -1;
                        if (fn.EndsWith("]"))
                        {
                            string idStr = fn.Substring(fn.LastIndexOf(" [") + 2, fn.Length - (fn.LastIndexOf(" [") + 2) - 1);
                            data.imageId = int.Parse(idStr);
                            fn = fn.Substring(0, fn.LastIndexOf(" ["));
                        }
                        node.Text = fn;
                        node.Tag = data;
                        treeView1.Nodes.Add(node);
                    }
                    btnBuild.Enabled = true;
                } else
                {
                    btnBuild.Enabled = false;
                }
            }
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            btnApply.Enabled = true;
            nodeData data = (nodeData)e.Node.Tag;
            txtImageID.Text = data.imageId.ToString();

            if (txtImageID.Text == "-1")
            {
                pictureBox1.Visible = false;
                return;
            }
            int id = int.Parse(txtImageID.Text);
            int boxId = 0;
            while(id >= 100)
            {
                id--;
                boxId++;
            }
            string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "iso", "cd");
            if(boxId > 0)
                path = Path.Combine(path, "BOX" + boxId);
            else
                path = Path.Combine(path, "BOX");

            path = Path.Combine(path, txtImageID.Text + "S.TGA");
            if (!File.Exists(path))
            {
                pictureBox1.Visible = false;
                return;
            }
            T = new TGA(path);
            pictureBox1.Image =(Bitmap)T;
            pictureBox1.Visible = true;
        }

        private void BtnBuild_Click(object sender, EventArgs e)
        {
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


            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "ISO (*.iso)|*.iso";
            fd.FileName = "satiator-rings.iso";
            fd.InitialDirectory = txtDir.Text.Substring(0, txtDir.Text.IndexOf(@"\"));
            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(fd.FileName))
                    File.Delete(fd.FileName);
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

        private void BtnApply_Click(object sender, EventArgs e)
        {
            nodeData data = (nodeData)treeView1.SelectedNode.Tag;
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

                    string newDirName = treeView1.SelectedNode.Text;
                    if (newDirName.LastIndexOf('/') > -1)
                    {
                        newDirName = newDirName.Substring(0, newDirName.LastIndexOf(" ["));
                    }
                    path = Path.Combine(path, id + "S.TGA");
                    if (File.Exists(path))
                        File.Delete(path);
                    T.Save(path);
                    File.Delete("tmp.png");
                    pictureBox1.Image = (Bitmap)T;
                    pictureBox1.Visible = true;

                    newDirName  = newDirName + " [" + id + "]";
                    newDirName = Path.Combine(Path.GetDirectoryName(data.fn), newDirName);
                    treeView1.SelectedNode.Tag = data;
                    if(data.fn != newDirName)
                        Directory.Move(data.fn, newDirName);
                    data.fn = newDirName;
                    data.imageId = id;
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
    }
}

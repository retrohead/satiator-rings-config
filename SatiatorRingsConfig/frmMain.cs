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
    public delegate void voidDelegate();
    public partial class frmMain : Form
    {
        public string appVer = "3.1";
        private class itemData
        {
            public string fn;
            public int imageId;
        }
        private class themeFileType
        {
            public int[] font = new int[3];
            public int[] bg = new int[3];
            public int[] selector = new int[3];

            public TGA img_logo;
            public TGA img_menu;
            public TGA img_gamelist;
            public TGA img_options;
            public TGA img_console;
            public TGA img_satiator;
            public TGA img_theme;
        }
        themeFileType themeFile;
        struct dir
        {
            public string path;
        }
        public int selectedId;
        public bool firstInstall = false;
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
            if (!success)
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
        private void listGames()
        {
            string[] dirs = Directory.GetDirectories(txtDir.Text);
            dir[] objs = new dir[dirs.Count()];
            int i = 0;
            foreach (string dir in dirs)
            {
                objs[i] = new dir();
                objs[i].path = dir;
                i++;
            }
            Array.Sort(objs, (x, y) => String.Compare(x.path, y.path));
            lstDir.Items.Clear();
            for (int j = 0; j < i; j++)
            {
                ListViewItem item = new ListViewItem();
                itemData data = new itemData();
                data.fn = objs[j].path;
                string fn = objs[j].path.Replace(txtDir.Text, "");
                if (fn.StartsWith("\\"))
                    fn = fn.Substring(1, fn.Length - 1);
                data.imageId = -1;
                if (fn.EndsWith("]"))
                {
                    string idStr = fn.Substring(fn.LastIndexOf(" [") + 2, fn.Length - (fn.LastIndexOf(" [") + 2) - 1);
                    if (!int.TryParse(idStr, out data.imageId))
                        data.imageId = -1;
                }
                if (fn.StartsWith("\\"))
                    fn = fn.Substring(1, fn.Length - 1);
                item.Text = fn;
                item.Tag = data;
                lstDir.Items.Add(item);
            }
            if (lstDir.Items.Count > 0)
                lstDir.Items[0].Selected = true;
        }

        private void listThemes()
        {
            string path = txtDir.Text.Substring(0, txtDir.Text.IndexOf(@"\"));
            path = Path.Combine(path, "satiator-rings", "themes");
            if (!Directory.Exists(path))
            {
                tabControl1.TabPages.Remove(tabPage2);
                return;
            }
            else if (tabControl1.TabPages.Count == 1)
                tabControl1.TabPages.Add(tabPage2);
            btnBuild.Text = "Update Satiator Rings";
            string[] dirs = Directory.GetDirectories(path);
            dir[] objs = new dir[dirs.Count()];
            int i = 0;
            foreach (string dir in dirs)
            {
                objs[i] = new dir();
                objs[i].path = dir;
                i++;
            }
            Array.Sort(objs, (x, y) => String.Compare(x.path, y.path));
            lstThemes.Items.Clear();
            for (int j = 0; j < i; j++)
            {
                ListViewItem item = new ListViewItem();
                itemData data = new itemData();
                data.fn = objs[j].path;
                string fn = objs[j].path.Replace(path, "");
                if (fn.StartsWith("\\"))
                    fn = fn.Substring(1, fn.Length - 1);
                item.Text = fn;
                item.Tag = data;
                if (fn != "default")
                    lstThemes.Items.Add(item);
            }
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            btnRemoveIDTag.Visible = false;
            btnAddImage.Enabled = false;
            btnGoogle.Enabled = false;
            btnDelete.Enabled = false;
            btnRename.Enabled = false;
            txtDir.Text = "";
            lstDir.Items.Clear();
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    txtDir.Text = fbd.SelectedPath;
                    listGames();
                    listThemes();
                    btnBuild.Enabled = true;
                    tabControl1.Enabled = true;
                }
                else
                {
                    btnBuild.Enabled = false;
                    tabControl1.Enabled = false;
                    tabControl1.SelectedIndex = 0;
                }
            }
        }

        private void BtnBuild_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists("data\\sd"))
            {
                MessageBox.Show("Menu files appear to be missing. Try updating the menu file the file menu", "Menu Files Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (MessageBox.Show("Are you sure you want to install Satiator Rings to '" + txtDir.Text.Substring(0, txtDir.Text.IndexOf(@"\")) + "'?", "Confirm Installation", MessageBoxButtons.OK, MessageBoxIcon.Question) != DialogResult.OK)
                return;
            update.moveDirectoryContents("data\\sd", txtDir.Text.Substring(0, txtDir.Text.IndexOf(@"\")), false);
            if (lstThemes.Items.Count == 0)
                listThemes();
            MessageBox.Show("The installation completed.\n\nLaunch the satiator-rings.iso from your Satiator.\n\nEnjoy :)", "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void addTGAtoDir(string tgaName, string dir, int w, int h)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Image Files (*.png;*.jpg;*.bmp;*.gif)|*.png;*.jpg;*.bmp;*.gif";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(Path.Combine(dir, tgaName)))
                {
                    if (MessageBox.Show("An image already exists in this directory, do you want to overwrite it?", "Confirm Replace", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        File.Delete(Path.Combine(dir, tgaName));
                }
                using (Image img = Image.FromFile(fd.FileName))
                {
                    Image newimg;
                    if (w < 0 || h < 0)
                    {
                        // presume boxart
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
                    }
                    else
                    {
                        newimg = ResizeImage(img, w, h);
                    }
                    newimg.Save("tmp.png");
                    newimg.Dispose();

                    using (Bitmap original = new Bitmap("tmp.png"))
                    using (Bitmap clone = new Bitmap(original))
                    using (Bitmap newbmp = clone.Clone(new Rectangle(0, 0, clone.Width, clone.Height), PixelFormat.Format24bppRgb))
                        T = (TGA)newbmp;
                    T.Save(Path.Combine(dir, tgaName));
                    pictureBox1.Image = (Bitmap)T;
                    pictureBox1.Visible = true;
                }
            }
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstDir.SelectedItems[0].Tag;
            addTGAtoDir("BOX.TGA", data.fn, -1, -1);
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
                btnAddImage.Enabled = false;
                btnGoogle.Enabled = false;
                btnDelete.Enabled = false;
                btnRename.Enabled = false;
                btnRemoveIDTag.Visible = false;
                pictureBox1.Image = null;
                return;
            }
            btnAddImage.Enabled = true;
            btnGoogle.Enabled = true;
            btnDelete.Enabled = true;
            btnRename.Enabled = true;
            itemData data = (itemData)lstDir.SelectedItems[0].Tag;

            if (data.imageId != -1)
                btnRemoveIDTag.Visible = true;
            else
                btnRemoveIDTag.Visible = false;

            string path = Path.Combine(data.fn, "BOX.TGA");
            if (!File.Exists(path))
            {
                pictureBox1.Image = Properties.Resources.satiator_small;
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
                data.imageId = -1;
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

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((tabControl1.SelectedIndex == 1) && (lstThemes.Items.Count > 0) && (lstThemes.SelectedItems.Count == 0))
                lstThemes.Items[0].Selected = true;
        }

        private void LstThemes_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (lstThemes.SelectedItems.Count == 0)
            {
                pnlTheme.Visible = false;
                return;
            }
            pnlTheme.Visible = true;
            themeFile = new themeFileType();
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            string fn = Path.Combine(data.fn, "theme.ini");
            if (!File.Exists(fn))
            {
                MessageBox.Show("There was an error opening the theme.ini file, make sure it exists.", "theme.ini missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            using (StreamReader sr = new StreamReader(fn))
            {
                string oneline = "";
                while (oneline != "[START]")
                    oneline = sr.ReadLine();

                oneline = sr.ReadLine();
                while (oneline != "[END]")
                {
                    string[] linedata;
                    if (oneline.StartsWith("font="))
                    {
                        oneline = oneline.Substring("font=".Length, oneline.Length - "font=".Length);
                        linedata = oneline.Split(',');
                        themeFile.font[0] = int.Parse(linedata[0]);
                        themeFile.font[1] = int.Parse(linedata[1]);
                        themeFile.font[2] = int.Parse(linedata[2]);
                    }
                    if (oneline.StartsWith("bg="))
                    {
                        oneline = oneline.Substring("bg=".Length, oneline.Length - "bg=".Length);
                        linedata = oneline.Split(',');
                        themeFile.bg[0] = int.Parse(linedata[0]);
                        themeFile.bg[1] = int.Parse(linedata[1]);
                        themeFile.bg[2] = int.Parse(linedata[2]);
                    }
                    if (oneline.StartsWith("selector="))
                    {
                        oneline = oneline.Substring("selector=".Length, oneline.Length - "selector=".Length);
                        linedata = oneline.Split(',');
                        themeFile.selector[0] = int.Parse(linedata[0]);
                        themeFile.selector[1] = int.Parse(linedata[1]);
                        themeFile.selector[2] = int.Parse(linedata[2]);
                    }
                    oneline = sr.ReadLine();
                }
                sr.Close();
            }
            // create the TGA files
            themeFile.img_logo = new TGA(Path.Combine(data.fn, "RINGS.TGA"));
            picLogo.Image = (Bitmap)themeFile.img_logo;
            themeFile.img_menu = new TGA(Path.Combine(data.fn, "MENU.TGA"));
            picMenu.Image = (Bitmap)themeFile.img_menu;
            themeFile.img_gamelist = new TGA(Path.Combine(data.fn, "GAME.TGA"));
            picGame.Image = (Bitmap)themeFile.img_gamelist;
            themeFile.img_console = new TGA(Path.Combine(data.fn, "CONSOLE.TGA"));
            picConsole.Image = (Bitmap)themeFile.img_console;
            themeFile.img_satiator = new TGA(Path.Combine(data.fn, "SIATOR.TGA"));
            picSatiator.Image = (Bitmap)themeFile.img_satiator;
            themeFile.img_options = new TGA(Path.Combine(data.fn, "OPTION.TGA"));
            picOptions.Image = (Bitmap)themeFile.img_options;
            themeFile.img_theme = new TGA(Path.Combine(data.fn, "THEME.TGA"));
            picTheme.Image = (Bitmap)themeFile.img_theme;

            // apply the theme to the gui
            txtThemeName.Text = Path.GetFileName(data.fn);
            btnFont.BackColor = Color.FromArgb(themeFile.font[0], themeFile.font[1], themeFile.font[2]);
            btnBg.BackColor = Color.FromArgb(themeFile.bg[0], themeFile.bg[1], themeFile.bg[2]);
            btnSelection.BackColor = Color.FromArgb(themeFile.selector[0], themeFile.selector[1], themeFile.selector[2]);
            udpateBgColours();
        }

        private void PicLogo_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            addTGAtoDir("RINGS.TGA", data.fn, 128, 16);
        }

        private void PicGame_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            addTGAtoDir("GAME.TGA", data.fn, 128, 16);
        }

        private void PicMenu_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            addTGAtoDir("MENU.TGA", data.fn, 128, 16);
        }

        private void PicOptions_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            addTGAtoDir("OPTION.TGA", data.fn, 128, 16);
        }

        private void PicConsole_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            addTGAtoDir("CONSOLE.TGA", data.fn, 128, 16);
        }

        private void PicSatiator_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            addTGAtoDir("SIATOR.TGA", data.fn, 128, 16);
        }

        private void PicTheme_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            addTGAtoDir("THEME.TGA", data.fn, 128, 16);
        }

        private void udpateBgColours()
        {
            picLogo.BackColor = btnBg.BackColor;
            picGame.BackColor = btnBg.BackColor;
            picMenu.BackColor = btnBg.BackColor;
            picOptions.BackColor = btnBg.BackColor;
            picConsole.BackColor = btnBg.BackColor;
            picSatiator.BackColor = btnBg.BackColor;
            picTheme.BackColor = btnBg.BackColor;
        }
        private void saveThemeIni()
        {
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            StreamWriter sw = new StreamWriter(Path.Combine(data.fn, "theme.ini"));
            sw.WriteLine("[START]");
            sw.WriteLine("font=" + btnFont.BackColor.R + "," + btnFont.BackColor.G + "," + btnFont.BackColor.B);
            sw.WriteLine("bg=" + btnBg.BackColor.R + "," + btnBg.BackColor.G + "," + btnBg.BackColor.B);
            sw.WriteLine("selector=" + btnSelection.BackColor.R + "," + btnSelection.BackColor.G + "," + btnSelection.BackColor.B);
            sw.WriteLine("[END]");
            sw.Close();
        }
        private void BtnThemeColour_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = ((Button)sender).BackColor;
            colorDialog1.FullOpen = true;
            colorDialog1.ShowDialog();
            if (colorDialog1.Color != ((Button)sender).BackColor)
            {
                ((Button)sender).BackColor = colorDialog1.Color;
                saveThemeIni();
            }
            udpateBgColours();
        }

        private void TextBox1_Leave(object sender, EventArgs e)
        {
            char[] chars = Path.GetInvalidPathChars();
            foreach (char c in chars)
            {
                if (txtThemeName.Text.Contains(c))
                {
                    MessageBox.Show("The name cannot include any illegal path characters, please remove '" + c + "' from the name", "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            if (txtThemeName.Text != lstThemes.SelectedItems[0].Text)
            {
                string newpath = Path.Combine(Path.GetDirectoryName(data.fn), txtThemeName.Text);
                if (Directory.Exists(newpath))
                {
                    MessageBox.Show("A theme already exists with this name, please try again", "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                Directory.Move(data.fn, newpath);
                data.fn = newpath;
                lstThemes.SelectedItems[0].Text = txtThemeName.Text;
                lstThemes.SelectedItems[0].Tag = data;
            }
        }

        private void TextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                TextBox1_Leave(sender, null);
        }

        private void BtnDeleteTheme_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the selected theme?", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            Directory.Delete(data.fn, true);
            lstThemes.Items.Remove(lstThemes.SelectedItems[0]);
        }

        private void BtnNewTheme_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to create a new theme?", "Confirm Theme Creation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            string path = Path.Combine(txtDir.Text.Substring(0, txtDir.Text.IndexOf(@"\")), "satiator-rings", "themes", "new theme");
            int num = 0;
            while (Directory.Exists(path))
            {
                num++;
                path = Path.Combine(txtDir.Text.Substring(0, txtDir.Text.IndexOf(@"\")), "satiator-rings", "themes", "new theme (" + num + ")");
            }
            Directory.CreateDirectory(path);
            itemData data = new itemData();
            data.fn = path;
            data.imageId = -1;
            ListViewItem item = new ListViewItem();
            item.Text = Path.GetFileName(path);
            item.Tag = data;
            lstThemes.Items.Add(item);
            update.moveDirectoryContents(@"data\sd\satiator-rings\themes\default", path, false);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnGoogle_Click(object sender, EventArgs e)
        {
            frmGoogleImages.SearchQuery query = new frmGoogleImages.SearchQuery();
            query.Query = "saturn cover " + lstDir.SelectedItems[0].Text;
            while (query.Query.LastIndexOf("(") > 0)
                query.Query = query.Query.Substring(0, query.Query.LastIndexOf("(") - 1);
            while (query.Query.LastIndexOf("[") > 0)
                query.Query = query.Query.Substring(0, query.Query.LastIndexOf("[") - 1);
            query.AdditionalVariables = query.Query;
            using (frmGoogleImages googler = new frmGoogleImages(this, query))
            {
                if (googler.ShowDialog(this) == DialogResult.OK)
                { 
                    //convert into a TGA
                    using (Image img = googler.result)
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
                        itemData data = (itemData)lstDir.SelectedItems[0].Tag;
                        if (File.Exists(Path.Combine(data.fn, "BOX.TGA")))
                            File.Delete(Path.Combine(data.fn, "BOX.TGA"));
                        T.Save(Path.Combine(data.fn, "BOX.TGA"));
                        pictureBox1.Image = (Bitmap)T;
                        pictureBox1.Visible = true;
                    }
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove the selected game from your SD card?\n\n" + lstDir.SelectedItems[0].Text, "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;
            itemData data = (itemData)lstDir.SelectedItems[0].Tag;
            Directory.Delete(data.fn, true);
            lstDir.Items.Remove(lstDir.SelectedItems[0]);
            MessageBox.Show("Item deleted successfully", "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnRename_Click(object sender, EventArgs e)
        {
            using (frmRename f = new frmRename(lstDir.SelectedItems[0].Text))
            {
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.Cancel)
                    return;
                itemData data = (itemData)lstDir.SelectedItems[0].Tag;
                string newname = Path.Combine(Path.GetDirectoryName(data.fn), f.newname);
                if (data.fn == newname)
                    return;
                if (Directory.Exists(newname))
                {
                    MessageBox.Show("A directory already exists with the name '" + newname + "'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                Directory.Move(data.fn, newname);
                data.fn = newname;
                lstDir.SelectedItems[0].Tag = data;
                lstDir.SelectedItems[0].Text = f.newname;
            }

        }
    }
}
    
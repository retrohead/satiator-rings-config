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
        public string appVer = "4.0";
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
            public int[] boxbg = new int[3];

            public TGA img_logo;
            public TGA img_menu;
            public TGA img_gamelist;
            public TGA img_options;
            public TGA img_console;
            public TGA img_satiator;
            public TGA img_theme;
            public TGA img_corner;
            public TGA img_shadow;
        }
        themeFileType themeFile;
        struct dir
        {
            public string path;
        }
        public int selectedId;
        public bool firstInstall = false;
        public bool firstboot = true;
        enum optionsType
        {
            OPTIONS_LIST_MODE,
            OPTIONS_LIST_CATEGORY,
            OPTIONS_SOUND_VOLUME,
            OPTIONS_AUTO_PATCH,
            OPTIONS_DESC_CACHE,
            /* end */
            OPTIONS_COUNT
        };
        int[] options = new int[(int)optionsType.OPTIONS_COUNT];

        TGA T;
        TGA TFAV;
        List<string> favs = new List<string>();

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
            if(!Directory.Exists(txtDir.Text))
            {
                lstDir.Items.Clear();
                MessageBox.Show("The selected directory does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
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

                // check to see if the item is in favs
                fn = data.fn;
                fn = fn.Replace("\\", "/");
                fn = fn.Substring(fn.IndexOf("/"), fn.Length - fn.IndexOf("/"));
                if (favs.Contains(fn))
                    item.ImageIndex = 0;
                else
                    item.ImageIndex = -1;

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
                if (tabControl1.TabPages.Contains(tabThemes))
                    tabControl1.TabPages.Remove(tabThemes);
                return;
            }
            else if (!tabControl1.TabPages.Contains(tabThemes))
                tabControl1.TabPages.Add(tabThemes);
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
            if (lstThemes.Items.Count > 0)
                lstThemes.Items[0].Selected = true;
        }
        private void loadFavourites()
        {
            // init favourites
            favs.Clear();
            lstFavs.Items.Clear();

            // load the favs file
            string path = txtDir.Text.Substring(0, txtDir.Text.IndexOf(@"\"));
            path = Path.Combine(path, "satiator-rings");
            if (!Directory.Exists(path))
            {
                if (tabControl1.TabPages.Contains(tabFavourites))
                    tabControl1.TabPages.Remove(tabFavourites);
                return;
            }
            else if (!tabControl1.TabPages.Contains(tabFavourites))
                tabControl1.TabPages.Add(tabFavourites);
            path = Path.Combine(path, "favs.ini");
            if (!File.Exists(path))
            {
                if (tabControl1.TabPages.Contains(tabFavourites))
                    tabControl1.TabPages.Remove(tabFavourites);
                return;
            }

            using (StreamReader sr = new StreamReader(path))
            {
                string oneline = "";
                while (oneline != "[START]")
                    oneline = sr.ReadLine();

                oneline = sr.ReadLine();
                while (oneline != "[END]")
                {
                    oneline = oneline.Trim();
                    if (oneline != "")
                    {
                        favs.Add(oneline);
                    }
                    oneline = sr.ReadLine();
                }
                sr.Close();
            }
            displayFavourites();
        }
        private void displayFavourites()
        { 
            lstFavs.Items.Clear();
            favs.Sort((x, y) => String.Compare(Path.GetFileName(x), Path.GetFileName(y)));

            string path = txtDir.Text.Substring(0, txtDir.Text.IndexOf(@"\"));
            for (int i=0; i < favs.Count;i++)
            {
                ListViewItem item = new ListViewItem();
                itemData data = new itemData();
                data.fn = path + favs[i].Replace("/", "\\");
                data.imageId = -1;
                item.Text = Path.GetFileName(data.fn);
                if (!Directory.Exists(data.fn) && !File.Exists(data.fn))
                    item.ImageIndex = 1;
                else
                    item.ImageIndex = 0;
                item.Tag = data;
                lstFavs.Items.Add(item);
            }
        }
        private void loadOptions()
        {
            // init options
            options[(int)optionsType.OPTIONS_AUTO_PATCH] = 0;
            options[(int)optionsType.OPTIONS_LIST_MODE] = 0;
            options[(int)optionsType.OPTIONS_LIST_CATEGORY] = 0;
            options[(int)optionsType.OPTIONS_SOUND_VOLUME] = 127;
            options[(int)optionsType.OPTIONS_DESC_CACHE] = 0;

            comboOptionFilter.Items.Clear();
            comboOptionFilter.Items.Add("Standard");
            comboOptionFilter.Items.Add("Favourites");
            comboOptionFilter.Items.Add("Recent History");
            comboOptionFilter.SelectedIndex = 0;

            comboOptionList.Items.Clear();
            comboOptionList.Items.Add("Text / Image");
            comboOptionList.Items.Add("Text Only");
            comboOptionList.SelectedIndex = 0;

            chkAutoPatch.Checked = false;
            chkDescCache.Checked = false;
            trackVolume.Value = 127;

            // load the otions file
            string path = txtDir.Text.Substring(0, txtDir.Text.IndexOf(@"\"));
            path = Path.Combine(path, "satiator-rings");
            if (!Directory.Exists(path))
            {
                if (tabControl1.TabPages.Contains(tabOptions))
                    tabControl1.TabPages.Remove(tabOptions);
                return;
            }
            else if (!tabControl1.TabPages.Contains(tabOptions))
                tabControl1.TabPages.Add(tabOptions);
            path = Path.Combine(path, "options.ini");
            if (!File.Exists(path))
            {
                if (tabControl1.TabPages.Contains(tabOptions))
                    tabControl1.TabPages.Remove(tabOptions);
                return;
            }

            using (StreamReader sr = new StreamReader(path))
            {
                string oneline = "";
                while (oneline != "[START]")
                    oneline = sr.ReadLine();

                oneline = sr.ReadLine();
                while (oneline != "[END]")
                {
                    if (oneline.StartsWith("autopatch"))
                        options[(int)optionsType.OPTIONS_AUTO_PATCH] = int.Parse(oneline.Substring("autopatch=".Length, oneline.Length - "autopatch=".Length));
                    if (oneline.StartsWith("listmode"))
                        options[(int)optionsType.OPTIONS_LIST_MODE] = int.Parse(oneline.Substring("listmode=".Length, oneline.Length - "listmode=".Length));
                    if (oneline.StartsWith("listcat"))
                        options[(int)optionsType.OPTIONS_LIST_CATEGORY] = int.Parse(oneline.Substring("listcat=".Length, oneline.Length - "listcat=".Length));
                    if (oneline.StartsWith("volume"))
                        options[(int)optionsType.OPTIONS_SOUND_VOLUME] = int.Parse(oneline.Substring("volume=".Length, oneline.Length - "volume=".Length));
                    if (oneline.StartsWith("desccache"))
                        options[(int)optionsType.OPTIONS_DESC_CACHE] = int.Parse(oneline.Substring("desccache=".Length, oneline.Length - "desccache=".Length));
                    oneline = sr.ReadLine();
                }
                sr.Close();
            }

            comboOptionFilter.SelectedIndex = options[(int)optionsType.OPTIONS_LIST_CATEGORY];
            comboOptionList.SelectedIndex = options[(int)optionsType.OPTIONS_LIST_MODE];
            if (options[(int)optionsType.OPTIONS_AUTO_PATCH] == 1)
                chkAutoPatch.Checked = true;
            if(options[(int)optionsType.OPTIONS_DESC_CACHE] == 1)
                chkDescCache.Checked = true;
            trackVolume.Value = options[(int)optionsType.OPTIONS_SOUND_VOLUME];
        }
        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            btnBuild.Text = "Install Satiator Rings";
            btnAddImage.Enabled = false;
            btnGoogle.Enabled = false;
            txtDir.Text = "";
            lstDir.Items.Clear();
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    txtDir.Text = fbd.SelectedPath;
                    loadOptions();
                    loadFavourites();
                    listGames();
                    listThemes();
                    btnBuild.Enabled = true;
                    btnRefresh.Enabled = true;
                    tabControl1.Enabled = true;
                }
                else
                {
                    btnBuild.Enabled = false;
                    btnRefresh.Enabled = false;
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
            if (MessageBox.Show("Are you sure you want to install Satiator Rings to '" + txtDir.Text.Substring(0, txtDir.Text.IndexOf(@"\")) + "'?", "Confirm Installation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;
            update.moveDirectoryContents("data\\sd", txtDir.Text.Substring(0, txtDir.Text.IndexOf(@"\")), false);
            if (lstThemes.Items.Count == 0)
                listThemes();
            MessageBox.Show("The installation completed.\n\nFlash the ar_patched-satiator-rings.bin on the root of your SD card or launch the satiator-rings.iso with the satiator menu.\n\nEnjoy :)", "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void addTGAtoDir(string tgaName, string dir, int w, int h, TGA tga, PictureBox picBox)
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
                        tga = (TGA)newbmp;
                    tga.Save(Path.Combine(dir, tgaName));
                    picBox.Image = (Bitmap)tga;
                    picBox.Visible = true;
                }
            }
        }
        private void btnAddImage_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstDir.SelectedItems[0].Tag;
            addTGAtoDir("BOX.TGA", data.fn, -1, -1, T, picBox);
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
                picBox.Image = Properties.Resources.satiator_small;
                return;
            }
            btnAddImage.Enabled = true;
            btnGoogle.Enabled = true;
            itemData data = (itemData)lstDir.SelectedItems[0].Tag;

            string path = Path.Combine(data.fn, "BOX.TGA");
            if (!File.Exists(path))
            {
                picBox.Image = Properties.Resources.satiator_small;
                return;
            }
            T = new TGA(path);
            picBox.Image = (Bitmap)T;
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
            if ((tabControl1.SelectedTab == tabThemes) && (lstThemes.Items.Count > 0) && (lstThemes.SelectedItems.Count == 0))
                lstThemes.Items[0].Selected = true;
            if ((tabControl1.SelectedTab == tabFavourites) && (lstFavs.Items.Count > 0) && (lstFavs.SelectedItems.Count == 0))
                lstFavs.Items[0].Selected = true;
            if (tabControl1.SelectedTab == tabGameList)
                picBox.Refresh();
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
                    if (oneline.StartsWith("boxbg="))
                    {
                        oneline = oneline.Substring("boxbg=".Length, oneline.Length - "boxbg=".Length);
                        linedata = oneline.Split(',');
                        themeFile.boxbg[0] = int.Parse(linedata[0]);
                        themeFile.boxbg[1] = int.Parse(linedata[1]);
                        themeFile.boxbg[2] = int.Parse(linedata[2]);
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
            themeFile.img_corner = new TGA(Path.Combine(data.fn, "CORNER.TGA"));
            picCorner.Image = (Bitmap)themeFile.img_corner;
            if (!File.Exists(Path.Combine(data.fn, "SHDW.TGA")))
                File.WriteAllBytes(Path.Combine(data.fn, "SHDW.TGA"), Properties.Resources.SHDW);
            themeFile.img_shadow = new TGA(Path.Combine(data.fn, "SHDW.TGA"));
            picShadow.Image = (Bitmap)themeFile.img_shadow;

            // apply the theme to the gui
            txtThemeName.Text = Path.GetFileName(data.fn);
            btnFont.BackColor = Color.FromArgb(themeFile.font[0], themeFile.font[1], themeFile.font[2]);
            btnBg.BackColor = Color.FromArgb(themeFile.bg[0], themeFile.bg[1], themeFile.bg[2]);
            btnSelection.BackColor = Color.FromArgb(themeFile.selector[0], themeFile.selector[1], themeFile.selector[2]);
            btnBoxBg.BackColor = Color.FromArgb(themeFile.boxbg[0], themeFile.boxbg[1], themeFile.boxbg[2]);
            udpateBgColours();
        }
        private void PicLogo_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            addTGAtoDir("RINGS.TGA", data.fn, 128, 16, themeFile.img_logo, picLogo);
        }
        private void PicGame_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            addTGAtoDir("GAME.TGA", data.fn, 128, 16, themeFile.img_gamelist, picGame);
        }
        private void PicMenu_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            addTGAtoDir("MENU.TGA", data.fn, 128, 16, themeFile.img_menu, picMenu);
        }
        private void PicOptions_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            addTGAtoDir("OPTION.TGA", data.fn, 128, 16, themeFile.img_options, picOptions);
        }
        private void PicConsole_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            addTGAtoDir("CONSOLE.TGA", data.fn, 128, 16, themeFile.img_console, picConsole);
        }
        private void PicSatiator_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            addTGAtoDir("SIATOR.TGA", data.fn, 128, 16, themeFile.img_satiator, picSatiator);
        }
        private void PicTheme_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            addTGAtoDir("THEME.TGA", data.fn, 128, 16, themeFile.img_theme, picTheme);
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

            picCornerBg.BackColor = btnBg.BackColor;
            picCorner.BackColor = btnBoxBg.BackColor;
            picShadow.BackColor = btnBoxBg.BackColor;

            btnBoxBg.ForeColor = btnFont.BackColor;
            btnSelection.ForeColor = btnFont.BackColor;
        }
        private void saveThemeIni()
        {
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            StreamWriter sw = new StreamWriter(Path.Combine(data.fn, "theme.ini"));
            sw.WriteLine("[START]");
            sw.WriteLine("font=" + btnFont.BackColor.R + "," + btnFont.BackColor.G + "," + btnFont.BackColor.B);
            sw.WriteLine("bg=" + btnBg.BackColor.R + "," + btnBg.BackColor.G + "," + btnBg.BackColor.B);
            sw.WriteLine("selector=" + btnSelection.BackColor.R + "," + btnSelection.BackColor.G + "," + btnSelection.BackColor.B);
            sw.WriteLine("boxbg=" + btnBoxBg.BackColor.R + "," + btnBoxBg.BackColor.G + "," + btnBoxBg.BackColor.B);
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
            using (frmGoogleImages googler = new frmGoogleImages(query))
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
                        picBox.Image = (Bitmap)T;
                        picBox.Visible = true;
                    }
                }
            }
        }
        private void LstDir_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            itemData data = (itemData)lstDir.SelectedItems[0].Tag;
            string fn = data.fn;
            fn = fn.Replace("\\", "/");
            fn = fn.Substring(fn.IndexOf("/"), fn.Length - fn.IndexOf("/"));
            if (favs.Contains(fn))
                addToFavouritesToolStripMenuItem.Text = "Delete From Favourites";
            else
                addToFavouritesToolStripMenuItem.Text = "Add To Favourites";
            this.contextMenuGames.Show((Control)this.lstDir, new Point(e.X, e.Y));
        }
        private void RenameToolStripMenuItem_Click(object sender, EventArgs e)
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
        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove the selected game from your SD card?\n\n" + lstDir.SelectedItems[0].Text, "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;
            itemData data = (itemData)lstDir.SelectedItems[0].Tag;
            Directory.Delete(data.fn, true);
            lstDir.Items.Remove(lstDir.SelectedItems[0]);
            MessageBox.Show("Item deleted successfully", "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void TrimBracketsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to trim the brackets on the seleted directories?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            for (int i = 0; i < lstDir.SelectedItems.Count; i++)
            {
                itemData data = (itemData)lstDir.SelectedItems[0].Tag;
                // rename the folder taking the ID off
                string newDirName = data.fn;
                if (newDirName.LastIndexOf(" [") > 0)
                    newDirName = newDirName.Substring(0, data.fn.LastIndexOf(" ["));
                if (newDirName.LastIndexOf(" (") > 0)
                    newDirName = data.fn.Substring(0, data.fn.LastIndexOf(" ("));
                if(data.fn == newDirName)
                    continue;
                Directory.Move(data.fn, newDirName);
                data.fn = newDirName;
                data.imageId = -1;
                lstDir.SelectedItems[0].Tag = data;
                lstDir.SelectedItems[0].Text = Path.GetFileName(data.fn);
            }
            MessageBox.Show("Brackets have been removed", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void TrackVolume_ValueChanged(object sender, EventArgs e)
        {
            lblVolume.Text = trackVolume.Value.ToString();
        }
        private void BtnSaveOptions_Click(object sender, EventArgs e)
        {
            // save the otions file
            string path = txtDir.Text.Substring(0, txtDir.Text.IndexOf(@"\"));
            path = Path.Combine(path, "satiator-rings");
            if (!Directory.Exists(path))
            {
                MessageBox.Show("Satiator Rings is not installed on the selected SD card", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // update the options with the values from the form
            options[(int)optionsType.OPTIONS_AUTO_PATCH] = 0;
            if(chkAutoPatch.Checked)
                options[(int)optionsType.OPTIONS_AUTO_PATCH] = 1;
            options[(int)optionsType.OPTIONS_LIST_MODE] = comboOptionList.SelectedIndex;
            options[(int)optionsType.OPTIONS_LIST_CATEGORY] = comboOptionFilter.SelectedIndex;
            options[(int)optionsType.OPTIONS_SOUND_VOLUME] = trackVolume.Value;
            options[(int)optionsType.OPTIONS_DESC_CACHE] = 0;
            if (chkDescCache.Checked)
                options[(int)optionsType.OPTIONS_DESC_CACHE] = 1;


            // write the options file

            path = Path.Combine(path, "options.ini");
            if (File.Exists(path))
                File.Delete(path);

            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine("[START]");
            sw.WriteLine("autopatch=" + options[(int)optionsType.OPTIONS_AUTO_PATCH]);
            sw.WriteLine("listmode=" + options[(int)optionsType.OPTIONS_LIST_MODE]);
            sw.WriteLine("listcat=" + options[(int)optionsType.OPTIONS_LIST_CATEGORY]);
            sw.WriteLine("volume=" + options[(int)optionsType.OPTIONS_SOUND_VOLUME]);
            sw.WriteLine("desccache=" + options[(int)optionsType.OPTIONS_DESC_CACHE]);
            sw.WriteLine("[END]");
            sw.Close();

            MessageBox.Show("The options file was saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void LstFavs_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;
            this.contextMenuStripFavs.Show((Control)this.lstFavs, new Point(e.X, e.Y));
        }
        public void disablemenuUpdate()
        {
            menuFileUpdateToolStripMenuItem.Enabled = false;
        }
        private void addRemoveFav(string fn)
        {
            fn = fn.Replace("\\", "/");
            fn = fn.Substring(fn.IndexOf("/"), fn.Length - fn.IndexOf("/"));

            bool delete = false;
            if (favs.Contains(fn))
            {
                // removing, confirm
                if (MessageBox.Show("Are you sure you want to remove this item from your favourites?", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                    return;
                favs.Remove(fn);
                delete = true;
            }
            else
            {
                // just add it
                favs.Add(fn);
            }
            // save the new favs file
            string path = txtDir.Text.Substring(0, txtDir.Text.IndexOf(@"\"));
            path = Path.Combine(path, "satiator-rings");
            if (!Directory.Exists(path))
                return;
            path = Path.Combine(path, "favs.ini");
            if (File.Exists(path))
                File.Delete(path);
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine("[START]");
            for (int i = 0; i < favs.Count; i++)
                sw.WriteLine(favs[i]);
            sw.WriteLine("[END]");
            sw.Close();
            // refresh the favs list
            int index = -1;

            if (lstFavs.SelectedItems.Count > 0)
                index = lstFavs.SelectedItems[0].Index;
            if (delete)
            {
                index--;
                if (index < 0)
                    index = 0;
            }
            displayFavourites();
            lstFavs.SelectedItems.Clear();
            if ((lstFavs.Items.Count > 0) && (index != -1))
            {
                lstFavs.Items[index].Selected = true;
                lstFavs.Items[index].EnsureVisible();
            }

            // refresh the games list
            index = lstDir.SelectedItems[0].Index;
            listGames();
            lstDir.SelectedItems.Clear();
            if (lstDir.Items.Count > index)
            {
                lstDir.Items[index].Selected = true;
                lstDir.Items[index].EnsureVisible();
            }
            lstDir.SelectedItems[0].EnsureVisible();
        }
        private void AddToFavouritesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstDir.SelectedItems[0].Tag;
            string fn = data.fn;
            addRemoveFav(fn);
        }
        private void DeleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstFavs.SelectedItems[0].Tag;
            string fn = data.fn;
            addRemoveFav(fn);
        }
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            loadOptions();
            loadFavourites();
            listGames();
            listThemes();
        }
        private void LstFavs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFavs.SelectedItems.Count == 0)
            {
                btnAddImageFavs.Enabled = false;
                btnGoogleFavs.Enabled = false;
                picBoxFavs.Image = Properties.Resources.satiator_small;
                return;
            }
            itemData data = (itemData)lstFavs.SelectedItems[0].Tag;
            btnAddImageFavs.Enabled = Directory.Exists(data.fn);
            btnGoogleFavs.Enabled = Directory.Exists(data.fn);

            if (!Directory.Exists(data.fn) && !File.Exists(data.fn))
            {
                if(MessageBox.Show("The selected path does not exist. Do you want to remove it from your favourites?", "Path Does Not Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    addRemoveFav(data.fn);
            }

            string path = Path.Combine(data.fn, "BOX.TGA");
            if (!File.Exists(path))
            {
                picBoxFavs.Image = Properties.Resources.satiator_small;
                return;
            }
            TFAV = new TGA(path);
            picBoxFavs.Image = (Bitmap)TFAV;
        }
        private void BtnAddImageFavs_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstFavs.SelectedItems[0].Tag;
            addTGAtoDir("BOX.TGA", data.fn, -1, -1, TFAV, picBoxFavs);
        }
        private void BtnGoogleFavs_Click(object sender, EventArgs e)
        {
            frmGoogleImages.SearchQuery query = new frmGoogleImages.SearchQuery();
            query.Query = "saturn cover " + lstFavs.SelectedItems[0].Text;
            while (query.Query.LastIndexOf("(") > 0)
                query.Query = query.Query.Substring(0, query.Query.LastIndexOf("(") - 1);
            while (query.Query.LastIndexOf("[") > 0)
                query.Query = query.Query.Substring(0, query.Query.LastIndexOf("[") - 1);
            query.AdditionalVariables = query.Query;
            using (frmGoogleImages googler = new frmGoogleImages(query))
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
                            TFAV = (TGA)newbmp;
                        itemData data = (itemData)lstFavs.SelectedItems[0].Tag;
                        if (File.Exists(Path.Combine(data.fn, "BOX.TGA")))
                            File.Delete(Path.Combine(data.fn, "BOX.TGA"));
                        TFAV.Save(Path.Combine(data.fn, "BOX.TGA"));
                        picBoxFavs.Image = (Bitmap)TFAV;
                        picBoxFavs.Visible = true;
                    }
                }
            }
        }
        private void PicCorner_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            addTGAtoDir("CORNER.TGA", data.fn, 8, 8, themeFile.img_corner, picCorner);
        }
        private void PicShadow_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            addTGAtoDir("SHDW.TGA", data.fn, 80, 8, themeFile.img_shadow, picShadow);
        }

        private void PicCornerBg_Click(object sender, EventArgs e)
        {
            PicCorner_Click(picCorner, e);
        }
    }
}
    
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Compression;
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
        public string appVer = "5.1";

        static int SAVE_DATA_SLOTS = 20;

        public class itemData
        {
            public string fn;
            public int imageId;
        }
        public class ipBinData
        {
            public string gameId;
            public string gameVer;
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
            OPTIONS_SKIP_SPLASH,
            OPTIONS_PERGAME_SAVE,
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

            comboSaveSlot.Items.Add("Off");
            for(int i=0;i< SAVE_DATA_SLOTS; i++)
            {
                comboSaveSlot.Items.Add("Slot #0" + i); ;
                comboLoadedSaveSlot.Items.Add("Slot #0" + i);

                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = "Slot #0" + i;
                item.Tag = i;
                item.Click += SaveSlotItem_Click;
                saveDataToolStripMenuItem.DropDownItems.Add(item);
            }
            comboLoadedSaveSlot.SelectedIndex = 0;
        }

        private void SaveSlotItem_Click(object sender, EventArgs e)
        {

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

        private void listGamesDir(TreeNode node, string path)
        {
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
            if (node != null)
                node.Nodes.Clear();
            for (int j = 0; j < i; j++)
            {
                TreeNode item = new TreeNode();
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
                item.Text = Path.GetFileName(fn);
                item.Tag = data;

                if ((item.Text == "satiator-rings") || (item.Text == "satiator-saves") || (item.Text == "System Volume Information"))
                    continue;

                // check to see if the item is in favs
                fn = data.fn;
                fn = fn.Replace("\\", "/");
                fn = fn.Substring(fn.IndexOf("/"), fn.Length - fn.IndexOf("/"));
                if (favs.Contains(fn))
                {
                    item.ImageIndex = 0;
                    item.SelectedImageIndex = 0;
                }
                else
                {
                    item.ImageIndex = 2;
                    item.SelectedImageIndex = 2;
                }

                listGamesDir(item, data.fn);
                if (node != null)
                    node.Nodes.Add(item);
                else
                    treeDirs.Nodes.Add(item);
            }
        }

        private void listGames()
        {
            treeDirs.Nodes.Clear();
            if (!Directory.Exists(txtDir.Text))
            {
                MessageBox.Show("The selected directory does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            listGamesDir(null, txtDir.Text);
            if (treeDirs.Nodes.Count > 0)
            {
                treeDirs.SelectedNode = treeDirs.Nodes[0];
            }
        }

        private void listSavesDir(TreeNode node, string path)
        {
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
            if (node != null)
                node.Nodes.Clear();
            for (int j = 0; j < i; j++)
            {
                TreeNode item = new TreeNode();
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
                item.Text = Path.GetFileName(fn);
                if ((item.Text == "_BACKUP") || (item.Text.StartsWith("[SLOT") && item.Text.EndsWith("]")))
                    continue;
                item.Tag = data;
                item.ImageIndex = 2;
                item.SelectedImageIndex = 2;

                listSavesDir(item, data.fn);
                if (node != null)
                    node.Nodes.Add(item);
                else
                    treeSaveData.Nodes.Add(item);
            }


            dirs = Directory.GetFiles(path, "*.bup");
            objs = new dir[dirs.Count()];
            i = 0;
            foreach (string dir in dirs)
            {
                objs[i] = new dir();
                objs[i].path = dir;
                i++;
            }
            Array.Sort(objs, (x, y) => String.Compare(x.path, y.path));

            for (int j = 0; j < i; j++)
            {
                TreeNode item = new TreeNode();
                itemData data = new itemData();
                data.fn = objs[j].path;
                string fn = objs[j].path.Replace(txtDir.Text, "");
                if (fn.StartsWith("\\"))
                    fn = fn.Substring(1, fn.Length - 1);
                data.imageId = -1;
                if (fn.StartsWith("\\"))
                    fn = fn.Substring(1, fn.Length - 1);
                item.Text = Path.GetFileName(fn);
                item.Tag = data;
                item.ImageIndex = 2;
                item.SelectedImageIndex = 2;
                if (node != null)
                    node.Nodes.Add(item);
                else
                    treeSaveData.Nodes.Add(item);
            }
        }

        private void listSaves()
        {
            treeSaveData.Nodes.Clear();
            if (!Directory.Exists(txtDir.Text))
            {
                MessageBox.Show("The selected directory does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string path = txtDir.Text.Substring(0, txtDir.Text.IndexOf(@"\"));
            if (path.EndsWith(":"))
                path = path + "\\";
            path = Path.Combine(path, "satiator-saves");
            if (!Directory.Exists(path))
                 Directory.CreateDirectory(path);
            if(comboLoadedSaveSlot.SelectedIndex > 0)
            {
                path = Path.Combine(path, "[SLOT" + comboLoadedSaveSlot.SelectedIndex.ToString("00") + "]");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            listSavesDir(null, path);
            if (treeSaveData.Nodes.Count > 0)
            {
                treeSaveData.SelectedNode = treeSaveData.Nodes[0];
            }
        }

        private void listThemes()
        {
            string path = txtDir.Text.Substring(0, txtDir.Text.IndexOf(@"\"));
            if(path.EndsWith(":"))
                path = path + "\\";
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
            options[(int)optionsType.OPTIONS_SKIP_SPLASH] = 0;
            options[(int)optionsType.OPTIONS_PERGAME_SAVE] = 0;

            comboOptionFilter.Items.Clear();
            comboOptionFilter.Items.Add("Standard");
            comboOptionFilter.Items.Add("Favourites");
            comboOptionFilter.Items.Add("Recent History");
            comboOptionFilter.SelectedIndex = 0;

            comboOptionList.Items.Clear();
            comboOptionList.Items.Add("Text / Image");
            comboOptionList.Items.Add("Text Only");
            comboOptionList.SelectedIndex = 0;
            comboRegionPatch.SelectedIndex = 0;
            chkDescCache.Checked = false;
            chkSkipSplash.Checked = false;
            comboSaveSlot.SelectedIndex = 0;

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
                    if (oneline.StartsWith("skipsplash"))
                        options[(int)optionsType.OPTIONS_SKIP_SPLASH] = int.Parse(oneline.Substring("skipsplash=".Length, oneline.Length - "skipsplash=".Length));
                    if (oneline.StartsWith("pergamesave"))
                        options[(int)optionsType.OPTIONS_PERGAME_SAVE] = int.Parse(oneline.Substring("pergamesave=".Length, oneline.Length - "pergamesave=".Length));
                    oneline = sr.ReadLine();
                }
                sr.Close();
            }

            comboOptionFilter.SelectedIndex = options[(int)optionsType.OPTIONS_LIST_CATEGORY];
            comboOptionList.SelectedIndex = options[(int)optionsType.OPTIONS_LIST_MODE];
            comboRegionPatch.SelectedIndex = options[(int)optionsType.OPTIONS_AUTO_PATCH];
            if (options[(int)optionsType.OPTIONS_DESC_CACHE] == 1)
                chkDescCache.Checked = true;
            if (options[(int)optionsType.OPTIONS_SKIP_SPLASH] == 1)
                chkSkipSplash.Checked = true;
            if (options[(int)optionsType.OPTIONS_PERGAME_SAVE] > 10)
                options[(int)optionsType.OPTIONS_PERGAME_SAVE] = 10;
            comboSaveSlot.SelectedIndex = options[(int)optionsType.OPTIONS_PERGAME_SAVE];
            comboLoadedSaveSlot.SelectedIndex = options[(int)optionsType.OPTIONS_PERGAME_SAVE] - 1;
            trackVolume.Value = options[(int)optionsType.OPTIONS_SOUND_VOLUME];
            btnSaveOptions.Visible = false;
        }
        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            if (!checkForNoSave())
                return;

            btnBuild.Text = "Install Satiator Rings";
            btnAddImage.Enabled = false;
            btnGoogle.Enabled = false;
            txtDir.Text = "";
            treeDirs.Nodes.Clear();
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
                    listSaves();
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
            MessageBox.Show("The installation completed, make sure you are using v65 or higher of the official menu to enjoy Satiator Rings autoboot.\n\nFlash the ar_patched-satiator-rings.bin on the root of your SD card to use A+B+C+Start for reset.\n\nEnjoy :)", "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static void addTGAtoDir(string tgaName, string dir, int w, int h, TGA tga, PictureBox picBox, string sourceFile = "")
        {
            if(sourceFile == "")
            {
                OpenFileDialog fd = new OpenFileDialog();
                fd.Filter = "Image Files (*.png;*.jpg;*.bmp;*.gif)|*.png;*.jpg;*.bmp;*.gif";
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(Path.Combine(dir, tgaName)))
                    {
                        if (MessageBox.Show("An image already exists in this directory, do you want to overwrite it?", "Confirm Replace", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                            return;
                    }
                    sourceFile = fd.FileName;
                } else
                {
                    return;
                }
            }
            string destPath = Path.Combine(dir, tgaName);
            if (sourceFile.ToLower().EndsWith(".tga"))
            {
                if (File.Exists(destPath))
                    File.Delete(destPath);
                File.Copy(sourceFile, destPath);
                return;
            }
            using (Image img = Image.FromFile(sourceFile))
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
                if (File.Exists(destPath))
                    File.Delete(destPath);

                using (Bitmap original = new Bitmap("tmp.png"))
                using (Bitmap clone = new Bitmap(original))
                using (Bitmap newbmp = clone.Clone(new Rectangle(0, 0, clone.Width, clone.Height), PixelFormat.Format24bppRgb))
                    tga = (TGA)newbmp;
                tga.Save(destPath);
                if (picBox != null)
                {
                    picBox.Image = (Bitmap)tga;
                    picBox.Visible = true;
                }
            }
        }
        private void btnAddImage_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)treeDirs.SelectedNode.Tag;
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

        public static string getGameHashCheck(string dir)
        {
            string hash = "";
            // scan the directory and see if there is an iso or cue file
            string[] files = Directory.GetFiles(dir, "*.cue");
            if (files.Length == 0)
                files = Directory.GetFiles(dir, "*.iso");
            if (files.Length == 1)
            {
                if (files[0].ToLower().EndsWith(".cue"))
                {
                    // open the cue and get the first file from it
                    bool gotFile = false;
                    using (StreamReader sr = new StreamReader(files[0]))
                    {
                        string oneline = "";
                        while (true)
                        {
                            oneline = sr.ReadLine();
                            if (sr.EndOfStream)
                                break;
                            if (oneline.Contains("FILE"))
                            {
                                try
                                {
                                    oneline = oneline.Substring(oneline.IndexOf("\"") + 1, oneline.Length - (oneline.IndexOf("\"") + 1));
                                    if (oneline.StartsWith("\\"))
                                        oneline = oneline.Substring(1, oneline.Length - 1);
                                    oneline = oneline.Substring(0, oneline.IndexOf("\""));
                                    files[0] = Path.Combine(Path.GetDirectoryName(files[0]), oneline);
                                    gotFile = true;
                                }
                                catch
                                {

                                }
                                break;
                            }
                        }
                        sr.Close();
                    }
                    if (!gotFile)
                    {
                        MessageBox.Show("Cue file error, does not contain a FILE line!", "CUE Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return hash;
                    }
                }
                // read the binary file
                using (BinaryReader br = new BinaryReader(File.OpenRead(files[0])))
                {
                    int bytesRead = 0x1000;
                    byte[] bytes = br.ReadBytes(16);
                    string text = System.Text.Encoding.Default.GetString(bytes);

                    if (text != "SEGA SEGASATURN ")
                    {
                        bytes = br.ReadBytes(16);
                        bytesRead += 16;
                        text = System.Text.Encoding.Default.GetString(bytes);
                        if (text != "SEGA SEGASATURN ")
                        {
                            br.Close();
                            MessageBox.Show("File error, does not contain the valid SEGA SATURN header", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return hash;
                        }
                    }
                    
                    br.ReadBytes(0x1000 - 16); // skip the ip bin
                    long remainBytes = br.BaseStream.Length - bytesRead;
                    if (remainBytes < 0x50000)
                        bytes = br.ReadBytes((int)remainBytes);
                    else
                        bytes = br.ReadBytes(0x50000);
                    br.Close();
                    using (var md5 = System.Security.Cryptography.MD5.Create())
                    {
                        md5.TransformFinalBlock(bytes, 0, bytes.Length);
                        byte[] hashbytes = md5.Hash;

                        // Create a New Stringbuilder to collect the bytes
                        // And create a string.
                        StringBuilder sBuilder = new StringBuilder();

                        // Loop through each byte of the hashed data 
                        // And format each one as a hexadecimal string.
                        int j;
                        for (j = 0; j < hashbytes.Length; j++)
                            sBuilder.Append(hashbytes[j].ToString("x2"));
                        // Return the hexadecimal string.
                        hash = sBuilder.ToString();
                        return hash;
                    }
                }
            }
            return hash;
        }
        public static ipBinData loadGameIpBin(string dir)
        {
            ipBinData ipBin = new ipBinData();
            ipBin.gameId = "";
            ipBin.gameVer = "";
            // scan the directory and see if there is an iso or cue file
            string[] files = Directory.GetFiles(dir, "*.cue");
            if (files.Length == 0)
                files = Directory.GetFiles(dir, "*.iso");
            if (files.Length == 1)
            {
                if (files[0].ToLower().EndsWith(".cue"))
                {
                    // open the cue and get the first file from it
                    bool gotFile = false;
                    using (StreamReader sr = new StreamReader(files[0]))
                    {
                        string oneline = "";
                        while (true)
                        {
                            oneline = sr.ReadLine();
                            if (sr.EndOfStream)
                                break;
                            if (oneline.Contains("FILE"))
                            {
                                try
                                {
                                    oneline = oneline.Substring(oneline.IndexOf("\"") + 1, oneline.Length - (oneline.IndexOf("\"") + 1));
                                    if (oneline.StartsWith("\\"))
                                        oneline = oneline.Substring(1, oneline.Length - 1);
                                    oneline = oneline.Substring(0, oneline.IndexOf("\""));
                                    files[0] = Path.Combine(Path.GetDirectoryName(files[0]), oneline);
                                    gotFile = true;
                                }
                                catch
                                {

                                }
                                break;
                            }
                        }
                        sr.Close();
                    }
                    if (!gotFile)
                    {
                        MessageBox.Show("Cue file error, does not contain a FILE line!", "CUE Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return ipBin;
                    }
                }
                // read the binary file
                using (BinaryReader br = new BinaryReader(File.OpenRead(files[0])))
                {
                    byte[] bytes = br.ReadBytes(16);
                    string text = System.Text.Encoding.Default.GetString(bytes);

                    if (text != "SEGA SEGASATURN ")
                    {
                        bytes = br.ReadBytes(16);
                        text = System.Text.Encoding.Default.GetString(bytes);
                        if (text != "SEGA SEGASATURN ")
                        {
                            br.Close();
                            MessageBox.Show("File error, does not contain the valid SEGA SATURN header", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return ipBin;
                        }
                    }
                    br.ReadBytes(16); // skip another 16 bytes

                    bytes = br.ReadBytes(16);
                    text = System.Text.Encoding.Default.GetString(bytes);
                    br.Close();
                    int pos = text.LastIndexOf(" ");
                    if (pos == -1)
                        pos = text.Length - 6;
                    ipBin.gameId = text.Substring(0, pos).Trim();
                    ipBin.gameVer = text.Substring(pos, text.Length - pos).Trim();
                }
            }
            return ipBin;
        }
        private void loadGameInformation(string dir)
        {
            ipBinData ipBin = loadGameIpBin(dir);
            txtGameID.Text = ipBin.gameId;
            txtVersion.Text = ipBin.gameVer;
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
            {
                picBox.Refresh();
                treeDirs.Focus();
            } else if (tabControl1.SelectedTab == tabPageSaveData)
            {
                treeSaveData.Focus();
            }
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

            setShadowColour();

            // apply the theme to the gui
            txtThemeName.Text = Path.GetFileName(data.fn);
            btnFont.BackColor = Color.FromArgb(themeFile.font[0], themeFile.font[1], themeFile.font[2]);
            btnBg.BackColor = Color.FromArgb(themeFile.bg[0], themeFile.bg[1], themeFile.bg[2]);
            btnSelection.BackColor = Color.FromArgb(themeFile.selector[0], themeFile.selector[1], themeFile.selector[2]);
            btnBoxBg.BackColor = Color.FromArgb(themeFile.boxbg[0], themeFile.boxbg[1], themeFile.boxbg[2]);
            udpateBgColours();
        }
        private void setShadowColour()
        {
            byte[] colourData = themeFile.img_shadow.ImageOrColorMapArea.ImageData;
            for (int i = 0; i < colourData.Length; i += 4)
            {
                if ((colourData[i + 0] != 0) || (colourData[i + 1] != 0) || (colourData[i + 2] != 0) || (colourData[i + 3] != 0))
                {
                    // shadow
                    btnShadow.BackColor = Color.FromArgb(colourData[i + 3], colourData[i + 2], colourData[i + 1], colourData[i + 0]);
                    return;
                }
            }
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
            updateCornerSprite();
            updateShadowSprite();
        }

        private void updateCornerSprite()
        {
            Color col = btnBoxBg.BackColor;
            Color shdw = btnShadow.BackColor;
            Color bg = btnBg.BackColor;

            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;

            // export a new corner sprite
            File.WriteAllBytes(Path.Combine(data.fn, "CORNER.TGA"), Properties.Resources.CORNER);
            themeFile.img_corner = TGA.FromFile(Path.Combine(data.fn, "CORNER.TGA"));

            byte[] colourData = themeFile.img_corner.ImageOrColorMapArea.ImageData;

            for(int i = 0;i< colourData.Length;i+=3)
            {
                if ((i == 144) || (i == 171))
                {
                    // shadow
                    colourData[i + 0] = shdw.B;
                    colourData[i + 1] = shdw.G;
                    colourData[i + 2] = shdw.R;
                }
                else if (i == 168)
                {
                    // bg
                    colourData[i + 0] = bg.B;
                    colourData[i + 1] = bg.G;
                    colourData[i + 2] = bg.R;
                } else
                {
                    colourData[i + 0] = col.B;
                    colourData[i + 1] = col.G;
                    colourData[i + 2] = col.R;
                }
            }
            themeFile.img_corner.ImageOrColorMapArea.ImageData = colourData;
            themeFile.img_corner.Save(Path.Combine(data.fn, "CORNER.TGA"));
            picCorner.Image = (Bitmap)themeFile.img_corner;
        }
        private void updateShadowSprite()
        {
            Color shdw = btnShadow.BackColor;
            itemData data = (itemData)lstThemes.SelectedItems[0].Tag;
            // export a new shadow sprite
            File.WriteAllBytes(Path.Combine(data.fn, "SHDW.TGA"), Properties.Resources.SHDW);
            themeFile.img_shadow = TGA.FromFile(Path.Combine(data.fn, "SHDW.TGA"));
            byte[] colourData = themeFile.img_shadow.ImageOrColorMapArea.ImageData;
            for (int i = 0; i < colourData.Length; i += 4)
            {
                if ((colourData[i + 0] != 0) || (colourData[i + 1] != 0) || (colourData[i + 2] != 0) || (colourData[i + 3] != 0))
                {
                    // shadow
                    colourData[i + 0] = shdw.B;
                    colourData[i + 1] = shdw.G;
                    colourData[i + 2] = shdw.R;
                    colourData[i + 3] = shdw.A;
                }
            }
            themeFile.img_shadow.ImageOrColorMapArea.ImageData = colourData;
            themeFile.img_shadow.Save(Path.Combine(data.fn, "SHDW.TGA"));
            picShadow.Image = (Bitmap)themeFile.img_shadow;
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
            query.Query = "saturn cover " + treeDirs.SelectedNode.Text;
            while (query.Query.LastIndexOf("(") > 0)
                query.Query = query.Query.Substring(0, query.Query.LastIndexOf("(") - 1);
            while (query.Query.LastIndexOf("[") > 0)
                query.Query = query.Query.Substring(0, query.Query.LastIndexOf("[") - 1);
            query.AdditionalVariables = txtGameID.Text;
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
                        itemData data = (itemData)treeDirs.SelectedNode.Tag;
                        if (File.Exists(Path.Combine(data.fn, "BOX.TGA")))
                            File.Delete(Path.Combine(data.fn, "BOX.TGA"));
                        T.Save(Path.Combine(data.fn, "BOX.TGA"));
                        picBox.Image = (Bitmap)T;
                        picBox.Visible = true;
                    }
                }
            }
        }
        private void RenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmRename f = new frmRename(treeDirs.SelectedNode.Text))
            {
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.Cancel)
                    return;
                itemData data = (itemData)treeDirs.SelectedNode.Tag;
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
                treeDirs.SelectedNode.Tag = data;
                treeDirs.SelectedNode.Text = f.newname;
            }
        }
        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove the selected game from your SD card?\n\n" + treeDirs.SelectedNode.Text, "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;
            itemData data = (itemData)treeDirs.SelectedNode.Tag;
            Directory.Delete(data.fn, true);
            if(treeDirs.SelectedNode.Parent == null)
                treeDirs.Nodes.Remove(treeDirs.SelectedNode);
            else
                treeDirs.SelectedNode.Parent.Nodes.Remove(treeDirs.SelectedNode);
            MessageBox.Show("Item deleted successfully", "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void TrimBracketsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to trim the brackets on the seleted directory?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            itemData data = (itemData)treeDirs.SelectedNode.Tag;
            // rename the folder taking the ID off
            string newDirName = data.fn;
            if (newDirName.LastIndexOf(" [") > 0)
                newDirName = newDirName.Substring(0, data.fn.LastIndexOf(" ["));
            if (newDirName.LastIndexOf(" (") > 0)
                newDirName = data.fn.Substring(0, data.fn.LastIndexOf(" ("));
            if (data.fn == newDirName)
                return;
            Directory.Move(data.fn, newDirName);
            data.fn = newDirName;
            data.imageId = -1;
            treeDirs.SelectedNode.Tag = data;
            treeDirs.SelectedNode.Text = Path.GetFileName(data.fn);
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
            options[(int)optionsType.OPTIONS_AUTO_PATCH] = comboRegionPatch.SelectedIndex;
            options[(int)optionsType.OPTIONS_LIST_MODE] = comboOptionList.SelectedIndex;
            options[(int)optionsType.OPTIONS_LIST_CATEGORY] = comboOptionFilter.SelectedIndex;
            options[(int)optionsType.OPTIONS_SOUND_VOLUME] = trackVolume.Value;
            options[(int)optionsType.OPTIONS_DESC_CACHE] = 0;
            if (chkDescCache.Checked)
                options[(int)optionsType.OPTIONS_DESC_CACHE] = 1;
            options[(int)optionsType.OPTIONS_SKIP_SPLASH] = 0;
            if (chkSkipSplash.Checked)
                options[(int)optionsType.OPTIONS_SKIP_SPLASH] = 1;
            options[(int)optionsType.OPTIONS_PERGAME_SAVE] = comboSaveSlot.SelectedIndex;


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
            sw.WriteLine("skipsplash=" + options[(int)optionsType.OPTIONS_SKIP_SPLASH]);
            sw.WriteLine("pergamesave=" + options[(int)optionsType.OPTIONS_PERGAME_SAVE]);
            sw.WriteLine("[END]");
            sw.Close();

            btnSaveOptions.Visible = false;
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
            TreeNode node = treeDirs.SelectedNode;
            listGames();
            treeDirs.SelectedNode = node;
        }
        private void AddToFavouritesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)treeDirs.SelectedNode.Tag;
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
            if (!checkForNoSave())
                return;

            loadOptions();
            loadFavourites();
            listGames();
            listThemes();
            listSaves();
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

        private List<itemData> addDataToDownloadQueue(TreeNode node, List<itemData> itemsData, bool ifMissing)
        {
            dynamic obj = node;
            if (node == null)
                obj = treeDirs;

            for (int i = 0; i < obj.Nodes.Count; i++)
            {
                itemData item = (itemData)obj.Nodes[i].Tag;
                if (!ifMissing || !File.Exists(Path.Combine(item.fn, "BOX.TGA")))
                    itemsData.Add(item);
                itemsData = addDataToDownloadQueue(obj.Nodes[i], itemsData, ifMissing);
            }
            return itemsData;
        }

        private void ConfigureScrapersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmScrapers frm = new frmScrapers();
            frm.ShowDialog();
        }

        private void TreeDirs_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            itemData data = (itemData)treeDirs.SelectedNode.Tag;
            string fn = data.fn;
            fn = fn.Replace("\\", "/");
            fn = fn.Substring(fn.IndexOf("/"), fn.Length - fn.IndexOf("/"));
            if (favs.Contains(fn))
                addToFavouritesToolStripMenuItem.Text = "Delete From Favourites";
            else
                addToFavouritesToolStripMenuItem.Text = "Add To Favourites";



            // look to see if save data exists for this id
            foreach (ToolStripMenuItem item in saveDataToolStripMenuItem.DropDownItems)
                item.DropDownItems.Clear();
            if (txtGameID.Text == "")
                return;
            for (int i = 0; i < SAVE_DATA_SLOTS; i++)
            {
                string path = txtDir.Text.Substring(0, txtDir.Text.IndexOf(@"\"));
                if (path.EndsWith(":"))
                    path = path + "\\";
                path = Path.Combine(path, "satiator-saves");
                if (i > 0)
                    path = Path.Combine(path, "[SLOT" + i.ToString("00") + "]");
                path = Path.Combine(path, txtGameID.Text);
                ToolStripMenuItem item = (ToolStripMenuItem)saveDataToolStripMenuItem.DropDownItems[i];
                if (Directory.Exists(path))
                {

                    ToolStripMenuItem createItem = new ToolStripMenuItem();
                    createItem.Text = "Add BUP Data";
                    createItem.Tag = i;
                    createItem.Click += AddSaveDataItem_Click;
                    createItem.Image = Properties.Resources.icon_add;
                    item.DropDownItems.Add(createItem);

                    ToolStripMenuItem deleteItem = new ToolStripMenuItem();
                    deleteItem.Text = "Delete Data";
                    deleteItem.Tag = i;
                    deleteItem.Click += DeleteSaveDataItem_Click;
                    deleteItem.Image = Properties.Resources.trash;
                    item.DropDownItems.Add(deleteItem);

                    ToolStripMenuItem modifyItem = new ToolStripMenuItem();
                    modifyItem.Text = "Modify Data";
                    modifyItem.Tag = i;
                    modifyItem.Click += ModifySaveDataItem_Click;
                    modifyItem.Image = Properties.Resources.item_rename;
                    item.DropDownItems.Add(modifyItem);

                    item.Image = Properties.Resources.success;
                }
                else
                {
                    ToolStripMenuItem createItem = new ToolStripMenuItem();
                    createItem.Text = "Create Data";
                    createItem.Tag = i;
                    createItem.Click += CreateSaveDataItem_Click;
                    createItem.Image = Properties.Resources.icon_add;
                    item.DropDownItems.Add(createItem);

                    item.Image = null;
                }
            }

            this.contextMenuGames.Show((Control)this.treeDirs, new Point(e.X, e.Y));
        }

        private void AddSaveDataItem_Click(object sender, EventArgs e)
        {
            int slot = int.Parse(((ToolStripMenuItem)sender).Tag.ToString());
            if (comboLoadedSaveSlot.SelectedIndex != slot)
                comboLoadedSaveSlot.SelectedIndex = slot;
            TreeNode t = findSaveDataTreeNode(txtGameID.Text);
            if (t == null)
            {
                MessageBox.Show("Failed to find the tree node for this item", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            treeSaveData.SelectedNode = t;
            AddFileToolStripMenuItem_Click(t, e);
        }

        private void TreeDirs_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeDirs.SelectedNode == null)
            {
                btnAddImage.Enabled = false;
                btnGoogle.Enabled = false;
                picBox.Image = Properties.Resources.satiator_small;
                return;
            }
            btnAddImage.Enabled = true;
            btnGoogle.Enabled = true;
            itemData data = (itemData)treeDirs.SelectedNode.Tag;

            string path = Path.Combine(data.fn, "BOX.TGA");
            if (!File.Exists(path))
            {
                picBox.Image = Properties.Resources.satiator_small;
                loadGameInformation(data.fn);
                return;
            }
            T = new TGA(path);
            picBox.Image = (Bitmap)T;
            loadGameInformation(data.fn);
            TreeNode t = findSaveDataTreeNode(txtGameID.Text);
            if (t != null)
                treeSaveData.SelectedNode = t;
        }

        private List<itemData> addDataToExportQueue(TreeNode node, List<itemData> itemsData)
        {
            dynamic obj = node;
            if (node == null)
                obj = treeDirs;
            for (int i = 0; i < obj.Nodes.Count; i++)
            {
                itemData item = (itemData)obj.Nodes[i].Tag;
                string fn = Path.Combine(item.fn, "BOX.TGA");
                if (File.Exists(fn))
                {
                    itemsData.Add(item);
                }
                itemsData = addDataToExportQueue(obj.Nodes[i], itemsData);
            }
            return itemsData;
        }

        private void ToolStripDropDownButton3_Click(object sender, EventArgs e)
        {

            if (treeDirs.Nodes.Count == 0)
            {
                MessageBox.Show("There are no games loaded", "No Games Loaded", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "Zip Files (*.zip)|*.zip";
            sd.FileName = "Satiator Rings Image Pack.zip";
            if (sd.ShowDialog() == DialogResult.OK)
            {
                if(!sd.FileName.EndsWith(".zip"))
                {
                    sd.FileName += ".zip";
                }

                string dir = Path.Combine("data/ziptemp");
                if (Directory.Exists(dir))
                {
                    Directory.Move(dir, dir + "2");
                    System.Threading.Thread.Sleep(500);
                    Directory.Delete(dir + "2", true);
                }
                Directory.CreateDirectory(dir);

                enableForm(false);

                List<itemData> itemsData = new List<itemData>();
                itemsData = addDataToExportQueue(null, itemsData);
                using (frmBoxartExport frm = new frmBoxartExport(itemsData, dir))
                {
                    frm.ShowDialog();
                    if(frm.DialogResult == DialogResult.OK)
                    {
                        if (File.Exists(sd.FileName))
                            File.Delete(sd.FileName);
                        ZipFile.CreateFromDirectory(dir, sd.FileName);
                    }
                    Directory.Delete(dir, true);
                    MessageBox.Show("The image pack was created successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                enableForm(true);
                updateProgressLabel("ready...");
            }
            else
            {
                return;
            }
        }

        private void AllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeDirs.Nodes.Count == 0)
            {
                MessageBox.Show("There are no games loaded", "No Games Loaded", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MessageBox.Show("Do you want to update all boxarts from the scrapers now?\n\nWarning - Existing images will be overwritten.", "Confirm Boxart Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            List<itemData> itemsData = new List<itemData>();
            itemsData = addDataToDownloadQueue(null, itemsData, false);
            if (itemsData.Count == 0)
            {
                MessageBox.Show("There are no folders with recogniesd game ID's", "No Game ID's", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            enableForm(false);
            frmBoxartUpdate frm = new frmBoxartUpdate(itemsData);
            frm.ShowDialog();
            enableForm(true);
            updateProgressLabel("ready...");
        }

        private void MissingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeDirs.Nodes.Count == 0)
            {
                MessageBox.Show("There are no games loaded", "No Games Loaded", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MessageBox.Show("Do you want to update all boxarts from the scrapers now?\n\nWarning - Existing images will be overwritten.", "Confirm Boxart Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            List<itemData> itemsData = new List<itemData>();
            itemsData = addDataToDownloadQueue(null, itemsData, true);
            if (itemsData.Count == 0)
            {
                MessageBox.Show("There are no missing boxarts", "No Missing Boxarts", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            enableForm(false);
            frmBoxartUpdate frm = new frmBoxartUpdate(itemsData);
            frm.ShowDialog();
            enableForm(true);
            updateProgressLabel("ready...");
        }

        private void treeSaveData_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // reset
            txtBupFn.Text = "";
            txtBupComment.Text = "";
            txtBupLang.Text = "";
            txtBupDate.Text = "";
            txtBupDataSize.Text = "";
            grupBUPData.Visible = false;
            if (treeSaveData.SelectedNode == null)
            {
                return;
            }
            itemData data = (itemData)treeSaveData.SelectedNode.Tag;
            TreeNode t = (TreeNode)treeSaveData.SelectedNode;
            t.EnsureVisible();
            if(data.fn.ToLower().EndsWith(".bup"))
            {
                bupData b = bupProcessor.parseFile(data.fn);
                if(b == null)
                {
                    txtBupFn.Text = "BUP ERROR";
                } else
                {
                    txtBupFn.Text = b.name;
                    txtBupComment.Text = b.comment;
                    txtBupLang.Text = bupProcessor.languageIdToString(b.lang);
                    txtBupDate.Text = bupProcessor.dateToString(b.date).ToString("yyyy-MM-dd hh:mm");
                    txtBupDataSize.Text = b.dataSize.ToString();
                    grupBUPData.Visible = true;
                }
            }
        }

        private void treeSaveData_MouseClick(object sender, MouseEventArgs e)
        {
            if((e.Button != MouseButtons.Right) || (treeSaveData.SelectedNode == null))
            {
                return;
            }
            treeSaveData.SelectedNode = treeSaveData.GetNodeAt(e.X, e.Y);
            ctxSaveData.Show(treeSaveData, new Point(e.X, e.Y));
        }

        private void DeleteToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)treeSaveData.SelectedNode.Tag;
            if (data.fn.ToLower().EndsWith(".bup"))
            {
                if (MessageBox.Show("Are you sure you want to delete this save data file?", "Confirm Delete BUP File", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                    return;
                File.Delete(data.fn);
            } else
            {
                if (MessageBox.Show("Are you sure you want to delete this save data directory?", "Confirm Delete Save Directory", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                    return;
                Directory.Delete(data.fn, true);
            }
            treeSaveData.Nodes.Remove(treeSaveData.SelectedNode);
            MessageBox.Show("Save Data Deleted", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AddFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            itemData data = (itemData)treeSaveData.SelectedNode.Tag;
            string dir = data.fn;
            TreeNode t = null;
            if (data.fn.ToLower().EndsWith(".bup"))
            {
                t = treeSaveData.SelectedNode;
                data = (itemData)t.Parent.Tag;
            }


            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Sega Saturn Save Files (*.bup)|*.bup";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                // validate bup

                bupData b = bupProcessor.parseFile(fd.FileName);
                if(b == null)
                {
                    MessageBox.Show("Invalid BUP file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                // verify there is enough room in the directory
                int dirSize = 0;
                string[] files = Directory.GetFiles(data.fn, "*.bup");
                if(files != null)
                {
                    
                    foreach (string file in files)
                    {
                        bupData checkbup = bupProcessor.parseFile(file);
                        if(checkbup != null)
                        {
                            dirSize += checkbup.dataSize;
                        }
                    }
                }
                if(dirSize + b.dataSize >= 32 * 1024)
                {
                    MessageBox.Show("Save directory is full, please make some space first or use another slot.", "Save Directory Full", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }


                string dest = Path.Combine(data.fn, b.name.Replace("\0","") + ".BUP");
                if (File.Exists(dest))
                {
                    if (MessageBox.Show("A file with the name '" + b.name + ".BUP' already exists in this directory, do you want to overwrite it?", "Confirm Replace", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;
                }
                File.Copy(fd.FileName, dest);
                listSaves();

                string id = data.fn.Substring(data.fn.LastIndexOf('\\') + 1, data.fn.Length - (data.fn.LastIndexOf('\\') + 1));
                t = findSaveDataTreeNode(id);
                if(t != null)
                {
                    t.Expand();
                    treeSaveData.SelectedNode = t;
                }
            }
            else
            {
                return;
            }
        }

        public TreeNode findSaveDataTreeNode(string id)
        {
            if (id.Trim() == "")
                return null;
            foreach (TreeNode t in treeSaveData.Nodes)
            {
                itemData data = (itemData)t.Tag;
                if (data.fn.EndsWith(id))
                {
                    return t;
                }
            }
            return null;
        }

        private void ModifySaveDataItem_Click(object sender, EventArgs e)
        {
            int slot = int.Parse(((ToolStripMenuItem)sender).Tag.ToString());
            if(comboLoadedSaveSlot.SelectedIndex != slot)
                comboLoadedSaveSlot.SelectedIndex = slot;
            TreeNode t = findSaveDataTreeNode(txtGameID.Text);
            if (t == null)
            {
                MessageBox.Show("Failed to find the tree node for this item", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            treeSaveData.SelectedNode = t;
            tabControl1.SelectedTab = tabPageSaveData;
        }

        private void CreateSaveDataItem_Click(object sender, EventArgs e)
        {
            int slot = int.Parse(((ToolStripMenuItem)sender).Tag.ToString());
            if (MessageBox.Show("Are you sure you want to create a save folder for this game in slot #" + slot + "?", "Confirm Create Save Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            if (!Directory.Exists(txtDir.Text))
            {
                MessageBox.Show("The selected directory does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string path = txtDir.Text.Substring(0, txtDir.Text.IndexOf(@"\"));
            if (path.EndsWith(":"))
                path = path + "\\";
            path = Path.Combine(path, "satiator-saves");
            if(slot > 0)
                path = Path.Combine(path, "[SLOT"+ slot.ToString("00") + "]");
            path = Path.Combine(path, txtGameID.Text);
            if (Directory.Exists(path))
            {
                MessageBox.Show("A save directory already exists for this item", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to create the save directory for this item\n\n" + ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            listSaves();
            ModifySaveDataItem_Click(sender, e);
            AddFileToolStripMenuItem_Click(sender, e);
        }

        private void DeleteSaveDataItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this save data file?", "Confirm Delete BUP File", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                    return;
            int slot = int.Parse(((ToolStripMenuItem)sender).Tag.ToString());
            if (comboLoadedSaveSlot.SelectedIndex != slot)
                comboLoadedSaveSlot.SelectedIndex = slot;
            TreeNode t = findSaveDataTreeNode(txtGameID.Text);
            if (t == null)
            {
                MessageBox.Show("Failed to find the tree node for this item", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            treeSaveData.SelectedNode = t;

            string path = txtDir.Text.Substring(0, txtDir.Text.IndexOf(@"\"));
            if (path.EndsWith(":"))
                path = path + "\\";
            path = Path.Combine(path, "satiator-saves");
            if (slot > 0)
                path = Path.Combine(path, "[SLOT" + slot.ToString("00") + "]");
            path = Path.Combine(path, txtGameID.Text);

            Directory.Delete(path, true);
            listSaves();
            MessageBox.Show("Save Data Deleted", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void ComboOptionFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSaveOptions.Visible = true;
        }

        private void ComboOptionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSaveOptions.Visible = true;
        }

        private void ComboSaveSlot_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSaveOptions.Visible = true;
        }

        private void TrackVolume_Scroll(object sender, EventArgs e)
        {
            btnSaveOptions.Visible = true;
        }

        private void ChkAutoPatch_CheckedChanged(object sender, EventArgs e)
        {
            btnSaveOptions.Visible = true;
        }

        private void ChkDescCache_CheckedChanged(object sender, EventArgs e)
        {
            btnSaveOptions.Visible = true;
        }

        private void ChkSkipSplash_CheckedChanged(object sender, EventArgs e)
        {
            btnSaveOptions.Visible = true;
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!checkForNoSave())
                e.Cancel = true;
        }

        private bool checkForNoSave()
        {
            if (btnSaveOptions.Visible)
            {
                if (MessageBox.Show("Options have been changed but have not been saved, are you sure you want to continue?", "Confirm Lose Changes", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                    return false;
            }
            return true;
        }

        private void ComboLoadedSaveSlot_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtDir.Text != "")
            {
                listSaves();

                if (tabControl1.SelectedTab == tabPageSaveData)
                {
                    TreeNode t = findSaveDataTreeNode(txtGameID.Text);
                    if (t != null)
                        treeSaveData.Focus();
                }
            }
        }
    }
}
    
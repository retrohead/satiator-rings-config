using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace SatiatorRingsConfig
{
    public partial class frmGoogleImages : Form
    {
        public Image result;
        List<string> downloadedUrls = new List<string>();

        public struct SearchQuery
        {
            public string Query;
            public string AdditionalVariables;
        }
        SearchQuery currentQuery;
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
        // Thanks Hackchi2CE
        public string[] GetImageUrls(string query, string additionalVariables = "")
        {
            updateProgressLabel("initialising");
            var url = string.Format("https://www.google.com/search?q={0}&source=lnms&tbm=isch{1}", HttpUtility.UrlEncode(query), additionalVariables.Length > 0 ? $"&{additionalVariables}" : "");
            var request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            (request as HttpWebRequest).UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
            request.Timeout = 10000;
            var response = request.GetResponse();
            updateProgressLabel("got response");
            Stream dataStream = response.GetResponseStream();
            updateProgressLabel("reading stream");
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            response.Close();
            //Trace.WriteLine("Web response: " + responseFromServer);

            var urls = new List<string>();

            string search = @"\""ou\""\:\""(?<url>.+?)\""";
            MatchCollection matches = Regex.Matches(responseFromServer, search);
            int i = 0;
            foreach (Match match in matches)
            {
                updateProgressLabel("parsing url list 1");
                updateProgress(i, matches.Count);
                urls.Add(HttpUtility.UrlDecode(match.Groups[1].Value.Replace("\\u00", "%")));
                i++;
            }

            // For some reason Google returns different data for different users (IPs?)
            // This is an alternative method
            search = @"imgurl=(.*?)&";
            matches = Regex.Matches(responseFromServer, search);
            i = 0;
            foreach (Match match in matches)
            {
                // Not sure about it.
                updateProgressLabel("parsing url list 2");
                updateProgress(i, matches.Count);
                urls.Add(HttpUtility.UrlDecode(match.Groups[1].Value.Replace("\\u00", "%")));
                i++;
            }

            // For some reason Google returns different data for different users (IPs?)
            // This is alternative method #2
            search = "\\]\\n?,\\[\"([^\"]+)\",\\d+,\\d+]";
            matches = Regex.Matches(responseFromServer, search);
            i = 0;
            foreach (Match match in matches)
            {
                // Not sure about it.
                updateProgressLabel("parsing url list 3");
                updateProgress(i, matches.Count);
                urls.Add(HttpUtility.UrlDecode(match.Groups[1].Value.Replace("\\u00", "%")));
                i++;
            }

            return urls.ToArray();
        }
        public static Image DownloadImage(string url)
        {
            var request = HttpWebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 5000;
            ((HttpWebRequest)request).UserAgent =
                "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.1.4) Gecko/20070515 Firefox/2.0.0.4";
            ((HttpWebRequest)request).KeepAlive = false;
            var response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            var image = Image.FromStream(dataStream);
            dataStream.Dispose();
            response.Close();
            return image;
        }

        public frmGoogleImages(SearchQuery query)
        {
            currentQuery = query;
            this.DialogResult = DialogResult.Cancel;
            InitializeComponent();
        }

        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            DialogResult = DialogResult.OK;
            result = listView1.SelectedItems[0].Tag as Image;
            Close();
        }

        protected void ShowImage(Image image)
        {
            try
            {
                if (this.Disposing) return;
                if (InvokeRequired)
                {
                    Invoke(new Action<Image>(ShowImage), new object[] { image });
                    return;
                }
                int i = imageList1.Images.Count;
                const int side = 256;
                var imageRect = new Bitmap(side, side, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                var gr = Graphics.FromImage(imageRect);
                gr.Clear(Color.White);
                if (image.Height > image.Width)
                    gr.DrawImage(image, new Rectangle((side - side * image.Width / image.Height) / 2, 0, side * image.Width / image.Height, side),
                        new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                else
                    gr.DrawImage(image, new Rectangle(0, (side - side * image.Height / image.Width) / 2, side, side * image.Height / image.Width),
                        new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                gr.Flush();
                imageList1.Images.Add(imageRect);
                var item = new ListViewItem(image.Width + "x" + image.Height);
                item.ImageIndex = i;
                item.Tag = image;
                listView1.Items.Add(item);
            }
            catch { }
        }
        private void BgWorkGetImages_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var urls = GetImageUrls(currentQuery.Query, currentQuery.AdditionalVariables);
                int i = 0;
                foreach (var url in urls)
                {
                    try
                    {
                        if (!downloadedUrls.Contains(url))
                        {
                            updateProgressLabel("downloading image " + (i + 1) + " of " + urls.Count());
                            updateProgress(i, urls.Count());
                            downloadedUrls.Add(url);
                            Trace.WriteLine("Downloading image: " + url);
                            var image = DownloadImage(url);
                            if (!bgWorkGetImages.CancellationPending)
                                ShowImage(image);
                            i++;
                        }
                    }
                    catch { }
                }
            }
            catch { }
        }

        private void BgWorkGetImages_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                return;
            if (Visible)
            {
                updateProgressLabel("completed");
                updateProgress(1, 1);
            }
        }

        private void FrmGoogleImages_Shown(object sender, EventArgs e)
        {
            downloadedUrls.Clear();
            imageList1.Images.Clear();
            listView1.Items.Clear();
            bgWorkGetImages.RunWorkerAsync();
        }

        private void FrmGoogleImages_Resize(object sender, EventArgs e)
        {
            listView1.Height = Height - 61;
        }

        private void FrmGoogleImages_FormClosing(object sender, FormClosingEventArgs e)
        {
            bgWorkGetImages.CancelAsync();
        }
    }
}

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Compression;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
namespace SatiatorRingsConfig
{
    public class update
    {
        private static frmMain mainFrm;
        private static BackgroundWorker bgwrk;
        private static BackgroundWorker bgwrkUpdate;
        public static bool allowDownload = true;
        public static byte[] downloadedData = new byte[512000];
        public static string downloadedDataName;

        public static dynamic completeFunctionObj;
        public static completeDelegate completeFunctionDelegate;

        public static dynamic updateCompleteFunctionObj;
        public static completeDelegate updateCompleteFunctionDelegate;

        private static bool success = false;
        private static bool updatesuccess = false;
        public class updateCSVInfo
        {
            public string fn;
            public string ver;
        }

        public delegate void completeDelegate(bool success);
        public static void checkForUpdates(frmMain mainWin, dynamic _completeFunctionObj, completeDelegate _completeFunctionDelegate)
        {
            mainFrm = mainWin;
            completeFunctionObj = _completeFunctionObj;
            completeFunctionDelegate = _completeFunctionDelegate;
            success = false;

            if (bgwrk == null)
            {
                bgwrk = new BackgroundWorker();
                bgwrk.DoWork += bgwrk_DoWork;
                bgwrk.RunWorkerCompleted += bgwrk_RunWorkerCompleted;
            }
            mainFrm.enableForm(false);
            mainFrm.updateProgressLabel("checking for updates");
            bgwrk.RunWorkerAsync();
        }

        private static void bgwrk_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\data"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\data");
                mainFrm.firstInstall = true;
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\data\\temp"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\data\\temp");
                mainFrm.firstInstall = true;
            }
            if (!downloadRequiredFile("SatiatorRingsConfigUpdater.exe", "The application will not start without this file.", 6656L))
            {
                MessageBox.Show("The application will now close", "Application Closing", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                mainFrm.BeginInvoke(new voidDelegate(() =>
                {
                    mainFrm.Close();
                }));
            }
        }

        private static void bgwrk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!mainFrm.firstInstall)
            {
                checkForUpdate(updateTypes.application, false, mainFrm, new completeDelegate(appUpdate_Complete));
            }
            else
            {
                mainFrm.enableForm(true);
                completeFunctionObj.BeginInvoke(completeFunctionDelegate, success);
            }
        }

        private static void appUpdate_Complete(bool updating)
        {
            if (!updating)
            {
                checkForUpdate(updateTypes.menu, false, mainFrm, new completeDelegate(menuUpdate_Complete));
            }
        }

        private static void menuUpdate_Complete(bool success)
        {
            mainFrm.enableForm(true);
            completeFunctionObj.BeginInvoke(completeFunctionDelegate, success);
        }

        public enum updateTypes
        {
            application,
            menu
        }

        private static updateTypes updateType = updateTypes.application;
        private static bool updateDownloadNew;
        public static void checkForUpdate(updateTypes type, bool download, dynamic _completeFunctionObj, completeDelegate _completeFunctionDelegate)
        {
            updateType = type;
            updateCompleteFunctionObj = _completeFunctionObj;
            updateCompleteFunctionDelegate = _completeFunctionDelegate;
            updateDownloadNew = download;
            if (bgwrkUpdate == null)
            {
                bgwrkUpdate = new BackgroundWorker();
                bgwrkUpdate.DoWork += bgwrkUpdate_DoWork;
                bgwrkUpdate.RunWorkerCompleted += bgwrkUpdate_RunWorkerCompleted;
            }
            bgwrkUpdate.RunWorkerAsync();
        }

        private static void bgwrkUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            updatesuccess = false;
            switch (updateType)
            {
                case updateTypes.application:
                    updatesuccess = checkAppUpdate(updateDownloadNew);
                    break;
                case updateTypes.menu:
                    updatesuccess = checkMenuUpdate(updateDownloadNew);
                    break;
            }
        }

        private static void bgwrkUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            updateCompleteFunctionObj.BeginInvoke(updateCompleteFunctionDelegate, updatesuccess);
        }

        private static bool downloadRequiredFile(string fn, string reason, long byteSize)
        {
            bool flag1 = true;
            bool flag2 = false;
            while (flag1)
            {
                if (!System.IO.File.Exists(Directory.GetCurrentDirectory() + "\\" + fn))
                {
                    if (MessageBox.Show(Path.GetFileName(fn) + " is a required file which is missing, corrupt or out of date.\n\nDo you want to download it now?\n\n" + reason, "Required File Missing or Corrupt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (downloadFile("http://files-ds-scene.net/retrohead/satiator/releases/" + Path.GetFileName(fn), "data\\temp\\", Path.GetFileName(fn)))
                        {
                            FileStream fs = System.IO.File.OpenRead("data\\temp\\" + Path.GetFileName(fn));
                            long downloadSize = fs.Length;
                            fs.Close();
                            if (downloadSize != byteSize)
                            {
                                MessageBox.Show(Path.GetFileName(fn) + " which was downloaded appears to be corrupt, please try again!", "Download Failure", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            }
                            else
                            {
                                System.IO.File.Copy("data\\temp\\" + Path.GetFileName(fn), fn);
                                MessageBox.Show(Path.GetFileName(fn) + " downloaded successfully.", "Download Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                flag1 = false;
                                flag2 = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show(fn + " failed to download, please check your internet connection\r\nor the site may be down!", "Download Failure", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }
                        if (flag1 && MessageBox.Show("Do you want to retry the download now?", "Try Again?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            flag1 = false;
                    }
                    else
                        flag1 = false;
                }
                else
                {
                    FileStream fileStream = System.IO.File.OpenRead(Directory.GetCurrentDirectory() + "\\" + fn);
                    long length = fileStream.Length;
                    fileStream.Close();
                    if (length != byteSize)
                    {
                        System.IO.File.Delete(Directory.GetCurrentDirectory() + "\\" + fn);
                    }
                    else
                    {
                        flag1 = false;
                        flag2 = true;
                    }
                }
            }
            return flag2;
        }

        private static bool checkAppUpdate(bool download)
        {
            string newVersion = "";
            string str1 = "latest_version.txt";
            if (downloadFile("http://files-ds-scene.net/retrohead/satiator/releases/" + str1, "data\\temp\\", "Update Check"))
            {
                try
                {
                    using (StreamReader streamReader = new StreamReader(new FileStream("data\\temp\\" + str1, FileMode.Open, FileAccess.Read)))
                    {
                        string str2;
                        if ((str2 = streamReader.ReadLine()) != null)
                            newVersion = str2;
                        streamReader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "checkAppUpdate failed to parse new info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (mainFrm.appVer != newVersion)
                {
                    if (download)
                    {
                        updateInfoForm updateViewer = new updateInfoForm();
                        if (!updateViewer.formSetup(newVersion))
                        {
                            return false;
                        }

                        try
                        {
                            mainFrm.BeginInvoke(new voidDelegate(() =>
                            {
                                switch (updateViewer.ShowDialog((IWin32Window)mainFrm))
                                {
                                    case DialogResult.Cancel:
                                        mainFrm.enableForm(true);
                                        break;
                                    case DialogResult.Yes:
                                        string str2 = "SatiatorRingsConfig";
                                        if (!downloadFile("http://files-ds-scene.net/retrohead/satiator/releases/" + str2 + " v" + newVersion + ".zip", "data/temp/", str2 + " v" + newVersion + ".zip", str2 + " new.zip"))
                                        {
                                            MessageBox.Show("Download Failed", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            mainFrm.enableForm(true);
                                            break;
                                        }
                                        mainFrm.updateProgressLabel("extracting zip file");
                                        ZipFile.ExtractToDirectory("data/temp/" + str2 + " new.zip", "data/temp/");
                                        System.IO.File.Delete("data/temp/" + str2 + " new.zip");
                                        System.IO.File.Delete("data/" + str1);
                                        System.IO.File.Move("data/temp/" + str1, "data/" + str1);
                                        System.IO.File.Delete("data/temp/" + str1);
                                        Process.Start("SatiatorRingsConfigUpdater.exe");
                                        Environment.Exit(0);
                                        break;
                                    case DialogResult.No:
                                        mainFrm.enableForm(true);
                                        break;
                                    default:
                                        MessageBox.Show("WTF!? Only yes/no buttons should be available!");
                                        mainFrm.enableForm(true);
                                        break;
                                }
                            }));
                        } catch(Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                        return true;
                    }
                    else
                    {
                        mainFrm.disablemenuUpdate();
                        MessageBox.Show("There is a new version of the application available.\r\nChoose update from the the file menu to install v" + newVersion + ".\n\nMenu file updates will be disbaled until you have updated the application as a new menu may require new files.", "New version available", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                else
                {
                    if (!mainFrm.firstboot)
                    {
                        MessageBox.Show("The latest version (" + newVersion + ") is already installed.", "Application is up to date", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Update check failed, please check your internet connection\r\nor the site may be down!", "Update Failure", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }
            return false;
        }

        public static void moveDirectoryContents(string sourcedir, string dest, bool deleteSource)
        {
            if (!Directory.Exists(dest))
                Directory.CreateDirectory(dest);
            string[] files = Directory.GetFiles(sourcedir);
            foreach(string file in files)
            {
                string srcPath = Path.Combine(sourcedir, Path.GetFileName(file));
                string destPath = Path.Combine(dest, Path.GetFileName(file));

                if (File.Exists(destPath))
                    File.Delete(destPath);
                if(deleteSource)
                    File.Move(srcPath, destPath);
                else
                    File.Copy(srcPath, destPath);
            }

            files = Directory.GetDirectories(sourcedir);
            foreach (string file in files)
            {
                string srcPath = Path.Combine(sourcedir, Path.GetFileName(file));
                string destPath = Path.Combine(dest, Path.GetFileName(file));

                if (!Directory.Exists(destPath))
                    Directory.CreateDirectory(destPath);
                moveDirectoryContents(srcPath, destPath, deleteSource);
                if (deleteSource)
                    Directory.Delete(srcPath, true);
            }
        }

        private static bool checkMenuUpdate(bool download)
        {
            string newVersion = "";
            string str1 = "latest_menu_version.txt";
            if (downloadFile("http://files-ds-scene.net/retrohead/satiator/releases/" + str1, "data\\temp\\", "Update Check"))
            {
                try
                {
                    using (StreamReader streamReader = new StreamReader(new FileStream("data\\temp\\" + str1, FileMode.Open, FileAccess.Read)))
                    {
                        string str2;
                        if ((str2 = streamReader.ReadLine()) != null)
                            newVersion = str2;
                        streamReader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "checkAppUpdate failed to parse new info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                string curVer = "";
                try
                {
                    using (StreamReader streamReader = new StreamReader(new FileStream("data/" + str1, FileMode.Open, FileAccess.Read)))
                    {
                        string str2;
                        if ((str2 = streamReader.ReadLine()) != null)
                            curVer = str2;
                        streamReader.Close();
                    }
                }
                catch
                {
                }
                mainFrm.BeginInvoke(new voidDelegate(() => {
                    mainFrm.lblMenuVer.Text = "Menu v" + curVer;
                }));
                if (curVer != newVersion)
                {
                    if (download)
                    {
                        if (downloadFile("http://files-ds-scene.net/retrohead/satiator/releases/satiator-rings_v" + newVersion + ".zip", "data\\temp\\", "satiator-rings_v" + newVersion + ".zip"))
                        {
                            mainFrm.updateProgressLabel("extracting zip file");
                            if (Directory.Exists("data/temp/sd"))
                                Directory.Delete("data/temp/sd", true);
                            Directory.CreateDirectory("data/temp/sd");
                            ZipFile.ExtractToDirectory("data/temp/satiator-rings_v" + newVersion + ".zip", "data/temp/sd");
                            File.Delete("data/temp/satiator-rings_v" + newVersion + ".zip");
                            // scan through moving all the files and directories

                            if (Directory.Exists("data\\sd"))
                                Directory.Delete("data\\sd", true);
                            moveDirectoryContents("data\\temp\\sd", "data\\sd", true);
                            Directory.Delete("data\\temp\\sd", true);
                            // update the version
                            if (File.Exists("data\\" + str1))
                                File.Delete("data\\" + str1);
                            File.Copy("data\\temp\\" + str1, "data\\" + str1);
                            mainFrm.BeginInvoke(new voidDelegate(() => {
                                mainFrm.lblMenuVer.Text = "Menu v" + newVersion;
                            }));
                            MessageBox.Show("The menu file was updated to version " + newVersion + " successfully", "Update Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        }
                        return false;
                    }
                    else
                    {
                        MessageBox.Show("There is a new version of the menu available.\r\nChoose update from the the file menu to install v" + newVersion, "New version available", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                else
                {
                    if (!mainFrm.firstboot)
                    {
                        MessageBox.Show("The latest menu version (" + newVersion + ") is already installed.", "Menu file is up to date", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Update check failed, please check your internet connection\r\nor the site may be down!", "Update Failure", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }
            return true;
        }
        public static bool downloadFile(string url, string dirdest, string progressTxt, string saveas = "", bool ignore404 = false)
        {
            if (!allowDownload)
            {
                MessageBox.Show("Please wait for the current download to finish", "Download already in progress", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            allowDownload = false;
            downloadedData = new byte[512000];
            downloadedDataName = "";

            int totalBytes = 0;
            try
            {
                WebResponse response = WebRequest.Create(url).GetResponse();
                Stream responseStream = response.GetResponseStream();
                int contentLength = (int)response.ContentLength;
                mainFrm.updateProgress(0, contentLength);
                for (int index = 3; index < url.Length; ++index)
                {
                    if (url.Substring(url.Length - index, 1) == "/")
                    {
                        downloadedDataName = url.Substring(url.Length - index + 1, url.Length - (url.Length - index) - 1);
                        break;
                    }
                }
                MemoryStream memoryStream = new MemoryStream();
                byte[] buffer = new byte[1024];
                int count = 0;
                do
                {
                    totalBytes += count;
                    mainFrm.updateProgress(totalBytes, contentLength);
                    mainFrm.updateProgressLabel("Downloading: " + progressTxt + " " + hexAndMathFunctions.getPercentage(totalBytes, contentLength) + "%");
                    count = responseStream.Read(buffer, 0, buffer.Length);
                    if (count == 0)
                    {
                        mainFrm.updateProgress(contentLength, contentLength);
                        mainFrm.updateProgressLabel("Downloading: " + progressTxt + " " + hexAndMathFunctions.getPercentage(contentLength, contentLength) + "%");
                        downloadedData = memoryStream.ToArray();
                        responseStream.Close();
                        memoryStream.Close();
                        break;
                    }
                    else
                        memoryStream.Write(buffer, 0, count);
                } while (true);
            }
            catch (Exception ex)
            {
                if (!ignore404 || !ex.Message.Contains("404"))
                    MessageBox.Show(ex.Message, "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                allowDownload = true;
                return false;
            }
            if (saveas != "")
                downloadedDataName = saveas;
            if (File.Exists(dirdest + downloadedDataName))
                File.Delete(dirdest + downloadedDataName);
            FileStream fileStream = new FileStream(dirdest + downloadedDataName, FileMode.Create);
            fileStream.Write(downloadedData, 0, downloadedData.Length);
            fileStream.Close();
            allowDownload = true;
            return true;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatiatorRingsConfig
{
    public partial class frmExistingImages : Form
    {
        TGA T;
        frmMain mainFrm;

        public frmExistingImages(frmMain mainForm)
        {
            mainFrm = mainForm;
            InitializeComponent();
            imageList1.Images.Clear();
            listView1.Items.Clear();
            mainFrm.selectedId = -1;
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
                        T = new TGA(path);
                        imageList1.Images.Add((Bitmap)T);

                        listView1.Items.Add("");
                        listView1.Items[listView1.Items.Count - 1].ImageIndex = imageList1.Images.Count - 1;
                        listView1.Items[listView1.Items.Count - 1].Tag = i + (j * 100);
                    }
                }
            }
        }

        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            mainFrm.selectedId = int.Parse(listView1.SelectedItems[0].Tag.ToString());
            this.Close();
        }
    }
}

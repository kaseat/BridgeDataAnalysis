using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DTO;
using Gui.Properties;
using System.Linq;
using Conv;

namespace Gui
{
    public partial class DatasetConverters : Form
    {
        private String path;
        private List<Bridge> brLst;

        public DatasetConverters()
        {
            InitializeComponent();
        }

        private void button1_Click(Object sender, EventArgs e)
        {
            button3.Enabled = false;
            var dlg = new OpenFileDialog
            {
                Filter = Resources.GuiRes,
                RestoreDirectory = true
            };

            if (dlg.ShowDialog() != DialogResult.OK)
            {
                label1.Text = Resources.OpnFileErr;
                button2.Enabled = false;
                return;
            }

            path = dlg.FileName;
            button2.Enabled = true;
            label1.Text = Resources.OpenedFile;
        }

        private void button2_Click(Object sender, EventArgs e)
        {
            if (path == null) return;
            try
            {
                brLst = Converter.Import(path)
                    .Where(x => x.Superstructures
                    .Sum(superstructure => superstructure.Defects.Count) > 5)
                    .Select(x => x)
                    .ToList();

                button2.Enabled = false;
                button3.Enabled = true;
                label1.Text = Resources.ConvOk;
            }
            catch (Exception)
            {
                label1.Text = Resources.ConvErr;
                button3.Enabled = false;
            }
        }

        private void button3_Click(Object sender, EventArgs e)
        {
            button2.Enabled = false;
            var dlg = new SaveFileDialog
            {
                Filter = Resources.GuiRes,
                RestoreDirectory = true
            };

            if (dlg.ShowDialog() != DialogResult.OK)
            {
                label1.Text = Resources.SavErr;
                button2.Enabled = true;
                button3.Enabled = true;
                return;
            }
            using (var fs = dlg.OpenFile())
            {
                if (Converter.SaveXml(brLst, fs))
                {
                    label1.Text = Resources.SavErr;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    fs.Close();
                    return;
                }
                label1.Text = Resources.SavOk;
                button2.Enabled = false;
                button3.Enabled = false;
                fs.Close();
            }
        }
    }
}

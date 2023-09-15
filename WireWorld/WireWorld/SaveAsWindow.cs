using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WireWorld
{
    public partial class SaveAsWindow : Form
    {
        public string FileName { get; private set; }
        private Grid grid1;

        public SaveAsWindow(Grid grid1)
        {
            InitializeComponent();
            this.grid1 = grid1;
            FileName = null;
        }

        //Save
        private void Save()
        {
            string[] data = new string[Height + 1];
            data[0] = grid1.Width + " " + grid1.Height;

            for (int y = 0; y < grid1.Height; y++)
            {
                for (int x = 0; x < grid1.Width; x++)
                {
                    data[y + 1] += grid1.Squares[x, y].OffValue + " ";
                }
            }

            File.WriteAllLines(FileName, data);
        }

        //Buttons
        private void OkayButton_Click(object sender, EventArgs e)
        {
            if (grid1 == null)
            {
                this.Close();
                return;
            }

            if (!fileNameBox.Text.EndsWith(".txt"))
            {
                fileNameBox.Text += ".txt";
            }

            if (File.Exists(fileNameBox.Text))
            {
                fileNameBox.Text = "0" + fileNameBox.Text;
            }
            else
            {
                FileName = fileNameBox.Text;
                Save();
                this.Close();
            }
        }
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

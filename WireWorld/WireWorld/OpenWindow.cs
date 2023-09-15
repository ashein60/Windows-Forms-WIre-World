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
    public partial class OpenWindow : Form
    {
        public Grid Grid1 { get; private set; }

        public OpenWindow()
        {
            InitializeComponent();
            Grid1 = null;
        }

        //Buttons
        private void OkayButton_Click(object sender, EventArgs e)
        {
            if (!fileNameBox.Text.EndsWith(".txt"))
            {
                fileNameBox.Text += ".txt";
            }

            if (File.Exists(fileNameBox.Text))
            {
                string[] data = File.ReadAllLines(fileNameBox.Text);

                if (SuccessfulDataTransfer(data))
                {
                    this.Close();
                }
                else
                {
                    fileNameBox.Text = "Invalid.txt";
                }
            }
            else
            {
                fileNameBox.Text = "Invalid.txt";
            }
        }
        private bool SuccessfulDataTransfer(string[] data)
        {
            if (data == null || data.Length < 2) return false;

            try
            {
                //Width and Height
                string temp = data[0];
                int spaceIndex = temp.IndexOf(" ");
                string widthS = temp.Remove(spaceIndex);
                string heightS = temp.Substring(spaceIndex + 1);
                int width, height;

                try
                {
                    width = Convert.ToInt32(widthS);
                    height = Convert.ToInt32(heightS);
                }
                catch (FormatException)
                {
                    return false;
                }

                //Check that all lengths are equal besides data[0]
                if (height > 2)
                {
                    for (int i = 2; i < height; i++)
                    {
                        if (width * 2 != data[i].Length)
                        {
                            return false;
                        }
                    }
                }

                //Attempt to Convert Data
                Grid1 = new Grid(width, height);
                int value;

                try
                {
                    for (int y = 1; y < height; y++)
                    {
                        temp = data[y];
                        for (int x = 0; x < width; x++)
                        {
                            if (x == width - 1)
                            {
                                value = Convert.ToInt32(temp);
                            }
                            else //Normal
                            {
                                spaceIndex = temp.IndexOf(" ");
                                value = Convert.ToInt32(temp.Remove(spaceIndex));
                                temp = temp.Substring(spaceIndex + 1);
                            }
                            
                            Grid1.SetSquare(x, y, value);
                        }
                    }
                }
                catch (FormatException)
                {
                    return false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }


            return true;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WireWorld
{
    public partial class ResizeWindow : Form
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public ResizeWindow(int width, int height)
        {
            InitializeComponent();

            Width = 0;
            Height = 0;

            WidthBox.Text = width.ToString();
            HeightBox.Text = height.ToString();
        }

        //Click Buttons
        private void OkayButton_Click(object sender, EventArgs e)
        {
            bool fail = false;

            try
            {
                Width = Convert.ToInt32(WidthBox.Text);

                if (Width > 1000)
                {
                    fail = true;
                    WidthBox.Text = "1000";
                }
                else if (Width < 1)
                {
                    fail = true;
                    WidthBox.Text = "1";
                }
            } 
            catch (FormatException)
            {
                WidthBox.Text = "25";
                fail = true;
            }

            try
            {
                Height = Convert.ToInt32(HeightBox.Text);

                if (Height > 1000)
                {
                    fail = true;
                    HeightBox.Text = "1000";
                }
                else if (Height < 1)
                {
                    fail = true;
                    HeightBox.Text = "1";
                }
            }
            catch (FormatException)
            {
                HeightBox.Text = "20";
                fail = true;
            }

            if (!fail)
            {
                this.Close();
            }
        }
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.IO;

namespace WireWorld
{
    public partial class Form1 : Form
    {
        private static string SaveFileName = null; //For Save file handling

        private static System.Timers.Timer updateTimer;
        private static readonly float resizeSpeed = 1f;
        private static readonly float moveSpeed = 7f;
        private static int clickValue = 3; //0, 1, 2, 3

        private static bool movingUp, movingRight, movingDown, movingLeft;
        private static bool mouseDown = false;

        private static Grid grid1;

        public Form1()
        {
            InitializeComponent();
            CreateTimer();
            this.MouseWheel += Form1_MouseWheel;
            NewClick();
            Render.SetUp(30, 0 + 5, 25 + 5);

        }

        //Update
        private void CreateTimer()
        {
            updateTimer = new System.Timers.Timer();
            updateTimer.Elapsed += RunningLoop;
            updateTimer.Interval = 10;
            updateTimer.AutoReset = true;
            updateTimer.Enabled = true;
        }
        private void RunningLoop(object sender, ElapsedEventArgs e)
        {
            if (grid1 == null) return;

            MoveCamera();

            if (grid1.Running)
            {
                grid1.Update();
            }

            this.Invalidate();
        }
        private void MoveCamera()
        {
            if (movingUp)
            {
                Render.MoveCamera(0, -moveSpeed);
            }
            if (movingRight)
            {
                Render.MoveCamera(moveSpeed, 0);
            }
            if (movingDown)
            {
                Render.MoveCamera(0, moveSpeed);
            }
            if (movingLeft)
            {
                Render.MoveCamera(-moveSpeed, 0);
            }
        }

        //Toggle Mode
        private void ToggleMode()
        {
            if (grid1 == null) return;

            if (grid1.Running)
            {
                Mode.Text = "Building";
                grid1.Stop();
            }
            else
            {
                Mode.Text = "Running";
                grid1.Start();
            }
        }

        //Click Menu Strip
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewClick();
        }
        private void NewClick()
        {
            grid1 = new Grid(25, 20);
            SaveFileName = null;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenClick();
        }
        private void OpenClick()
        {
            OpenWindow window = new OpenWindow();
            window.ShowDialog(this);

            if (window.Grid1 != null)
            {
                grid1 = window.Grid1;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveClick();
        }
        private void SaveClick()
        {
            if (grid1 == null) return;

            if (SaveFileName == null)
            {
                SaveAsClick();
            }
            else
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

                File.WriteAllLines(SaveFileName, data);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAsClick();
        }
        private void SaveAsClick()
        {
            if (grid1 == null) return;

            SaveAsWindow window = new SaveAsWindow(grid1);
            window.ShowDialog(this);

            if (window.FileName != null)
            {
                SaveFileName = window.FileName;
            }
        }

        private void resizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grid1 == null) return;

            ResizeWindow window = new ResizeWindow(grid1.Width, grid1.Height);
            window.ShowDialog(this);

            if (window.Width == 0 || window.Height == 0)
            {
                return;
            }

            grid1.Resize(window.Width, window.Height);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpWindow window = new HelpWindow();
            window.ShowDialog(this);
        }

        private void Mode_Click(object sender, EventArgs e)
        {
            ToggleMode();
        }

        //Key Events
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //File Handling
            if (ModifierKeys == Keys.Control)
            {
                if (e.KeyCode == Keys.N)
                {
                    NewClick();
                }
                else if (e.KeyCode == Keys.O)
                {
                    OpenClick();
                }
                else if (e.KeyCode == Keys.S)
                {
                    SaveClick();
                }

                return;
            }

            if (grid1 == null) return;

            //Toggle Mode
            if (e.KeyCode == Keys.Space)
            {
                ToggleMode();
            }

            //Change ClickValue
            if (!grid1.Running)
            {
                if (e.KeyCode == Keys.D1)
                {
                    clickValue = 0;
                    ClickValue.Text = "Empty";
                }
                else if (e.KeyCode == Keys.D2)
                {
                    clickValue = 1;
                    ClickValue.Text = "Head";
                }
                else if (e.KeyCode == Keys.D3)
                {
                    clickValue = 2;
                    ClickValue.Text = "Tail";
                }
                else if (e.KeyCode == Keys.D4)
                {
                    clickValue = 3;
                    ClickValue.Text = "Conductor";
                }
            }

            //Move Camera
            if (e.KeyCode == Keys.W)
            {
                movingUp = true;
            }
            else if (e.KeyCode == Keys.D)
            {
                movingRight = true;
            }
            else if (e.KeyCode == Keys.S)
            {
                movingDown = true;
            }
            else if (e.KeyCode == Keys.A)
            {
                movingLeft = true;
            }

        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (grid1 == null) return;

            if (e.KeyCode == Keys.W)
            {
                movingUp = false;
            }
            else if (e.KeyCode == Keys.D)
            {
                movingRight = false;
            }
            else if (e.KeyCode == Keys.S)
            {
                movingDown = false;
            }
            else if (e.KeyCode == Keys.A)
            {
                movingLeft = false;
            }
        }

        //Mouse Events
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (grid1 == null) return;

            mouseDown = true;

            if (!grid1.Running)
            {
                Render.ClickGrid(grid1, clickValue, e.X, e.Y);
            }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (grid1 == null) return;

            mouseDown = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (grid1 == null) return; 

            if (mouseDown && !grid1.Running)
            {
                Render.ClickGrid(grid1, clickValue, e.X, e.Y);
            }
        }
        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (grid1 == null) return;

            if (e.Delta > 0) //Up
            {
                Render.ResizeScale(resizeSpeed);
            }
            else //Down
            {
                Render.ResizeScale(-resizeSpeed);
            }
        }

        //Paint
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Render.PaintGrid(e, grid1);
        } 
    }
}

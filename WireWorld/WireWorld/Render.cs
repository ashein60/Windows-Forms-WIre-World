using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace WireWorld
{
    public static class Render
    {
        private static float renderScale = 1;

        private static float cameraX = 0;
        private static float cameraY = 0;

        private static float offsetX = 0;
        private static float offsetY = 0;

        //SetUp
        public static void SetUp(float scale, float offSetX, float offSetY)
        {
            renderScale = scale;

            offsetX = offSetX;
            offsetY = offSetY;
        }

        //Camera
        public static void MoveCamera(float distanceX, float distanceY)
        {
            cameraX += distanceX;
            cameraY += distanceY;
        }

        //Resize
        public static void ResizeScale(float distance)
        {
            renderScale += distance;

            if (renderScale < 2)
            {
                renderScale = 2;
            }
        }

        public static void ClickGrid(Grid grid1, int value, int mouseX, int mouseY)
        {
            if (grid1 == null) return;

            int x = (int) Math.Floor((mouseX - offsetX - cameraX) / renderScale);
            int y = (int)Math.Floor((mouseY - offsetY - cameraY) / renderScale);

            grid1.SetSquare(x, y, value);
        }

        //Paint
        public static void PaintGrid(PaintEventArgs e, Grid grid1)
        {
            if (grid1 == null) return;

            Brush empty = new SolidBrush(Color.Black);
            Brush head = new SolidBrush(Color.Blue);
            Brush tail = new SolidBrush(Color.Red);
            Brush conductor = new SolidBrush(Color.Yellow);

            float paintX, paintY;
            float paintSize = renderScale - 1;

            for (int y = 0; y < grid1.Height; y++)
            {
                for (int x = 0; x < grid1.Width; x++)
                {
                    paintX = x * renderScale + cameraX + offsetX;
                    paintY = y * renderScale + cameraY + offsetY;

                    if (grid1.Squares[x, y].OnValue == 0)
                    {
                        e.Graphics.FillRectangle(empty, paintX, paintY, paintSize, paintSize);
                    }
                    else if (grid1.Squares[x, y].OnValue == 1)
                    {
                        e.Graphics.FillRectangle(head, paintX, paintY, paintSize, paintSize);
                    }
                    else if (grid1.Squares[x, y].OnValue == 2)
                    {
                        e.Graphics.FillRectangle(tail, paintX, paintY, paintSize, paintSize);
                    }
                    else //if (grid1.Squares[x, y].OnValueCurrent == 3)
                    {
                        e.Graphics.FillRectangle(conductor, paintX, paintY, paintSize, paintSize);
                    }

                }
            }

            empty.Dispose();
            head.Dispose();
            tail.Dispose();
            conductor.Dispose();
        }
    }
}

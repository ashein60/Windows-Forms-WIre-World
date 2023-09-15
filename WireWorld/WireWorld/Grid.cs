using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireWorld
{
    public class Grid
    {
        public struct Square
        {
            public int OffValue;
            public bool WasConductor;
            public int OnValue;

            public Square(int value)
            {
                WasConductor = false;
                OffValue = value;
                OnValue = value;
            }

            public void SetAllValues(int value)
            {
                OffValue = value;
                OnValue = value;
            }

            public void TurnOff()
            {
                WasConductor = false;
                OnValue = OffValue;
            }
        }

        public Square[,] Squares { get; private set; }

        public bool Running { get; set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        private static int maxCounter = 25;
        private int counter;

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;

            Running = false;

            counter = 0;

            Squares = new Square[width, height];
            ClearValues();
        }

        //Default Values
        public void ClearValues()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Squares[x, y].SetAllValues(0);
                }
            }
        }

        //Resize
        public void Resize(int newWidth, int newHeight)
        {
            //Within Range
            if (newWidth < 1)
            {
                newWidth = 1;
            }
            if (newHeight < 1)
            {
                newHeight = 1;
            }
            if (newWidth > 1000)
            {
                newWidth = 1000;
            }
            if (newHeight > 1000)
            {
                newHeight = 1000;
            }

            //Find Smallest
            int smallestWidth, smallestHeight;

            if (newWidth < Width)
            {
                smallestWidth = newWidth;
            }
            else
            {
                smallestWidth = Width;
            }

            if (newHeight < Height)
            {
                smallestHeight = newHeight;
            }
            else
            {
                smallestHeight = Height;
            }

            //Fill Array
            Square[,] old = Squares;
            Squares = new Square[newWidth, newHeight];

            for (int y = 0; y < smallestHeight; y++)
            {
                for (int x = 0; x < smallestWidth; x++)
                {
                    Squares[x, y] = old[x, y];
                }
            }

            //Assign new Width and Height
            Width = newWidth;
            Height = newHeight;
        }

        //SetValue
        public void SetSquare(int x, int y, int value)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height || value < 0 || value > 3) return;
            Squares[x, y].SetAllValues(value);
        }

        //Running
        public void Start()
        {
            if (!Running)
            {
                Running = true;
            }
        }
        public void Stop()
        {
            if (Running)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        Squares[x, y].TurnOff();
                    }
                }

                Running = false;
            }
        }
        public void Update()
        {
            if (!Running) return;

            counter++;
            if (counter >= maxCounter)
            {
                counter = 0;
            }
            else
            {
                return;
            }

            //Conductors first
            int countTouching;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (Squares[x, y].OnValue == 3)
                    {
                        countTouching = 0;

                        //Vertical and Horizontal
                        if (y - 1 > 0 && Squares[x, y - 1].OnValue == 1 && !Squares[x, y - 1].WasConductor) //Up
                        {
                            countTouching++;
                        }
                        if (x + 1 < Width && Squares[x + 1, y].OnValue == 1 && !Squares[x + 1, y].WasConductor) //Right
                        {
                            countTouching++;
                        }
                        if (y + 1 < Height && Squares[x, y + 1].OnValue == 1 && !Squares[x, y + 1].WasConductor) //Down
                        {
                            countTouching++;
                        }
                        if (x - 1 > 0 && Squares[x - 1, y].OnValue == 1 && !Squares[x - 1, y].WasConductor) //Left
                        {
                            countTouching++;
                        }

                        //Diagonal
                        if (y - 1 > 0 && x - 1 > 0 && Squares[x - 1, y - 1].OnValue == 1 && !Squares[x - 1, y - 1].WasConductor) //Up Left
                        {
                            countTouching++;
                        }
                        if (y - 1 > 0 && x + 1 < Width && Squares[x + 1, y - 1].OnValue == 1 && !Squares[x + 1, y - 1].WasConductor) //Up Right
                        {
                            countTouching++;
                        }
                        if (y + 1 < Height && x + 1 < Width && Squares[x + 1, y + 1].OnValue == 1 && !Squares[x + 1, y + 1].WasConductor) //Down Right
                        {
                            countTouching++;
                        }
                        if (y + 1 < Height && x - 1 > 0 && Squares[x - 1, y + 1].OnValue == 1 && !Squares[x - 1, y + 1].WasConductor) //Down Left
                        {
                            countTouching++;
                        }

                        //Assignment
                        if (countTouching == 1 || countTouching == 2)
                        {
                            Squares[x, y].OnValue = 1;
                            Squares[x, y].WasConductor = true;
                        }

                    }
                }
            }

            //Heads and Tails
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (Squares[x, y].OnValue == 1)
                    {
                        if (!Squares[x, y].WasConductor)
                        {
                            Squares[x, y].OnValue = 2;
                        }
                        else
                        {
                            Squares[x, y].WasConductor = false;
                        }
                    }
                    else if (Squares[x, y].OnValue == 2)
                    {
                        Squares[x, y].OnValue = 3;
                    }

                }
            }
        }
    }
}

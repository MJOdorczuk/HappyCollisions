using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace HappyCollisions.Display
{
    public class Camera
    {
        private readonly object lockObject = new object();

        private double x = 0.0;
        private double y = 0.0;
        private double dx = 1.0;
        private double dy = 1.0;

        public void Offset(Point offset)
        {
            lock (lockObject)
            {
                this.x += offset.X * this.dx;
                this.y += offset.Y * this.dy;
            }
        }

        public void Scale(double scale)
        {
            lock (lockObject)
            {
                this.dx *= scale;
                this.dy *= scale;
            }
        }

        public void DrawMesh(BufferedGraphics buffer, Rectangle rectangle, Point offset)
        {
            double x, y, dx, dy;
            lock (lockObject)
            {
                x = this.x + offset.X * this.dx;
                y = this.y + offset.Y * this.dy;
                dx = this.dx;
                dy = this.dy;
            }
            var width = rectangle.Width;
            var height = rectangle.Height;
            var rightX = x + dx * width * 0.5;
            var leftX = x - dx * width * 0.5;
            var topY = y - dy * height * 0.5;
            var bottomY = y + dy * height * 0.5;
            var maxDim = Math.Max(width * dx, height * dy);

            double diff = 1.0;
            if (diff > maxDim)
            {
                while(diff > maxDim)
                {
                    diff /= 5;
                }
            }
            else
            {
                while(diff * 5 < maxDim)
                {
                    diff *= 5;
                }
            }
            diff /= 25;

            int a = (int)Math.Ceiling(leftX / diff);
            for (int i = a; i * diff < rightX; i++)
            {
                int _x = (int)(0.5 * width + (i * diff - x) / dx);
                if (i % 25 == 0)
                {
                    var pen = new Pen(Color.FromArgb((int)(3187 * diff / maxDim), 255, 255, 255))
                    {
                        Width = (float)Math.Sqrt((int)(625 * diff / maxDim) / dx)
                    };
                    buffer.Graphics.DrawLine(pen, _x, 0, _x, height);
                }
                else if (i % 5 == 0)
                {
                    var pen = new Pen(Color.FromArgb((int)(1593 * diff / maxDim), 255, 255, 255))
                    {
                        Width = (float)Math.Sqrt((int)(125 * diff / maxDim) / dx)
                    };
                    buffer.Graphics.DrawLine(pen, _x, 0, _x, height);
                }
                else
                {
                    var pen = new Pen(Color.FromArgb((int)(796 * diff / maxDim), 255, 255, 255))
                    {
                        Width = (float)Math.Sqrt((int)(25 * diff / maxDim) / dx)
                    };
                    buffer.Graphics.DrawLine(pen, _x, 0, _x, height);
                }
            }
            a = (int)Math.Ceiling(topY / diff);
            for (int i = a; i * diff < bottomY; i++)
            {
                int _y = (int)(0.5 * height + (i * diff - y) / dy);
                if (i % 25 == 0)
                {
                    var pen = new Pen(Color.FromArgb((int)(3187 * diff / maxDim), 255, 255, 255))
                    {
                        Width = (float)Math.Sqrt((int)(625 * diff / maxDim) / dy)
                    };
                    buffer.Graphics.DrawLine(pen, 0, _y, width, _y);
                }
                else if (i % 5 == 0)
                {
                    var pen = new Pen(Color.FromArgb((int)(1593 * diff / maxDim), 255, 255, 255))
                    {
                        Width = (float)Math.Sqrt((int)(125 * diff / maxDim) / dy)
                    };
                    buffer.Graphics.DrawLine(pen, 0, _y, width, _y);
                }
                else
                {
                    var pen = new Pen(Color.FromArgb((int)(796 * diff / maxDim), 255, 255, 255))
                    {
                        Width = (float)Math.Sqrt((int)(25 * diff / maxDim) / dy)
                    };
                    buffer.Graphics.DrawLine(pen, 0, _y, width, _y);
                }
            }
        }
    }
}

using System;
using System.Drawing;

namespace HappyCollisions.Display
{
    public class Camera
    {
        private static readonly int MESH_BASE = 5;

        private readonly object lockObject = new object();

        private float x = 0;
        private float y = 0;
        private float dx = 1;
        private float dy = 1;
        private float angle = 0;

        public PointF Focus(Point offset)
        {
            var rotated = RotatePoint(offset, this.angle);
            return new PointF(this.x + rotated.X * this.dx, this.y + rotated.Y * this.dy);
        }

        public float DX { get => dx; }
        public float DY { get => dy; }

        public void Translate(Point offset)
        {
            var rotated = RotatePoint(offset, this.angle);
            lock (lockObject)
            {
                this.x += rotated.X * this.dx;
                this.y += rotated.Y * this.dy;
            }
        }

        public void Zoom(float scale, Point target)
        {
            var rotated = RotatePoint(target, - this.angle);
            var newFocus = new PointF(this.x + target.X * this.dx * (1 - scale), 
                                      this.y + target.Y * this.dy * (1 - scale));
            lock (lockObject)
            {
                this.x = newFocus.X;
                this.y = newFocus.Y;
                this.dx *= scale;
                this.dy *= scale;
            }
        }

        public void Rotate(float angle)
        {
            lock (lockObject)
            {
                this.angle += angle;
            }
        }

        public void DrawMesh(BufferedGraphics buffer, Rectangle rectangle, Point offset)
        {
            var width = rectangle.Width;
            var height = rectangle.Height;
            var maxDim = Math.Max(width * this.dx, height * this.dy);
            var rotated = RotatePoint(offset, this.angle);
            var ox = this.x + rotated.X * this.dx;
            var oy = this.y + rotated.Y * this.dy;
            var leftX = ox - width * this.dx * 0.5;
            var topY = oy - height * this.dy * 0.5;
            var rightX = ox + width * this.dx * 0.5;
            var bottomY = oy + height * this.dy * 0.5;

            var diff = 1.0;
            if (diff > maxDim)
            {
                while(diff > maxDim)
                {
                    diff /= MESH_BASE;
                }
            }
            else
            {
                while(diff * MESH_BASE < maxDim)
                {
                    diff *= MESH_BASE;
                }
            }
            diff /= MESH_BASE * MESH_BASE;

            int a = (int)Math.Ceiling(leftX / diff);
            for (int i = a; i * diff < rightX; i++)
            {
                int x = (int)((i * diff - ox) / this.dx);
                if (i % (MESH_BASE * MESH_BASE) == 0)
                {
                    DrawVerticalLine(diff / (width * this.dx), 
                                     x, 
                                     buffer.Graphics, 
                                     height);
                }
                else if (i % MESH_BASE == 0)
                {
                    DrawVerticalLine(diff / (width * MESH_BASE * this.dx), 
                                     x, 
                                     buffer.Graphics, 
                                     height);
                }
                else
                {
                    DrawVerticalLine(diff / (width * MESH_BASE * MESH_BASE * this.dx), 
                                     x, 
                                     buffer.Graphics, 
                                     height);
                }
            }
            a = (int)Math.Ceiling(topY / diff);
            for (int i = a; i * diff < bottomY; i++)
            {
                int y = (int)((i * diff - oy) / this.dy);
                if (i % (MESH_BASE * MESH_BASE) == 0)
                {
                    DrawHorizontalLine(diff / (height * this.dx), 
                                       y, 
                                       buffer.Graphics, 
                                       width);
                }
                else if (i % MESH_BASE == 0)
                {
                    DrawHorizontalLine(diff / (height * MESH_BASE * this.dx), 
                                       y, 
                                       buffer.Graphics, 
                                       width);
                }
                else
                {
                    DrawHorizontalLine(diff / (height * MESH_BASE * MESH_BASE * this.dx), 
                                       y, 
                                       buffer.Graphics, 
                                       width);
                }
            }
        }

        public void Adjust(BufferedGraphics buffer, Rectangle rectangle)
        {
            buffer.Graphics.TranslateTransform(rectangle.Width / 2, rectangle.Height / 2);
            buffer.Graphics.RotateTransform((float)angle);
        }

        private void DrawVerticalLine(double fract, int x, Graphics graphics, int height)
        {
            var pen = new Pen(Color.FromArgb((int)Math.Sqrt(255 * 255 * fract), 255, 255, 255))
            {
                Width = 4
            };
            graphics.DrawLine(pen, x, -height / 2, x, height / 2);
        }

        private void DrawHorizontalLine(double fract, int y, Graphics graphics, int width)
        {
            var pen = new Pen(Color.FromArgb((int)Math.Sqrt(255 * 255 * fract), 255, 255, 255))
            {
                Width = 4
            };
            graphics.DrawLine(pen, -width / 2, y, width / 2, y);
        }

        private Point RotatePoint(Point point, double angle)
        {
            angle *= Math.PI / 180;
            return new Point((int)(point.X * Math.Cos(angle) + point.Y * Math.Sin(angle)),
                             (int)(point.Y * Math.Cos(angle) - point.X * Math.Sin(angle)));
        }
    }
}

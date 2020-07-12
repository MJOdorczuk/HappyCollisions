using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HappyCollisions.Display
{
    public class DisplayManager
    {
        private bool read = true;
        private bool disposing = false;
        private bool mouseLocked = false;
        private Point lastMouseLock;
        private Point currentMouse;
        private Graphics displayGraphics;
        private Rectangle displayRectangle;
        private readonly object lockObject = new object();
        private readonly Camera camera = new Camera();

        private bool Disposing
        {
            get
            {
                bool ret;
                lock (lockObject)
                {
                    ret = disposing;
                }
                return ret;
            }
            set
            {
                lock (lockObject)
                {
                    disposing = value;
                }
            }
        }
        private bool Read
        {
            get
            {
                bool ret;
                lock (lockObject)
                {
                    ret = read;
                }
                return ret;
            }
            set
            {
                lock (lockObject)
                {
                    read = value;
                }
            }
        }
        public Graphics DisplayGraphics
        {
            get
            {
                Graphics ret;
                lock (lockObject)
                {
                    ret = displayGraphics;
                    read = true;
                }
                return ret;
            }
            set
            {
                lock (lockObject)
                {
                    displayGraphics = value;
                    read = false;
                }
            }
        }
        public Rectangle DisplayRectangle
        {
            get
            {
                Rectangle ret;
                lock (lockObject)
                {
                    ret = displayRectangle;
                }
                return ret;
            }
            set
            {
                var rectangle = new Rectangle(
                    value.X, 
                    value.Y, 
                    Math.Max(1, value.Width), 
                    Math.Max(1, value.Height));
                lock (lockObject)
                {
                    displayRectangle = rectangle;
                }
            }
        }
        public Point CurrentMouse
        {
            get
            {
                Point ret;
                lock (lockObject)
                {
                    ret = currentMouse;
                }
                return ret;
            }
            set
            {
                lock (lockObject)
                {
                    currentMouse = value;
                }
            }
        }

        public DisplayManager(Control parent)
        {
            parent.Disposed += (sender, e) => Disposing = true;
            new Task(() =>
            {
                while (!Disposing)
                {
                    if (!Read)
                    {
                        Tick();
                    }
                }
            }).Start();
        }

        public void LockMouse(Point mouse)
        {
            lock (lockObject)
            {
                if (!mouseLocked)
                {
                    mouseLocked = true;
                }
                lastMouseLock = mouse;
            }
        }

        public void UnlockMouse()
        {
            lock (lockObject)
            {
                mouseLocked = false;
                camera.Translate(new Point(lastMouseLock.X - currentMouse.X, 
                                           lastMouseLock.Y - currentMouse.Y));
            }
        }

        public void Scale(double scale)
        {
            camera.Scale(scale);
        }

        public void Rotate(double angle)
        {
            this.camera.Rotate(angle);
        }

        private void Tick()
        {
            var graphics = DisplayGraphics;
            var rectangle = Expand(DisplayRectangle);
            if(graphics is null)
            {
                return;
            }
            var buffer = BufferedGraphicsManager.Current.Allocate(graphics, rectangle);
            var offset = new Point(0, 0);
            if (mouseLocked)
            {
                offset = new Point(lastMouseLock.X - currentMouse.X, lastMouseLock.Y - currentMouse.Y);
            }
            camera.Adjust(buffer, DisplayRectangle);
            buffer.Graphics.Clear(Color.FromArgb(0, 0, 64));
            camera.DrawMesh(buffer, rectangle, offset);
            
            try
            {
                buffer.Render();
                buffer.Dispose();
            }
            catch (Exception e)
            {

            }
        }

        private Rectangle Expand(Rectangle rectangle)
        {
            var diagonal = (int)Math.Sqrt(rectangle.Width * rectangle.Width + rectangle.Height * rectangle.Height);
            var x0 = rectangle.X + rectangle.Width / 2;
            var y0 = rectangle.Y + rectangle.Height / 2;
            return new Rectangle(x0 - (diagonal / 2), y0 - (diagonal / 2), diagonal, diagonal);
        }
    }
}

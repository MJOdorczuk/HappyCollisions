using HappyCollisions.Display;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HappyCollisions
{
    public partial class MainForm : Form
    {
        private readonly DisplayManager displayManager;
        private readonly Timer timer = new Timer();

        public MainForm()
        {
            displayManager = new DisplayManager(this);
            InitializeComponent();
            timer.Interval = 20;
            timer.Tick += (sender, e) => TickDataUpdate();
            timer.Start();
        }

        private void TickDataUpdate()
        {
            displayManager.DisplayGraphics = this.CreateGraphics();
            displayManager.DisplayRectangle = this.DisplayRectangle;
            displayManager.CurrentMouse = this.PointToClient(Cursor.Position);
        }

        private void MouseDownUpdate(MouseEventArgs mouse)
        {
            displayManager.LockMouse(mouse.Location);
        }

        private void MouseUpUpdate()
        {
            displayManager.UnlockMouse();
        }

        private void ScrollUpdate(double diff)
        {
            displayManager.Scale(1 + (60 - diff) * 0.001);
        }

        private void KeyDownUpdate(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    displayManager.Rotate(-5);
                    break;
                case Keys.Right:
                    displayManager.Rotate(5);
                    break;
            }
        }
    }
}

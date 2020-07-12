namespace HappyCollisions
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Text = "Happy Collisions";
            this.Disposed += (sender, e) => timer.Dispose();
            this.MouseDown += (sender, e) => MouseDownUpdate(e);
            this.MouseUp += (sender, e) => MouseUpUpdate();
            this.MouseMove += (sender, e) => { };
            this.MouseClick += (sender, e) => { };
            this.MouseWheel += (sender, e) => ScrollUpdate(e.Delta, e);
            this.KeyDown += (sender, e) => KeyDownUpdate(e);
        }

        #endregion
    }
}
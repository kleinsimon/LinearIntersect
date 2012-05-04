namespace LinearIntersect
{
    partial class ImageForm
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
            this.SuspendLayout();
            // 
            // ImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 367);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ImageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImageForm";
            this.Activated += new System.EventHandler(this.ImageForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImageForm_FormClosing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ImageForm_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImageForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImageForm_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
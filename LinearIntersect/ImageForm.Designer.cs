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
            this.status = new System.Windows.Forms.StatusStrip();
            this.statusStats = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusDirty = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusZoom = new System.Windows.Forms.ToolStripStatusLabel();
            this.status.SuspendLayout();
            this.SuspendLayout();
            // 
            // status
            // 
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusZoom,
            this.statusStats,
            this.StatusDirty});
            this.status.Location = new System.Drawing.Point(0, 344);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(377, 23);
            this.status.SizingGrip = false;
            this.status.TabIndex = 0;
            this.status.Text = "statusStrip1";
            // 
            // statusStats
            // 
            this.statusStats.Name = "statusStats";
            this.statusStats.Size = new System.Drawing.Size(109, 18);
            this.statusStats.Text = "toolStripStatusLabel1";
            // 
            // StatusDirty
            // 
            this.StatusDirty.AutoSize = false;
            this.StatusDirty.Name = "StatusDirty";
            this.StatusDirty.Size = new System.Drawing.Size(120, 18);
            // 
            // statusZoom
            // 
            this.statusZoom.Name = "statusZoom";
            this.statusZoom.Size = new System.Drawing.Size(135, 18);
            this.statusZoom.Text = "toolStripDropDownButton1";
            // 
            // ImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 367);
            this.Controls.Add(this.status);
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
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.ToolStripStatusLabel StatusDirty;
        private System.Windows.Forms.ToolStripStatusLabel statusStats;
        private System.Windows.Forms.ToolStripStatusLabel statusZoom;
    }
}
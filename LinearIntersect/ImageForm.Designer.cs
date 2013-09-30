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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageForm));
            this.status = new System.Windows.Forms.StatusStrip();
            this.statusZoom = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStats = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusDirty = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ImgPanel = new System.Windows.Forms.PictureBox();
            this.calibStat = new System.Windows.Forms.ToolStripStatusLabel();
            this.status.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImgPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // status
            // 
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusZoom,
            this.statusStats,
            this.StatusDirty,
            this.calibStat});
            this.status.Location = new System.Drawing.Point(0, 515);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(977, 22);
            this.status.SizingGrip = false;
            this.status.TabIndex = 0;
            this.status.Text = "statusStrip1";
            // 
            // statusZoom
            // 
            this.statusZoom.Name = "statusZoom";
            this.statusZoom.Size = new System.Drawing.Size(151, 17);
            this.statusZoom.Text = "toolStripDropDownButton1";
            // 
            // statusStats
            // 
            this.statusStats.Name = "statusStats";
            this.statusStats.Size = new System.Drawing.Size(118, 17);
            this.statusStats.Text = "toolStripStatusLabel1";
            // 
            // StatusDirty
            // 
            this.StatusDirty.Name = "StatusDirty";
            this.StatusDirty.Size = new System.Drawing.Size(0, 17);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.ImgPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(977, 515);
            this.panel1.TabIndex = 1;
            // 
            // ImgPanel
            // 
            this.ImgPanel.Location = new System.Drawing.Point(0, 0);
            this.ImgPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ImgPanel.Name = "ImgPanel";
            this.ImgPanel.Size = new System.Drawing.Size(801, 471);
            this.ImgPanel.TabIndex = 2;
            this.ImgPanel.TabStop = false;
            this.ImgPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.ImageForm_Paint);
            this.ImgPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImageForm_MouseDown);
            this.ImgPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImageForm_MouseMove);
            // 
            // calibStat
            // 
            this.calibStat.Name = "calibStat";
            this.calibStat.Size = new System.Drawing.Size(118, 17);
            this.calibStat.Text = "toolStripStatusLabel1";
            // 
            // ImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(5, 5);
            this.AutoScrollMinSize = new System.Drawing.Size(200, 200);
            this.ClientSize = new System.Drawing.Size(977, 537);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.status);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ImageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImageForm";
            this.Activated += new System.EventHandler(this.ImageForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImageForm_FormClosing);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImgPanel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripStatusLabel statusStats;
        private System.Windows.Forms.ToolStripStatusLabel statusZoom;
        public System.Windows.Forms.StatusStrip status;
        public System.Windows.Forms.ToolStripStatusLabel StatusDirty;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox ImgPanel;
        private System.Windows.Forms.ToolStripStatusLabel calibStat;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace LinearIntersect
{
    public partial class ImageForm : Form
    {
        Image BaseImage;
        Image tmpImage;
        string imgPath;
        public Overlay CurOverlay = new Overlay();
        public float lineSize = 1f;
        private float contrast = 1f;
        private float _zoom = 1f;
        public string Zoom
        {
            get
            {
                return ((int)(_zoom * 100f)).ToString() + " %";
            }
            set
            {
                try
                {
                    int z = int.Parse(value.Replace(@"%", "").Trim());
                    _zoom = (float)(((float)z) / 100f);
                    CurOverlay.zoom = _zoom;
                    setScale();
                }
                catch
                {
                    Debug.WriteLine("Falsches Format in der Zoom-Eingabe");
                }
            }
        }
        public bool exportDone = false;
        public int Contrast
        {
            get
            {
                return ((int)(contrast * 255f));
            }
            set
            {
                contrast = ((float)((float)value / 255f));
                setContrast();
                Debug.WriteLine(contrast.ToString() + "gestartet");
            }
        }
        bool init = false;
        public mainForm prnt;

        public ImageForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            CurOverlay.PropertyChanged += new PropertyChangedEventHandler(CurOverlay_PropertyChanged);
        }

        void CurOverlay_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Refresh();
            Debug.WriteLine("PropertyChanged raised");
        }

        public void setImageFile(string ImageFile)
        {
            BaseImage = new Bitmap(Image.FromFile(ImageFile));


            this.Text = Path.GetFileName(ImageFile);

            imgPath = ImageFile;

            setScale();
            init = true;
        }

        public void setScale()
        {
            Size newSize = new Size((int)((float)BaseImage.Width * _zoom), (int)((float)BaseImage.Height * _zoom));
            this.ClientSize = newSize;
            CurOverlay.ImageSize = BaseImage.Size;

            //tmpImage = (Image)BaseImage.Clone();
            tmpImage = new Bitmap(BaseImage, newSize);

            CurOverlay.createGrid();
            
            this.Refresh();
        }

        public void setContrast()
        {
            float[][] ptsArray ={
                    new float[] {contrast, 0, 0, 0, 0}, // scale red
                    new float[] {0, contrast, 0, 0, 0}, // scale green
                    new float[] {0, 0, contrast, 0, 0}, // scale blue
                    new float[] {0, 0, 0, 1.0f, 0}, // don't scale alpha
                    new float[] {0, 0, 0, 0, 1f}};

            ImageAttributes iA = new ImageAttributes();
            iA.ClearColorMatrix();
            //iA.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            if (contrast <= 0.1f) contrast = 0.11f;
            iA.SetGamma(contrast);
            Graphics g = Graphics.FromImage(tmpImage);
            g.DrawImage(BaseImage, new Rectangle(0, 0, tmpImage.Width, tmpImage.Height)
                , 0, 0, BaseImage.Width, BaseImage.Height,
                GraphicsUnit.Pixel, iA);

            Refresh();
        }

        private void ImageForm_Paint(object sender, PaintEventArgs e)
        {
            redraw(e.Graphics);
        }

        private void redraw(Graphics go)
        {
            if (!init) return;

            go.DrawImage(tmpImage, new Rectangle(0, 0, tmpImage.Width, tmpImage.Height));

            int crossSize = 10;
            Point pp = CurOverlay.previewPoint;

            switch (CurOverlay.Orientation)
            {
                case GridOrientation.Horizontal:
                    foreach (int g in CurOverlay.GridPosition)
                    {
                        go.DrawLine(
                            new Pen(prnt.Data.GridColor, lineSize),
                            0,
                            (int)((float)g * _zoom),
                            tmpImage.Width,
                            (int)((float)g * _zoom)
                            );
                    }

                    go.DrawLine(
                        new Pen(prnt.Data.CursorColor, lineSize),
                        (int)((float)pp.X * _zoom),
                        (int)((float)pp.Y * _zoom) - crossSize,
                        (int)((float)pp.X * _zoom),
                        (int)((float)pp.Y * _zoom) + crossSize
                        );

                    foreach (Point p in CurOverlay.Points)
                    {
                        go.DrawLine(
                            new Pen(prnt.Data.PointColor, lineSize),
                            (int)((float)p.X * _zoom),
                            (int)((float)p.Y * _zoom) - crossSize,
                            (int)((float)p.X * _zoom),
                            (int)((float)p.Y * _zoom) + crossSize
                            );
                    }
                    break;

                case GridOrientation.Vertical:
                    foreach (int g in CurOverlay.GridPosition)
                    {
                        go.DrawLine(
                            new Pen(prnt.Data.GridColor, lineSize),
                            (int)((float)g * _zoom),
                            0,
                            (int)((float)g * _zoom),
                            tmpImage.Height
                            );
                    }

                    go.DrawLine(
                        new Pen(prnt.Data.CursorColor, lineSize),
                        (int)((float)pp.X * _zoom) - crossSize,
                        (int)((float)pp.Y * _zoom),
                        (int)((float)pp.X * _zoom) + crossSize,
                        (int)((float)pp.Y * _zoom)
                        );

                    foreach (Point p in CurOverlay.Points)
                    {
                        go.DrawLine(
                            new Pen(prnt.Data.PointColor, lineSize),
                            (int)((float)p.X * _zoom) - crossSize,
                            (int)((float)p.Y * _zoom),
                            (int)((float)p.X * _zoom) + crossSize,
                            (int)((float)p.Y * _zoom)
                            );
                    }
                    break;
            }
        }

        private void ImageForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                CurOverlay.AddPoint(new Point(
                    (int)((float)(e.X - this.ClientRectangle.X) / _zoom),
                    (int)((float)(e.Y - this.ClientRectangle.Y) / _zoom)
                    ));
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                CurOverlay.removePointAt(new Point(
                    (int)((float)(e.X - this.ClientRectangle.X) / _zoom),
                    (int)((float)(e.Y - this.ClientRectangle.Y) / _zoom)
                    ));

            exportDone = false;
            prnt.dataDirty(true);
            this.Refresh();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }

        private void ImageForm_MouseMove(object sender, MouseEventArgs e)
        {
            CurOverlay.PreviewPoint(new Point(
                    (int)((float)(e.X - this.ClientRectangle.X) / _zoom),
                    (int)((float)(e.Y - this.ClientRectangle.Y) / _zoom)
                ));
            this.Refresh();
        }

        private int CalcContrastColor(int crBg)
        {
            if (
                Math.Abs(((crBg) & 0xFF) - 0x80) <= 0x40 &&
                Math.Abs(((crBg >> 8) & 0xFF) - 0x80) <= 0x40 &&
                Math.Abs(((crBg >> 16) & 0xFF) - 0x80) <= 0x40

            ) return (0x7F7F7F + crBg) & 0xFFFFFF;

            else return crBg ^ 0xFFFFFF;
        }

        public void exportData()
        {
            Debug.WriteLine("Export gestartet");
            List<string> output = new List<string>();
            int i = 1;
            string nl = Environment.NewLine;
            Dictionary<int, List<int>> LineInfo = CurOverlay.getLineInfo();

            output.Add("\"File Name:\" \"" + Path.GetFileName(imgPath) + "\"");
            output.Add("\"Resolution:\" \"" + BaseImage.Width + "x" + BaseImage.Height + "\"");
            output.Add("\"Orientation:\" \"" + CurOverlay.Orientation.ToString() + "\"");
            output.Add("\"Distance:\" \"" + CurOverlay.DSTString + "\"");
            output.Add("\"Start:\" \"" + CurOverlay.StartString + "\"" + Environment.NewLine);
            output.Add("Nummer\tSehnenlänge");

            foreach (KeyValuePair<int, List<int>> info in LineInfo)
            {
                int start = 0;
                info.Value.Sort();
                foreach (int pos in info.Value)
                {
                    if (start == 0)
                    {
                        start = pos;
                        continue;
                    }
                    output.Add(string.Format("{0}\t{1}", i, pos - start));
                    start = pos;
                    i++;
                }
            }

            File.WriteAllLines(
                Path.GetDirectoryName(imgPath) +
                Path.DirectorySeparatorChar +
                Path.GetFileNameWithoutExtension(imgPath) +
                ((CurOverlay.Orientation == GridOrientation.Horizontal) ? "_h" : "_v") +
                ".txt"
                , output);
            exportDone = true;
        }

        private void ImageForm_Activated(object sender, EventArgs e)
        {
            //MessageBox.Show("fdf");
            prnt.activeImage = this;
        }

        private void ImageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CurOverlay.Points.Count > 0 && !exportDone)
            {
                if (MessageBox.Show(
                    "Daten nicht exportiert, wirklich schließen?",
                    "Schließen...",
                    MessageBoxButtons.YesNoCancel
                    ) != System.Windows.Forms.DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }

            }
            prnt.removeImage(this);
        }
    }
}

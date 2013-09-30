using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace LinearIntersect
{
    public partial class mainForm : Form
    {
        List<ImageForm> ImageForms = new List<ImageForm>();
        private ImageForm _aI;
        public Settings SetFrm;
        public SettingsData Data = new SettingsData();
        public ImageForm activeImage
        {
            get
            {
                return _aI;
            }
            set
            {
                bool refresh = false;
                if (!value.Equals(_aI)) refresh = true;
                _aI = value;
                if (refresh) onActiveImageChanged();
            }
        }
        private string[] zoomLevels = { "25 %", "50 %", "100 %", "150 %", "200 %" };

        public mainForm()
        {
            InitializeComponent();

            Properties.Settings.Default.Reload();
            if (Properties.Settings.Default.Calibrations != null)
                Data.Calibration = Properties.Settings.Default.Calibrations;
            Data.GridColor = Properties.Settings.Default.ColorGrid;
            Data.PointColor = Properties.Settings.Default.ColorPoint;
            Data.CursorColor = Properties.Settings.Default.ColorCursor;
            Data.DefaultDistance = Properties.Settings.Default.GridDistance;
            Data.DefaultStart = Properties.Settings.Default.GridStart;
            Data.DefaultDir = Properties.Settings.Default.GridDir;


            comboBox1.Items.Clear();
            comboBox1.DataSource = Enum.GetValues(typeof(GridOrientation));
            comboBoxZoom.DataSource = zoomLevels;

            comboBoxCalib.DataSource = Data.Calibration;
            //comboBoxCalib.DataBindings.Add(new Binding("SelectedItem", Data, "Calibration"));
            comboBoxCalib.DisplayMember = "Key";
            comboBoxCalib.ValueMember = "Value";

            lockControls();

            Data.PropertyChanged += new PropertyChangedEventHandler(Data_PropertyChanged);

            Debug.WriteLine("gestartet");
        }

        void Data_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            foreach (ImageForm ifrm in ImageForms)
            {
                ifrm.Refresh();
            }
        }

        private void onActiveImageChanged()
        {
            comboBox1.DataBindings.Clear();
            comboBox1.DataBindings.Add("SelectedItem", activeImage.CurOverlay, "Orientation", false, DataSourceUpdateMode.Never);
            comboBox1.DataBindings.Add("SelectedValue", activeImage.CurOverlay, "Orientation", true, DataSourceUpdateMode.Never);

            textBoxDist.DataBindings.Clear();
            textBoxDist.DataBindings.Add("Text", activeImage.CurOverlay, "DSTString", true, DataSourceUpdateMode.Never);

            textBoxStart.DataBindings.Clear();
            textBoxStart.DataBindings.Add("Text", activeImage.CurOverlay, "StartString", true, DataSourceUpdateMode.Never);

            trackBar1.Minimum = 0;
            trackBar1.Maximum = 700;
            trackBar1.DataBindings.Clear();
            trackBar1.DataBindings.Add("Value", activeImage, "Contrast", false, DataSourceUpdateMode.OnPropertyChanged);

            comboBoxZoom.DataBindings.Clear();
            comboBoxZoom.DataBindings.Add("Text", activeImage, "Zoom", false, DataSourceUpdateMode.OnValidation);

            comboBoxCalib.SelectedItem = null;
            comboBoxCalib.SelectedItem = activeImage.CurOverlay.Calibration;

            Debug.WriteLine("Binding erneuert auf " + activeImage.Text);
        }

        private void AddImage(string path)
        {
            try
            {
                Image tmp = Image.FromFile(path);
            }
            catch
            {
                MessageBox.Show("Keine gültige Bilddatei");
                return;
            }

            ImageForm tmpFrm = new ImageForm();
            tmpFrm.prnt = this;
            tmpFrm.setImageFile(path);
            tmpFrm.CurOverlay.DSTString = Data.DefaultDistance;
            tmpFrm.CurOverlay.StartString = Data.DefaultStart;
            tmpFrm.CurOverlay.Orientation = Data.DefaultDir;
            tmpFrm.CurOverlay.createGrid();
            tmpFrm.Show();
            activeImage = tmpFrm;
            unlockControls();

            ImageForms.Add(tmpFrm);
        }

        private void File_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void File_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string f in files)
            {
                if (File.Exists(f))
                {
                    AddImage(f);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (activeImage == null) return;
            Debug.WriteLine("Export geclickt");
            DialogResult DR;
            if (activeImage.CurOverlay.Calibration == 1f)
            {
                DR = MessageBox.Show("Kein Kalibrationsfaktor eingestellt. Fortfahren?", "Ohne Kalibrierung exportieren?", MessageBoxButtons.YesNoCancel);

                if (DR != System.Windows.Forms.DialogResult.Yes)
                    return;
            }
            activeImage.exportData();
            dataDirty(false);
        }

        public void dataDirty(bool State)
        {
            //if (State)
            //{
            //    buttonSetCalib.Text = "Auswerten";
            //    buttonSetCalib.Enabled = true;
            //}
            //else
            //{
            //    buttonSetCalib.Text = "Fertig";
            //    buttonSetCalib.Enabled = false;
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            setValues();
        }

        public void setValues()
        {
            activeImage.CurOverlay.Orientation = (GridOrientation)comboBox1.SelectedItem;
            activeImage.CurOverlay.DSTString = textBoxDist.Text;
            activeImage.CurOverlay.StartString = textBoxStart.Text;
            activeImage.setContrast();
            activeImage.CurOverlay.clear();
        }

        public void lockControls()
        {
            panel1.Enabled = false;
        }

        public void unlockControls()
        {
            panel1.Enabled = true;
        }

        public void removeImage(ImageForm ifrm)
        {
            Debug.WriteLine("Form closing");
            ImageForms.Remove(ifrm);
            ifrm.Dispose();
            if (ImageForms.Count > 0)
            {
                ImageForms[0].Activate();
                Debug.WriteLine(ImageForms[0].Text + "Activated");
            }
            else
            {
                lockControls();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Punkte löschen?", "Leeren...", MessageBoxButtons.YesNoCancel)
                == System.Windows.Forms.DialogResult.Yes)
            {
                activeImage.CurOverlay.Points.Clear();
                activeImage.CurOverlay.clear();
            }
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            List<ImageForm> tmpLst = new List<ImageForm>(ImageForms);
            foreach (ImageForm ifrm in tmpLst)
            {
                ifrm.Close();
            }

            if (ImageForms.Count > 0)
            {
                e.Cancel = true;
                return;
            }

            Properties.Settings.Default.ColorGrid = Data.GridColor;
            Properties.Settings.Default.ColorPoint = Data.PointColor;
            Properties.Settings.Default.ColorCursor = Data.CursorColor;
            Properties.Settings.Default.GridDistance = Data.DefaultDistance;
            Properties.Settings.Default.GridStart = Data.DefaultStart;
            Properties.Settings.Default.GridDir = Data.DefaultDir;
            Properties.Settings.Default.Calibrations = Data.Calibration;
            Properties.Settings.Default.Save();
        }

        private void comboBoxZoom_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBoxZoom.DataBindings.Count > 0)
            {
                comboBoxZoom.DataBindings[0].WriteValue();
                Debug.WriteLine("Zoom Selection change commited ");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetFrm = new Settings();
            SetFrm.setData(Data);
            SetFrm.Show(this);
        }

        private void buttonSetCalib_Click(object sender, EventArgs e)
        {
            CalibrationConfig Cfrm = new CalibrationConfig();
            Cfrm.TopMost = true;
            Cfrm.MainForm = this;
            Cfrm.Show();
        }

        private void comboBoxCalib_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCalib.SelectedItem == null)
                Debug.WriteLine("ist null");
            try
            {
                Debug.WriteLine(comboBoxCalib.SelectedText + activeImage.CurOverlay.Calibration.ToString());
                activeImage.CurOverlay.Calibration = ((CalibrationSet)comboBoxCalib.SelectedItem).Value;
            }
            catch
            {

            }
        }

        private void öffnenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = getSupportedCodecs();
            openFileDialog1.FilterIndex = 0;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Debug.WriteLine("Adding " + openFileDialog1.FileNames.Length + " Files");
                foreach (string p in openFileDialog1.FileNames)
                {
                    Debug.WriteLine("Adding File " + p);
                    AddImage(p);
                }
            }
            else
            {
                return;
            }
        }

        private string getSupportedCodecs()
        {
            string res = "";
            string tmp = "";
            string allext = "";

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            foreach (var c in codecs)
            {
                string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                tmp = String.Format("{0}{1}{2} ({3})|{3}", tmp, "|", codecName, c.FilenameExtension);
                allext += c.FilenameExtension + ";";
            }

            res = String.Format("{0}{1}{2} ({3})|{3}", res, "", "Alle Bilder", allext);
            res += tmp;
            res = String.Format("{0}{1}{2} ({3})|{3}", res, "|", "Alle Dateien", "*.*");

            return res;
        }
    }
}

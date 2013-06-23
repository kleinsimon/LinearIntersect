using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace LinearIntersect
{
    public partial class CalibrationConfig : Form
    {
        private bool measuring = false;
        ResizableControl KalibTool;
        private mainForm _mainfrm;
        public mainForm MainForm
        {
            get
            {
                return _mainfrm;
            }
            set
            {
                _mainfrm = value;
                refreshBinding();
            }
        }

        public CalibrationConfig()
        {
            InitializeComponent();
        }

        private void refreshBinding()
        {
            dGVCalib.AutoGenerateColumns = false;
            dGVCalib.DataSource = MainForm.Data.Calibration;

            DataGridViewTextBoxColumn name = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn value = new DataGridViewTextBoxColumn();
            DataGridViewComboBoxColumn opt = new DataGridViewComboBoxColumn();

            name.DataPropertyName = "Key";
            name.Name = "Name";
            name.ValueType = typeof(string);
            name.SortMode = DataGridViewColumnSortMode.Automatic;

            value.DataPropertyName = "Value";
            value.Name = "Wert";
            value.ValueType = typeof(float);
            value.SortMode = DataGridViewColumnSortMode.Automatic;

            opt.DataPropertyName = "Opt";
            opt.Name = "Einheit";
            opt.ValueType = typeof(Units);
            opt.DataSource = Enum.GetValues(typeof(Units));
            opt.SortMode = DataGridViewColumnSortMode.Automatic;

            dGVCalib.Columns.Add(name);
            dGVCalib.Columns.Add(value);
            dGVCalib.Columns.Add(opt);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dGVCalib.EndEdit();
            dGVCalib.CommitEdit(DataGridViewDataErrorContexts.Commit);

            dGVCalib.BindingContext[dGVCalib.DataSource].EndCurrentEdit();
        }

        private void dGVCalib_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[0].Value = "Neu";
            e.Row.Cells[1].Value = 1f;
            e.Row.Cells[2].Value = Units.µm;
        }

        private void CreateCalibBar()
        {
            KalibTool = new ResizableControl();
            KalibTool.Parent = MainForm.activeImage;
            MainForm.activeImage.StatusDirty.Text = "Balken zum Messen positionieren, doppelklick für vertikal";
            KalibTool.Left = 100;
            KalibTool.Top = 100;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            float tmp;
            if (!measuring)
            {
                CreateCalibBar();
                textBoxSoll.Text = "";
                textBoxSoll.Visible = true;
                label1.Visible = true;
                measuring = true;
                button2.Text = @"Wert übernehmen";
            }
            else if (measuring && float.TryParse(textBoxSoll.Text, out tmp))
            {
                dGVCalib.Rows[dGVCalib.SelectedCells[0].RowIndex].Cells[1].Value = tmp / (float) KalibTool.Length;

                KalibTool.Dispose();
                measuring = false;
                textBoxSoll.Visible = false;
                label1.Visible = false;
                button2.Text = @"Kalibrierung messen";
            }
            else
            {
                //textBoxSoll.BackColor = Color.Orange;
            }
        }

        private void textBoxSoll_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) ||
                char.IsSymbol(e.KeyChar) ||
                char.IsWhiteSpace(e.KeyChar) ||
                char.IsPunctuation(e.KeyChar))
                e.Handled = true;
        }

        private void CalibrationConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            dGVCalib.EndEdit();
            dGVCalib.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dGVCalib_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dGVCalib.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
    }
}

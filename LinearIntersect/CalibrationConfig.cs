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

            name.DataPropertyName = "Key";
            name.Name = "Name";
            name.ValueType = typeof(string);
            name.SortMode = DataGridViewColumnSortMode.Automatic;

            value.DataPropertyName = "Value";
            value.Name = "Wert";
            value.ValueType = typeof(float);
            value.SortMode = DataGridViewColumnSortMode.Automatic;

            dGVCalib.Columns.Add(name);
            dGVCalib.Columns.Add(value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dGVCalib.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dGVCalib_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[0].Value = "Neu";
            e.Row.Cells[1].Value = 0f;
        }
    }
}

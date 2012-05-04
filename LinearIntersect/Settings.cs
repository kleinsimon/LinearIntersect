using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LinearIntersect
{
    public partial class Settings : Form
    {
        SettingsData Data;
        public Settings()
        {
            InitializeComponent();
        }

        public void setData(SettingsData data)
        {
            Data = data;

            colorButtonGrid.DataBindings.Clear();
            colorButtonPoint.DataBindings.Clear();
            colorButtonCursor.DataBindings.Clear();
            textBoxDistance.DataBindings.Clear();
            textBoxStart.DataBindings.Clear();
            comboBoxDir.DataBindings.Clear();

            colorButtonGrid.DataBindings.Add("CurrentColor", Data, "GridColor", false, DataSourceUpdateMode.OnPropertyChanged);
            colorButtonPoint.DataBindings.Add("CurrentColor", Data, "PointColor", false, DataSourceUpdateMode.OnPropertyChanged);
            colorButtonCursor.DataBindings.Add("CurrentColor", Data, "CursorColor", false, DataSourceUpdateMode.OnPropertyChanged);
            textBoxDistance.DataBindings.Add("Text", Data, "DefaultDistance", false, DataSourceUpdateMode.OnPropertyChanged);
            textBoxStart.DataBindings.Add("Text", Data, "DefaultStart", false, DataSourceUpdateMode.OnPropertyChanged);
            comboBoxDir.DataSource = Enum.GetValues(typeof(GridOrientation));
            comboBoxDir.DataBindings.Add("SelectedItem", Data, "DefaultDir", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxDir_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBoxDir.DataBindings.Count > 0)
                comboBoxDir.DataBindings[0].WriteValue();
        }
    }
}

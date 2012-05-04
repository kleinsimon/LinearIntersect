using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LinearIntersect
{
    public partial class ColorButton : UserControl
    {
        public Color CurrentColor
        {
            get
            {
                return button1.BackColor;
            }
            set
            {
                button1.BackColor = value;
                NotifyPropertyChanged("CurrentColor");
            }
        }

        public string Label
        {
            get
            {
                return button1.Text;
            }
            set
            {
                button1.Text = value;
            }
        }
        public ColorButton()
        {
            InitializeComponent();
        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = this.CurrentColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                CurrentColor = colorDialog1.Color;
                if (DataBindings.Count > 0)
                    DataBindings[0].WriteValue();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}

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
        public string Label
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
            }
        }
        public ColorButton()
        {
            InitializeComponent();
            
        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = this.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.BackColor = colorDialog1.Color;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.InvokeOnClick(this, e);
        }
    }
}

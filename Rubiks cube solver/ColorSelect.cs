using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rubiks_cube_solver_app
{
    public partial class ColorSelect : Form
    {
        public ColorSelect()
        {
            InitializeComponent();
        }

        private void ColorSelect_Load(object sender, EventArgs e)
        {


        }

        public Color ColorSelected;

        private void Green_Click(object sender, EventArgs e)
        {
            ColorSelected = Color.Green;
            DialogResult = DialogResult.OK;
            
        }

        private void Orange_Click(object sender, EventArgs e)
        {
            ColorSelected = Color.Orange;
            DialogResult = DialogResult.OK;
        }

        private void Blue_Click(object sender, EventArgs e)
        {
            ColorSelected = Color.Blue;
            DialogResult = DialogResult.OK;
        }

        private void Yellow_Click(object sender, EventArgs e)
        {
            ColorSelected = Color.Yellow;
            DialogResult = DialogResult.OK;
        }

        private void Red_Click(object sender, EventArgs e)
        {
            ColorSelected = Color.Red;
            DialogResult = DialogResult.OK;
        }

        private void White_Click(object sender, EventArgs e)
        {
            ColorSelected = Color.White;
            DialogResult = DialogResult.OK;
        }
    }
}

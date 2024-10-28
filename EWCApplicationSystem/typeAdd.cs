using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EWMCalculationSoftware
{
    public partial class typeAdd : Form
    {
        public typeAdd()
        {
            InitializeComponent();
        }
        public string zbmc { set; get; }
        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (tb_zbmc.Text == "")
            {
                MessageBox.Show("indicator type cannot be empty!");
            }
            else
            {
                zbmc = tb_zbmc.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}

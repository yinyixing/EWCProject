using EWCLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EWCApplicationSystem
{
    public partial class Sij_Rij : Form
    {
        public Sij_Rij()
        {
            InitializeComponent();
        }
        private int sample_count = 0;
        public Sij_Rij(int sampleCount)
        {
            InitializeComponent();
            sample_count = sampleCount;
            if (sample_count == 1)
            {
                groupBox2.Visible = false;
                groupBox1.Location =new Point(23, 12);
                btn_cancel.Location = new Point(560, 335);
                btn_dm.Location = new Point(760, 335);
                this.Height = 420;
            }
        }
        private void Sij_Rij_Load(object sender, EventArgs e)
        {
            if (GlobalMethods.sij_rij != "")
            {
                if (GlobalMethods.sij_rij == "Sij")
                {
                    rdb_Sij.Checked = true;
                }
                else if (GlobalMethods.sij_rij == "Rij")
                {
                    rdb_Rij.Checked = true;
                }
            }
        }
        private void btn_dm_Click(object sender, EventArgs e)
        {
            if (rb_no.Checked == true)
            {
                GlobalMethods.is_avg = false;
            }
            else if (rb_yes.Checked == true)
            {
                GlobalMethods.is_avg = true;
            }
            if (rdb_Rij.Checked == true)
            {
                GlobalMethods.sij_rij = "Rij";
            }
            else if (rdb_Sij.Checked == true)
            {
                GlobalMethods.sij_rij = "Sij";
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

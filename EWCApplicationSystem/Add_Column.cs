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
    public partial class Add_Column : Form
    {
        public Add_Column()
        {
            InitializeComponent();
        }
        public string columnName { set; get; }
        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (txt_cName.Text == "")
            {
                MessageBox.Show("'Column Name' cannot be empty!");
            }
            else
            {
                columnName = txt_cName.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}

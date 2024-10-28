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
    public partial class OperatingSteps : Form
    {
        public OperatingSteps()
        {
            InitializeComponent();
        }
        string msg = "";
        public OperatingSteps(string msgTip)
        {
            InitializeComponent();
            msg = msgTip;
        }
        private void OperatingSteps_Load(object sender, EventArgs e)
        {
            lbl_msg.Text = "Incorrect operation:" + msg.ToString();
        }
    }
}

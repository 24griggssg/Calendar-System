using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calendar_System
{
    public partial class Manager : Form
    {
        public Manager(int EmployeeID, Database db)
        {
            InitializeComponent();
        }

        private void tableLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button_Click(object sender, EventArgs e) {
            Button button = (Button)sender;

            int day = Int32.Parse(button.Name.Substring(6));

            MessageBox.Show("Selected Day " + day);
        }
    }
}

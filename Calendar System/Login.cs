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
    public partial class Login : Form
    {
        public String username = "Admin";
        public String password = "Password";
        public bool isManager = false;
        public const int EmployeeID = 1;

        public Login()
        {
            InitializeComponent();
            //get values from database here and set username, password, and isManager
        }
        //login button
        private void confirmButton_Click(object sender, EventArgs e)
        {
            String inputUsername = textBox1.Text;
            String inputPassword = textBox2.Text;

            if (inputUsername == username && inputPassword == password)
            {
                if (isManager)
                {
                    Manager managerForm = new Manager(EmployeeID);
                    managerForm.Show();
                    this.Hide();
                }
                else
                {
                    Employee employeeForm = new Employee(EmployeeID);
                    employeeForm.Show();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Incorrect username or password. Please try again.");
                textBox1.Clear();
                textBox2.Clear();
            }
        }
    }
}

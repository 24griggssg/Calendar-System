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
        public Database db = new Database();

        public Login()
        {
            InitializeComponent();
        }
        //login button
        //author: Harrison Mesplay
        private void confirmButton_Click(object sender, EventArgs e)
        {
            String inputUsername = textBox1.Text;
            String inputPassword = textBox2.Text;
            //calls the Login method in the Database class, which returns a tuple with three values: success, isManager, and employeeID
            var result = db.Login(inputUsername, inputPassword);
            if (result.success)
            {
                //loads the appropriate form based on whether the user is a manager or not
                if (result.isManager)
                {
                    Manager managerForm = new Manager(result.userID, db);
                    managerForm.Show();
                    this.Hide();
                }
                else
                {
                    Employee employeeForm = new Employee(result.userID, db);
                    employeeForm.Show();
                    this.Hide();
                }
            }
            //if login fails, show error message and clear textboxes
            else
            {
                MessageBox.Show("Incorrect username or password. Please try again.");
                textBox1.Clear();
                textBox2.Clear();
            }
        }
    }
}

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
    public partial class Employee : Form
    {
        private int currentYear = 2026;
        private int currentMonth = 3;
        public Employee()
        {
            InitializeComponent();
            LoginBox.Show();
        }
        private void logoutButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Confirm", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                //delete meeting code here
                MessageBox.Show("Logout Sucessful!");
                this.Close();
            }
        }
        private void confirmButton_Click(object sender, EventArgs e)
        {   
            LoginBox.Hide();
            selectedMeetingBox.Hide();
            addEventBox.Hide();
            editEventBox.Hide();
            meetingsBox.Show();
        }
        private void DateButton_Click(object sender, EventArgs e)
        {
            //button ref
            Button clicked = sender as Button;
            //if empty, return
            if (clicked.Text == "")
            {
                return;
            }
            //convert text to day
            int day = Convert.ToInt32(clicked.Text);
            //create date from day, month, year
            DateTime selectedDate = new DateTime(currentYear, currentMonth, day);
            //update selected date label
            selectedDateL.Text = selectedDate.ToString("M/d/yyyy");
            selectedDateL2.Text = selectedDate.ToString("M/d/yyyy");
            editingDateL.Text = selectedDate.ToString("M/d/yyyy");
            addingDateL.Text = selectedDate.ToString("M/d/yyyy");
            //show/hide boxes
            selectedMeetingBox.Hide();
            meetingsBox.Show();
        }
        private void meeting1Button_Click(object sender, EventArgs e)
        {
            meetingsBox.Hide();
            addEventBox.Hide();
            editEventBox.Hide();
            selectedMeetingBox.Show();
        }
        private void addEventB_Click(object sender, EventArgs e)
        {
            meetingsBox.Hide();
            selectedMeetingBox.Hide();
            editEventBox.Hide();
            addEventBox.Show();
        }
        private void editEventB_Click(object sender, EventArgs e)
        {
            selectedMeetingBox.Hide();
            meetingsBox.Hide();
            addEventBox.Hide();
            editEventBox.Show();
        }
        private void deleteEventB_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this meeting?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) {
                //delete meeting code here
                MessageBox.Show("Meeting deleted.");
            }
        }
        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }
    }
}

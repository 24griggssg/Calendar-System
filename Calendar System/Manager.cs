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
        int ID;
        Database db;

        int chosenDay;
        int chosenMonth = 3;
        int chosenYear = 2026;
        int chosenHour;
        int chosenMinute;



        public Manager(int EmployeeID, Database db)
        {
            this.ID = EmployeeID;
            this.db = db;

            InitializeComponent();
            newEventPanel.Hide();
        }

        private void tableLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button_Click(object sender, EventArgs e) {
            /*  Method: button_Click        
             *      Generic method for all the days in the Manager Calendar. 
             *      Grabs the day using the .Text parameter of the pressed Button.
             *  Author: Samuel Griggs
             */
            Button button = (Button)sender;

            chosenDay = Int32.Parse(button.Text);

            selectedDateL.Text = "3/" + chosenDay + "/26";
        }

        private void addEventB_Click(object sender, EventArgs e) {
            Event ev = new Event();
            newEventPanel.Show();
        }

        private void finishEventButton_Click(object sender, EventArgs e) {
            try {
                /*  Method: finishEventButton_Click
                 *      Takes input from a Manager for the creation of an event.
                 *      Creates an event in the database using the acquired values.
                 *  Author: Samuel Griggs
                 */

                if (!eventNameBox.Text.Equals("") && !startTimeBox.Text.Equals("") && !endTimeBox.Text.Equals("") && Int32.Parse(endTimeBox.Text) > Int32.Parse(startTimeBox.Text)) {
                    string name = eventNameBox.Text;
                    string desc = descriptionBox.Text;

                    chosenHour = Int32.Parse(startTimeBox.Text) / 100;
                    chosenMinute = Int32.Parse(startTimeBox.Text) % 100;
                    DateTime startTime = new DateTime(chosenYear, chosenMonth, chosenDay, chosenHour, chosenMinute, 0);

                    chosenHour = Int32.Parse(endTimeBox.Text) / 100;
                    chosenMinute = Int32.Parse(endTimeBox.Text) % 100;
                    DateTime endTime = new DateTime(chosenYear, chosenMonth, chosenDay, chosenHour, chosenMinute, 0);

                    db.AddEvent(ID, name, startTime, endTime, desc);

                    newEventPanel.Hide();
                }
                else
                    MessageBox.Show("Name and times are required. End time must be later than start.");
            }
            catch (FormatException) {
                MessageBox.Show("One or both times are Not a Number");
            }
            catch (Exception ex) {

                throw;
            }
        }

        private void cancelNewEventButton_Click(object sender, EventArgs e) {
            eventNameBox.Text = "";
            startTimeBox.Text = "";
            endTimeBox.Text = "";
            descriptionBox.Text = "";

            newEventPanel.Hide();
        }
    }
}

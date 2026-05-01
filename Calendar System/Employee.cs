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
        //calendar buttons
        private List<Button> calendarButtons = new List<Button>();
        //current date
        private DateTime currentDateTime = DateTime.Now;
        //user selected date to view meetings
        private DateTime selectedDate;
        private List<Event> allEvents = new List<Event>();
        public Employee(int EmployeeID)
        {
            InitializeComponent();
            selectedDateL.Text = currentDateTime.ToString("M/d/yyyy");

            //organize group boxes
            selectedMeetingBox.Hide();
            addEventBox.Hide();
            editEventBox.Hide();
            selectedDayBox.Show();

            //create calendar buttons
            BuildCalendar();

            //update mainmenu
            this.Load += MainMenu_Load;
        }
        //load meetings for current day
        private void MainMenu_Load(object sender, EventArgs e)
        {
            selectedDate = currentDateTime.Date;
            UpdateCalendar();
            LoadMeetingsForSelectedDate();
        }
        //create calendar buttons
        private void BuildCalendar()
        {
            calendarButtons.Clear();

            foreach (Control c in calendarPanel.Controls)
            {
                if (c is Button)
                {
                    calendarButtons.Add((Button)c);
                }
            }

            //reverse list to match calendar layout (Sunday button is last in panel but should be first in list)
            calendarButtons.Reverse();
            DateTime firstDay = new DateTime(currentDateTime.Year, currentDateTime.Month, 1);

            int startIndex = (int)firstDay.DayOfWeek;
            int daysInMonth = DateTime.DaysInMonth(currentDateTime.Year, currentDateTime.Month);

            //clear buttons
            foreach (var btn in calendarButtons)
            {
                btn.Text = "";
                btn.Enabled = false;
            }

            //set day numbers
            for (int i = 0; i < daysInMonth; i++)
            {
                int buttonIndex = startIndex + i + 1;

                if (buttonIndex >= calendarButtons.Count)
                    break;

                calendarButtons[buttonIndex].Text = (i + 1).ToString();
                calendarButtons[buttonIndex].Enabled = true;
            }

            //update month label
            monthL.Text = currentDateTime.ToString("MMMM yyyy");
        }
        private void UpdateCalendar()
        {
            foreach (var btn in calendarButtons)
            {
                if (btn.Text == "")
                {
                    btn.BackColor = Color.White;
                    continue;
                }

                int day = Convert.ToInt32(btn.Text);

                DateTime thisDate = new DateTime(currentDateTime.Year, currentDateTime.Month, day);

                //color rules
                if (thisDate.Date == selectedDate.Date)
                {
                    btn.BackColor = Color.LightGray;
                }
                else if (HasEventOnThisDate(thisDate))
                {
                    btn.BackColor = Color.LightBlue;
                }
                else
                {
                    btn.BackColor = Color.White;
                }
            }
        }
        //checks if event on date
        private bool HasEventOnThisDate(DateTime date)
        {
            return allEvents.Any(e => e.StartTime.Date == date.Date);
        }
        //load meetings for selected date when date button is clicked
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
            selectedDate = new DateTime(currentDateTime.Year, currentDateTime.Month, day);

            //update selected date label
            selectedDateL.Text = selectedDate.ToString("M/d/yyyy");
            selectedDateL2.Text = selectedDate.ToString("M/d/yyyy");
            editingDateL.Text = selectedDate.ToString("M/d/yyyy");
            addingDateL.Text = selectedDate.ToString("M/d/yyyy");

            //update
            UpdateCalendar();
            LoadMeetingsForSelectedDate();

            //show/hide boxes
            selectedMeetingBox.Hide();
            addEventBox.Hide(); 
            editEventBox.Hide();
            selectedDayBox.Show();
        }
        private void LoadMeetingsForSelectedDate()
        {
            //meetings from db (temp test data for now)
            List<Event> events = new List<Event>();

            //find meetings on selected date
            foreach (Event e in allEvents)
            {
                if (e.StartTime.Date == selectedDate.Date)
                {
                    events.Add(e);
                }
            }
            //clear previous buttons
            meetingListBox.Controls.Clear();

            foreach (Event e in events)
            {
                //create button for each meeting
                Button btn = new Button();
                //title
                btn.Text = $"{e.Title}\n{e.StartTime:h:mm tt} - {e.EndTime:h:mm tt}";

                //position, style, font, etc.
                btn.UseVisualStyleBackColor = true;
                btn.Height = 60;
                btn.Dock = DockStyle.Top;
                btn.Font = new Font("Microsoft Sans Serif", 14.25F);
                btn.TextAlign = ContentAlignment.MiddleLeft;
                btn.Margin = new Padding(3, 3, 3, 3);

                //store event in tag
                btn.Tag = e;

                //add click event to load meeting details
                btn.Click += meetingButton_Click;

                //add button to panel
                meetingListBox.Controls.Add(btn);
            }
        }
        private void meetingButton_Click(object sender, EventArgs e)
        {
            Button clicked = sender as Button;
            Event selectedEvent = clicked.Tag as Event;
            showMeetingsDetails(selectedEvent);
        }
        private void showMeetingsDetails(Event e)
        {
            //switch panels
            selectedDayBox.Hide();
            addEventBox.Hide();
            editEventBox.Hide();
            selectedMeetingBox.Show();

            //update labels with meeting details
            TitleL2.Text = e.Title;
            selectedDateL2.Text = e.StartTime.ToString("M/d/yyyy");
            startTimeD.Text = e.StartTime.ToString("hh:mm tt");
            endTimeD.Text = e.EndTime.ToString("hh:mm tt");
            DescriptionD.Text = e.Description;
        }
        //sends user to add event panel
        private void addEventB_Click(object sender, EventArgs e)
        {
            selectedDayBox.Hide();
            selectedMeetingBox.Hide();
            editEventBox.Hide();
            addEventBox.Show();
        }
        //user clicks confirm after filling out add event form
        private void saveEventBtn_Click(object sender, EventArgs e)
        {
            // get values from inputs
            string title = AddTitleText.Text;
            string description = AddDescText.Text;
            //time
            DateTime startInput = DateTime.Parse(AddStartText.Text);
            DateTime endInput = DateTime.Parse(AddEndText.Text);

            DateTime startTime = selectedDate.Date.Add(startInput.TimeOfDay);
            DateTime endTime = selectedDate.Date.Add(endInput.TimeOfDay);

            //title validation
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Title is required.");
                return;
            }
            //time validation
            if (endTime <= startTime)
            {
                MessageBox.Show("End time must be after start time.");
                return;
            }
            //can schedule (no time conflicts)
            // check for time conflicts
            foreach (Event ev in allEvents)
            {
                if (ev.StartTime.Date == selectedDate.Date)
                {
                    if (startTime < ev.EndTime && endTime > ev.StartTime)
                    {
                        MessageBox.Show("Time conflict with another event.");
                        return;
                    }
                }
            }
            //create event
            Event newEvent = new Event
            {
                EventID = allEvents.Count + 1,
                Title = title,
                StartTime = startTime,
                EndTime = endTime,
                Description = description
            };

            //store it
            allEvents.Add(newEvent);

            //refresh UI
            UpdateCalendar();
            LoadMeetingsForSelectedDate();

            //clear textfields
            AddTitleText.Text = "";
            AddDescText.Text = "";
            AddStartText.Text = "";
            AddEndText.Text = "";

            //switch back to day view
            addEventBox.Hide();
            selectedDayBox.Show();
        }
        private void editEventB_Click(object sender, EventArgs e)
        {
            selectedMeetingBox.Hide();
            selectedDayBox.Hide();
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
        private void prevMonth_Click(object sender, EventArgs e)
        {
            currentDateTime = currentDateTime.AddMonths(-1);

            //set selected date to first of month to avoid issues with months that have less days
            selectedDate = new DateTime(currentDateTime.Year, currentDateTime.Month, 1);

            //rebuild calendar for new month
            BuildCalendar();
            UpdateCalendar();
            LoadMeetingsForSelectedDate();
        }
        private void nextMonth_Click(object sender, EventArgs e)
        {
            currentDateTime = currentDateTime.AddMonths(1);

            //set selected date to first of month to avoid issues with months that have less days
            selectedDate = new DateTime(currentDateTime.Year, currentDateTime.Month, 1);

            //rebuild calendar for new month
            BuildCalendar();
            UpdateCalendar();
            LoadMeetingsForSelectedDate();
        }
        private void logoutButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Confirm", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                //delete meeting code here
                MessageBox.Show("Logout Sucessful!");
                this.Close();
                new Login().Show();
            }
        }

        private void addCancel_Click(object sender, EventArgs e)
        {
            //switch back to day view
            addEventBox.Hide();
            selectedDayBox.Show();
        }
    }
}

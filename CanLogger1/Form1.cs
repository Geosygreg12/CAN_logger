using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;


namespace CanLogger1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.FilterIndex = 1; // the number of files that can be selected at a time

                    //the only formats that are supported .asc files and .txt files
                    openFileDialog.Filter = "ASC file (*.asc) | *.asc|Text file (*.txt) | *.txt ";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //make the textbox text to be the name of the directory of the log file
                        DirText.Text = openFileDialog.FileName;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("An exception occured! Exception: " + exception.Message);
            }            
        }

        private void DirText_KeyDown(object sender, KeyEventArgs e)
        {
            //enter is pressed after entering the directory text then focus on the next textbox
            if (e.KeyCode == Keys.Enter) timeText.Focus();
        }

        private void DirText_TextChanged(object sender, EventArgs e)
        {
            // if text change is true, delete previous list of can messages so new ones can be populated
            if (listOfLoggedValues.Count > 1) StopButton_Click(sender, e);
            listOfLoggedValues.Clear();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            //initialize the start up values
            if (File.Exists(DirText.Text))
            {
                streamReader = new StreamReader(DirText.Text, Encoding.ASCII);
                timer1.Enabled = true;
                PauseButton.Visible = true;
                status = false;
                PauseButton.Text = "Pause";
                Console.WriteLine("Transmission has started");
                progressBar.Visible = true;
                progressLabel.Visible = true;
                progressPercent = 0;
                streamVar = 0;
                timeLabel.Visible = true;
                timeUpdateText.Visible = true;
            }
            else MessageBox.Show("Wrong Directory! Try Again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            //reset the relevant params
            Console.WriteLine("Transmission has stopped");
            timer1.Enabled = false;
            streamReader.Close();
            PauseButton.Visible = false;
            progressBar.Visible = false;
            progressLabel.Visible = false;
            timeLabel.Visible = false;
            timeUpdateText.Visible = false;
            listOfLoggedValues.Clear();
            streamVar = 1;
            data.Message_Time = 0;
            progressLabel.Text = "NO Transmission";
        }

        //if the user presses enter after inputing start time
        private void TimeText_KeyDown(object sender, KeyEventArgs e) 
        {
            if (e.KeyCode == Keys.Enter) StartButton_Click(sender, e);
        }

        private void ReadCANLogFile()
        {
            try
            {
                streamLength = (long)(streamReader.BaseStream.Length / 51.3);

                if(progressPercent <=99) progressPercent = (int)((streamVar++ * 100) / streamLength);

                progressLabel.Text = string.Format("Processing ... {0}%", progressPercent);
                progressBar.Value = progressPercent;
                progressBar.Update();

                if (!string.IsNullOrEmpty(loggedMessage)) if (loggedMessage.EndsWith("measurement")) status = true;

                if (status) //status is a bool that is used to indicate where CAN data starts in the log file
                {
                    loggedMessage = streamReader.ReadLine();

                    if (!string.IsNullOrEmpty(loggedMessage)) listOfLoggedValues = loggedMessage.Split(' ').ToList();

                    //remove empty or white spaces
                    while (listOfLoggedValues.Contains(string.Empty)) listOfLoggedValues.Remove(string.Empty);

                    //initialize the dataparams with values from the log file
                    if (!float.TryParse(listOfLoggedValues[TIME_INDEX], out data.Message_Time))
                    {
                        if (listOfLoggedValues[TIME_INDEX].StartsWith("End")) //if we have reached to end of the log file
                        {
                            Thread.Sleep(1000);
                            StopButton_Click(this, EventArgs.Empty);
                            MessageBox.Show("Transmission has finished", "Message"); //end transmission and return
                            return;
                        }
                        else MessageBox.Show("Time is not a real number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    if (!int.TryParse(listOfLoggedValues[LENGTH_BIT_INDEX], out data.Message_Length))
                        MessageBox.Show("Error in Log file!\nCheck the bit length", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                    data.Channel_ID = listOfLoggedValues[CHANNEL_ID_INDEX];
                    data.CAN_Message = new System.Collections.ArrayList();

                    for (int i = MESSAGE_INDEX; i < listOfLoggedValues.Count; i++) data.CAN_Message.Add(listOfLoggedValues[i]);
                }
                else loggedMessage = streamReader.ReadLine();
            }
            catch (Exception exception)
            {
                Console.WriteLine("An Exception occurred!!! Exception: " + exception.Message);
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            //get which radio button is checked
            var Var = radioPanel.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            float messageTime = (float) data.Message_Time * 1000;

            switch (Var.Name)
            {
                case "singleRadio":
                    if (int.TryParse(timeText.Text, out startTime)) 
                    {
                        if (startTime == messageTime) //for single transmission, is the message time = starttime? yes, transmit
                        {
                            //transmit current message
                            Console.WriteLine("The time index is: " + listOfLoggedValues[TIME_INDEX]);

                            if (data.Message_Time < previousTime) ReadCANLogFile();
                            else previousTime = (long)data.Message_Time + timer1.Interval;
                        }
                        else
                        {
                            if (startTime > messageTime) //else is start time still ahead? yes, read file
                            {
                                ReadCANLogFile();
                                previousTime = (long)data.Message_Time + timer1.Interval;
                            } 
                            else //else we have passed requested time, stop transmission
                            {
                                StopButton_Click(this, EventArgs.Empty);
                                MessageBox.Show("Transmission finished", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }
                    else
                    {
                        StopButton_Click(this, EventArgs.Empty);
                        MessageBox.Show("Enter only numbers in the time search space", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }                    
                    break;

                case "tillEndRadio":

                default:

                    if (int.TryParse(timeText.Text, out startTime)) //get the start time
                    {
                        if (startTime <= messageTime) //from the start time transmit until end of file
                            if (listOfLoggedValues.Count > 1) Console.WriteLine("The time index is: " + listOfLoggedValues[TIME_INDEX]);
                    }
                    else
                    {
                        //transmit logged data
                        if (listOfLoggedValues.Count > 1) Console.WriteLine("The time index is: " + listOfLoggedValues[TIME_INDEX]);
                    }

                    if (data.Message_Time < previousTime) ReadCANLogFile();
                    else previousTime = (long)data.Message_Time + timer1.Interval;
                    break;
            }

            timeUpdateText.Text = messageTime.ToString(); //send live update to UI to keep track of message
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            switch (PauseButton.Text)
            {
                case "Pause":
                    Console.WriteLine("Transmission has stopped");
                    timer1.Enabled = false;
                    PauseButton.Text = "Continue";
                    break;
                case "Continue":
                    Console.WriteLine("Transmission has started");
                    timer1.Enabled = true;
                    PauseButton.Text = "Pause";
                    break;
            }
        }
    }
}

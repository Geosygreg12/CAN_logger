using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace CanLogger1
{
    public partial class Form1 : Form
    {
        public CANTransmitterClass CANTransmitter = new CANTransmitterClass();
        public Form1()
        {
            CANTransmitter.form1 = this;
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
            switch (InterfaceComboBox.SelectedIndex)
            {
                case 0:
                case 1:
                    if(!status) CANTransmitter.initialise();
                    break;
                default:
                    MessageBox.Show("Kindly Select an Interface from the Options given", "Message");
                    return; 
            }

            if (File.Exists(DirText.Text))
            {
                if (!timer1.Enabled && !PauseButton.Visible) streamReader = new StreamReader(DirText.Text, Encoding.ASCII);
                timer1.Enabled = true;
                timeLabel.Visible = true;
                timeUpdateText.Visible = true;
                PauseButton.Visible = true;
                PauseButton.Text = "Pause";
                Console.WriteLine("Transmission has started");
                progressBar.Visible = true;
                progressLabel.Visible = true;
                progressPercent = 0;
            }
            else MessageBox.Show("Wrong Directory! Try Again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            //reset the relevant params
            Thread.Sleep(TimeSpan.Zero); //this is to enable all timer tasks to complete
            Console.WriteLine("Transmission has stopped");
            timer1.Enabled = false;
            timeLabel.Visible = false;
            timeUpdateText.Visible = false;
            if(streamReader != null) streamReader.Close();
            streamVar = 1;
            previousTime = 0;
            PauseButton.Visible = false;
            progressBar.Visible = false;
            progressLabel.Visible = false;
            if (status) CANTransmitter.Close();
            status = false;
            control = false;
            tracker = false;
            listOfLoggedValues.Clear();
            data.Message_Time = 0;
            progressLabel.Text = "NO Transmission";
        }

        //if the user presses enter after inputing start time
        private void TimeText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) StartButton_Click(sender, e);
        }

        bool control = false;
        private void ReadCANLogFile()
        {
            try
            {
                streamLength = (long)(streamReader.BaseStream.Length / 51.3);

                if (progressPercent <= 99) progressPercent = (int)((streamVar++ * 100) / streamLength);

                progressLabel.Text = string.Format("Processing ... {0}%", progressPercent);
                progressBar.Value = progressPercent;
                progressBar.Update();

                if (!string.IsNullOrEmpty(loggedMessage))
                    if (loggedMessage.EndsWith("measurement")) { status = true; control = true; }
                    else if (control) status = true;

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

                            status = false;
                        }
                        else
                        {
                            MessageBox.Show("Time is not a real number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            status = false;
                            StopButton_Click(this, EventArgs.Empty);
                        }
                    }

                    if (!int.TryParse(listOfLoggedValues[LENGTH_BIT_INDEX], out data.Message_Length))
                    {
                        DialogResult dialogResult = MessageBox.Show("Error in Log file!\nCheck the bit length", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        status = false;
                        if (dialogResult.Equals(DialogResult.OK)) return;
                    }

                    data.Message_ID = listOfLoggedValues[MESSAGE_ID_INDEX];
                    data.CAN_Message = new List<string>();

                    for (int i = MESSAGE_INDEX; i < (MESSAGE_INDEX + data.Message_Length); i++)
                        data.CAN_Message.Add(listOfLoggedValues[i]);
                }
                else loggedMessage = streamReader.ReadLine();
            }
            catch (Exception exception)
            {
                Console.WriteLine("An Exception occurred!!! Exception: " + exception.Message);
            }
        }

        bool tracker = false; //tracks whenever we read from file to enable transmission
        private Stopwatch stopwatch = new Stopwatch(); //makes sure transmitted messages are timed
        private void Timer1_Tick(object sender, EventArgs e)
        {
            //get which radio button is checked
            var Var = radioPanel.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked); //get the transmission mode
            float messageTime = (float)data.Message_Time* 1000; //convert to milliseconds

            switch (Var.Name)
            {
                case "singleRadio":
                    if (int.TryParse(timeText.Text, out startTime))
                    {
                        if (startTime == messageTime) //for single transmission, is the message time = starttime? yes, transmit
                        {
                            //transmit current message
                            if (status) //if the parameter are parsed successfully, then status is true
                            {
                                CANTransmitter.Transmitter();
                                Console.WriteLine("The time index is: " + listOfLoggedValues[TIME_INDEX]);
                            }

                            if (messageTime <= previousTime) ReadCANLogFile();
                            else previousTime = (long) messageTime + timer1.Interval;
                        }
                        else
                        {
                            if (startTime > messageTime) //else is start time still ahead? yes, read file
                            {
                                ReadCANLogFile();
                                previousTime = (long) messageTime + timer1.Interval;
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
                    switch (int.TryParse(timeText.Text, out startTime))
                    {
                        case true:
                            if (startTime <= messageTime) goto default;
                            break;

                        case false:                      
                        default:
                            if (status && tracker) //status = did we read log file successfully?, tracker = have we read the log file? 
                            {
                                CANTransmitter.Transmitter();
                                Console.WriteLine("The time index is: " + listOfLoggedValues[TIME_INDEX]);
                            }
                            break;
                    }

                    if (messageTime <= previousTime)
                    {
                        tracker = true; //set tracker to true whenever we read the log file
                        ReadCANLogFile();
                    }
                    else //else set tracker to false
                    {
                        if (stopwatch.IsRunning)
                        {
                            if (stopwatch.ElapsedMilliseconds >= (messageTime - previousTime))
                            {
                                previousTime = (long) messageTime + timer1.Interval; //update previous time 
                                stopwatch.Stop();
                                stopwatch.Reset();
                            }
                        }
                        else stopwatch.Start();

                        tracker = false; //set tracker to false if we have not read the log file
                    }
                    break;
            }

            timeUpdateText.Text = messageTime.ToString(); //send live update to UI to keep track of message
        }

        private void PauseButton_Click(object sender, EventArgs e) //pause and continue transmission 
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

        private void InterfaceComboBox_SelectedIndexChanged(object sender, EventArgs e) //which can interface is selected?
        {
            if (status)
            {
                DialogResult result = MessageBox.Show("You just changed the interface, Transmission will stop now" +
                            " You will have to restart the application", "Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (result.Equals(DialogResult.OK))
                {
                    StopButton_Click(this, EventArgs.Empty);
                    Environment.Exit(1);
                }
                else InterfaceComboBox.SelectedIndex = 0;
            }

            INTERFACE = InterfaceComboBox.SelectedIndex;
        }
    }
}

﻿using System;
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
            ProgressChanged += updateProgressbar;
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
                    if(!PauseButton.Visible) CANTransmitter.initialise();
                    break;
                default:
                    MessageBox.Show("Kindly Select an Interface from the Options given", "Message");
                    return; 
            }

            if (File.Exists(DirText.Text))
            {
                if (!PauseButton.Visible) streamReader = new StreamReader(DirText.Text, Encoding.ASCII);
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

            thread = new Thread(backgroundFunction);
            pause = true;
            thread.Start();
        }

        
        private void StopButton_Click(object sender, EventArgs e)
        {
            //reset the relevant params
            this.BeginInvoke((Action)delegate ()
            {
                Thread.Sleep(TimeSpan.Zero); //this is to enable all timer tasks to complete
                Console.WriteLine("Transmission has stopped");
                timeLabel.Visible = false;
                timeUpdateText.Visible = false;
                if(streamReader != null) streamReader.Close();
                streamVar = 1;
                previousTime = 0;
                PauseButton.Visible = false;
                progressBar.Visible = false;
                progressLabel.Visible = false;
                if (control || tracker) CANTransmitter.Close();
                status = false;
                control = false;
                tracker = false;
                listOfLoggedValues.Clear();
                canData.Clear();
                data.Message_Time = 0;
                progressLabel.Text = "NO Transmission";
                pause = false;
                stopwatch.Reset();
                stopwatch.Stop();
            });
        }

        //if the user presses enter after inputing start time
        private void TimeText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) StartButton_Click(sender, e);
        }

        bool control = false; //used to get when CAN data started in the log file
        private void ReadCANLogFile()
        {
            try
            {
                streamLength = (long)(streamReader.BaseStream.Length / 50);

                if (progressPercent <= 99) progressPercent = (int)((streamVar++ * 100) / streamLength);

                if (!tracker) OnProgressChanged();

                if (!string.IsNullOrEmpty(loggedMessage))
                    if (loggedMessage.EndsWith("measurement")) { status = true; control = true; }
                    else if (control) status = true;

                if (status) //status is a bool that is used to indicate where CAN data starts in the log file
                {
                    loggedMessage = streamReader.ReadLine();

                    if (!string.IsNullOrEmpty(loggedMessage)) listOfLoggedValues = loggedMessage.Split(' ').ToList();

                    //remove empty or white spaces
                    while (listOfLoggedValues.Contains(string.Empty)) listOfLoggedValues.Remove(string.Empty);

                    if (listOfLoggedValues[TIME_INDEX].StartsWith("End")) //if we have reached to end of the log file
                    {
                        OnProgressChanged();
                        tracker = true;
                        status = false;
                        return;
                    }

                    if (!listOfLoggedValues.Contains("Rx"))
                    {
                        status = false;
                        return;
                    }

                    //initialize the dataparams with values from the log file
                    if (!float.TryParse(listOfLoggedValues[TIME_INDEX], out data.Message_Time)) { status = false; return; }

                    if (!int.TryParse(listOfLoggedValues[LENGTH_BIT_INDEX], out data.Message_Length)) { status = false; return; } 

                    data.Message_ID = listOfLoggedValues[MESSAGE_ID_INDEX];
                    data.CAN_Message = new string[data.Message_Length];

                    for (int i = MESSAGE_INDEX, j = 0; i < (MESSAGE_INDEX + data.Message_Length); i++, j++)
                        data.CAN_Message[j] = listOfLoggedValues[i];

                    if(status) canData.Add(data);
                }
                else loggedMessage = streamReader.ReadLine();
            }
            catch (Exception exception)
            {
                status = false;
                Console.WriteLine("An Exception occurred!!! Exception: " + exception.Message);
            }
        }

        private void updateProgressbar(object source, EventArgs e)
        {
            this.BeginInvoke((Action)delegate ()
           {
               if (tracker)
               {
                   progressLabel.Text = "Waiting for transmission to finish...";
                   progressBar.Value = 100;
                   progressBar.Update();
               }
               else if (progressBar.Maximum >= progressPercent)
               {
                   progressLabel.Text = string.Format("Processing ... {0}%", progressPercent);
                   progressBar.Value = progressPercent;
                   progressBar.Update();
               }

               if(canData.Count > 0) timeUpdateText.Text = (canData[0].Message_Time * 1000).ToString(); //send live update to UI to keep track of message time
           });
        }

        bool tracker = false; //tracks whenever we read from file to enable transmission
        private Stopwatch stopwatch = new Stopwatch(); //makes sure transmitted messages are timed
        private void readLogTransmitEnable()
        {
            //get which radio button is checked
            if (canData.Count == 0 && control && tracker && !status)
            {
                StopButton_Click(this, EventArgs.Empty);
                MessageBox.Show("Transmission finished", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var Var = radioPanel.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked); //get the transmission mode
            float messageTime = 0;
            if (canData.Count > 0) messageTime = (float)canData[0].Message_Time * 1000;

            switch (Var.Name)
            {
                case "singleRadio":
                    if (int.TryParse(timeText.Text, out startTime))
                    {
                        if (startTime == Math.Floor(messageTime / 1000) * 1000) //for single transmission, is the message time approx = starttime? yes, transmit
                        {
                            //transmit current message
                            if (control && (canData.Count > 0)) //if the parameters/data are parsed successfully, then status is true
                            {
                                Console.WriteLine("The time index is: " + listOfLoggedValues[TIME_INDEX]);
                                CANTransmitter.Transmitter();
                            }
                        }
                        else
                        {
                            if (startTime < messageTime) //else is start time still ahead? no, finish
                            {
                                StopButton_Click(this, EventArgs.Empty);
                                MessageBox.Show("Transmission finished", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else { if(canData.Count > 0) canData.RemoveAt(0); }
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
                            if ((messageTime <= previousTime) && (canData.Count > 0))
                            {
                                Console.WriteLine("The time index is: " + canData[0].Message_Time);
                                OnProgressChanged();
                                CANTransmitter.Transmitter();
                            }
                            else if (stopwatch.IsRunning)
                            {
                                if (stopwatch.ElapsedMilliseconds >= ((long)messageTime - previousTime))
                                {
                                    previousTime = (long)messageTime + 1; //update previous time 
                                    stopwatch.Restart();
                                }
                            }
                            else stopwatch.Start();
                            break;
                    }
                    break;
            }

            ReadCANLogFile();
        }

        private void PauseButton_Click(object sender, EventArgs e) //pause and continue transmission 
        {
            switch (PauseButton.Text)
            {
                case "Pause":
                    Console.WriteLine("Transmission has stopped");
                    pause = false;
                    PauseButton.Text = "Continue";
                    break;
                case "Continue":
                    Console.WriteLine("Transmission has started");
                    thread = new Thread(backgroundFunction);
                    pause = true;
                    thread.Start();
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

        private void TimeText_MouseClick(object sender, MouseEventArgs e)
        {
            timeText.SelectAll();
            timeText.Focus(); 
        }

        public delegate void updateProgressBarEventHandler(object source, EventArgs e);

        public event updateProgressBarEventHandler ProgressChanged;
        protected virtual void OnProgressChanged()
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, EventArgs.Empty);
            }
        }
    }
}

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
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

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

        private void InterfaceComboBox_SelectedIndexChanged(object sender, EventArgs e) //which can interface is selected?
        {
            if (play)
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

        string str = string.Empty;
        private async void DirText_TextChanged(object sender, EventArgs e)
        {
            // if text change is true, delete previous list of can messages so new ones can be populated
            if (play) StopButton_Click(sender, e);
            listOfLoggedValues.Clear();

            await Task.Run(() => 
            {
                if (!str.Equals(DirText.Text))
                {
                    if (!string.IsNullOrEmpty(str)) streamReader.Close();

                    str = DirText.Text;
                    canData.Clear();
                    if (File.Exists(str)) streamReader = new StreamReader(str, Encoding.ASCII);
                    else
                    {
                        MessageBox.Show("Wrong Directory! Try Again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                
                while (!streamReader.EndOfStream) ReadCANLogFile();

                if (streamReader != null && streamReader.EndOfStream) streamReader.Close(); 
            });
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            //initialize the start up values
            switch (InterfaceComboBox.SelectedIndex)
            {
                case 0:
                case 1:
                    if(!play) CANTransmitter.initialise(); //add another control variable
                    break;
                default:
                    MessageBox.Show("Kindly Select an Interface from the Options given", "Message");
                    return; 
            }

            if (File.Exists(DirText.Text))
            {
                timeLabel.Visible = true;
                timeUpdateText.Visible = true;
                PauseButton.Visible = true;
                PauseButton.Text = "Pause";
                Console.WriteLine("Transmission has started");
                progressBar.Visible = true;
                progressLabel.Visible = true;
                progressBarTimer.Enabled = true;
            }

            Var = radioPanel.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked); //get the transmission mode
            play = true;
            transmitLogthread = new Thread(backgroundFuncToTransmitLog);
            transmitLogthread.Start();
            stopwatch.Start();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            //reset the relevant params 
            this.BeginInvoke((Action)delegate ()
            {
                Console.WriteLine("Transmission has stopped");
                timeLabel.Visible = false;
                timeUpdateText.Visible = false;
                PauseButton.Visible = false;
                progressBar.Visible = false;
                progressLabel.Visible = false;
                if (play) CANTransmitter.Close();
                status = false;
                control = false;
                listOfLoggedValues.Clear();
                data.Message_Time = 0;
                progressLabel.Text = "NO Transmission";
                timeUpdateText.Text = string.Empty;
                play = false;
                stopwatch.Reset();
                progressBarTimer.Enabled = false;
                CANTransmitterClass.num = 0;
            });
        }

        private void PauseButton_Click(object sender, EventArgs e) //pause and continue transmission 
        {
            switch (PauseButton.Text)
            {
                case "Pause":
                    play = false;
                    PauseButton.Text = "Continue";
                    stopwatch.Stop();
                    break;
                case "Continue":
                    play = true;
                    transmitLogthread = new Thread(backgroundFuncToTransmitLog);
                    transmitLogthread.Start();
                    stopwatch.Start();
                    PauseButton.Text = "Pause";
                    break;
            }
        }

        //if the user presses enter after inputing start time
        private void TimeText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) StartButton_Click(sender, e);
        }

        private void TimeText_MouseClick(object sender, MouseEventArgs e)
        {
            timeText.SelectAll();
            timeText.Focus(); 
        }

        bool control = false; //used to get when CAN data started in the log file
        private void ReadCANLogFile()
        {
            try
            {
                if (!string.IsNullOrEmpty(loggedMessage))
                    if (loggedMessage.EndsWith("measurement")) control = true; 

                if (control) //control is a bool that is used to indicate where CAN data starts in the log file
                {
                    status = true;
                    loggedMessage = streamReader.ReadLine();

                    if (!string.IsNullOrEmpty(loggedMessage)) listOfLoggedValues = loggedMessage.Split(' ').ToList();

                    //remove empty or white spaces
                    while (listOfLoggedValues.Contains(string.Empty)) listOfLoggedValues.Remove(string.Empty);

                    if (listOfLoggedValues[TIME_INDEX].StartsWith("End")) { status = false; return; } //if we have reached to end of the log file

                    if (!listOfLoggedValues.Contains("Rx")) { status = false; return; }

                    //initialize the dataparams with values from the log file
                    if (!float.TryParse(listOfLoggedValues[TIME_INDEX], out data.Message_Time)) { status = false; return; }

                    if ((!int.TryParse(listOfLoggedValues[LENGTH_BIT_INDEX], out data.Message_Length)) || ( data.Message_Length == 0)) { status = false; return; } 

                    data.Message_ID = listOfLoggedValues[MESSAGE_ID_INDEX];
                    data.CAN_Message = new Byte[data.Message_Length];

                    for (int i = MESSAGE_INDEX, j = 0; i < (MESSAGE_INDEX + data.Message_Length); i++, j++)
                    {
                        byte Byte = 0;

                        try
                        {
                            Byte = Convert.ToByte(listOfLoggedValues[i], 16);
                        }
                        catch (Exception exc)
                        { Console.WriteLine("Error :" + exc.Message); }

                        data.CAN_Message[j] = Byte;
                    }    

                    if(status && (data.CAN_Message.Length > 0)) canData.Add(data);
                }
                else loggedMessage = streamReader.ReadLine();
            }
            catch (Exception exception)
            {
                status = false;
                Console.WriteLine("An Exception occurred!!! Exception: " + exception.Message);
            }
        }
        
        private Stopwatch stopwatch = new Stopwatch(); //makes sure transmitted messages are timed
        private void readLogTransmitEnable()
        {
            if (canData.Count == CANTransmitterClass.num && play)
            {
                StopButton_Click(this, EventArgs.Empty);
                MessageBox.Show("Transmission finished", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            float messageTime = 0;
            if (canData.Count > CANTransmitterClass.num) messageTime = (float)canData[CANTransmitterClass.num].Message_Time * 1000;
            else return;

            switch (Var.Name)
            {
                case "singleRadio":

                    singleModeSelected(messageTime);
                    break;

                case "tillEndRadio":
                default:

                    tillEndOfFileModeSelected(messageTime);
                    break;    
            }
        }

        private void singleModeSelected(float messageTime)
        {
            if (int.TryParse(timeText.Text, out startTime))
            {
                if (startTime == Math.Floor(messageTime / 1000) * 1000) //for single transmission, is the message time approx = starttime? yes, transmit
                {
                    //transmit current message
                    if (control && (canData.Count > CANTransmitterClass.num)) CANTransmitter.Transmitter();
                }
                else
                {
                    if (startTime < messageTime) //else is start time passed? no, finish
                    {
                        StopButton_Click(this, EventArgs.Empty);
                        MessageBox.Show("Transmission finished", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else CANTransmitterClass.num++;
                }
            }
            else
            {
                StopButton_Click(this, EventArgs.Empty);
                MessageBox.Show("Enter only numbers in the time search space", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tillEndOfFileModeSelected(float messageTime)
        {
            switch (int.TryParse(timeText.Text, out startTime))
            {
                case true:
                    if (startTime <= messageTime) goto default;
                    break;

                case false:
                default:

                    if (stopwatch.IsRunning)
                    {
                        if ((stopwatch.ElapsedMilliseconds >= messageTime) && (canData.Count > CANTransmitterClass.num))
                        {
                            CANTransmitter.Transmitter();
                        }
                    }

                    break;
            }
        }
                
        private void backgroundFuncToTransmitLog()
        {
            while (play) readLogTransmitEnable();
        }

        private void ProgressBarTimer_Tick(object sender, EventArgs e)
        {
            this.BeginInvoke((Action) delegate()
            {
                if (CANTransmitterClass.num <= canData.Count)
                {
                    progressBar.Value = (CANTransmitterClass.num * 100)/canData.Count;
                    progressLabel.Text = string.Format("Processing ... {0}%", progressBar.Value);
                    progressBar.Update();
                    timeUpdateText.Text = canData[CANTransmitterClass.num].Message_Time.ToString();
                }
            });
        }
    }
}

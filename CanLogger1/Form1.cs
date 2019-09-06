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
            listOfLoggedValues.Clear();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
             //isRead is a variable that reflects whether the file was read successfully

            if (!backgroundWorker1.IsBusy)
                backgroundWorker1.RunWorkerAsync();
            else MessageBox.Show("Press the stop button first to stop previous transmissions and Start again", "Message");

            if (isRead) MessageBox.Show("Transmission has started!");

        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
                MessageBox.Show("Transmission has stopped!", "Message");
            }else MessageBox.Show("There is no ongoing transmission to stop!", "Message",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void TimeSearchButton_Click(object sender, EventArgs e)
        {
            /**
             * TODO::
             * make the textbox show the time it started if time exist or message box for wrong time
            and textbox text show the default message. **/
            if (!timeText.Text.Equals("Enter the time in seconds")) startTime = timeText.Text;
            else
            {
                startTime = "\0";
            } 
        }

        //if the user presses enter after inputing start time
        private void TimeText_KeyDown(object sender, KeyEventArgs e) 
        {
            if (e.KeyCode == Keys.Enter)
            {
                TimeSearchButton_Click(sender, e);
                StartButton_Click(sender, e);
            }
        }
      
        private void readAndTransmitFile()
        {
            try
            {
                if (File.Exists(DirText.Text))
                {
                    using (StreamReader streamReader = new StreamReader(DirText.Text, Encoding.ASCII))
                    {
                        isRead = true; string loggedMessage = string.Empty; bool status = false;

                        while (!streamReader.EndOfStream)
                        {
                            if (loggedMessage.EndsWith("measurement")) status = true;

                            if (status) //status is a bool that is used to indicate when CAN data starts in the log file
                            {
                                loggedMessage = streamReader.ReadLine();
                                listOfLoggedValues = loggedMessage.Split(' ').ToList();

                                //remove empty or white spaces
                                while (listOfLoggedValues.Contains(string.Empty)) listOfLoggedValues.Remove(string.Empty);

                                //initialize the dataparams with values from the log file
                                if (!float.TryParse(listOfLoggedValues[TIME_INDEX], out data.Message_Time))
                                    MessageBox.Show("Enter a real number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                if (!int.TryParse(listOfLoggedValues[LENGTH_BIT_INDEX], out data.Message_Length))
                                    MessageBox.Show("Error in Log file!\nCheck the bit length", "Error", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);

                                data.Channel_ID = listOfLoggedValues[CHANNEL_ID_INDEX];

                                for (int i = MESSAGE_INDEX; i < listOfLoggedValues.Count; i++)
                                    data.CAN_Message += listOfLoggedValues[i];

                                //transmit the data
                                TransmitMethod(data);
                            }
                            else loggedMessage = streamReader.ReadLine();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Wrong Directory! Try Again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isRead = false;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("An Exception occurred!!! Exception: " + exception.Message);
                isRead = false;
            }
        }
    }
}

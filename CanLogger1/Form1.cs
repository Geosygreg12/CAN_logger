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
            if (isRead)
            {
                //if time exists! -CONDITION WILL NOT NECESSARY BE ISREAD!!!!!!
                //Let the name of the textbox be the time chosen
                startTime = timeText.Text;
            }
            else
            {
                //display a textbox showing that time is wrong and user should pick another time
                MessageBox.Show("The time you picked does not exist in " +
                                "this log kindly pick another time. \nThank you", "Message");
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
            //THIS METHOD NEEDS TO BE MODIFIED TO TRANSMIT THE LOG FILE
            //format how the time column is written in the log file
            try
            {
                if (File.Exists(DirText.Text))
                {
                    using (StreamReader streamReader = new StreamReader(DirText.Text, Encoding.ASCII))
                    {
                        isRead = true;

                        while (!streamReader.EndOfStream)
                        {
                            //add each row in the log file to the list
                            listOfLoggedValues.Add(streamReader.ReadLine());
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

            RegexMethod();
        }

        private DataParameters RegexMethod()
        {
            DataParameters dataParameters;
            dataParameters.CAN_Message = string.Empty;
            dataParameters.Channel_ID = string.Empty;
            dataParameters.Message_Length = 0;
            dataParameters.Message_Time = 0;

            regexTime = new Regex(@"\s\d.\d\d\d\d\d\d");
            MatchCollection matchCollection = regexTime.Matches("    0.003000 1 4a1  " +
                                                                "Rx d 6 0d 08 b3 1b c1 00 ");
            //matchCollection.Cast<List<string>>();
            float time = 0;
            if(matchCollection.Count > 0)
                float.TryParse(matchCollection[0].ToString(), out time);
            Console.WriteLine(time);

            return dataParameters;
        }
    }
}

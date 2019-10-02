using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CanLogger1
{
    public partial class Form1 : Form
    {
        public CANTransmitterClass CANTransmitter = new CANTransmitterClass();
        public Form1()
        {
            CANTransmitter.Form_1 = this;
            InitializeComponent();
        }

        //get selected interface
        private void InterfaceComboBox_SelectedIndexChanged(object sender, EventArgs e) 
        {
            if (play)                                  //play is a bool that controls transmission. Is transmission ongoing? Yes, fatal error, exit.
            {
                DialogResult result = MessageBox.Show("You just changed the interface, Transmission will stop now" +
                            " You will have to restart the application", "Fatal Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                if (result.Equals(DialogResult.OK))
                {
                    StopButton_Click(this, EventArgs.Empty);
                    Environment.Exit(1);
                }
                else InterfaceComboBox.SelectedIndex = 0;
            }

            INTERFACE = InterfaceComboBox.SelectedIndex;//populate the selected interface
        }

        //browse file directory to get the desired log file to read
        private void BrowseButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.FilterIndex = 1;                 // the number of files that can be selected at a time

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

        //focus on the next textbox when enter is pressed in the directory textbox
        private void DirText_KeyDown(object sender, KeyEventArgs e)
        {
            //enter is pressed after entering the directory text then focus on the next textbox
            if (e.KeyCode == Keys.Enter) timeText.Focus();
        }

        string str = string.Empty; //holds the value of the directory or log file

        // if directory textbox text is changed, then read new log file
        private async void DirText_TextChanged(object sender, EventArgs e)
        {            
            if (play) StopButton_Click(sender, e);                          //is transmission ongoing? Yes, stop it.
            listOfLoggedValues.Clear();                                     //clear previous list of can messages so new ones can be populated

            //run an asynchronous task on another thread to read the log file
            await Task.Run(() => 
            {
                if (!str.Equals(DirText.Text))
                {
                    if (!string.IsNullOrEmpty(str)) streamReader.Close();   //close the reader if it was initialised

                    str = DirText.Text;                                     // update the str string
                    canData.Clear();                                        //clear any previous list of can data
                    if (File.Exists(str)) streamReader = new StreamReader(str, Encoding.ASCII); //initialise the streamreader using the new file dir
                    else
                    {
                        //if directory or log file does not exist, show user a message and return
                        MessageBox.Show("Wrong Directory! Try Again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                
                while (!streamReader.EndOfStream) ReadCANLogFile();         //read the log file to the end

                if (streamReader != null && streamReader.EndOfStream) streamReader.Close(); //close the log file after reading
            });
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            //initialize the start up values
            switch (InterfaceComboBox.SelectedIndex)
            {
                case 0:
                case 1:
                    if(!play && !CANTransmitterClass.isInitialised) CANTransmitter.Initialise();    // initialise the can transmitter class
                    break;
                default:
                    MessageBox.Show("Kindly Select an Interface from the Options given", "Message"); // if no interface is selected, incline user to select and return
                    return; 
            }

            if (File.Exists(DirText.Text))
            {
                //make the time label, progress bar and label to be visible
                timeLabel.Visible = true;
                timeUpdateText.Visible = true;
                PauseButton.Visible = true;
                PauseButton.Text = "Pause";
                Console.WriteLine("Transmission has started");
                progressBar.Visible = true;
                progressLabel.Visible = true;
                progressBarTimer.Enabled = true;
            }

            //get the mode of transmission selected from the group of radio buttons
            Var = radioPanel.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);         
            play = true;                                                    //set true to start transmission
            transmitLogthread = new Thread(BackgroundFuncToTransmitLog);
            transmitLogthread.Start();                                      //start transmission on a new thread
            stopwatch.Start();                                              // start the stopwatch, to time the can message transmission
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            //reset the relevant params 
            this.BeginInvoke((Action)delegate ()
            {
                //make the time lable, progress label and bar invisible 
                Console.WriteLine("Transmission has stopped");
                timeLabel.Visible = false;
                timeUpdateText.Visible = false;
                PauseButton.Visible = false;
                progressBar.Visible = false;
                progressBar.Value = 0;
                progressBar.Update();
                progressLabel.Visible = false;
                if (play) CANTransmitter.Close();               //close the transmitter
                status = false;
                control = false;                                //set control variable to false
                listOfLoggedValues.Clear();                     //clear list
                data.Message_Time = 0;
                progressLabel.Text = "NO Transmission";
                timeUpdateText.Text = string.Empty;
                play = false;
                stopwatch.Reset();                              //reset stop watch
                progressBarTimer.Enabled = false;
                CANTransmitterClass.num = 0;                    //reset the transmission index to zero, num is an in variable that indicates the particular can message to transmit
            });
        }

        //when the pause button is pressed
        private void PauseButton_Click(object sender, EventArgs e) //pause and continue transmission 
        {
            switch (PauseButton.Text)
            {
                case "Pause":
                    play = false;                                       //stop 'playing' transmission 
                    PauseButton.Text = "Continue";                      //change the text to continue
                    stopwatch.Stop();                                   //stop the stop watch
                    break;
                case "Continue":
                    play = true;                                        //start 'playing' transmission
                    transmitLogthread = new Thread(BackgroundFuncToTransmitLog); 
                    transmitLogthread.Start();                          //restart the transmission on another background thread
                    stopwatch.Start();                                  //start the stopwatch
                    PauseButton.Text = "Pause";
                    break;
            }
        }

        //if the user presses enter after inputing start time, then start transmission
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

        //this method is used to read the log file and process the can values
        private void ReadCANLogFile()
        {
            try
            {
                if (!string.IsNullOrEmpty(loggedMessage))
                    if (loggedMessage.EndsWith("measurement")) control = true; 

                if (control)                                            //control is a bool that indicates where the can messages started in the file
                {
                    status = true;                                      //status indicates where the message is parsed successfully into the relevant can variables
                    loggedMessage = streamReader.ReadLine();            //read the next can message from the log file

                    if (!string.IsNullOrEmpty(loggedMessage)) listOfLoggedValues = loggedMessage.Split(' ').ToList();

                    //remove empty or white spaces
                    while (listOfLoggedValues.Contains(string.Empty)) listOfLoggedValues.Remove(string.Empty);

                    if (listOfLoggedValues[TIME_INDEX].StartsWith("End")) { status = false; return; }   //have we have reached to end of the log file?

                    if (!listOfLoggedValues.Contains("Rx")) { status = false; return; }                 //we want to retransmit only the previously received 'Rx' can messages

                    //initialize the dataparams with values from the log file
                    if (!float.TryParse(listOfLoggedValues[TIME_INDEX], out data.Message_Time)) { status = false; return; }

                    if ((!int.TryParse(listOfLoggedValues[LENGTH_BIT_INDEX], out data.Message_Length)) || ( data.Message_Length == 0)) { status = false; return; } 

                    data.Message_ID = listOfLoggedValues[MESSAGE_ID_INDEX];
                    data.CAN_Message = new Byte[data.Message_Length];

                    //here we process the can messages and convert them to bytes, ready for retransmission
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

                    if(status && (data.CAN_Message.Length > 0)) canData.Add(data);                      //add can data to the list for retransmission
                }
                else loggedMessage = streamReader.ReadLine();                                           //else keep reading file until we get to the start of the can messages
            }
            catch (Exception exception)
            {
                status = false;
                Console.WriteLine("An Exception occurred!!! Exception: " + exception.Message);  //handle any exception
            }
        }
        
        private Stopwatch stopwatch = new Stopwatch();  
        private void ReadLogTransmitEnable()
        {
            if (canData.Count == CANTransmitterClass.num && play)                               //if we have reached the end of log file then end transmission
            {
                StopButton_Click(this, EventArgs.Empty);
                MessageBox.Show("Transmission finished", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            float messageTime; //get the message time for the can data to be transmitted in milliseconds
            if (canData.Count > CANTransmitterClass.num) messageTime = (float)canData[CANTransmitterClass.num].Message_Time * 1000;
            else return;

            //transmit just the current message or until end of file
            switch (Var.Name)
            {
                case "singleRadio":

                    SingleModeSelected(messageTime); 
                    break;

                case "tillEndRadio":
                default:

                    TillEndOfFileModeSelected(messageTime);
                    break;    
            }
        }

        //handle single mode transmission
        private void SingleModeSelected(float messageTime)
        {
            if (int.TryParse(timeText.Text, out startTime))
            {
                if (startTime == Math.Floor(messageTime / 1000) * 1000)     //for single transmission, is the message time approx = starttime? yes, transmit
                {
                    //transmit current message if current index is less than total can messages
                    if (canData.Count > CANTransmitterClass.num) CANTransmitter.Transmitter();
                }
                else
                {
                    if (startTime < messageTime)                            //else is start time passed? no, finish
                    {
                        StopButton_Click(this, EventArgs.Empty);
                        MessageBox.Show("Transmission finished", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else CANTransmitterClass.num++;
                }
            }
            else                                                            //else if the start time is wrong alert the user
            {
                StopButton_Click(this, EventArgs.Empty);
                MessageBox.Show("Enter only numbers in the time search space", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //handle till end of log file mode of transmission
        private void TillEndOfFileModeSelected(float messageTime)
        {
            switch (int.TryParse(timeText.Text, out startTime))
            {
                case true:
                    if (startTime <= messageTime) goto default;         //transmit if the message time at the current index is greater than the start time
                    else CANTransmitterClass.num++;                     //else go to the next index
                    break;

                case false:
                default:

                    //use the stop watch to time the transmission time properly
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
              
        //background method that is run on a different thread 
        private void BackgroundFuncToTransmitLog()
        {
            while (play) ReadLogTransmitEnable(); //while transmission is ongoing continue transmission
        }

        //invoke the ui thread and update the progressbar every second
        private void ProgressBarTimer_Tick(object sender, EventArgs e)
        {
            this.BeginInvoke((Action) delegate()
            {
                //update relevant parameters for user to monitor transmission progress
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

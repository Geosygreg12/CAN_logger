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
    public partial class CAN_Channel : Form
    {
        public CANTransmitterClass CANTransmitter = new CANTransmitterClass();

        int numChans = 0;

        public CAN_Channel(int numChan = 1)
        {
            this.numChans = numChan; 
            InitializeComponent();

            BAUDRATE = new int[2] { -1, -1};
            numOfChanUpnDwn.Minimum = 1;
            numOfChanUpnDwn.Maximum = numChan;
            CANTransmitter.Form_1 = this;
            this.FormClosing += CAN_Channel_FormClosing;
            startButton.Enabled = false;
        }

        private void CAN_Channel_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopButton_Click(this, EventArgs.Empty);
        }


        //get selected interface
       


        //browse file directory to get the desired log file to read
        private void BrowseButton_Click(object sender, EventArgs e)
        {
            try
            {

                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {

                    // the number of files that can be selected at a time
                    openFileDialog.FilterIndex = 1;                 

                    //the only formats that are supported .asc files and .txt files
                    openFileDialog.Filter = "ASC file (*.asc) | *.asc|Text file (*.txt) | *.txt ";


                    //make the textbox text to be the name of the directory of the log file
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                        DirText.Text = openFileDialog.FileName;

                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("An exception occured! Exception: " + exception.Message);
            }
        }


        //focus on the next textbox when enter is pressed in the directory textbox
        private  void DirText_KeyDown(object sender, KeyEventArgs e)
        {
            //enter is pressed after entering the directory text then focus on the next textbox
            if (e.KeyCode == Keys.Enter) timeText.Focus();
        }


        //holds the value of the directory or log file
        string str = string.Empty; 


        // if directory textbox text is changed, then read new log file
        private async void DirText_TextChanged(object sender, EventArgs e)
        {

            //is transmission ongoing? Yes, stop it.
            if (play) StopButton_Click(sender, e);

            //clear previous list of can messages so new ones can be populated
            listOfLoggedValues.Clear();

            

            //run an asynchronous task on another thread to read the log file
            await Task.Run(() =>
            {

                if (!str.Equals(DirText.Text))
                {
                    //close the reader if it was initialised
                    if (!string.IsNullOrEmpty(str)) streamReader.Close();


                    // update the str string
                    str = DirText.Text;

                    //clear any previous list of can data
                    canData.Clear();

                    //initialise the streamreader using the new file dir
                    if (File.Exists(str)) streamReader = new StreamReader(str, Encoding.ASCII);
                    else
                    {
                        //if directory or log file does not exist, show user a message and return
                        MessageBox.Show("Wrong Directory! Try Again", "Error", MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);


                        return;
                    }
                }


                    //read the log file to the end
                while (!streamReader.EndOfStream) ReadCANLogFile();


                //close the log file after reading
                if (streamReader != null && streamReader.EndOfStream)
                    streamReader.Close();
            });
        }

        private void StartButton_Click(object sender, EventArgs e)
        {

            //initialize the start up values
            switch (InterfaceComboBox.SelectedIndex)
            {
                case 0:
                case 1:

                    switch (baudRateComboBox.SelectedIndex)
                    {
                        case 0:
                        case 1:
                            TransmissionSpeed = (short)SpeedComboBox.SelectedIndex;

                            // initialise the can transmitter class
                            if (!play && !(CANTransmitterClass.isKvaserInitialised || CANTransmitterClass.isPeakInitialiased) && File.Exists(DirText.Text))
                            {
                                for (int i = 0; i < numOfChanUpnDwn.Maximum; i++)
                                {
                                    CANTransmitter.Initialise(GetInterface[i], BAUDRATE[i]);
                                }
                            }
                            
                            break;

                        default:

                            // if no interface is selected, incline user to select and return
                            MessageBox.Show("Kindly Select a Baudrate from the Options Given " +
                                            "and Choose a Valid Log File", "Message");

                            return;
                    }
                    break;

                default:

                    // if no interface is selected, incline user to select and return
                    MessageBox.Show("Kindly Select an Interface from the Options Given " +
                                    "and Choose a Valid Log File", "Message");


                    return;
            }


            if (File.Exists(DirText.Text))
            {

                //make the time label, progress bar and label to be visible
                PauseButton.Visible = true;
                PauseButton.Text = "Pause";
                Console.WriteLine("Transmission has started");
                progressBar.Visible = true;
                progressLabel.Visible = true;
                progressBarTimer.Enabled = true;

            }
            else
            {

                MessageBox.Show("Wrong Directory! Try Again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;

            }

            //get the mode of transmission selected from the group of radio buttons
            Var = radioPanel.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);

            //set true to start transmission
            play = true;
            transmitLogthread = new Thread(BackgroundFuncToTransmitLog);

            //start transmission on a new thread
            transmitLogthread.Start();

            //start the stopwatch, to time the can message transmission
            stopwatch.Start();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {

            //reset the relevant params 
            this.BeginInvoke((Action)delegate ()
            {

                //make the time lable, progress label and bar invisible 
                Console.WriteLine("Transmission has stopped");
                
                progressBar.Visible = false;
                progressBar.Value = 0;
                progressBar.Update();
                progressLabel.Visible = false;
                
                
                //close the transmitter
                if (play || PauseButton.Visible)
                {
                    CANTransmitter.Close(GetInterface);
                }
                

                PauseButton.Visible = false;
                //clear list
                listOfLoggedValues.Clear();

                
                data.Message_Time = 0;
                progressLabel.Text = "NO Transmission";
                play = false;
                timeReached = true;
                stopwatch.Reset();                              
                
                //reset stop watch
                progressBarTimer.Enabled = false;

                /**reset the transmission index to zero, num is a variable 
                   that indicates the particular can message to transmit */
                CANTransmitterClass.num = 0;          
                
            });
        }

        //when the pause button is pressed
        private void PauseButton_Click(object sender, EventArgs e) //pause and continue transmission 
        {

            switch (PauseButton.Text)
            {

                case "Pause":

                    //stop 'playing' transmission 
                    play = false;

                    //change the text to continue
                    PauseButton.Text = "Continue";

                    //stop the stop watch
                    stopwatch.Stop();                                   

                    break;

                case "Continue":

                    //start 'playing' transmission
                    play = true;                                        
                    transmitLogthread = new Thread(BackgroundFuncToTransmitLog);

                    //restart the transmission on another background thread
                    transmitLogthread.Start();

                    //start the stopwatch
                    stopwatch.Start();                                  
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

        //this method is used to read the log file and process the can values
        private void ReadCANLogFile()
        {

            try
            {

                data = new DataParameters();

                //read the next can message from the log file
                loggedMessage = streamReader.ReadLine();

                if (!string.IsNullOrEmpty(loggedMessage))
                    listOfLoggedValues = loggedMessage.Split(' ').ToList();

                //remove empty or white spaces
                while (listOfLoggedValues.Contains(string.Empty))
                    listOfLoggedValues.Remove(string.Empty);


                //have we reached to end of the log file?
                if (listOfLoggedValues[TIME_INDEX].StartsWith("End")) return; 



                //we want to retransmit only the previously received 'Rx' can messages
                if (!listOfLoggedValues.Contains("Rx")) return; 

                //initialize the dataparams with values from the log file
                if (!float.TryParse(listOfLoggedValues[TIME_INDEX], out data.Message_Time)) return;

                if (int.TryParse(listOfLoggedValues[CAN_CHANNEL_INDEX], out data.CAN_Channel_ID))
                {
                    if (data.CAN_Channel_ID > 2) return; 
                }
                else return;

                if ((!int.TryParse(listOfLoggedValues[LENGTH_BIT_INDEX], out data.Message_Length)) ||
                    (data.Message_Length == 0)) return; 

                if (listOfLoggedValues[MESSAGE_ID_INDEX].ToCharArray().Contains('x'))
                {
                    char[] ch = listOfLoggedValues[MESSAGE_ID_INDEX].ToCharArray();

                    ch = ch.Where(val => val != 'x').ToArray();

                    data.Message_ID = new string(ch);
                    data.Extended = true;
                }
                else
                {
                    data.Extended = false;
                    data.Message_ID = listOfLoggedValues[MESSAGE_ID_INDEX];
                }

                data.CAN_Message = new byte[8];

                //here we process the can messages and convert them to bytes, ready for retransmission
                for (int i = MESSAGE_INDEX, j = 0; i < (MESSAGE_INDEX + data.Message_Length); i++, j++)
                {
                    byte Byte = 0;

                    try
                    {
                        Byte = Convert.ToByte(listOfLoggedValues[i], 16);
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine("Error :" + exc.Message);
                    }

                    data.CAN_Message[j] = Byte;
                }

                //add can data to the list for retransmission
                if (data.CAN_Message.Length > 0) canData.Add(data);
            }
            catch (Exception exception)
            {
                //handle any exception
                Console.WriteLine("An Exception occurred!!! Exception: " + exception.Message);
            }
        }
        
        private Stopwatch stopwatch = new Stopwatch();  
        private void ReadLogTransmitEnable()
        {

            //if we have reached the end of log file then end transmission
            if (canData.Count == CANTransmitterClass.num && play)                               
            {
                StopButton_Click(this, EventArgs.Empty);
                MessageBox.Show("Transmission finished", "Message", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            //get the message time for the can data to be transmitted in milliseconds
            float messageTime;

            if (canData.Count > CANTransmitterClass.num) {

                switch (TransmissionSpeed) 
                {
                    case 0:
                        messageTime = (float)canData[CANTransmitterClass.num].Message_Time * 1000; //transmit in milliseconds
                        break;
                    case 1:
                        messageTime = (float)canData[CANTransmitterClass.num].Message_Time * 500; //transmit 
                        break;
                    case 2:
                        messageTime = (float)canData[CANTransmitterClass.num].Message_Time * 400;
                        break;
                    case 3:
                        messageTime = (float)canData[CANTransmitterClass.num].Message_Time / 1000000000; //transmit till end of file
                        break;

                    default:
                        messageTime = messageTime = (float)canData[CANTransmitterClass.num].Message_Time * 1000;
                        break;
                }
                
            }
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

                //for single transmission, is the message time approx = starttime? yes, transmit
                if (startTime == Math.Floor(messageTime / 1000) * 1000)     
                {
                    //transmit current message if current index is less than total can messages
                    if (canData.Count > CANTransmitterClass.num)
                    {
                        if (numOfChanUpnDwn.Maximum == 2)
                        {
                            CANTransmitter.Transmitter(
                                CANInterface: INTERFACE[(canData[CANTransmitterClass.num].CAN_Channel_ID) - 1],
                                whichCAN: canData[CANTransmitterClass.num].CAN_Channel_ID);
                        }
                        else if (numOfChanUpnDwn.Maximum == 1)
                        {

                            CANTransmitter.Transmitter(
                            CANInterface: INTERFACE[0],
                            whichCAN: 1);
                        }
                    }
                }
                else
                {

                    //else has start time passed? yes, finish
                    if (startTime < messageTime)                            
                    {
                        StopButton_Click(this, EventArgs.Empty);
                        MessageBox.Show("Transmission finished", "Message", MessageBoxButtons.OK, 
                            MessageBoxIcon.Information);

                        return;
                    }
                    else CANTransmitterClass.num++;

                }
            }
            else                                                            
            {

                //else if the start time is wrong alert the user
                StopButton_Click(this, EventArgs.Empty);
                MessageBox.Show("Enter only numbers in the time search space", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }


        bool timeReached = true;

        public int[] BAUDRATE { get; private set; }

        //handle till end of log file mode of transmission
        private void TillEndOfFileModeSelected(float messageTime)
        {
            switch (int.TryParse(timeText.Text, out startTime))
            {
                case true:

                    //transmit if the message time at the current index is greater than the start time
                    if (startTime <= messageTime && canData.Count > CANTransmitterClass.num)
                    {
                        if (startTime == (Math.Floor(messageTime / 1000) * 1000) && timeReached)
                        {
                            //restart the stopwatch to work with the custom time
                            stopwatch.Restart();

                            //time reached is bool used to make this code run once
                            timeReached = false;                        
                        }

                        //transmit if elapsed milliseconds is greater than message time intervals
                        float customTime = messageTime - startTime;
                        if (stopwatch.IsRunning && (stopwatch.ElapsedMilliseconds >= customTime))
                        {
                            if (numOfChanUpnDwn.Maximum == 2)
                            {
                                CANTransmitter.Transmitter(
                                    CANInterface: INTERFACE[(canData[CANTransmitterClass.num].CAN_Channel_ID) - 1],
                                    whichCAN: canData[CANTransmitterClass.num].CAN_Channel_ID);
                            }
                            else if (numOfChanUpnDwn.Maximum == 1)
                            {
                                CANTransmitter.Transmitter(
                                CANInterface: INTERFACE[0],
                                whichCAN: 1);
                            }
                        }

                    } 
                    else CANTransmitterClass.num++;  //else go to the next index

                    break;

                case false:

                default:

                    //use the stop watch to time the transmission time properly
                    if (stopwatch.IsRunning)
                    {
                        if ((stopwatch.ElapsedMilliseconds >= messageTime) &&
                            (canData.Count > CANTransmitterClass.num))
                        {
                            if (numOfChanUpnDwn.Maximum == 2)
                            {
                                CANTransmitter.Transmitter(
                                    CANInterface: INTERFACE[(canData[CANTransmitterClass.num].CAN_Channel_ID) - 1],
                                    whichCAN: canData[CANTransmitterClass.num].CAN_Channel_ID);
                            }
                            else if (numOfChanUpnDwn.Maximum == 1) {

                                    CANTransmitter.Transmitter(
                                    CANInterface: INTERFACE[0],
                                    whichCAN: 1);
                            }
                        }

                    }

                    break;
            }
        }
              

        //background method that is run on a different thread 
        private void BackgroundFuncToTransmitLog()
        {

            //while transmission is ongoing continue transmission
            while (play) ReadLogTransmitEnable(); 
        }

        //invoke the ui thread and update the progressbar every second
        private void ProgressBarTimer_Tick(object sender, EventArgs e)
        {
            this.BeginInvoke((Action) delegate()
            {

                //update relevant parameters for user to monitor transmission progress
                if (CANTransmitterClass.num <= canData.Count)
                {
                    
                    progressBar.Value = (int) (((decimal)CANTransmitterClass.num / canData.Count) * 100);
                    progressLabel.Text = string.Format("Processing ... {0}%", progressBar.Value);
                    progressBar.Update();
                }

            });

        }

        private void SetButton_Click(object sender, EventArgs e)
        {
            if (play)
            {
                StopButton_Click(this, EventArgs.Empty);
                DialogResult result = MessageBox.Show("You just changed the Baudrate or Interface, " +
                                                      "Transmission will stop now " +
                                                      "You will have to restart the application",
                                                      "Fatal Error", MessageBoxButtons.OKCancel,
                                                       MessageBoxIcon.Error);

                this.Close();
            }

            //populate the selected interface
            if (numOfChanUpnDwn.Value == 1)
            {

                BAUDRATE[0] = baudRateComboBox.SelectedIndex;
                INTERFACE[0] = InterfaceComboBox.SelectedIndex;
            }

            if (numOfChanUpnDwn.Value == 2)
            {

                BAUDRATE[1] = baudRateComboBox.SelectedIndex;
                INTERFACE[1] = InterfaceComboBox.SelectedIndex;
            }

            if (numOfChanUpnDwn.Maximum == 1) startButton.Enabled = true;
            else if (!(BAUDRATE[1] == -1 && INTERFACE[1] == -1) && !(BAUDRATE[0] == -1 && INTERFACE[0] == -1)) {
                startButton.Enabled = true;
            }
        }
    }
}

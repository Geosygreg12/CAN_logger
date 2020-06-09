using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CanLogger1
{


    public partial class CAN_Channel : Form
    {

        public static int numOfChan =                           0;
        string[] lastDirPath =                                  new string[2]; 
        float timeOffset =                                      0.0f;
        public static bool setUpComp =                          false;
        static bool isPaused =                                  false;
        static int[] progressIndex =                            { 0,0};
        static DateTime startTime;
        static DateTime transmissionTime;

        public static CAN_INTERFACE[] _INTERFACEs =             new CAN_INTERFACE[2];
        public static CAN_BAUDRATE[] _BAUDRATEs =               new CAN_BAUDRATE[2];

        public bool[] ChanSet;
        public static bool[] taskCompleted;

        public string[] CAN_Directories =                       new string[2];

        static List<List<DataParameters>> CAN_INFO =            new List<List<DataParameters>>();

        DateTime pauseTime =                                    DateTime.Now;
        TimeSpan pauseTimeSpan =                                TimeSpan.Zero;

        System.Windows.Forms.Timer progressBarTimer =           new System.Windows.Forms.Timer() { 
        
            Interval = 100,
        };

        public CAN_Channel(int numChan = 1)
        {
            InitializeComponent();

            canChanNum.Maximum =                                numChan;
            numOfChan =                                         numChan;
            ChanSet =                                           new bool[numChan];
            taskCompleted =                                     new bool[numChan];
            this.FormClosing +=                                 CAN_Channel_FormClosing;
            progressBarTimer.Tick +=                            updateProgressBar;
        }

        private void updateProgressBar(object sender, EventArgs e)
        {
            decimal value =                                     (decimal)((progressIndex[1] + 1) * 100) / CAN_INFO[progressIndex[0]].Count();

            if (value > 100)                                    value =100;

            progressBar1.Value =                                (int) value;

            if (!taskCompleted.Contains(false))                 StopButton_Click(this, EventArgs.Empty);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            try
            {
                if ((CAN_INFO.Count > 0) && setUpComp)
                {
                    PauseButton.Text =                          "Pause";
                    BrowseButton.Enabled =                      false;
                    SetButton.Enabled =                         false;
                    startButton.Enabled =                       false;
                    startTime =                                 DateTime.Now;
                    transmissionTime =                          DateTime.Now;
                    progressBarTimer.Enabled =                  true;

                    CANTransmitterClass.initialiseCAN();

                    isStarted = true;
                    transmitLogthread =                         new Thread(BackgroundFuncToTransmitLog);


                    transmitLogthread.Start();

                }
                else
                {

                    MessageBox.Show("Wrong Directory! Try Again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex) {

                Console.WriteLine(ex.Message);
            }
            
        }



        private void StopButton_Click(object sender, EventArgs e)
        {
            try
            {
                //reset the relevant params 
                this.BeginInvoke((Action)delegate ()
                {

                    BrowseButton.Enabled =                      true;
                    SetButton.Enabled =                         true;

                    startButton.Enabled =                       true;
                    isStarted =                                 false;
                    progressBarTimer.Enabled =                  false;
                    taskCompleted =                             taskCompleted.Select(value => true ? false : false).ToArray();

                    progressBar1.Value =                        0;
                    if (transmitLogthread != null)              transmitLogthread.Abort();

                    CANTransmitterClass.Close();

                });
            }
            catch (Exception ex) {

                Console.WriteLine(ex.Message);
            }
        }

        //when the pause button is pressed
        //pause and continue transmission 
        private void PauseButton_Click(object sender, EventArgs e) 
        {

            switch (PauseButton.Text)
            {

                case "Pause":

                    isPaused =                                  true;
                    PauseButton.Text =                          "Continue";
                    pauseTime =                                 DateTime.Now;                               

                    break;

                case "Continue":

                    isPaused =                                  false;
                    pauseTimeSpan =                             DateTime.Now - pauseTime;
                    startTime =                                 startTime.Add(pauseTimeSpan);                             
                    PauseButton.Text =                          "Pause";

                    break;

            }
        }


        //browse file directory to get the desired log file to read
        private void BrowseButton_Click(object sender, EventArgs e)
        {
            try
            {

                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {

                    openFileDialog.FilterIndex =                4;                 
                    openFileDialog.Filter =                     "CSV File (*.csv) | *.csv " +
                                                                "| ASC file (*.asc) | *.asc" +
                                                                "|Text file (*.txt) | *.txt " +
                                                                "| All Files (*.*)| *.*";

                    if (openFileDialog.ShowDialog() ==DialogResult.OK) DirText.Text = openFileDialog.FileName;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("An exception occured! Exception: " + exception.Message);
            }
        }

        
        public enum CAN_INTERFACE
        {

            KVASER,
            PEAK
        }
        public enum CAN_BAUDRATE
        {

            _250K,
            _500K
        }


        private void SetButton_Click(object sender, EventArgs e)
        {

            int channelIndex =                              (int)canChanNum.Value - 1;

            switch (InterfaceComboBox.SelectedIndex)
            {

                case 1:

                    _INTERFACEs[channelIndex] =             CAN_INTERFACE.PEAK;
                    break;

                case 0:

                default:

                    _INTERFACEs[channelIndex] =             CAN_INTERFACE.KVASER;
                    break;
            }

            switch (baudRateComboBox.SelectedIndex)
            {

                case 0:

                    _BAUDRATEs[channelIndex] =              CAN_BAUDRATE._250K;
                    break;
                case 1:

                default:

                    _BAUDRATEs[channelIndex] =              CAN_BAUDRATE._500K;
                    break;
            }

            switch (channelIndex)
            {

                case 0:

                case 1:

                    ChanSet[channelIndex] =                 true;
                    CAN_Directories[channelIndex] =         DirText.Text;

                    break;
            }

            setUpComp =                                     !ChanSet.Contains(false);

            if (setUpComp)
            {

                processCAN_Dir();
                startButton.Enabled =                       true;
                CANTransmitterClass.initialiseCAN();
            }
        }


        private async void processCAN_Dir() {

            List<Task> CAN_ReadTasks =                      new List<Task>();
            List<DataParameters> can_INFO =                 new List<DataParameters>();
            bool csvFlag =                                  false;
            Encoding encoding =                             Encoding.UTF8;

            for (int i = 0; i < numOfChan; i++) {
                
                int index =                                 i;


                switch (Path.GetExtension(CAN_Directories[i]).ToLower()) {

                    case ".csv":

                        csvFlag =                           true;
                        encoding =                          Encoding.UTF8;

                        goto default;

                    case ".asc":

                        csvFlag =                           false;
                        encoding =                          Encoding.ASCII;

                        goto default;

                    default:

                        ReadDelegate read = (_index) =>
                        {

                            Action act = (Action)delegate ()
                            {

                                if(!lastDirPath.SequenceEqual(CAN_Directories))
                                {
                                    Array.Copy(CAN_Directories, lastDirPath, CAN_Directories.Length);
                                    CAN_INFO.Clear();

                                    if (File.Exists(lastDirPath[_index]))
                                    {

                                        using (streamReader = new StreamReader(lastDirPath[_index], encoding))
                                        {

                                            while (!streamReader.EndOfStream)
                                            {

                                                DataParameters data = ReadCANLogFile(csvFlag);
                                                if (data != null) can_INFO.Add(data);

                                            }

                                        }

                                        CAN_INFO.Add(can_INFO);
                                        timeOffset = 0.0f;
                                        
                                    }
                                    else
                                    {

                                        MessageBox.Show("Wrong Directory! Try Again", "Error", MessageBoxButtons.OK,
                                                        MessageBoxIcon.Information);

                                        Console.WriteLine(CAN_Directories[_index]);
                                    }
                                }
                            };

                            return act;
                        };

                        CAN_ReadTasks.Add(Task.Run(read(index)));
                        break;
                }
            }

            await Task.WhenAll(CAN_ReadTasks);
        }

        delegate Action ReadDelegate(int index);        
        
        private DataParameters ReadCANLogFile(bool csvFlag = false)
        {                               
            
            try
            {
                float messageTime =                         0.0f;
                byte dataLength =                           0;
                int channelID =                             0;
                string messageID =                          "";
                bool extended =                             false;
                byte[] messageData =                        new byte[8];
                List<string> listOfLoggedValues =           new List<string>();

                loggedMessage =                             streamReader.ReadLine();

                if (!csvFlag)
                {
                    if (!string.IsNullOrEmpty(loggedMessage)) listOfLoggedValues = loggedMessage.Split(' ').ToList();
                }
                else {
                    if (!string.IsNullOrEmpty(loggedMessage)) listOfLoggedValues = loggedMessage.Split(',').ToList();
                }

                if (!csvFlag) {
                    if (!listOfLoggedValues.Contains("Rx")) return null; 
                }

                while (listOfLoggedValues.Contains(string.Empty)) listOfLoggedValues.Remove(string.Empty);

                if (!float.TryParse(listOfLoggedValues[TIME_INDEX], out messageTime)) return null;
                else
                {
                    if (timeOffset == 0.0f) timeOffset = messageTime;

                    if (!csvFlag)
                    {
                        if (int.TryParse(listOfLoggedValues[CAN_CHANNEL_INDEX], out channelID))
                        {
                            if (channelID > 2)              return null;

                            if (channelID > numOfChan)      return null;
                        }
                        else                                return null;
                    }
                    else {

                        channelID =                         1;
                    }

                    if (!csvFlag)
                    {
                        if (byte.TryParse(listOfLoggedValues[LENGTH_BIT_INDEX], out dataLength))
                        {

                            if (dataLength == 0)            return null;
                        }
                    }
                    else {

                        dataLength =                        8;
                    }

                    if (!csvFlag)
                    {
                        if (listOfLoggedValues[MESSAGE_ID_INDEX].ToCharArray().Contains('x'))
                        {
                            char[] ch =                     listOfLoggedValues[MESSAGE_ID_INDEX].ToCharArray();

                            ch =                            ch.Where(val => val != 'x').ToArray();

                            messageID =                     new string(ch);
                            extended =                      true;
                        }
                        else
                        {
                            messageID =                     listOfLoggedValues[MESSAGE_ID_INDEX];
                            extended =                      false;

                        }
                    }
                    else {
                        extended =                          false;
                        int ID =                            0;
                        int.TryParse(listOfLoggedValues[1], out ID);

                        messageID =                         ID.ToString("X");
                    }

                    if (!csvFlag)
                    {
                        for (int i = MESSAGE_INDEX; i < (MESSAGE_INDEX + dataLength); i++)
                        {

                            messageData[i - MESSAGE_INDEX] = Convert.ToByte(
                                value:                      listOfLoggedValues[i],
                                fromBase:                   16
                                );
                        }
                    }
                    else {

                        ulong var =                         ulong.Parse(listOfLoggedValues[2], System.Globalization.NumberStyles.HexNumber);
                        byte[] bytes =                      BitConverter.GetBytes(var);
                        messageData =                       bytes;
                    }

                    return new DataParameters()
                    {
                        Message_Time =                      messageTime - timeOffset,
                        Message_ID =                        messageID,
                        Message_Length =                    dataLength,
                        CAN_Message =                       messageData,
                        Extended =                          extended,
                        CAN_Channel_ID =                    channelID

                    };
                }

            }
            catch (Exception exception)
            {
                //handle any exception
                Console.WriteLine("An Exception occurred!!! Exception: " + exception.Message);
            }

            return null;
        }

               
        delegate Action ParentTransmitDelegate(int index_1);

        ParentTransmitDelegate parentTransmitDelegate = delegate (int parentIndex) {

            return (Action)(() => {

                for (int i = 0; i < CAN_INFO[parentIndex].Count(); i++) {

                    while (isPaused) {

                        if (!isStarted)                     return;
                    }

                    if (!isStarted)                         return;

                    progressIndex[0] =                      parentIndex;
                    progressIndex[1] =                      i;

                    DataParameters dataToTransmit =         CAN_INFO[parentIndex][i];
                    TimeSpan transmitTimeSpan =             TimeSpan.FromSeconds(dataToTransmit.Message_Time);
                    transmissionTime =                      startTime.Add(transmitTimeSpan);

                    while (DateTime.Now  < transmissionTime)
                    {

                        if (!isStarted)                     return;
                    }

                    CANTransmitterClass.Transmitter(data: dataToTransmit, dataToTransmit.CAN_Channel_ID);

                    if (CAN_INFO[parentIndex].Count == (i + 1)) {

                        Task.Delay(5000).GetAwaiter().GetResult();
                        taskCompleted[parentIndex] = true;
                    }                  

                }
            });
        };

        private async void BackgroundFuncToTransmitLog()
        {
            List<Task> CAN_Trasmit_Tasks =                  new List<Task>();

            for (int i = 0; i < numOfChan; i++) {

                int it =                                    i;
                CAN_Trasmit_Tasks.Add(Task.Run( parentTransmitDelegate(it)));
            }

            await Task.WhenAll(CAN_Trasmit_Tasks);
        }

        private void CAN_Channel_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(isStarted)                                   StopButton_Click(this, EventArgs.Empty);

            if(CANTransmitterClass.CanInit)                 CANTransmitterClass.Close();
        }

        private void canChanNum_ValueChanged(object sender, EventArgs e)
        {
            DirText.Clear();
        }
    }
}

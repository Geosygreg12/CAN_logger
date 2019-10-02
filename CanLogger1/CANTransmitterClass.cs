using System;
using canlibCLSNET;

namespace CanLogger1
{
    public class CANTransmitterClass
    {
        Canlib.canStatus status;
        int canHandle;       

        ushort peakHandle;
        TPCANBaudrate pCANBaudrate;
        TPCANMsg pCANMsg;
        TPCANStatus pCANStatus;

        public Form1 Form_1 { get; set; }
        public CANTransmitterClass() { } //public constructor

        //initialise the usb interface library based on which interface was selected
        public void Initialise()
        {
            switch (Form_1.GetInterface)
            {
                case 0: //0 means kvaser was selected

                    Canlib.canInitializeLibrary();
                    canHandle = Canlib.canOpenChannel(0, Canlib.canOPEN_ACCEPT_VIRTUAL);                //get the handle to the open channel
                    ErrorControl(handle: canHandle, location: "canOpenChannel: initialise()");          //check whether handle was gotten successfully
                    status = Canlib.canSetBusParams(canHandle, Canlib.canBITRATE_500K, 0, 0, 0, 0, 0);  //set the relevant can bus parameters
                    ErrorControl(status: this.status, location: "canSetBusParams: initialise()");
                    Canlib.canSetBusOutputControl(canHandle, Canlib.canDRIVER_NORMAL);
                    Canlib.canBusOn(canHandle);                                                         //turn the bus on with a handle to the open channel to write data

                    break;

                case 1: //1 means peak can was selected

                    peakHandle = PCANBasic.PCAN_USBBUS1;
                    pCANBaudrate = TPCANBaudrate.PCAN_BAUD_500K;

                    if (PCANBasic.Initialize(peakHandle, pCANBaudrate) == TPCANStatus.PCAN_ERROR_INITIALIZE)
                    {
                        ErrorControl(-1, location: "PCANBasic.initialize: initialise()");
                        return;
                    }

                    break;
            }

            isInitialised = true; //set control variable that monitors whether can bus was initialised or not to true
        }

        public static int num = 0; //index for current message to transmit
        internal static bool isInitialised = false;

        public void Transmitter()
        {
            switch (Form_1.GetInterface)
            {
                case 0:

                    if (Form_1.GetData.Count > num)
                    {
                        //write data to the can bus
                        Canlib.canWrite(canHandle, Convert.ToInt32(Form_1.GetData[num].Message_ID, 16), Form_1.GetData[num].CAN_Message, 8, Canlib.canMSG_EXT);
                    }
                    else return;   
                    
                    break;

                case 1:

                    if (Form_1.GetData.Count > num)
                    {
                        pCANMsg.DATA = Form_1.GetData[num].CAN_Message;
                        pCANMsg.ID = Convert.ToUInt32(Form_1.GetData[num].Message_ID, 16);
                        pCANMsg.LEN = Convert.ToByte(Form_1.GetData[num].Message_Length);

                        pCANStatus = PCANBasic.Write(peakHandle, ref pCANMsg);
                    }
                    else return;

                    if (pCANStatus < 0) //report any failure to the developer
                    {
                        Console.WriteLine("Writing file failed,  can status: " + pCANStatus +
                                           "\nThe message time is: " + Form_1.GetData[num].Message_Time +
                                           "\nThe message ID is: " + Form_1.GetData[num].Message_ID);
                    }

                    break;
            }

            num++;
        }

        //close the can bus after transmission
        public void Close()
        {
            switch (Form_1.GetInterface)
            {
                case 0:
                    Canlib.canBusOff(canHandle);
                    status = Canlib.canClose(canHandle);
                    ErrorControl(status: this.status, location: "canClose: Close()"); //check if bus closed successfully
                    canHandle = 0;
                    break;

                case 1:
                    
                    PCANBasic.Uninitialize(peakHandle);
                    break;
            }

            isInitialised = false;
        }

        private void ErrorControl(int handle = 1, Canlib.canStatus status = Canlib.canStatus.canOK, string location = "\0")
        {
            if (handle < 0)
            {
                Canlib.canGetErrorText((Canlib.canStatus)handle, out string msg); //get the error message
                Console.WriteLine("Handle error: " + msg + " \nThe location is: " + location); //write the error message and the location it occurred.

                Environment.Exit(1); //close this interface
            }

            if (status != Canlib.canStatus.canOK)
            {
                Console.WriteLine("A Can operation has failed: " + location);
                System.Windows.Forms.MessageBox.Show("A Can operation has failed: " + location, "Error",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
    }
}

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

        public CANTransmitterClass() { } 


        //initialise the usb interface library based on which interface was selected
        public void Initialise()
        {
            switch (Form_1.GetInterface)
            {

                //0 means kvaser was selected
                case 0: 

                    Canlib.canInitializeLibrary();

                    //get the handle to the open channel
                    canHandle = Canlib.canOpenChannel(0, Canlib.canOPEN_ACCEPT_VIRTUAL);

                    //check whether handle was gotten successfully
                    ErrorControl(handle: canHandle, location: "canOpenChannel: initialise()");

                    //set the relevant can bus parameters
                    status = Canlib.canSetBusParams(canHandle, Canlib.canBITRATE_500K, 0, 0, 0, 0, 0);  
                    ErrorControl(status: this.status, location: "canSetBusParams: initialise()");
                    Canlib.canSetBusOutputControl(canHandle, Canlib.canDRIVER_NORMAL);

                    //turn the bus on with a handle to the open channel to write data
                    Canlib.canBusOn(canHandle);                                                         

                    break;

                //1 means peak can was selected
                case 1: 

                    peakHandle = PCANBasic.PCAN_USBBUS1;
                    pCANBaudrate = TPCANBaudrate.PCAN_BAUD_500K;

                    if (PCANBasic.Initialize(peakHandle, pCANBaudrate) == TPCANStatus.PCAN_ERROR_INITIALIZE)
                    {
                        ErrorControl(-1, location: "PCANBasic.initialize: initialise()");
                        return;
                    }

                    break;
            }

            //set control variable that monitors whether can bus was initialised or not to true
            isInitialised = true; 
        }

        //index for current message to transmit
        public static int num = 0; 

        internal static bool isInitialised = false;

        public void Transmitter()
        {
            switch (Form_1.GetInterface)
            {
                case 0:

                    if (Form_1.GetData.Count > num)
                    {
                        //write data to the can bus
                        switch (Form_1.GetData[num].Extended)
                        {

                            case true:

                                Canlib.canWrite(canHandle, Convert.ToInt32(Form_1.GetData[num].Message_ID, 16),
                                    Form_1.GetData[num].CAN_Message, 8, Canlib.canMSG_EXT);

                                break;

                            case false:

                                Canlib.canWrite(canHandle, Convert.ToInt32(Form_1.GetData[num].Message_ID, 16),
                                    Form_1.GetData[num].CAN_Message, 8, 0);

                                break;
                        }

                    }
                    else return;   
                    
                    break;

                case 1:

                    if (Form_1.GetData.Count > num)
                    {
                        switch (Form_1.GetData[num].Extended) 
                        {

                            case true:

                                pCANMsg.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_EXTENDED; 
                                goto default; 

                            case false: 
                                
                                pCANMsg.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_STANDARD;
                                goto default;

                            default:
                                pCANMsg.DATA = Form_1.GetData[num].CAN_Message;
                                pCANMsg.ID = Convert.ToUInt32(Form_1.GetData[num].Message_ID, 16);
                                pCANMsg.LEN = Convert.ToByte(Form_1.GetData[num].Message_Length);

                                pCANStatus = PCANBasic.Write(peakHandle, ref pCANMsg);

                                break;
                        
                        } 
                    }
                    else return;

                    //report any failure to the developer
                    if (pCANStatus < 0) 
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

                    //check if bus closed successfully
                    ErrorControl(status: this.status, location: "canClose: Close()"); 
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

                //get the error message
                Canlib.canGetErrorText((Canlib.canStatus)handle, out string msg);

                //write the error message and the location it occurred.
                Console.WriteLine("Handle error: " + msg + " \nThe location is: " + location);

                //close this interface
                Environment.Exit(1); 

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

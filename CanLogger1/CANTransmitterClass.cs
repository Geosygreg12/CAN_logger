using System;
using canlibCLSNET;

namespace CanLogger1
{
    public class CANTransmitterClass
    {
        Canlib.canStatus status;

        int numOfPeakChannels = 0;
        int numOfKvaserChannels = 0;
        public static bool isKvaserInitialised = false;
        public static bool isPeakInitialiased = false;

        int[] canHandle = new int[2];
        ushort[] peakHandle = new ushort[2];
        TPCANBaudrate[] pCANBaudrate = new TPCANBaudrate[2]{ 0, 0};
        TPCANMsg pCANMsg;
        TPCANStatus pCANStatus;

        enum CAN_Interface { 
            
            Kvaser,
            Peak
        }

        public CAN_Channel Form_1 { get; set; }

        public CANTransmitterClass() {}

        //initialise the usb interface library based on which interface was selected
        public void Initialise(int CANInterface = 0, int baudrate = 0)
        {

            switch (CANInterface)
            {

                //0 means kvaser was selected
                case (int) CAN_Interface.Kvaser:

                    Canlib.canInitializeLibrary();

                    //get the handle to the open channel
                    canHandle[numOfKvaserChannels] = Canlib.canOpenChannel(numOfKvaserChannels, Canlib.canOPEN_ACCEPT_VIRTUAL);

                    //check whether handle was gotten successfully
                    ErrorControl(handle: canHandle[CANInterface], location: "canOpenChannel: initialise()");
                    
                    //set the relevant can bus parameters
                    switch (baudrate)
                    {
                        case 0:
                            status = Canlib.canSetBusParams(canHandle[numOfKvaserChannels], Canlib.canBITRATE_250K, 0, 0, 0, 0, 0);
                            break;

                        case 1:
                            status = Canlib.canSetBusParams(canHandle[numOfKvaserChannels], Canlib.canBITRATE_500K, 0, 0, 0, 0, 0);
                            break;

                        default:
                            status = Canlib.canSetBusParams(canHandle[numOfKvaserChannels], Canlib.canBITRATE_500K, 0, 0, 0, 0, 0);
                            break;
                    }


                    

                    ErrorControl(status: this.status, location: "canSetBusParams: initialise()");
                    Canlib.canSetBusOutputControl(canHandle[numOfKvaserChannels], Canlib.canDRIVER_NORMAL);

                    //turn the bus on with a handle to the open channel to write data
                    Canlib.canBusOn(canHandle[numOfKvaserChannels]);

                    numOfKvaserChannels++;
                    isKvaserInitialised = true;
                    break;

                //1 means peak can was selected
                case (int)CAN_Interface.Peak:

                    if (numOfPeakChannels == 1)
                    {
                        peakHandle[numOfPeakChannels] = PCANBasic.PCAN_USBBUS2;
                    }
                    else peakHandle[numOfPeakChannels] = PCANBasic.PCAN_USBBUS1;

                    switch (baudrate) {
                        case 0: pCANBaudrate[numOfPeakChannels] = TPCANBaudrate.PCAN_BAUD_250K;
                            break;

                        case 1: pCANBaudrate[numOfPeakChannels] = TPCANBaudrate.PCAN_BAUD_500K;
                            break;

                        default: pCANBaudrate[numOfPeakChannels] = TPCANBaudrate.PCAN_BAUD_500K;
                            break;
                    }
                    

                    if (PCANBasic.Initialize(peakHandle[numOfPeakChannels], pCANBaudrate[numOfPeakChannels]) == TPCANStatus.PCAN_ERROR_INITIALIZE)
                    {
                        ErrorControl(-1, location: "PCANBasic.initialize: initialise()");
                        return;
                    }

                    numOfPeakChannels++;
                    isPeakInitialiased = true;
                    break;
            }
        }

        //index for current message to transmit
        public static int num = 0; 

        public void Transmitter(int CANInterface, int whichCAN )
        {
            switch (CANInterface)
            {
                case (int) CAN_Interface.Kvaser:

                    if (Form_1.GetData.Count > num)
                    {
                        //write data to the can bus
                        switch (Form_1.GetData[num].Extended)
                        {

                            case true:

                                if ((whichCAN == 2) && (numOfKvaserChannels == 2))
                                {

                                    Canlib.canWrite(canHandle[1], Convert.ToInt32(Form_1.GetData[num].Message_ID, 16),
                                    Form_1.GetData[num].CAN_Message, 8, Canlib.canMSG_EXT);
                                }
                                else {

                                    Canlib.canWrite(canHandle[0], Convert.ToInt32(Form_1.GetData[num].Message_ID, 16),
                                    Form_1.GetData[num].CAN_Message, 8, Canlib.canMSG_EXT);

                                }

                                break;

                            case false:


                                if ((whichCAN == 2) && (numOfKvaserChannels == 2))
                                {
                                    Canlib.canWrite(canHandle[1], Convert.ToInt32(Form_1.GetData[num].Message_ID, 16),
                                    Form_1.GetData[num].CAN_Message, 8, 0);
                                }
                                else
                                {
                                    Canlib.canWrite(canHandle[0], Convert.ToInt32(Form_1.GetData[num].Message_ID, 16),
                                    Form_1.GetData[num].CAN_Message, 8, 0);
                                }

                                break;
                        }

                    }
                    else return;   
                    
                    break;

                case (int) CAN_Interface.Peak:

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

                                if (whichCAN == 2 && numOfPeakChannels == 2)
                                {

                                    pCANStatus = PCANBasic.Write(peakHandle[1], ref pCANMsg);
                                }
                                else {

                                    pCANStatus = PCANBasic.Write(peakHandle[0], ref pCANMsg);
                                }
                                

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
        public void Close(int[] CANInterface)
        {
            numOfKvaserChannels = 0;
            numOfPeakChannels = 0;

            for (int i = 0; i < 2; i++) {


                switch (CANInterface[i])
                {
                    case (int) CAN_Interface.Kvaser:

                        if (numOfKvaserChannels > 1)
                        {

                            Canlib.canBusOff(canHandle[i]);
                            status = Canlib.canClose(canHandle[i]);
                            canHandle[i] = 0;
                        }
                        else {


                            Canlib.canBusOff(canHandle[0]);
                            status = Canlib.canClose(canHandle[0]);
                            canHandle[0] = 0;
                        }
                        

                        //check if bus closed successfully

                        ErrorControl(status: this.status, location: "canClose: Close()");
                        isKvaserInitialised = false;

                        break;

                    case (int) CAN_Interface.Peak :

                        if (numOfPeakChannels > 1)
                        {

                            PCANBasic.Uninitialize(peakHandle[i]);
                        }
                        else {
                            PCANBasic.Uninitialize(peakHandle[0]);
                        }
                        
                        isPeakInitialiased = false;

                        break;
                }

            }


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
                Close(Form_1.GetInterface);
                Form_1.Close();

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

using System;
using canlibCLSNET;

namespace CanLogger1
{
    public class CANTransmitterClass
    {
        static Canlib.canStatus status;

        static int[] canHandle =                            new int[2];

        static TPCANBaudrate[] pCANBaudrate =               new TPCANBaudrate[2]{ 0, 0};
        static TPCANMsg pCANMsg;
        static TPCANStatus pCANStatus;

        static bool KvaserInit =                            false;
        public static bool CanInit =                        false;

        static int numOfKvaser =                            0;
        static int numOfPeak =                              0;
        
        public CANTransmitterClass() {}


        //initialise the usb interface library based on which interface was selected
        public static void initialiseCAN()
        {

            for (int i = 0; i < CAN_Channel.numOfChan; i++) {

                switch (CAN_Channel._INTERFACEs[i]) {

                    case CAN_Channel.CAN_INTERFACE.KVASER:


                        if (!KvaserInit)                    Canlib.canInitializeLibrary();

                        if (CanInit)                        Close();

                        numOfKvaser++;

                        canHandle[i] =                      Canlib.canOpenChannel(numOfKvaser - 1, Canlib.canOPEN_ACCEPT_VIRTUAL); 
                        KvaserInit =                        true;

                        //check whether handle was gotten successfully
                        ErrorControl(handle: canHandle[i], location: "canOpenChannel: initialise()");

                        switch (CAN_Channel._BAUDRATEs[i])
                        {

                            case CAN_Channel.CAN_BAUDRATE._250K:

                                status =                    Canlib.canSetBusParams(canHandle[i], Canlib.canBITRATE_250K, 0, 0, 0, 0, 0);

                                break;

                            case CAN_Channel.CAN_BAUDRATE._500K:

                                status =                    Canlib.canSetBusParams(canHandle[i], Canlib.canBITRATE_500K, 0, 0, 0, 0, 0);

                                break;

                            case CAN_Channel.CAN_BAUDRATE._1M:

                                status = Canlib.canSetBusParams(canHandle[i], Canlib.canBITRATE_1M, 0, 0, 0, 0, 0);
                                break;
                        }

                        ErrorControl(status: status, location: "canSetBusParams: initialise()");
                        Canlib.canSetBusOutputControl(canHandle[i], Canlib.canDRIVER_NORMAL);

                        //turn the bus on with a handle to the open channel to write data
                        Canlib.canBusOn(canHandle[i]);
                                                
                        break;

                    case CAN_Channel.CAN_INTERFACE.PEAK:

                        if (CanInit)                        Close();

                        numOfPeak++;

                        if (numOfPeak == 1)                 canHandle[i] = PCANBasic.PCAN_USBBUS1;
                        if (numOfPeak == 2)                 canHandle[i] = PCANBasic.PCAN_USBBUS2;

                        switch (CAN_Channel._BAUDRATEs[i])
                        {

                            case CAN_Channel.CAN_BAUDRATE._250K:

                                pCANBaudrate[numOfPeak - 1] = TPCANBaudrate.PCAN_BAUD_250K;

                                break;

                            case CAN_Channel.CAN_BAUDRATE._500K:

                                pCANBaudrate[numOfPeak - 1] = TPCANBaudrate.PCAN_BAUD_500K;

                                break;

                            case CAN_Channel.CAN_BAUDRATE._1M:

                                pCANBaudrate[numOfPeak - 1] = TPCANBaudrate.PCAN_BAUD_1M;
                                break;
                        }


                        if (PCANBasic.Initialize((ushort)canHandle[i], pCANBaudrate[numOfPeak - 1]) == TPCANStatus.PCAN_ERROR_INITIALIZE)
                        {
                            ErrorControl(-1, location: "PCANBasic.initialize: initialise()");
                            return;
                        }

                                                
                        break;


                }

                
            }

            CanInit =                                       true;
            numOfPeak =                                     0;
            numOfKvaser =                                   0;
        }



        
        public static void Transmitter(CAN_Channel.DataParameters data, int CAN_ID = 1 )
        {
            switch (CAN_Channel._INTERFACEs[CAN_ID - 1]) {

                case CAN_Channel.CAN_INTERFACE.KVASER:

                    //write data to the can bus
                    switch (data.Extended)
                    {
                        case true:

                            Canlib.canWrite(canHandle[CAN_ID - 1], Convert.ToInt32(data.Message_ID, 16),
                                data.CAN_Message, 8, Canlib.canMSG_EXT);

                            break;

                        case false:

                            Canlib.canWrite(canHandle[CAN_ID - 1], Convert.ToInt32(data.Message_ID, 16),
                            data.CAN_Message, 8, 0);

                            break;
                    }


                    break;

                case CAN_Channel.CAN_INTERFACE.PEAK:


                    switch (data.Extended)
                    {

                        case true:

                            pCANMsg.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_EXTENDED;
                            goto default;

                        case false:

                            pCANMsg.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_STANDARD;
                            goto default;

                        default:
                            pCANMsg.DATA = data.CAN_Message;
                            pCANMsg.ID = Convert.ToUInt32(data.Message_ID, 16);
                            pCANMsg.LEN = Convert.ToByte(data.Message_Length);

                            pCANStatus = PCANBasic.Write((ushort)canHandle[CAN_ID - 1], ref pCANMsg);

                            break;

                    }

                    //report any failure to the developer
                    if (pCANStatus < 0)
                    {
                        Console.WriteLine("Writing file failed,  can status: " + pCANStatus +
                                           "\nThe message time is: " + data.Message_Time +
                                           "\nThe message ID is: " + data.Message_ID);
                    }

                    break;
            
            }
        }

        //close the can bus after transmission
        public static void Close()
        {

            for (int i = 0; i < CAN_Channel.numOfChan; i++)
            {

                switch (CAN_Channel._INTERFACEs[i]) {

                    case CAN_Channel.CAN_INTERFACE.KVASER:

                        Canlib.canBusOff(canHandle[i]);
                        status = Canlib.canClose(canHandle[i]);
                        canHandle[i] = 0;
                        KvaserInit = false;

                        ErrorControl(status: status, location: "canClose: Close()");
                        break;

                    case CAN_Channel.CAN_INTERFACE.PEAK:

                        PCANBasic.Uninitialize((ushort) canHandle[i]);

                        break;
                }
            }

            CanInit = false;
        }

        static private void ErrorControl(int handle = 1, Canlib.canStatus status = Canlib.canStatus.canOK, string location = "\0")
        {
            if (handle < 0)
            {

                //get the error message
                Canlib.canGetErrorText((Canlib.canStatus)handle, out string msg);

                //write the error message and the location it occurred.
                Console.WriteLine("Handle error: " + msg + " \nThe location is: " + location);

                //close this interface
                Close();
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

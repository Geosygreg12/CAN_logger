using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using canlibCLSNET;

namespace CanLogger1
{
    public class CANTransmitterClass
    {
        Timer transmitterTimer;
        Canlib.canStatus status;
        int canHandle;
        public Form1 form1 = new Form1();

        ushort peakHandle;
        TPCANBaudrate pCANBaudrate;
        TPCANMsg pCANMsg;
        TPCANStatus pCANStatus;

        public bool tracker = false;

        public CANTransmitterClass() { form1.CANTransmitter = this;  initialise(); } //public constructor
        public void Transmitter()
        {
            transmitterTimer.Enabled = true;
            transmitterTimer.Start();
        }

        private void initialise()
        {
            switch (form1.GetInterface)
            {
                case 1:

                    Canlib.canInitializeLibrary();
                    canHandle = Canlib.canOpenChannel(1, Canlib.canOPEN_EXCLUSIVE);
                    errorControl(handle: canHandle, location: "canOpenChannel: initialise()");
                    status = Canlib.canSetBusParams(canHandle, Canlib.canBITRATE_500K, 0, 0, 0, 0, 0);
                    errorControl(status: this.status, location: "canSetBusParams: initialise()");
                    Canlib.canSetBusOutputControl(canHandle, Canlib.canDRIVER_NORMAL);
                    Canlib.canBusOn(canHandle);

                    break;

                case 2:

                    peakHandle = PCANBasic.PCAN_USBBUS1;
                    pCANBaudrate = TPCANBaudrate.PCAN_BAUD_500K;

                    if (PCANBasic.Initialize(peakHandle, pCANBaudrate) == TPCANStatus.PCAN_ERROR_INITIALIZE)
                    {
                        errorControl(-1, location: "PCANBasic.initialize: initialise()");
                        return;
                    }

                    break;
            }

            transmitterTimer = new Timer();
            transmitterTimer.Interval = 1;
            transmitterTimer.Elapsed += TransmitterTimer_Elapsed;
            transmitterTimer.Enabled = false;
        }

        private void TransmitterTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            switch (form1.GetInterface)
            {
                case 1:
                    Canlib.canStatus writeStatus = Canlib.canStatus.canOK;
                    byte[] msg = new byte[form1.GetData.Message_Length];

                    for (int i = 0; i < form1.GetData.Message_Length; i++)
                    {     
                        for (int j = 0; j < form1.GetData.Message_Length; j++)
                        {
                            byte Byte = Convert.ToByte(form1.GetData.CAN_Message[j]);
                            msg[j] = Byte;
                        }

                        Canlib.canWrite(canHandle, Convert.ToInt32(form1.GetData.Message_ID), msg, 8, Canlib.canMSG_EXT);
                        writeStatus = Canlib.canWriteSync(canHandle, 500);
                        tracker = true;

                        if (writeStatus < 0)
                        {
                            tracker = false;
                            Console.WriteLine("Writing file failed,  can status: " + writeStatus +
                                               "\nThe message is: " + form1.GetData.CAN_Message[i] +
                                               "\nThe message ID is: " + form1.GetData.Message_ID);
                            break;
                        }
                    }

                    break;

                case 2:
                    byte[] Msg = new byte[form1.GetData.Message_Length];

                    for (int i = 0; i < form1.GetData.Message_Length; i++)
                    {
                        for (int j = 0; j < form1.GetData.Message_Length; j++)
                        {
                            byte Byte = Convert.ToByte(form1.GetData.CAN_Message[j]);
                            Msg[j] = Byte;
                        }
                        pCANMsg.DATA = Msg;
                        pCANMsg.ID = Convert.ToUInt32(form1.GetData.Message_ID);
                        pCANMsg.LEN = Convert.ToByte(form1.GetData.Message_Length);
                        
                        pCANStatus = PCANBasic.Write(peakHandle, ref pCANMsg);
                        tracker = true;

                        if (pCANStatus < 0)
                        {
                            tracker = false;
                            Console.WriteLine("Writing file failed,  can status: " + pCANStatus +
                                               "\nThe message is: " + form1.GetData.CAN_Message[i] +
                                               "\nThe message ID is: " + form1.GetData.Message_ID);
                            break;
                        }
                    }

                    break;
            }

            transmitterTimer.Enabled = false;
        }

        public void Close()
        {
            switch (form1.GetInterface)
            {
                case 1:

                    Canlib.canBusOff(canHandle);
                    status = Canlib.canClose(canHandle);
                    errorControl(status: this.status, location: "canClose: Close()");

                    break;

                case 2:
                    
                    PCANBasic.Uninitialize(peakHandle);
                    break;
            }

            transmitterTimer.Stop();
            transmitterTimer.Enabled = false;
        }

        private void errorControl(int handle = 1, Canlib.canStatus status = Canlib.canStatus.canOK, string location = "\0")
        {
            if (handle < 0)
            {
                string msg = "\0";
                Canlib.canGetErrorText((Canlib.canStatus)handle, out msg);
                Console.WriteLine("Handle error: " + msg + " The location is: " + location);

                Environment.Exit(1);
                return;
            }

            if (status != Canlib.canStatus.canOK)
            {
                Console.WriteLine("A Can operation has failed: " + location);
                throw new Exception("CAN Operation cannot be executed at: " + location);
            }
        }
    }
}

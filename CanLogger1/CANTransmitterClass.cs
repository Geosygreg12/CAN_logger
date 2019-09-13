using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using canlibCLSNET;

namespace CanLogger1
{
    class CANTransmitterClass
    {
        Timer transmitterTimer;
        Canlib.canStatus status;
        int canHandle;
        Form1 form1 = new Form1();
        public CANTransmitterClass() { initialise(); } //public constructor
        public void Transmitter(bool enable)
        {
            transmitterTimer.Enabled = enable;
            if (enable) transmitterTimer.Start();
        }

        private void initialise()
        {
            Canlib.canInitializeLibrary();
            canHandle = Canlib.canOpenChannel(1, Canlib.canOPEN_EXCLUSIVE);
            errorControl(handle: canHandle);
            status = Canlib.canSetBusParams(canHandle, Canlib.canBITRATE_500K, 0, 0, 0, 0, 0);
            errorControl(status: this.status, location: "canSetBusParams: initialise()");
            Canlib.canSetBusOutputControl(canHandle, Canlib.canDRIVER_NORMAL);
            Canlib.canBusOn(canHandle);

            transmitterTimer = new Timer();
            transmitterTimer.Interval = 1;
            transmitterTimer.Elapsed += TransmitterTimer_Elapsed;
        }

        private void TransmitterTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Canlib.canStatus writeStatus = Canlib.canStatus.canOK;

            for (int i = 0; i < form1.GetData.CAN_Message.Count; i++)
            {
                byte[] msg = Encoding.ASCII.GetBytes(form1.GetData.CAN_Message[i]);
                Canlib.canWrite(canHandle, Convert.ToInt32(form1.GetData.Message_ID), msg, 8, Canlib.canMSG_EXT);
                writeStatus = Canlib.canWriteSync(canHandle, 500);

                if (writeStatus < 0)
                {
                    Console.WriteLine("Writing file failed,  can status: " + writeStatus + 
                                       "\nThe message is: " + form1.GetData.CAN_Message[i] +
                                       "\nThe message ID is: " + form1.GetData.Message_ID);
                    break;
                }
            }
        }

        public void Close()
        {
            transmitterTimer.Enabled = false;
            Canlib.canBusOff(canHandle);
            status = Canlib.canClose(canHandle);
            errorControl(status: this.status, location: "canClose: Close()");
        }

        private void errorControl(int handle = 1, Canlib.canStatus status = Canlib.canStatus.canOK, string location = "\0")
        {
            if (handle < 0)
            {
                string msg = "\0";
                Canlib.canGetErrorText((Canlib.canStatus)handle, out msg);
                Console.WriteLine("Handle error: " + msg);

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

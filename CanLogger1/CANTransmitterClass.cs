using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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

        public bool tracker = false;
        public Form1 form1 { get; set; }
        public CANTransmitterClass() { } //public constructor
        public void initialise()
        {
            switch (form1.GetInterface)
            {
                case 0:

                    Canlib.canInitializeLibrary();
                    canHandle = Canlib.canOpenChannel(0, Canlib.canOPEN_ACCEPT_VIRTUAL);
                    errorControl(handle: canHandle, location: "canOpenChannel: initialise()");
                    status = Canlib.canSetBusParams(canHandle, Canlib.canBITRATE_500K, 0, 0, 0, 0, 0);
                    errorControl(status: this.status, location: "canSetBusParams: initialise()");
                    Canlib.canSetBusOutputControl(canHandle, Canlib.canDRIVER_NORMAL);
                    Canlib.canBusOn(canHandle);

                    break;

                case 1:

                    peakHandle = PCANBasic.PCAN_USBBUS1;
                    pCANBaudrate = TPCANBaudrate.PCAN_BAUD_500K;

                    if (PCANBasic.Initialize(peakHandle, pCANBaudrate) == TPCANStatus.PCAN_ERROR_INITIALIZE)
                    {
                        errorControl(-1, location: "PCANBasic.initialize: initialise()");
                        return;
                    }

                    break;
            }
        }
        public static int num = 0;
        public static bool cancel = false;
        CancellationTokenSource cts = new CancellationTokenSource();
        public void Transmitter()
        {
            CancellationToken ct = cts.Token;

            switch (form1.GetInterface)
            {
                case 0:

                    Canlib.canStatus writeStatus = Canlib.canStatus.canOK;
                    //byte[] msg;
                    //if (form1.GetData.Count > num) msg = new byte[form1.GetData[num].Message_Length];
                    //else { msg = null; return; }

                    //for (int j = 0; j < form1.GetData[num].Message_Length; j++)
                    //{
                    //    byte Byte = 0;
                    //    try { Byte = Convert.ToByte(form1.GetData[num].CAN_Message[j], 16); }
                    //    catch (Exception exc) { Console.WriteLine("Error :" + exc.Message); }
                    //    msg[j] = Byte;
                    //}

                    Canlib.canWrite(canHandle, Convert.ToInt32(form1.GetData[num].Message_ID, 16), form1.GetData[num].CAN_Message, 8, Canlib.canMSG_EXT);
                    writeStatus = Canlib.canWriteSync(canHandle, 500);
                    tracker = true;

                    if (writeStatus < 0)
                    {
                        //tracker = false;
                        Console.WriteLine("Writing file failed,  can status: " + writeStatus +
                                           "\nThe message is: " + form1.GetData[num].CAN_Message.ToString() +
                                           "\nThe message ID is: " + form1.GetData[num].Message_ID);
                        return;
                    }

                    num++;
                    break;

                case 1: 
                    //byte[] Msg = new byte[form1.GetData[0].Message_Length];
                    
                    //for (int j = 0; j < form1.GetData[0].Message_Length; j++)
                    //{
                    //    byte Byte = Convert.ToByte(form1.GetData[0].CAN_Message[j], 16);
                    //    Msg[j] = Byte;
                    //}
                    pCANMsg.DATA = form1.GetData[num].CAN_Message;
                    pCANMsg.ID = Convert.ToUInt32(form1.GetData[0].Message_ID, 16);
                    pCANMsg.LEN = Convert.ToByte(form1.GetData[0].Message_Length);

                    pCANStatus = PCANBasic.Write(peakHandle, ref pCANMsg);
                    tracker = true;

                    if (pCANStatus < 0)
                    {
                        //tracker = false;
                        Console.WriteLine("Writing file failed,  can status: " + pCANStatus +
                                           "\nThe message is: " + form1.GetData[0].CAN_Message.ToString() +
                                           "\nThe message ID is: " + form1.GetData[0].Message_ID);
                        return;
                    }

                    break;
            }

        }

        public void Close()
        {
            switch (form1.GetInterface)
            {
                case 0:
                    Canlib.canBusOff(canHandle);
                    status = Canlib.canClose(canHandle);
                    errorControl(status: this.status, location: "canClose: Close()");
                    canHandle = 0;
                    break;

                case 1:
                    
                    PCANBasic.Uninitialize(peakHandle);
                    break;
            }

            tracker = false;
        }

        private void errorControl(int handle = 1, Canlib.canStatus status = Canlib.canStatus.canOK, string location = "\0")
        {
            if (handle < 0)
            {
                string msg = "\0";
                Canlib.canGetErrorText((Canlib.canStatus)handle, out msg);
                //System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Handle error: " + msg + " \nThe location is: " + location, "Error",
                //System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Console.WriteLine("Handle error: " + msg + " \nThe location is: " + location);

                Environment.Exit(1);
            }

            if (status != Canlib.canStatus.canOK)
            {
                Console.WriteLine("A Can operation has failed: " + location);
                System.Windows.Forms.MessageBox.Show("A Can operation has failed: " + location, "Error",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                //throw new Exception("CAN Operation cannot be executed at: " + location);
            }
        }
    }
}

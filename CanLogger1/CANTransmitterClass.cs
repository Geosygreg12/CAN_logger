using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using canlibCLSNET;

namespace CanLogger1
{
    class CANTransmitterClass
    {
        public CANTransmitterClass() { } //public constructor
        public void Transmitter(bool enable)
        {
            Canlib.canInitializeLibrary();
        }
    }
}

using brainflow;
using System.ComponentModel;

namespace Bonsai.BrainFlow
{
    [Description("Captures biosignals from the OpenBCI 4-channel Ganglion board.")]
    public class GanglionBoard : BrainFlowBoard
    {
        [TypeConverter(typeof(PortNameConverter))]
        [Description("The name of the serial port assigned to the Ganglion board.")]
        public string PortName { get; set; }

        [Description("The optional MAC address of the Ganglion board. If empty, BrainFlow will try to autodiscover the board.")]
        public string MacAddress { get; set; }

        [Description("The optional timeout for connecting to the Ganglion board.")]
        public int Timeout { get; set; }

        internal override BoardIds ConfigureBoard(BrainFlowInputParams inputParams)
        {
            inputParams.serial_port = PortName;
            inputParams.mac_address = MacAddress;
            inputParams.timeout = Timeout;
            return BoardIds.GANGLION_BOARD;
        }
    }
}

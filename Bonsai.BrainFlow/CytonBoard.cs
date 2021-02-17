using brainflow;
using System.ComponentModel;

namespace Bonsai.BrainFlow
{
    [Description("Captures biosignals from the OpenBCI 8-channel Cyton Board.")]
    public class CytonBoard : BrainFlowBoard
    {
        [TypeConverter(typeof(PortNameConverter))]
        [Description("The name of the serial port assigned to the Cyton board.")]
        public string PortName { get; set; }

        internal override BoardIds ConfigureBoard(BrainFlowInputParams inputParams)
        {
            inputParams.serial_port = PortName;
            return BoardIds.CYTON_BOARD;
        }
    }
}

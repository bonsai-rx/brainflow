using brainflow;
using System.ComponentModel;

namespace Bonsai.BrainFlow
{
    [Description("Captures biosignals from the OpenBCI 16-channel Cyton+Daisy module.")]
    public class CytonDaisyBoard : BrainFlowBoard
    {
        [TypeConverter(typeof(PortNameConverter))]
        [Description("The name of the serial port assigned to the Cyton board.")]
        public string PortName { get; set; }

        internal override BoardIds ConfigureBoard(BrainFlowInputParams inputParams)
        {
            inputParams.serial_port = PortName;
            return BoardIds.CYTON_DAISY_BOARD;
        }
    }
}

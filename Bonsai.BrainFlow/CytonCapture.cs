using brainflow;
using System.ComponentModel;

namespace Bonsai.BrainFlow
{
    [Description("Captures EEG signals from the OpenBCI 8-channel Cyton Biosensing Board.")]
    public class CytonCapture : BrainFlowCapture
    {
        [TypeConverter("Bonsai.IO.PortNameConverter, Bonsai.System")]
        [Description("The name of the serial port assigned to the Cyton board.")]
        public string PortName { get; set; }

        internal override BoardIds ConfigureBoard(BrainFlowInputParams inputParams)
        {
            inputParams.serial_port = PortName;
            return BoardIds.CYTON_BOARD;
        }
    }
}

﻿using brainflow;
using System.ComponentModel;

namespace Bonsai.BrainFlow
{
    [Description("Captures biosignals streaming over WiFi using the WiFi shield mounted on the OpenBCI 16-channel Cyton+Daisy module.")]
    public class CytonDaisyWiFiBoard : BrainFlowBoard
    {
        [Description("The IP address of the WiFi shield. If empty, BrainFlow will try to autodiscover the board.")]
        public string Address { get; set; }

        [Description("The local IP port used for the receiving socket.")]
        public int Port { get; set; }
        
        [Description("The optional timeout for connecting to the Cyton+Daisy WiFi shield.")]
        public int Timeout { get; set; }

        internal override BoardIds ConfigureBoard(BrainFlowInputParams inputParams)
        {
            inputParams.ip_address = Address;
            inputParams.ip_port = Port;
            inputParams.timeout = Timeout;
            return BoardIds.CYTON_DAISY_WIFI_BOARD;
        }
    }
}

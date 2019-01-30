using Serilog;
using System;

namespace BSM.DataServer
{
    class BSMDataProcessor : IDataProcessor
    {
        public void Process(Datagram datagram)
        {
            if (datagram.State != DatagramState.OK)
            {
                Log.Error("Datagram with {0}: {1}", datagram.State.ToString("G"), BitConverter.ToString(datagram.Bytes));
                return;
            }

            if (datagram.Type == Datagram.TYPE_DATA)
            {
                this.ProcessReceivedData(datagram);
            }
            else if (datagram.Type == Datagram.TYPE_HEART_BEAT)
            {
                this.ProcessHeartBeat(datagram);
            }
            else if (datagram.Type == Datagram.TYPE_REGISTER)
            {
                this.ProcessRegister(datagram);
            }
            else if (datagram.Type == Datagram.TYPE_ADDR_SET_COMMIT)
            {
                this.ProcessAddressSetCommit(datagram);
            }
            else if (datagram.Type == Datagram.TYPE_TILT_RESET_COMMIT)
            {
                this.ProcessTiltResetCommit(datagram);
            }
        }

        private void ProcessReceivedData(Datagram datagram)
        {

        }

        private void ProcessHeartBeat(Datagram datagram)
        {

        }

        private void ProcessRegister(Datagram datagram)
        {

        }

        private void ProcessAddressSetCommit(Datagram datagram)
        {

        }

        private void ProcessTiltResetCommit(Datagram datagram)
        {

        }
    }
}

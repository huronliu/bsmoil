using BSM.Common.DB;
using BSM.Common.Model;
using Serilog;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BSM.DataServer
{
    class BSMDataProcessor : IDataProcessor
    {
        public async Task Process(Datagram datagram)
        {
            if (datagram.State != DatagramState.OK)
            {
                Log.Error("Datagram with {0}: {1}", datagram.State.ToString("G"), BitConverter.ToString(datagram.Bytes));
                return;
            }

            if (datagram.Type == Datagram.TYPE_DATA)
            {
                await this.ProcessReceivedData(datagram);
            }
            else if (datagram.Type == Datagram.TYPE_HEART_BEAT)
            {
                await this.ProcessHeartBeat(datagram);
            }
            else if (datagram.Type == Datagram.TYPE_REGISTER)
            {
                await this.ProcessRegister(datagram);
            }
            else if (datagram.Type == Datagram.TYPE_ADDR_SET_COMMIT)
            {
                await this.ProcessAddressSetCommit(datagram);
            }
            else if (datagram.Type == Datagram.TYPE_TILT_RESET_COMMIT)
            {
                await this.ProcessTiltResetCommit(datagram);
            }
        }

        private async Task ProcessReceivedData(Datagram datagram)
        {
            //parse the post data according to protocol ver1.0
            StationData sd = new StationData();
            sd.Data = datagram.Data;
            sd.ReceivedAt = DateTime.Now;
            string cord_addr = BitConverter.ToString(sd.Data, 0, 4);

            using(BSMContext context = new BSMContext(Config.DBConnection))
            {
                var coordinator = await context.Coordinators.SingleOrDefaultAsync(cord => cord.Address.Equals(cord_addr, StringComparison.OrdinalIgnoreCase));
                
                if (coordinator != null)
                {
                    coordinator.IPHost = datagram.From.Address.ToString();
                    coordinator.IPPort = datagram.From.Port;

                    sd.StationId = coordinator.StationId;
                    sd.SeqId = coordinator.SeqId;

                    sd.Tilt1_Addr = BitConverter.ToString(sd.Data, 4, 1);
                    sd.Tilt1_X_Positive = sd.Data[5];
                    sd.Tilt1_X_Degree = sd.Data[6];
                    sd.Tilt1_X_Minute = sd.Data[7];
                    sd.Tilt1_X_Second = sd.Data[8];
                    sd.Tilt1_Y_Positive = sd.Data[9];
                    sd.Tilt1_Y_Degree = sd.Data[10];
                    sd.Tilt1_Y_Minute = sd.Data[11];
                    sd.Tilt1_Y_Second = sd.Data[12];

                    sd.Tilt2_Addr = BitConverter.ToString(sd.Data, 13, 1);
                    sd.Tilt2_X_Positive = sd.Data[14];
                    sd.Tilt2_X_Degree = sd.Data[15];
                    sd.Tilt2_X_Minute = sd.Data[16];
                    sd.Tilt2_X_Second = sd.Data[17];
                    sd.Tilt2_Y_Positive = sd.Data[18];
                    sd.Tilt2_Y_Degree = sd.Data[19];
                    sd.Tilt2_Y_Minute = sd.Data[20];
                    sd.Tilt2_Y_Second = sd.Data[21];

                    sd.Tilt3_Addr = BitConverter.ToString(sd.Data, 22, 1);
                    sd.Tilt3_X_Positive = sd.Data[23];
                    sd.Tilt3_X_Degree = sd.Data[24];
                    sd.Tilt3_X_Minute = sd.Data[25];
                    sd.Tilt3_X_Second = sd.Data[26];
                    sd.Tilt3_Y_Positive = sd.Data[27];
                    sd.Tilt3_Y_Degree = sd.Data[28];
                    sd.Tilt3_Y_Minute = sd.Data[29];
                    sd.Tilt3_Y_Second = sd.Data[30];

                    sd.Tilt4_Addr = BitConverter.ToString(sd.Data, 31, 1);
                    sd.Tilt4_X_Positive = sd.Data[32];
                    sd.Tilt4_X_Degree = sd.Data[33];
                    sd.Tilt4_X_Minute = sd.Data[34];
                    sd.Tilt4_X_Second = sd.Data[35];
                    sd.Tilt4_Y_Positive = sd.Data[36];
                    sd.Tilt4_Y_Degree = sd.Data[37];
                    sd.Tilt4_Y_Minute = sd.Data[38];
                    sd.Tilt4_Y_Second = sd.Data[39];

                    context.Add(sd);
                    await context.SaveChangesAsync();
                }
                else
                {
                    throw new ApplicationException($"没有找到协调器地址 '{cord_addr}',请先在数据库中注册该协调器");
                }
            }
        }

        private async Task ProcessHeartBeat(Datagram datagram)
        {
            string coord_addr = BitConverter.ToString(datagram.Data, 0, 4);
            using (BSMContext context = new BSMContext(Config.DBConnection))
            {
                Coordinator coordinator = await context.Coordinators.SingleOrDefaultAsync(coor =>
                        coor.Address.Equals(coord_addr, StringComparison.OrdinalIgnoreCase));
                if (coordinator != null)
                {
                    coordinator.IPHost = datagram.From.Address.ToString();
                    coordinator.IPPort = datagram.From.Port;
                    await context.SaveChangesAsync();
                }
            }
        }

        private async Task ProcessRegister(Datagram datagram)
        {
            string coord_addr = BitConverter.ToString(datagram.Data, 0, 4);
            using(BSMContext context = new BSMContext(Config.DBConnection))
            {
                Coordinator coordinator = await context.Coordinators.SingleOrDefaultAsync(coor => 
                        coor.Address.Equals(coord_addr, StringComparison.OrdinalIgnoreCase));
                if (coordinator != null)
                {
                    coordinator.IPHost = datagram.From.Address.ToString();
                    coordinator.IPPort = datagram.From.Port;
                    await context.SaveChangesAsync();
                }
            }
        }

        private async Task ProcessAddressSetCommit(Datagram datagram)
        {

        }

        private async Task ProcessTiltResetCommit(Datagram datagram)
        {

        }
    }
}

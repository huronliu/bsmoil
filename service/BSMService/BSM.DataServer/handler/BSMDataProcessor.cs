using BSM.Common.DB;
using BSM.Common.Model;
using Serilog;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

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
            
            sd.ReceivedAt = DateTime.Now;
            string cord_addr = BitConverter.ToString(datagram.Data, 0, 4);

            using(BSMContext context = new BSMContext(Config.DBConnection))
            {
                var coordinator = await context.Coordinators   
                                            .Where(cord => cord.Address.Equals(cord_addr, StringComparison.OrdinalIgnoreCase))
                                            .Include(coor => coor.Station)
                                            .SingleOrDefaultAsync();
                
                if (coordinator != null)
                {
                    coordinator.IPHost = datagram.From.Address.ToString();
                    coordinator.IPPort = datagram.From.Port;

                    sd.StationId = coordinator.StationId;
                    sd.SeqId = coordinator.SeqId;
                    float height = coordinator.Station.Height; //单位米

                    //第一倾角传感器
                    sd.Tilt1_Addr = BitConverter.ToString(datagram.Data, 4, 1);
                    sd.Tilt1_X_Positive = datagram.Data[5];
                    sd.Tilt1_X_Degree = datagram.Data[6];
                    sd.Tilt1_X_Minute = datagram.Data[7];
                    sd.Tilt1_X_Second = datagram.Data[8];
                    sd.Tilt1_Y_Positive = datagram.Data[9];
                    sd.Tilt1_Y_Degree = datagram.Data[10];
                    sd.Tilt1_Y_Minute = datagram.Data[11];
                    sd.Tilt1_Y_Second = datagram.Data[12];
                    sd.Tilt1X = (float)(sd.Tilt1_X_Degree + (float)sd.Tilt1_X_Minute / 60 + (float)sd.Tilt1_X_Second / 3600);
                    sd.Tilt1Y = (float)(sd.Tilt1_Y_Degree + (float)sd.Tilt1_Y_Minute / 60 + (float)sd.Tilt1_Y_Second / 3600);
                    sd.SkewingX = height * 1000 * Math.Sin(Math.PI * sd.Tilt1X.Value / 180.0);
                    sd.SkewingY = height * 1000 * Math.Sin(Math.PI * sd.Tilt1Y.Value / 180.0);

                    //第二倾角传感器
                    sd.Tilt2_Addr = BitConverter.ToString(datagram.Data, 13, 1);
                    sd.Tilt2_X_Positive = datagram.Data[14];
                    sd.Tilt2_X_Degree = datagram.Data[15];
                    sd.Tilt2_X_Minute = datagram.Data[16];
                    sd.Tilt2_X_Second = datagram.Data[17];
                    sd.Tilt2_Y_Positive = datagram.Data[18];
                    sd.Tilt2_Y_Degree = datagram.Data[19];
                    sd.Tilt2_Y_Minute = datagram.Data[20];
                    sd.Tilt2_Y_Second = datagram.Data[21];
                    sd.Tilt2X = (float)(sd.Tilt2_X_Degree + (float)sd.Tilt2_X_Minute / 60 + (float)sd.Tilt2_X_Second / 3600);
                    sd.Tilt2Y = (float)(sd.Tilt2_Y_Degree + (float)sd.Tilt2_Y_Minute / 60 + (float)sd.Tilt2_Y_Second / 3600);
                    //第三倾角传感器
                    sd.Tilt3_Addr = BitConverter.ToString(datagram.Data, 22, 1);
                    sd.Tilt3_X_Positive = datagram.Data[23];
                    sd.Tilt3_X_Degree = datagram.Data[24];
                    sd.Tilt3_X_Minute = datagram.Data[25];
                    sd.Tilt3_X_Second = datagram.Data[26];
                    sd.Tilt3_Y_Positive = datagram.Data[27];
                    sd.Tilt3_Y_Degree = datagram.Data[28];
                    sd.Tilt3_Y_Minute = datagram.Data[29];
                    sd.Tilt3_Y_Second = datagram.Data[30];
                    sd.Tilt3X = (float)(sd.Tilt3_X_Degree + (float)sd.Tilt3_X_Minute / 60 + (float)sd.Tilt3_X_Second / 3600);
                    sd.Tilt3Y = (float)(sd.Tilt3_Y_Degree + (float)sd.Tilt3_Y_Minute / 60 + (float)sd.Tilt3_Y_Second / 3600);
                    //第四倾角传感器
                    sd.Tilt4_Addr = BitConverter.ToString(datagram.Data, 31, 1);
                    sd.Tilt4_X_Positive = datagram.Data[32];
                    sd.Tilt4_X_Degree = datagram.Data[33];
                    sd.Tilt4_X_Minute = datagram.Data[34];
                    sd.Tilt4_X_Second = datagram.Data[35];
                    sd.Tilt4_Y_Positive = datagram.Data[36];
                    sd.Tilt4_Y_Degree = datagram.Data[37];
                    sd.Tilt4_Y_Minute = datagram.Data[38];
                    sd.Tilt4_Y_Second = datagram.Data[39];
                    sd.Tilt4X = (float)(sd.Tilt4_X_Degree + (float)sd.Tilt4_X_Minute / 60 + (float)sd.Tilt4_X_Second / 3600);
                    sd.Tilt4Y = (float)(sd.Tilt4_Y_Degree + (float)sd.Tilt4_Y_Minute / 60 + (float)sd.Tilt4_Y_Second / 3600);
                    //风速传感器
                    if (datagram.Data.Length > 44)
                    {
                        sd.Speed = BitConverter.ToSingle(datagram.Data, 41);
                    }
                    //温度传感器
                    if (datagram.Data.Length > 49)
                    {
                        sd.Temperature = BitConverter.ToSingle(datagram.Data, 46);
                    }
                    //雨量传感器
                    if (datagram.Data.Length > 54)
                    {
                        sd.Rain = BitConverter.ToSingle(datagram.Data, 51);
                    }
                    //水位传感器
                    if (datagram.Data.Length > 59)
                    {
                        sd.Water = BitConverter.ToSingle(datagram.Data, 56);
                    }
                    //位移1
                    if (datagram.Data.Length > 64)
                    {
                        sd.Move1 = BitConverter.ToSingle(datagram.Data, 61);
                    }
                    //位移2
                    if (datagram.Data.Length > 69)
                    {
                        sd.Move2 = BitConverter.ToSingle(datagram.Data, 66);
                    }
                    //位移3
                    if (datagram.Data.Length > 74)
                    {
                        sd.Move3 = BitConverter.ToSingle(datagram.Data, 71);
                    }
                    //位移4
                    if (datagram.Data.Length > 79)
                    {
                        sd.Move4 = BitConverter.ToSingle(datagram.Data, 76);
                    }

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

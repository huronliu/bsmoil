using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BSM.Common.Model
{
    public class StationData
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        public long StationId { get; set; }

        [Required]
        public int SeqId { get; set; }

        public float? Tilt1X { get; set; } //单位度
        public float? Tilt1Y { get; set; } //单位度
        public float? Tilt2X { get; set; }
        public float? Tilt2Y { get; set; }
        public float? Tilt3X { get; set; }
        public float? Tilt3Y { get; set; }
        public float? Tilt4X { get; set; }
        public float? Tilt4Y { get; set; }
        public float? Speed { get; set; } //单位 米/秒
        public float? Temperature { get; set; } //单位 度
        public float? Rain { get; set; }
        public double? SkewingX { get; set; } //单位毫米
        public double? SkewingY { get; set; } //单位毫米
        public float? Water { get; set; }
        public float? Move1 { get; set; }
        public float? Move2 { get; set; }
        public float? Move3 { get; set; }
        public float? Move4 { get; set; }

        public string Tilt1_Addr { get; set; }
        public byte? Tilt1_X_Positive { get; set; }
        public byte? Tilt1_X_Degree { get; set; }
        public byte? Tilt1_X_Minute { get; set; }
        public byte? Tilt1_X_Second { get; set; }
        public byte? Tilt1_Y_Positive { get; set; }
        public byte? Tilt1_Y_Degree { get; set; }
        public byte? Tilt1_Y_Minute { get; set; }
        public byte? Tilt1_Y_Second { get; set; }

        public string Tilt2_Addr { get; set; }
        public byte? Tilt2_X_Positive { get; set; }
        public byte? Tilt2_X_Degree { get; set; }
        public byte? Tilt2_X_Minute { get; set; }
        public byte? Tilt2_X_Second { get; set; }
        public byte? Tilt2_Y_Positive { get; set; }
        public byte? Tilt2_Y_Degree { get; set; }
        public byte? Tilt2_Y_Minute { get; set; }
        public byte? Tilt2_Y_Second { get; set; }

        public string Tilt3_Addr { get; set; }
        public byte? Tilt3_X_Positive { get; set; }
        public byte? Tilt3_X_Degree { get; set; }
        public byte? Tilt3_X_Minute { get; set; }
        public byte? Tilt3_X_Second { get; set; }
        public byte? Tilt3_Y_Positive { get; set; }
        public byte? Tilt3_Y_Degree { get; set; }
        public byte? Tilt3_Y_Minute { get; set; }
        public byte? Tilt3_Y_Second { get; set; }

        public string Tilt4_Addr { get; set; }
        public byte? Tilt4_X_Positive { get; set; }
        public byte? Tilt4_X_Degree { get; set; }
        public byte? Tilt4_X_Minute { get; set; }
        public byte? Tilt4_X_Second { get; set; }
        public byte? Tilt4_Y_Positive { get; set; }
        public byte? Tilt4_Y_Degree { get; set; }
        public byte? Tilt4_Y_Minute { get; set; }
        public byte? Tilt4_Y_Second { get; set; }

        public DateTime? ReceivedAt { get; set; }
    }
}

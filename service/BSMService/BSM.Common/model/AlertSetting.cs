using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BSM.Common.Model
{
    public class AlertSetting
    {
        [Key]
        public long Id { get; set; }

        public float? TiltThreshold { get; set; }

        public float? SkewingThreshold { get; set; }

        public float? SpeedThreshold { get; set; }

        public float? TempThreshold { get; set; }
    }
}

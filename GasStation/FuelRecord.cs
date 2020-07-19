using GasStationProject.Models.Enums;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace GasStationProject
{
    // Record for vehicle statistic
    public class FuelRecord
    {
        
        public int LineId { get; set; }
        public FuelType FuelType { get; set; }
        public DateTime FuelDateTime { get; set; }
        public float PumpWorkingTime { get; set; }
        public int CarId { get; set; }

       



    }
}

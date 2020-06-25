using GasStationProject.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GasStationProject.Models
{
    public class GasPump
    {
        //Tracking of current car to be able to release when car leaves
        public int CurrentCarId { get; set; }

        //Fuel type of GasPump
        public FuelType FuelType { get; set; }

        //Indicator if pump is being used
        public bool Taken { get; set; }
    }
}

using GasStationProject.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GasStationProject.Models
{
    public class GasStation
    {
        //lsit of static length of lanes
        public Lane[] Lanes { get; set; }
        public List<FuelRecord> FuelRecords { get; set; }
        public GasStation()
        {
            //3 lanes per station
            this.Lanes = new Lane[3];

            //create lanes with id 0,1,2
            this.Lanes[0] = new Lane(0);
            this.Lanes[1] = new Lane(1);
            this.Lanes[2] = new Lane(2);

            this.FuelRecords = new List<FuelRecord>();
        }

    }
}

using GasStationProject.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GasStationProject
{
    public class ProgramConstatns
    {
        public const float MinWaitTimeInSeconds = 10.0f;

        public const float MaxWaitTimeInSeconds = 30.0f;

        public const float BusFuelTime = 140;
        public const float CarFuelTime = 120;
        public const float TrukFuelTime = 100;

        public const FuelType BusFuel = FuelType.Unleaded;
        public const FuelType TruckFuel = FuelType.Disel;
        public const FuelType CarFuel = FuelType.Gasoline;

        public const string CarName = "Car";
        public const string BusName = "Bus";
        public const string TruckName = "Truck";

        public const float TickTimeInSeconds = 1;

        public const int MaxCycles = 1000;

        public const int CarArrivalChange = 50;

        public const int CarChance = 33;

        public const int BusChance = 33;

        public const float SecondsPerLitre = 3.00F;
    }
}

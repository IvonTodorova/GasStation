using GasStationProject.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GasStationProject.Models
{
    public class Car
    {
        //Holds fuel type
        public FuelType FuelType { get; set; }

        //For the time needed fueling
        public float FuelTimeMax { get; set; }

        //For the time the car has been fueling
        public float CurrentFuelTime { get; set; }

        //For the time a car is willing to wait
        public float WaitTimeMax { get; set; }

        //For the current time the car has waiting
        public float CurrentWaitTime { get; set; }

        //Indicating if fueling or waiting
        public bool IsFueling { get; set; } 
       
        //Name of the Car - Car, Bus, Truck
        public string Name { get; set; }

        //Car Id
        public int CarId { get; set; }


        public Car()
        {
            //Randome wait time of non integer value between min and max
            this.WaitTimeMax = (float)Program.rng.NextDouble() * (ProgramConstatns.MaxWaitTimeInSeconds - ProgramConstatns.MinWaitTimeInSeconds) + ProgramConstatns.MinWaitTimeInSeconds;
            //All cars are waiting by default
            this.IsFueling = false;

            //No car has started waiting or fueling before the first tick
            this.CurrentWaitTime = 0;
            this.CurrentFuelTime = 0;

            //Get Id from the Id provider
            this.CarId = Program.GetCarId();

            //Get chance value
            var chanceType = Program.rng.Next(0, 101);

            //Case depending on a roll between 0 and 100
            if (chanceType <= ProgramConstatns.CarChance)
            {
                //All cars have the same stats for fuel and fuel loading time
                this.FuelTimeMax = ProgramConstatns.CarFuelTime;
                this.FuelType = ProgramConstatns.CarFuel;
                this.Name = ProgramConstatns.CarName;
            }
            else if (chanceType > ProgramConstatns.CarChance && chanceType <=ProgramConstatns.CarChance+ ProgramConstatns.BusChance)
            {
                //All buses have the same stats for fuel and fuel loading time
                this.FuelTimeMax = ProgramConstatns.BusFuelTime;
                this.FuelType = ProgramConstatns.BusFuel;
                this.Name = ProgramConstatns.BusName;
            }
            else
            {
                //All trucks have the same stats for fuel and fuel loading time
                this.FuelTimeMax = ProgramConstatns.TrukFuelTime;
                this.FuelType = ProgramConstatns.TruckFuel;
                this.Name = ProgramConstatns.TruckName;
            }
            
            //Output
            Console.WriteLine($"{this.Name}  with ID:  {this.CarId} Brand type {RandBrand()}  created with fueling time {this.FuelTimeMax} sec , que wait time {Math.Round(this.WaitTimeMax,2)} sec , and fuel type {this.FuelType.ToString()}");
        }

        //get random Brand from Brand.Enums 
        public Enum RandBrand()
        {
            Random r = new Random();
            var randBrand = (Brand)r.Next(0, System.Enum.GetValues(typeof(Brand)).Length);
      
           return randBrand ;
          
        }
    }
}

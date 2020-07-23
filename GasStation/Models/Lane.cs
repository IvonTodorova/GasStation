using GasStationProject.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GasStationProject.Models
{
    public class Lane
    {
        //array for pumps
        public GasPump[] GasPumps { get; set; }

        //queue of cars - structure for first come first out;
        public Queue<Car> CarQueue { get; set; }


        //lane id
        public int LaneId { get; set; }

        public Lane(int laneId)
        {
            //Initilize 3 pumps per lane
            this.GasPumps = new GasPump[3];
            
            //One pump of each time
            this.GasPumps[0] = new GasPump() { FuelType = FuelType.Disel, Taken = false};
            this.GasPumps[1] = new GasPump() { FuelType = FuelType.Gasoline, Taken = false };
            this.GasPumps[2] = new GasPump() { FuelType = FuelType.Unleaded, Taken = false };

            //Set lane Id
            this.LaneId = laneId;

            //Create queue for cars
            this.CarQueue = new Queue<Car>();
        }

        //Add car to queue and output
        public void QueueCar(Car car)
        {
            this.CarQueue.Enqueue(car);

            Console.WriteLine($"\t\tVehicle with ID:{car.CarId} is queued at Lane:{this.LaneId}");
        }


        //Procedure to progress queue
        public void ProgressFuelingQue()
        {
            //if no cars in que no need to do anything
            if (this.CarQueue.Count == 0)
            {
                return;
            }
            //Car can be removed from the global tracker but will still be in queue so we need to check for such "ghost cars"
            this.CheckForLeftCars();

            //if no cars in que no need to do anything
            if (this.CarQueue.Count == 0)
            {
                return;
            }
            //Get first car from queue without removeing it
            var nextCar = this.CarQueue.Peek();

            foreach (var pump in this.GasPumps)
            {
                //condition for the free pump and car moving in the que
                if (pump.FuelType == nextCar.FuelType && !pump.Taken)
                {
                    //pump is takne
                    pump.Taken = true;
                    pump.CurrentCarId = nextCar.CarId;

                    //car is fueling
                    nextCar.IsFueling = true;

                    Console.WriteLine($"\t\t\tVehicle with ID: {nextCar.CarId} is fueling at lane: {this.LaneId}");

                    //remove car from que
                    this.CarQueue.Dequeue();
                }
            }
        }

        private void CheckForLeftCars()
        {
            //Get next car in queue without removeing it
            //just see next car 
            var nextCar = this.CarQueue.Peek();

            //if not in the global storage remove it from the que and check the next car
            while (!Program.Cars.ContainsKey(nextCar.CarId))
            {
                this.CarQueue.Dequeue();
                if (this.CarQueue.Count > 0)
                {
                    nextCar = this.CarQueue.Peek();
                }
                else
                {
                    break;
                }
            }
        }

    }
}

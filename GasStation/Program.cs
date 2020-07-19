using GasStationProject.Models;
using GasStationProject.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GasStationProject
{
    class Program
    {
        private static int carId = 0;

        //Initalize rng provider
        public static Random rng = new Random();

        //Inialize gloabal car tracker
        public static Dictionary<int, Car> Cars = new Dictionary<int, Car>();

        //Initialize Gasstatioon
        public static GasStation GasStation = new GasStation();



        static void Main(string[] args)
        {
            //Initalize cycle count
            var maxCycles = ProgramConstatns.MaxCycles;

            //Initialize current cycle index
            var currentCycle = 0;

            while (currentCycle < maxCycles)
            {
                //Progress cycle count
                currentCycle++;

                //Step1 - handle arriving cars
                ArrivingCarsStep();

                //Step2 - handle all queues
                ProgressFuelingQueuesStep();

                //Step3 handle cars that are done waiting
                WaitingCarsTickStep();

                //Step 4 handle cars that are done fueling
                FuelingCarsTickStep();

                //Step 5 
                if (LogHistoryFuelRecords())
                {
                    break;
                }
                //wait for TickTimeInSeconds*1000 miliseconds
                Thread.Sleep((int)ProgramConstatns.TickTimeInSeconds * 1000);
            }
        }
        private static void GetAllVehiclesPerDay()
        {//The count of serviced vehicles for the day 
            var vehicleCount = GasStation.FuelRecords.Where(x => x.FuelDateTime.Date == DateTime.Now.Date).Count();
            Console.WriteLine("Vehicles serviced today :" + vehicleCount);

        }
        private static float GetFuelPerDays(bool isItCalledFromEarnedMoneyForADay)
        {
            //colection for the week fuel records using using System.Linq;
            var daylyFuelRecords = GasStation.FuelRecords.Where(x => x.FuelDateTime.Date == DateTime.Now.Date);

            float daylySumRecord = 0;//fuel for the day
            float dieselDayRecord = 0;//litres diesel for the day 
            float gasDaylyRecord = 0;//gas litres for the day 
            float unleadedDaylyRecord = 0;//unleaded litres for the day 
            
            //count for the serviced cars with different fuels 
            float dieselCarCount = 0;
            float gasCarCount = 0;
            float unleadedCarCount = 0;

            //car ID and fueled litres 
            Dictionary<int, float> dieselCARcountID = new Dictionary<int, float>();
            Dictionary<int, float> gasCARcountID = new Dictionary<int, float>();
            Dictionary<int, float> unleadedCARcountID = new Dictionary<int, float>();

            //dayly profit for the  LINE 0  with different pumps 
            List<float> dieselFuelProfit0 = new List<float>();
            List<float> gasFuelProfit0 = new List<float>();
            List<float> unleadedFuelProfit0 = new List<float>();

            //dayly profit for the  LINE 1  with different pumps 
            List<float> dieselFuelProfit1 = new List<float>();
            List<float> gasFuelProfit1 = new List<float>();
            List<float> unleadedFuelProfit1 = new List<float>();

            //dayly profit for the  LINE 2  with different pumps
            List<float> dieselFuelProfit3 = new List<float>();
            List<float> gasFuelProfit3 = new List<float>();
            List<float> unleadedFuelProfit3 = new List<float>();



            //loop for the dayly recors 
            foreach (var daylyRecord in daylyFuelRecords)
            {
                daylySumRecord += daylyRecord.PumpWorkingTime;

                if (daylyRecord.FuelType == FuelType.Disel)
                {

                    //sum the pump working time for the day 
                    dieselDayRecord += daylyRecord.PumpWorkingTime;

                    dieselCarCount++;

                    //add to the dictionary  vehicle ID and the fueled litres for each diesel vehicle 
                    dieselCARcountID.Add(daylyRecord.CarId, daylyRecord.PumpWorkingTime / ProgramConstatns.SecondsPerLitre);

                    if (daylyRecord.LineId == 0)
                    {
                        //add to the list diesel record / 3 seconds to add the sum for the fuel for LINE 0  
                        dieselFuelProfit0.Add(daylyRecord.PumpWorkingTime / (ProgramConstatns.SecondsPerLitre) * 2.49F);
                    }
                    else if (daylyRecord.LineId == 1)
                    {
                        //add to the list diesel record / 3 seconds to add the sum for the fuel for LINE 1  

                        dieselFuelProfit1.Add(daylyRecord.PumpWorkingTime / (ProgramConstatns.SecondsPerLitre) * 2.49F);

                    }
                    else
                    {
                        //add to the list diesel record / 3 seconds to add the sum for the fuel for LINE 2  
                        dieselFuelProfit3.Add(daylyRecord.PumpWorkingTime / (ProgramConstatns.SecondsPerLitre) * 2.49F);
                    }
                }
                //records for the gas 
                else if (daylyRecord.FuelType == FuelType.Gasoline)
                {
                    //dayly sum gas pump time
                    gasDaylyRecord += daylyRecord.PumpWorkingTime;
                    gasCarCount++;

                    //add to the dictionary one vehicle with ID and fueled litres 
                    gasCARcountID.Add(daylyRecord.CarId, daylyRecord.PumpWorkingTime / ProgramConstatns.SecondsPerLitre);

                    if (daylyRecord.LineId == 0)
                    {
                        // add to the list profit for line 0 with gas 
                        gasFuelProfit0.Add(daylyRecord.PumpWorkingTime / (ProgramConstatns.SecondsPerLitre) * 2.49F);
                    }
                    else if (daylyRecord.LineId == 1)
                    {
                        // add to the list profit for line 1 with gas 

                        gasFuelProfit1.Add(daylyRecord.PumpWorkingTime / (ProgramConstatns.SecondsPerLitre) * 2.49F);
                    }
                    else
                    {
                        // add to the list profit for line 2 with gas 

                        gasFuelProfit3.Add(daylyRecord.PumpWorkingTime / (ProgramConstatns.SecondsPerLitre) * 2.49F);
                    }

                }
                //records for the unleaded 
                else if (daylyRecord.FuelType == FuelType.Unleaded)
                {
                    //sum the pump working time for unleaded for the daily statistic
                    unleadedDaylyRecord += daylyRecord.PumpWorkingTime;
                    unleadedCarCount++;

                    //add to the dictionary one vehicle with ID and fueled litres 
                    unleadedCARcountID.Add(daylyRecord.CarId, daylyRecord.PumpWorkingTime / ProgramConstatns.SecondsPerLitre);

                    if (daylyRecord.LineId == 0)
                    {
                        // add to the list profit for line 0 with unleaded

                        unleadedFuelProfit0.Add(daylyRecord.PumpWorkingTime / (ProgramConstatns.SecondsPerLitre) * 2.49F);
                    }
                    else if (daylyRecord.LineId == 1)
                    {
                        // add to the list profit for line 1 with unleaded

                        unleadedFuelProfit1.Add(daylyRecord.PumpWorkingTime / (ProgramConstatns.SecondsPerLitre) * 2.49F);
                    }
                    else
                    {
                        // add to the list profit for line 2 with unleaded 

                        unleadedFuelProfit3.Add(daylyRecord.PumpWorkingTime / (ProgramConstatns.SecondsPerLitre) * 2.49F);
                    }

                }

            }
            //find the profit like divide the day pump working time and * the price for the fuel 
            float dieselDayProfit = (float)Math.Round(dieselDayRecord / ProgramConstatns.SecondsPerLitre * 2.49F, 2);
            float gasDayProfit = (float)Math.Round(gasDaylyRecord / ProgramConstatns.SecondsPerLitre * 2.49F, 2);
            float unliededDayProfit = (float)Math.Round(unleadedDaylyRecord / ProgramConstatns.SecondsPerLitre * 2.49F, 2);

            if (!isItCalledFromEarnedMoneyForADay)
            {

                Console.WriteLine("Gas Station Pumpt today : {0} litres. ", (float)Math.Round(daylySumRecord / ProgramConstatns.SecondsPerLitre, 2));
                Console.WriteLine("Diesel pumpt today :" + (float)Math.Round(dieselDayRecord / ProgramConstatns.SecondsPerLitre, 2) + " litres ,");
                Console.WriteLine("Gas pumpt today " + (float)Math.Round(gasDaylyRecord / ProgramConstatns.SecondsPerLitre, 2) + " litres,");
                Console.WriteLine("Unleaded pumpt today: " + (float)Math.Round(unleadedDaylyRecord / ProgramConstatns.SecondsPerLitre, 2) + " litres");
                Console.WriteLine("Dayly profit from diesel : " + dieselDayProfit + "$");
                Console.WriteLine("Gas dayly profit: " + gasDayProfit + "$");
                Console.WriteLine("Uneaded dayly profit: " + unliededDayProfit + "$");
                Console.WriteLine("Vehicles fueled with diesel today are : " + dieselCarCount);
                Console.WriteLine("Vehicles fueled with gas today are :" + gasCarCount);
                Console.WriteLine("Vehicles fueled with unleades today are : " + unleadedCarCount);
                Console.WriteLine();
                //loops for the lists
                foreach (var item in dieselCARcountID)
                {
                    Console.WriteLine("Vehicle with ID " + item.Key + " fualed with diesel today: " + (float)Math.Round(item.Value, 2) + " litres ");
                }

                foreach (var item in gasCARcountID)
                {
                    Console.WriteLine("Vehicle with ID " + item.Key + " fualed with diesel today: " + (float)Math.Round(item.Value, 2) + " litres ");
                }
                foreach (var item in unleadedCARcountID)
                {
                    Console.WriteLine("Vehicle with ID " + item.Key + " fualed with diesel today: " + (float)Math.Round(item.Value, 2) + " litres ");
                }
                Console.WriteLine();
                foreach (var item in dieselFuelProfit0)
                {
                    Console.WriteLine("LINE 0  profit  with DIESEL today " + (float)Math.Round(item, 2) + " $");
                }
                foreach (var item in dieselFuelProfit1)
                {
                    Console.WriteLine("LINE 1  profit  with DIESEL today " + (float)Math.Round(item, 2) + " $");
                }
                foreach (var item in dieselFuelProfit3)
                {
                    Console.WriteLine("LINE 2  profit  with DIESEL today " + (float)Math.Round(item, 2) + " $ ");
                }
                foreach (var item in gasFuelProfit0)
                {
                    Console.WriteLine("LINE 1  profit  with GAS today " + (float)Math.Round(item, 2) + " $ ");
                }
                foreach (var item in gasFuelProfit1)
                {
                    Console.WriteLine("LINE 2  profit  with GAS today " + (float)Math.Round(item, 2) + " $ ");
                }
                foreach (var item in gasFuelProfit3)
                {
                    Console.WriteLine("LINE 1  profit  with GAS today " + (float)Math.Round(item, 2) + " $");
                }
                foreach (var item in unleadedFuelProfit0)
                {
                    Console.WriteLine("LINE 2  profit  with UNLEADED today " + (float)Math.Round(item, 2) + " $ ");
                }
                foreach (var item in unleadedFuelProfit1)
                {
                    Console.WriteLine("LINE 1  profit  with UNLEADED today " + (float)Math.Round(item, 2) + " $");
                }
                foreach (var item in unleadedFuelProfit3)
                {
                    Console.WriteLine("LINE 2  profit  with UNLEADED today " + (float)Math.Round(item, 2) + " $ ");
                }

            }
            Console.WriteLine();

            return (float)Math.Round(daylySumRecord / ProgramConstatns.SecondsPerLitre, 2);

        }
        private static float GetFuelPerWeeks(bool isItCalledFromEarnedMoneyForAWeek)
        {
            //get all records 7 days back from now
            //colection for the week fuel records using using System.Linq;

            var weeksFuelRecords = GasStation.FuelRecords.Where(x => x.FuelDateTime.Date >= DateTime.Now.AddDays(-7));
            //fuel pumpt for the week 
            float weekSumRecord = 0;

            float dieselWeekRecord = 0;
            float gasWeeklyRecord = 0;
            float unleadedWeeklyRecord = 0;

            //count the cars fueled with different type of fuel 
            float dieselCarCount = 0;
            float gasCarCount = 0;
            float unleadedCarCount = 0;

            foreach (var daylyRecord in weeksFuelRecords)
            {
                //sum the week pumpworking time 
                weekSumRecord += daylyRecord.PumpWorkingTime;

                if (daylyRecord.FuelType == FuelType.Disel)
                {
                    //sum the diesel pump working time 
                    dieselWeekRecord += daylyRecord.PumpWorkingTime;
                    dieselCarCount++;

                }
                else if (daylyRecord.FuelType == FuelType.Gasoline)
                {
                    //sum the gas working time 
                    gasWeeklyRecord += daylyRecord.PumpWorkingTime;
                    gasCarCount++;
                }
                else if (daylyRecord.FuelType == FuelType.Unleaded)
                {
                    //sum the unleaded working time 
                    unleadedWeeklyRecord += daylyRecord.PumpWorkingTime;
                    unleadedCarCount++;
                }

            }
            //find weekly profit for the different types of fuel 
            //find the profit like divide the week pump working time and * the price for the fuel 

            float dieselWeekProfit = (float)Math.Round(dieselWeekRecord / ProgramConstatns.SecondsPerLitre * 2.49F, 2);
            float gasWeekProfit = (float)Math.Round(gasWeeklyRecord / ProgramConstatns.SecondsPerLitre * 2.49F, 2);
            float unliededWeekProfit = (float)Math.Round(unleadedWeeklyRecord / ProgramConstatns.SecondsPerLitre * 2.49F, 2);
            if (!isItCalledFromEarnedMoneyForAWeek)
            {
                Console.WriteLine("Gas Station Pumpt for a week  :  " + (float)Math.Round(weekSumRecord / ProgramConstatns.SecondsPerLitre, 2) + " litres");
                //week fuel diesel, gas ,unleaded
                Console.WriteLine("Diesel pumpt for a week :" + (float)Math.Round(dieselWeekRecord / ProgramConstatns.SecondsPerLitre, 2) + " litres ");
                Console.WriteLine("Gas pumpt for a week " + (float)Math.Round(gasWeeklyRecord / ProgramConstatns.SecondsPerLitre, 2) + " litres");
                Console.WriteLine("Unleaded pumpt for a week :" + (float)Math.Round(unleadedWeeklyRecord / ProgramConstatns.SecondsPerLitre, 2) + " litres");
                Console.WriteLine("Weekly profit from diesel : " + dieselWeekProfit + "$");
                Console.WriteLine("Gas weekly profit: " + gasWeekProfit + "$");
                Console.WriteLine("Unleaded weekly profit: " + unliededWeekProfit + "$");
                Console.WriteLine("Vehicles fueled with diesel for a week are : " + dieselCarCount);
                Console.WriteLine("Vehicles fueled with gas for a week  are :" + gasCarCount);
                Console.WriteLine("Vehicles fueled with unleades for a week are : " + unleadedCarCount);

            }
            Console.WriteLine();
            return (weekSumRecord / ProgramConstatns.SecondsPerLitre);
        }
        private static float GetFuelPerMonth(bool isItCalledFromEarnedMoneyForAMonth)
        {
            //get all records 7 days back from now

            //colection for the week fuel records using using System.Linq;

            var weeksFuelRecords = GasStation.FuelRecords.Where(x => x.FuelDateTime.Date >= DateTime.Now.AddDays(-30));

            float monthSumRecord = 0;

            //sum the pump working time for the different types ot fuel for the month 
            float dieselMonthRecord = 0;
            float gasMonthlyRecord = 0;
            float unleadedMonthlyRecord = 0;

            // vehicles monthly count for the different types of fuel 

            float dieselCarCount = 0;
            float gasCarCount = 0;
            float unleadedCarCount = 0;

            foreach (var daylyRecord in weeksFuelRecords)
            {
                monthSumRecord += daylyRecord.PumpWorkingTime;

                if (daylyRecord.FuelType == FuelType.Disel)
                {
                    //monthly diesel pump working time record 
                    dieselMonthRecord += daylyRecord.PumpWorkingTime;
                    dieselCarCount++;

                }
                else if (daylyRecord.FuelType == FuelType.Gasoline)
                {
                    //monthly gas pump working time record 

                    gasMonthlyRecord += daylyRecord.PumpWorkingTime;
                    gasCarCount++;
                }
                else if (daylyRecord.FuelType == FuelType.Unleaded)
                {
                    //monthly unleaded pump working time record 

                    unleadedMonthlyRecord += daylyRecord.PumpWorkingTime;
                    unleadedCarCount++;
                }

            }
            //find the profit like divide the month pump working time and * the price for the fuel 
            float dieselMonthProfit = (float)Math.Round(dieselMonthRecord / ProgramConstatns.SecondsPerLitre * 2.49F, 2);
            float gasMonthProfit = (float)Math.Round(gasMonthlyRecord / ProgramConstatns.SecondsPerLitre * 2.49F, 2);
            float unliededMonthProfit = (float)Math.Round(unleadedMonthlyRecord / ProgramConstatns.SecondsPerLitre * 2.49F, 2);
            if (!isItCalledFromEarnedMoneyForAMonth)
            {
                Console.WriteLine("Gas Station Pumpt for a month  :  " + (float)Math.Round(monthSumRecord / ProgramConstatns.SecondsPerLitre, 2) + " litres");
                //week fuel diesel, gas ,unleaded
                Console.WriteLine("Diesel pumpt for a month :" + (float)Math.Round(dieselMonthRecord / ProgramConstatns.SecondsPerLitre, 2) + " litres");
                Console.WriteLine("Gas pumpt for a month " + (float)Math.Round(gasMonthlyRecord / ProgramConstatns.SecondsPerLitre, 2) + " litres ");
                Console.WriteLine("Unleaded pumpt for a month :" + (float)Math.Round(unleadedMonthlyRecord / ProgramConstatns.SecondsPerLitre, 2) + " litres");
                Console.WriteLine("Monthly profit from diesel : " + dieselMonthProfit + "$");
                Console.WriteLine("Gas monthly profit: " + gasMonthProfit + "$ ");
                Console.WriteLine("Uneaded monthly profit :" + unliededMonthProfit + "$");
                Console.WriteLine("Vehicles fueled with diesel for a month are : " + dieselCarCount);
                Console.WriteLine("Vehicles fueled with gas for a month  are :" + gasCarCount);
                Console.WriteLine("Vehicles fueled with unleaded for a month are : " + unleadedCarCount);
            }
            Console.WriteLine();
            return (monthSumRecord / ProgramConstatns.SecondsPerLitre);
        }
        public static void EarnedmoneyForDay()
        {
            //call the method for the daily fuel records and save the daily profit 
            float earnedMoneyForADay = GetFuelPerDays(true) * 2.49F;
            // 0,1 percent for the owner
            float onePercentForTheOwner = (earnedMoneyForADay * 0.01F);
            //2 digit accuracy
            float manager = (float)Math.Round(onePercentForTheOwner, 2);
            float all = (float)Math.Round(earnedMoneyForADay, 2);
            Console.WriteLine("Earned Money For a day from fuel are " + all + "$ and the Profit for the manager is " + manager + "$");



        }
        public static void EarnedMoneyForAWeek()
        {
            //call function for weekly fuel records and save the weekly  profit 
            float earnedMoneyForWeek = GetFuelPerWeeks(true) * 2.49F;
            //percent for the owner
            float percentigeForTheManager = (earnedMoneyForWeek * 0.01F);
            // 2 digits accuracy
            float manager = (float)Math.Round(percentigeForTheManager, 2);
            float all = (float)Math.Round(earnedMoneyForWeek, 2);


            Console.WriteLine("Earned Money For a week from fuel are " + all + "$ and the Profit for the manager is " + manager + "$");
        }
        public static void EarnedMoneyForMonth()
        {
            //cal method for the monthly fuel records and save monthly profit
            float earnedMoneyForMonth = GetFuelPerMonth(true) * 2.49F;
            float percentigeForTheManager = (earnedMoneyForMonth * 0.01F);
            // 2 digits accuracy
            float manager = (float)Math.Round(percentigeForTheManager, 2);
            float all = (float)Math.Round(earnedMoneyForMonth, 2);


            Console.WriteLine("Earned Money For a month from fuel are " + all + "$ and the Profit for the manager is " + manager + "$");
            Console.WriteLine( );
        }



        private static bool LogHistoryFuelRecords()
        {
            //Here we call all the methods for the statistic
            if (GasStation.FuelRecords.Count > 0 && GasStation.FuelRecords.Count % 5 == 0)
            {
                Console.WriteLine("  STATISTICS  ");

                foreach (var record in GasStation.FuelRecords)
                {
                    Console.WriteLine("Line ID : "+ record.LineId + " at Date and Time " + record.FuelDateTime +   "  with Fuel  :  "+  record.FuelType + " With Pump working time  : " + record.PumpWorkingTime + " And  Vehicle ID  " + record.CarId );

                }
                Console.WriteLine();
                GetAllVehiclesPerDay();
                CarStatisticPerLinePerDay();
                CarStatisticPerLinePerWEEK();
                CarStatisticPerLinePerMonth();
                GetFuelPerDays(false);
                GetFuelPerWeeks(false);
                GetFuelPerMonth(false);
                EarnedMoneyForAWeek();
                EarnedmoneyForDay();
                EarnedMoneyForMonth();
                //if the statistic is too long comment this method 
                LeftCarsCount();


                return true;
            }
            else
            {
                return false;
            }

        }
        //LINE daily statistic count vehicles for each line 
        public static void CarStatisticPerLinePerDay()
        {
            int line0CarsCount = 0;
            int line1CarsCount = 0;
            int line2CarsCount = 0;
            var daylyFuelRecords = GasStation.FuelRecords.Where(x => x.FuelDateTime.Date == DateTime.Now.Date);
            foreach (var lane in daylyFuelRecords)
            {
                if (lane.LineId == 0) line0CarsCount++;
                else if (lane.LineId == 1) line1CarsCount++;
                else line2CarsCount++;

            }
            Console.WriteLine("Vehicles Fueled today at Line 0 are :" + line0CarsCount);
            Console.WriteLine("Vehicles Fueled today at Line 1 are :" + line1CarsCount);
            Console.WriteLine("Vehicles Fueled today at Line 2 are :" + line2CarsCount);
            Console.WriteLine();
        }
        //LINE weekly statistics count vehicles for each line 
        public static void CarStatisticPerLinePerWEEK()
        {
            int line0CarsCount = 0;
            int line1CarsCount = 0;
            int line2CarsCount = 0;
            var weeksFuelRecords = GasStation.FuelRecords.Where(x => x.FuelDateTime.Date >= DateTime.Now.AddDays(-7));
            foreach (var lane in weeksFuelRecords)
            {
                if (lane.LineId == 0) line0CarsCount++;
                else if (lane.LineId == 1) line1CarsCount++;
                else line2CarsCount++;

            }
            Console.WriteLine("Vehicle Fueled at Line 0 for a week are :" + line0CarsCount);
            Console.WriteLine("Vehicle Fueled at Line 1 for a week are :" + line1CarsCount);
            Console.WriteLine("Vehicle Fueled at Line 2 for a week are :" + line2CarsCount);
            Console.WriteLine();
        }
        //LINE monthly statistics count vehicles for each line 

        public static void CarStatisticPerLinePerMonth()
        {
            int line0CarsCount = 0;
            int line1CarsCount = 0;
            int line2CarsCount = 0;
            var monthFuelRecords = GasStation.FuelRecords.Where(x => x.FuelDateTime.Date >= DateTime.Now.AddDays(-30));
            foreach (var lane in monthFuelRecords)
            {
                if (lane.LineId == 0) line0CarsCount++;
                else if (lane.LineId == 1) line1CarsCount++;
                else line2CarsCount++;

            }
            Console.WriteLine("Vehicles Fueled at Line 0 for a month are :" + line0CarsCount);
            Console.WriteLine("Vehicles Fueled at Line 1 for a month are :" + line1CarsCount);
            Console.WriteLine("Vehicles Fueled at Line 2 for a month are :" + line2CarsCount);
            Console.WriteLine();
        }
        //We have a list of left cars record 
        private static void LeftCarsCount()
        {
            //collection for the day left cars using System.Linq;

            var todaysLefCars = GasStation.LeftCars.Where(x => x.FuelDateTime.Date == DateTime.Now.Date);
            var count = todaysLefCars.Count();//count the vehicles who had left today
            foreach (var item in todaysLefCars)
            {
                //we comment it because the console is goint to be full 
              //  Console.WriteLine("Vehicle with ID =>" + item.CarId + "left before fueling today ");
            }
            //
            Console.WriteLine("The count of the vehicles who had left today are :" + count);
            //cars who had left for 7 days from today 
            //collection for the week left cars using System.Linq;

            var weekLefCars = GasStation.LeftCars.Where(x => x.FuelDateTime.Date >= DateTime.Now.Date.AddDays(-7));
            var count1 = weekLefCars.Count();//weekly count for left cars 

            foreach (var item in weekLefCars)
            {
                //we comment it because the console is goint to be full 

                //Console.WriteLine("Vehicle with ID =>" + item.CarId + "left before fueling this week ");

            }
            Console.WriteLine("The count of the vehicles who had left for 7 days are :" + count1);
            //collection for the monthly left cars using System.Linq;

            var monthLefCars = GasStation.LeftCars.Where(x => x.FuelDateTime.Date >= DateTime.Now.Date.AddDays(-30));
            var count2 = monthLefCars.Count();//monthly count for left cars 

            foreach (var item in monthLefCars)
            {
                //comment because the console statistic is too big 
               // Console.WriteLine("Car with ID => "+item.CarId  +"left before fueling this month ");

            }
            Console.WriteLine("The count of the vehicles who had left for 30 days are :" + count1);


        }

        private static void ArrivingCarsStep()
        {
            //Get random result for car -> could be null;
            var newCar = CreateCars();

            //If not null que the car
            if (newCar != null)
            {
                //Add car to global list
                Cars.Add(newCar.CarId, newCar);

                //Queue car
                QueueCar(newCar);
            }
        }

        private static Car CreateCars()
        {
            //generate random numbert beteween 0 and 100
            var carChance = rng.Next(0, 101);

            //if value is greater than chance create a random car
            if (carChance > ProgramConstatns.CarArrivalChange)
            {
                return new Car();
            }
            else
            {
                return null;
            }
        }

        //Adds car to queue
        private static void QueueCar(Car car)
        {
            //initialize index and value for simple algorithm to find smalles value in a array. If all are equal the first lane will be chosen
            var smallestQueueIndex = -1;
            var smallestQueueLenghtr = int.MaxValue;

            for (int i = 0; i < GasStation.Lanes.Length; i++)
            {
                var lane = GasStation.Lanes[i];
                //Condition for a smalles value
                if (lane.GasPumps.Any(x => x.FuelType == car.FuelType) && lane.CarQueue.Count < smallestQueueLenghtr)
                {
                    //replace old info for smallest value
                    smallestQueueIndex = i;
                    smallestQueueLenghtr = lane.CarQueue.Count;
                }
            }


            //Add Queue Car to lane
            GasStation.Lanes[smallestQueueIndex].QueueCar(car);
        }

        //Process Queue - Step
        private static void ProgressFuelingQueuesStep()
        {
            //Run the code for processing queue for each lane
            foreach (var lane in GasStation.Lanes)
            {
                lane.ProgressFuelingQue();
            }
        }

        //Process Cars that are done waiting - Step
        private static void WaitingCarsTickStep()
        {
            //Get cars that are ready to leave
            var carsThatWillLeave = GetCarsIdThatAreDoneWaiting();

            //Remove the found cars
            RemoveCarsThatAreDoneWaiting(carsThatWillLeave);

        }
        private static List<int> GetCarsIdThatAreDoneWaiting()
        {
            //Get all cars that are just waiting
            var waitingCars = Cars.Values.Where(x => x.IsFueling == false);
            //list to store their ids
            var carsThatWillLeave = new List<int>();

            //Progress the time for each waiting car and check if they are done waiting
            foreach (var car in waitingCars)
            {
                car.CurrentWaitTime += ProgramConstatns.TickTimeInSeconds;

                //Condition for done waiting
                if (car.CurrentWaitTime > car.WaitTimeMax)
                {
                    carsThatWillLeave.Add(car.CarId);

                }
            }

            return carsThatWillLeave;
        }

        private static void RemoveCarsThatAreDoneWaiting(List<int> readyCarIds)
        {
            //Remove and output to console
            foreach (var carId in readyCarIds)
            {
                //here we create left vehicle and add properties 
                //in gas station have list of left cars 
                //create NEW LEFT CAR
                LeftCars currentCar = new LeftCars();

                Cars.Remove(carId);

                currentCar.CarId = (carId);
                currentCar.FuelDateTime = DateTime.Now;

                Console.WriteLine($"\t\t\t\tVehicle with ID:{carId} left before fueling");
                GasStation.LeftCars.Add(currentCar);
            }


        }


        //Process cars that are done Fueling - Step
        private static void FuelingCarsTickStep()
        {
            //Get all cars that are fueling
            var carsThatWillLeave = GetCarsIdThatAreFueledUp();

            //Release all pumps that are taken by cars that are done fueling
            ReleasePumpForReadyCars(carsThatWillLeave);

            //Remove cars that are done fueling
            RemoveCarsThatAreFueledUp(carsThatWillLeave);

        }

        private static void ReleasePumpForReadyCars(List<int> readyCarIds)
        {
            //Run code for each lane
            foreach (var lane in GasStation.Lanes)
            {
                //Run code for each pump in lane
                foreach (var pump in lane.GasPumps)
                {
                    //condition for a car that is done fueling
                    if (readyCarIds.Contains(pump.CurrentCarId))
                    {
                        //realease pump
                        pump.Taken = false;
                        pump.CurrentCarId = -1;

                        //Output to console
                        Console.WriteLine("Pump released");
                    }
                }
            }
        }

        private static void RemoveCarsThatAreFueledUp(List<int> readyCarIds)
        {
            //Remove and output to consople
            foreach (var carId in readyCarIds)
            {
                Cars.Remove(carId);

                Console.WriteLine($"\t\t\t\tVehicle with ID:{carId} left after fueling");
            }
        }

        // Get cars that are done fuling
        private static List<int> GetCarsIdThatAreFueledUp()
        {
            //Get the car that are fueling
            var waitingCars = Cars.Values.Where(x => x.IsFueling == true);

            //Init a place to store the value
            var carsThatWillLeave = new List<int>();


            foreach (var car in waitingCars)
            {

                //progress time
                car.CurrentFuelTime += ProgramConstatns.TickTimeInSeconds;

                //condition for done fueling
                if (car.CurrentFuelTime > car.FuelTimeMax)
                {

                    carsThatWillLeave.Add(car.CarId);
                    //current fieling statistic create NEW FUEL RECORD 
                    FuelRecord currentRecord = new FuelRecord();
                    //searching where in which line is our car 
                    currentRecord.FuelType = car.FuelType;

                    currentRecord.PumpWorkingTime = car.FuelTimeMax;
                    currentRecord.FuelDateTime = DateTime.Now;
                    foreach (var gasLine in GasStation.Lanes)
                    {
                        if (gasLine.GasPumps.Any(x => x.CurrentCarId == car.CarId))
                        {
                            //here we log the current fueling in line x 
                            currentRecord.LineId = gasLine.LaneId;
                            currentRecord.CarId = car.CarId;

                        }
                    }
                    GasStation.FuelRecords.Add(currentRecord);


                }
            }

            return carsThatWillLeave;
        }


        //ID provider for cars
        public static int GetCarId()
        {
            var currentCarId = carId;

            carId++;

            return currentCarId;
        }
    }
}
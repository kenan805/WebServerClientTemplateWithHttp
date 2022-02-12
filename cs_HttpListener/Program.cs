using CarsData.DAL.Context;
using CarsData.Entities;
using CarsData.Unit_Of_Work.Abstract;
using CarsData.Unit_Of_Work.Concrete;
using cs_HttpListener.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;

namespace cs_HttpListener
{
    internal class Program
    {
        static IUnitOfWork _unitOfWork;
        static void Main(string[] args)
        {
            Console.WriteLine("Listener...");
            _unitOfWork = new UnitOfWork(new CarDbContext());
            if (_unitOfWork.CarRepository.Count() == 0)
                FillData();
            StartServer();
        }

        private static void FillData()
        {
            var json = File.ReadAllText("./Cars_Data.json");
            var users = JsonSerializer.Deserialize<List<Car>>(json);
            _unitOfWork.CarRepository.AddRange(users);
            _unitOfWork.Complete();
        }

        private static void StartServer()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:45001/");
            listener.Start();

            HomeController controller = new HomeController();
            controller.ConnectClients(listener, _unitOfWork);

        }
    }
}

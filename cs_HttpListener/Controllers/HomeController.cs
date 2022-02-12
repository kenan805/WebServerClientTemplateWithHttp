using CarsData.Entities;
using CarsData.Unit_Of_Work.Abstract;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace cs_HttpListener.Controllers
{
    internal class HomeController
    {
        internal void ConnectClients(HttpListener listener, IUnitOfWork _unitOfWork)
        {
            while (true)
            {
                var ctx = listener.GetContext();

                var request = ctx.Request;
                var response = ctx.Response;

                var reader = new StreamReader(request.InputStream);
                var writer = new StreamWriter(response.OutputStream);

                switch (request.HttpMethod)
                {
                    case "GET":
                        HttpGet(writer, response, request, _unitOfWork);
                        break;
                    case "POST":
                        HttpPost(writer, reader, response, _unitOfWork);
                        break;
                    case "PUT":
                        HttpPut(writer, reader, response, _unitOfWork);
                        break;
                    case "DELETE":
                        HttpDelete(writer, response, request, _unitOfWork);
                        break;
                    default:
                        break;
                }



                response.Close();
            }

        }

        public void HttpGet(StreamWriter writer, HttpListenerResponse response, HttpListenerRequest request, IUnitOfWork _unitOfWork)
        {
            string json = JsonSerializer.Serialize(_unitOfWork.CarRepository.GetAll().ToList());
            writer.WriteLine(json);
            writer.Flush();
            response.StatusCode = 200;
            response.ContentType = "application/json";
        }

        public void HttpPost(StreamWriter writer, StreamReader reader, HttpListenerResponse response, IUnitOfWork _unitOfWork)
        {
            var carJson = reader.ReadToEnd();
            var car = JsonSerializer.Deserialize<Car>(carJson);

            _unitOfWork.CarRepository.Add(car);
            _unitOfWork.Complete();

            writer.Flush();
            response.StatusCode = 201;
            response.ContentType = "application/json";
        }

        public void HttpPut(StreamWriter writer, StreamReader reader, HttpListenerResponse response, IUnitOfWork _unitOfWork)
        {
            var carJson = reader.ReadToEnd();
            var car = JsonSerializer.Deserialize<Car>(carJson);

            var updateCar = _unitOfWork.CarRepository.Find(c => c.Id == car.Id);
            updateCar.Make = car.Make;
            updateCar.Model = car.Model;
            updateCar.Color = car.Color;
            updateCar.Volume = car.Volume;
            updateCar.Year = car.Year;

            _unitOfWork.Complete();
            writer.Flush();
            response.StatusCode = 200;
            response.ContentType = "application/json";
        }

        public void HttpDelete(StreamWriter writer, HttpListenerResponse response, HttpListenerRequest request, IUnitOfWork _unitOfWork)
        {
            if (request.QueryString.HasKeys())
            {
                int id = int.Parse(request.QueryString["deletedCarId"]);
                _unitOfWork.CarRepository.Remove(_unitOfWork.CarRepository.Find(c => c.Id == id));
                _unitOfWork.Complete();
                writer.Flush();
                response.StatusCode = 200;
                response.ContentType = "application/json";
            }
            else
            {
                response.StatusCode = 404;
            }
        }
    }
}

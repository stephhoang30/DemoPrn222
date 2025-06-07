using Microsoft.EntityFrameworkCore;
using ServerApp2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServerApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // step 1: initial and start server
                TcpListener server = new TcpListener(IPAddress.Any, 5001);
                server.Start();
                Console.WriteLine("Server is connected");

                // loop accept client
                while (true)
                {
                    using (TcpClient client = server.AcceptTcpClient())
                    {
                        NetworkStream stream = client.GetStream();
                        //TODO
                        // get data on stream
                        byte[] byteRequest = new byte[8000];
                        int bytesRead = stream.Read(byteRequest, 0, byteRequest.Length);

                        while (bytesRead > 0)
                        {
                            // byte -> string
                            string message = Encoding.UTF8.GetString(byteRequest, 0, bytesRead);

                            // string -> object
                            Request request = JsonSerializer.Deserialize<Request>(message);

                            // convert string to byte using Encoding
                            byte[] byteResponse = Encoding.UTF8.GetBytes(getResponseData(request));


                            // write on stream
                            stream.Write(byteResponse, 0, byteResponse.Length);
                            
                        }
                        client.Close();
                    }
                    break;
                    
                }
                
            }
            catch (Exception ex)
            {

                Console.WriteLine("Server fail: " + ex.Message);
            }
        }


        private static string getResponseData(Request request)
        {
            string message = "";

            if (request.Action.Equals("add"))
            {
                Product product = new Product();

                JsonElement dataElement = (JsonElement)request.Data;

                var p = dataElement.Deserialize<Product>();

                product.ProductName = p.ProductName;
                product.UnitPrice = p.UnitPrice;
                product.UnitsInStock = p.UnitsInStock;
                product.Image = p.Image;
                product.CategoryId = p.CategoryId;

                MySaleDbContext.INS.Products.Add(product);
                MySaleDbContext.INS.SaveChanges();
            }
            else if (request.Action.Equals("view"))
            {
                // connect DB to get list Product to send to Client
                var listProduct = MySaleDbContext.INS.Products
                    .Include(x => x.Category) // filter out null categories
                    .ToList();

                // get list Category 
                var listCate = MySaleDbContext.INS.Categories.Select(x => x.CategoryName).ToList();

                Response res = new Response();

                res.listProducts = listProduct;
                res.listCateNames = listCate;

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                };

                // convert list to string using JsonSerializer
                // Serialize: object -> string
                // DeSerialize: string -> object
                message = JsonSerializer.Serialize(res, options);
            }

            return message;
        }
    }
}

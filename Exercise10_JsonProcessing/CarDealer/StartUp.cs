using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var res = "";
            var context = new CarDealerContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();



            // task 9
             var suppliersJson = File.ReadAllText("../../../datasets/suppliers.json");
             res = ImportSuppliers(context, suppliersJson);

            // task 10
             var partsJson = File.ReadAllText("../../../datasets/parts.json");
             res = ImportParts(context, partsJson);

            // task 11
            var carsJson = File.ReadAllText("../../../datasets/cars.json");
            res = ImportCars(context, carsJson);

            Console.WriteLine(res);
            ;
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<Supplier[]>(inputJson);

            context.Suppliers.AddRange(suppliers);

            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var supplierIds = context.Suppliers
                .Select(s => s.Id)
                .ToHashSet<int>();


            var parts = JsonConvert.DeserializeObject<Part[]>(inputJson)
                .Where(p => supplierIds.Contains(p.SupplierId))
                .ToArray();

            context.Parts.AddRange(parts);

            context.SaveChanges();

            return $"Successfully imported {parts.Length}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {

            var partIds = context.Parts
                .Select(p => p.Id)
                .ToHashSet<int>();

            var cars = JsonConvert.DeserializeObject<Car[]>(inputJson)
                .ToArray();
            ;
            context.Cars.AddRange(cars);

            context.SaveChanges();

            return $"Successfully imported {cars.Length}.";

        }
    }
}
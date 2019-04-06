
namespace CarDealer
{
    using AutoMapper;
    using CarDealer.Data;
    using CarDealer.Dtos.Import;
    using CarDealer.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<CarDealerProfile>();
            });

            using (CarDealerContext context = new CarDealerContext())
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                string result = string.Empty;
                var suppliersXml = File.ReadAllText("../../../Datasets/suppliers.xml");
                var partsXml = File.ReadAllText("../../../Datasets/parts.xml");
                var carsXml = File.ReadAllText("../../../Datasets/cars.xml");
                var customersXml = File.ReadAllText("../../../Datasets/customers.xml");
                var salesXml = File.ReadAllText("../../../Datasets/sales.xml");
                // task 9
                // result = ImportSuppliers(context, suppliersXml);

                // task 10
                // result = ImportParts(context, partsXml);

                // task 11
                //result = ImportCars(context, carsXml);

                //task 12
                //result = ImportCustomers(context, customersXml);



                Console.WriteLine(result);
                ;
            }


        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSaleDto[]),
                new XmlRootAttribute("Sales"));

            var importSalesDto = (ImportSaleDto[])xmlSerializer
                .Deserialize(new StringReader(inputXml));

            var validCarIds = context.Cars
                .Select(c => c.Id)
                .ToHashSet<int>();

            var sales = new List<Sale>();
            foreach(var importSaleDto in importSalesDto)
            {
                if (validCarIds.Contains(importSaleDto.CarId)==false)
                {
                    continue;
                }

                Sale sale = Mapper.Map<Sale>(importSaleDto);
                sales.Add(sale);

            }

            context.AddRange(sales);

            context.SaveChanges();

            return $"Successfully imported {sales.Count}";

        }
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCustomerDto[]),
                new XmlRootAttribute("Customers"));

            var customersDto = (ImportCustomerDto[])xmlSerializer
                .Deserialize(new StringReader(inputXml));

            var customers = new List<Customer>();

            foreach(var customerDto in customersDto)
            {
                Customer customer = Mapper.Map<Customer>(customerDto);

                customers.Add(customer);
            }

            context.Customers.AddRange(customers);

            context.SaveChanges();

            return $"Successfully imported {customers.Count}";

        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCarDto[]),
                new XmlRootAttribute("Cars"));

            var validPartIds = context.Parts
                .Select(p => p.Id)
                .ToHashSet<int>();

            var carsDto =
                (ImportCarDto[])xmlSerializer.Deserialize(new StringReader(inputXml));


            var cars = new List<Car>();

            foreach (var importCarDto in carsDto)
            {
                importCarDto.PartCars = importCarDto.PartCars
                    .Where(p => validPartIds.Contains(p.Id))
                    .ToArray();
                ;

                Car car = new Car
                {
                    Make = importCarDto.Make,
                    Model = importCarDto.Model,
                    PartCars = importCarDto
                       .PartCars
                       .GroupBy(p => p.Id)
                       .Select(p => p.First())
                       .Select(p => new PartCar
                       {
                           PartId = p.Id
                       })
                        .ToArray()
                };

                cars.Add(car);
            }

            context.Cars.AddRange(cars);

            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ImportSupplierDto[]),
                new XmlRootAttribute("Suppliers"));

            var suppliersDtos = (ImportSupplierDto[])xmlSerializer.Deserialize(new StringReader(inputXml));


            List<Supplier> suppliers = new List<Supplier>();

            foreach (var supplierDto in suppliersDtos)
            {
                Supplier supplier = new Supplier
                {
                    Name = supplierDto.Name,
                    IsImporter = supplierDto.IsImporter
                };

                suppliers.Add(supplier);
            }

            context.Suppliers.AddRange(suppliers);

            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPartDto[]),
                new XmlRootAttribute("Parts"));

            var importPartDtos =
                (ImportPartDto[])xmlSerializer.Deserialize(new StringReader(inputXml));

            List<Part> parts = new List<Part>();

            var supplierIds = context.Suppliers
                .Select(s => s.Id)
                .ToHashSet<int>();

            foreach (var importPartDto in importPartDtos)
            {
                if (supplierIds.Contains(importPartDto.SupplierId) == false)
                {
                    continue;
                }

                Part part = Mapper.Map<Part>(importPartDto);

                parts.Add(part);
            }

            context.Parts.AddRange(parts);

            context.SaveChanges();

            return $"Successfully imported {parts.Count}";

        }

    }
}
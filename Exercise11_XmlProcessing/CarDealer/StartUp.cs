
namespace CarDealer
{
    using AutoMapper;
    using CarDealer.Data;
    using CarDealer.Dtos.Export;
    using CarDealer.Dtos.Import;
    using CarDealer.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
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
                // context.Database.EnsureDeleted();
                // context.Database.EnsureCreated();

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
                // result = ImportCars(context, carsXml);

                // task 12
                // result = ImportCustomers(context, customersXml);

                // task 13
                // result = ImportSales(context, salesXml);

                // task 14
                // result = GetCarsWithDistance(context);

                // task 15
                // result = GetCarsFromMakeBmw(context);

                // task 16
                // result = GetLocalSuppliers(context);

                // task 17
                // result = GetCarsWithTheirListOfParts(context);

                // task 18
                // result = GetTotalSalesByCustomer(context);

                // task 19
                result = GetSalesWithAppliedDiscount(context);
                Console.WriteLine(result);
                ;
            }
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var salesDto = context.Sales
                .Select(s => new ExportSaleDiscountDto
                {
                   Car = new ExportSaleCarDto
                   {
                       Make = s.Car.Make,
                       Model = s.Car.Model,
                       TravelledDistance = s.Car.TravelledDistance
                   },
                   Discount = decimal.Parse(s.Discount.ToString("f0")),
                   CustomerName = s.Customer.Name,
                   Price = Math.Round(s.Car.PartCars.Sum(pc =>pc.Part.Price), 2),
                   PriceWithDiscount = (s.Car.PartCars
                                .Sum(pc => pc.Part.Price) -
                                (s.Car.PartCars.Sum(pc => pc.Part.Price) *
s.Discount / 100m))

                }).ToArray();

            StringBuilder sb = new StringBuilder();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportSaleDiscountDto[]), new XmlRootAttribute("sales"));

            var ns =new XmlSerializerNamespaces();
            ns.Add("","");

            xmlSerializer.Serialize(new StringWriter(sb), salesDto, ns);
            //var writer = new StreamWriter(@"../../../cars-and-parts.xml");

            //using (writer)
            //{
            //    xmlSerializer.Serialize(writer, salesDto);
            //}
            return sb.ToString();

        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customersDto = context.Customers
                .Where(c => c.Sales.Count > 0)
                .Include(c => c.Sales)
                .Select(c => new ExportCustomerTotalSalesDto
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
                }).OrderByDescending(x => x.SpentMoney)
                .ToArray();
            ;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCustomerTotalSalesDto[]), new XmlRootAttribute("customers"));

            StringBuilder sb = new StringBuilder();

            var ns = new XmlSerializerNamespaces();

            ns.Add("", "");

            xmlSerializer.Serialize(new StringWriter(sb), customersDto, ns);

            return sb.ToString();
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsDto = context.Cars
                .Select(c => new ExportCarListPartsDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    Parts = c.PartCars.Select(p =>
                        new ExportPartDto
                        {
                            Name = p.Part.Name,
                            Price = p.Part.Price
                        })
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                }).OrderByDescending(c => c.TravelledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCarListPartsDto[]), new XmlRootAttribute("cars"));

            StringBuilder sb = new StringBuilder();

            var ns = new XmlSerializerNamespaces();

            ns.Add("", "");

            xmlSerializer.Serialize(new StringWriter(sb), carsDto, ns);


            return sb.ToString();
        }

        //TODO fix
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliersDto = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new ExportLocalSuppliersDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })

                .ToArray();

            XmlSerializer xmlserializer = new XmlSerializer(typeof(ExportLocalSuppliersDto[]), new XmlRootAttribute("suppliers"));

            StringBuilder sb = new StringBuilder();

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            xmlserializer.Serialize(new StringWriter(sb), suppliersDto, ns);

            return sb.ToString();
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var bmwCars = context.Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToArray();

            var bmwCarsDto = bmwCars
                .Select(c => new ExportCarMakeDto
                {
                    Id = c.Id,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .ToArray();
            ;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCarMakeDto[]), new XmlRootAttribute("cars"));

            StringBuilder sb = new StringBuilder();

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            xmlSerializer.Serialize(new StringWriter(sb), bmwCarsDto, ns);

            return sb.ToString();
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.TravelledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .Select(c => new ExportCarDistanceDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .ToArray();
            ;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCarDistanceDto[]),
                new XmlRootAttribute("cars"));


            StringBuilder sb = new StringBuilder();

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            xmlSerializer.Serialize(new StringWriter(sb), cars, ns);

            return sb.ToString();
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
            foreach (var importSaleDto in importSalesDto)
            {
                if (validCarIds.Contains(importSaleDto.CarId) == false)
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

            foreach (var customerDto in customersDto)
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
                    TravelledDistance = importCarDto.TravelledDistance,
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
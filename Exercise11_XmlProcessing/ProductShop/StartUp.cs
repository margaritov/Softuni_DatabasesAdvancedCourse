namespace ProductShop
{
    using AutoMapper;
    using ProductShop.Data;
    using ProductShop.Dtos.Export;
    using ProductShop.Dtos.Import;
    using ProductShop.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    

    public class StartUp
    {
        public static void Main(string[] args)
        {


            Mapper.Initialize(x =>
           {
               x.AddProfile<ProductShopProfile>();
           });

            using (ProductShopContext context = new ProductShopContext())
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                //task 1
                //var usersXml = File.ReadAllText("../../../datasets/users.xml");
                //var users = ImportUsers(context, usersXml);


                // task 2 
                // var productsXml = File.ReadAllText("../../../datasets/products.xml");
                // var products = ImportProducts(context, productsXml)

                // task 3
                // var categoriesXml = File.ReadAllText("../../../datasets/categories.xml");
                // var categories = ImportCategories(context, categoriesXml);

                // task 4
                // var categoriesProductsXml = File.ReadAllText("../../../datasets/categories-products.xml");
                // var res = ImportCategoryProducts(context, categoriesProductsXml);

                // task 5
                // var res = GetProductsInRange(context);

                // task 6
                //var res = GetSoldProducts(context);

                //task 7

                var res = GetUsersWithProducts(context);

                Console.WriteLine(res);
                ;
            }
        }

        public static string ImportUsers(ProductShopContext context,
            string inputXml)
        {
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ImportUserDto[]), new XmlRootAttribute("Users"));

            var usersDto = (ImportUserDto[])xmlSerializer.Deserialize(new StringReader(inputXml));
            var users = new List<User>();
            foreach (var userDto in usersDto)
            {
                var user = Mapper.Map<User>(userDto);
                users.Add(user);
            }
            ;
            context.Users.AddRange(users);

            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ImportProductDto[]), new XmlRootAttribute("Products"));

            var productsDto = (ImportProductDto[])xmlSerializer.Deserialize(new StringReader(inputXml));
            var products = new List<Product>();
            foreach (var productDto in productsDto)
            {
                var product = Mapper.Map<Product>(productDto);

                // OR without automapper
                //var product = new Product
                //{
                //    Name = productDto.Name,
                //    Price = productDto.Price,
                //    SellerId = productDto.SellerId,
                //    BuyerId = productDto.BuyerId
                //};

                products.Add(product);
            }

            context.Products.AddRange(products);

            context.SaveChanges();
            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCategoryDto[]),
                new XmlRootAttribute("Categories"));

            var categoriesDto = (ImportCategoryDto[])xmlSerializer
                .Deserialize(new StringReader(inputXml));

            var categories = new List<Category>();

            foreach (var categoryDto in categoriesDto)
            {

                if (categoryDto.Name == null)
                {
                    continue;
                }

                var category = Mapper.Map<Category>(categoryDto);

                categories.Add(category);
            }

            context.Categories.AddRange(categories);

            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCategoryProductDto[]),
                new XmlRootAttribute("CategoryProducts")
                );

            var categoryProductsDto =
                (ImportCategoryProductDto[])xmlSerializer
                .Deserialize(new StringReader(inputXml));

            var categoryProducts = new List<CategoryProduct>();

            foreach (var categoryProductDto in categoryProductsDto)
            {
                var product = context.Products.Find(categoryProductDto.ProductId);

                var category = context.Categories.Find(categoryProductDto.CategoryId);

                if (product == null || category == null)
                {
                    continue;
                }

                var categoryProduct = Mapper.Map<CategoryProduct>(categoryProductDto);

                categoryProducts.Add(categoryProduct);
            }

            context.CategoryProducts.AddRange(categoryProducts);

            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(x => new ExportProductInRangeDto
                {
                    Name = x.Name,
                    Price = x.Price,
                    Buyer = x.Buyer.FirstName + " " + x.Buyer.LastName
                })
                .OrderBy(x => x.Price)
                .Take(10)
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportProductInRangeDto[]),
                new XmlRootAttribute("Products"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
             {
                new XmlQualifiedName("", "")
            });

            xmlSerializer.Serialize(new StringWriter(sb), products, namespaces);

            return sb.ToString().TrimEnd(); ;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(p => p.ProductsSold.Any())
                .Select(u => new ExportUserSoldProductDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    ProductsDto = u.ProductsSold.Select(p => new ProductDto
                    {
                        Name = p.Name,
                        Price = p.Price
                    }).ToArray()
                })
                .OrderBy(l => l.LastName)
                .ThenBy(f => f.FirstName)
                .Take(5)
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportUserSoldProductDto[]),
                new XmlRootAttribute("Users"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                XmlQualifiedName.Empty
            });

            xmlSerializer.Serialize(new StringWriter(sb), users, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .Select(x => new ExportUserAndProductDto
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age,
                    SoldProductsDto = new SoldProductDto
                    {
                        Count = x.ProductsSold.Count,
                        ProductDtos = x.ProductsSold
                        .Select(p => new ProductDto
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                    }
                })
                .OrderByDescending(x => x.SoldProductsDto.Count)
                
                .ToArray();

            var customExport = new ExportCustomUserProductDto
            {
                Count = users.Count(),
                ExportUserAndProductDto = users.Take(10).ToArray()
            };

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCustomUserProductDto),
               new XmlRootAttribute("Users"));
            ;
            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
            {
                XmlQualifiedName.Empty
            });

            xmlSerializer.Serialize(new StringWriter(sb), customExport, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
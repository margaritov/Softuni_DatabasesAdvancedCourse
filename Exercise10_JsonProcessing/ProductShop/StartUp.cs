
namespace ProductShop
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using ProductShop.Data;
    using ProductShop.Dtos.Export;
    using ProductShop.Models;
    using System;
    using System.Linq;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            string res = "";

            // task 1 
            // string usersJson = File.ReadAllText("./../../../Datasets/users.json");
            // res = ImportUsers(context, usersJson);

            // task 2
            // string productsJson = File.ReadAllText("./../../../Datasets/products.json");
            // res = ImportProducts(context, productsJson);

            // task 3
            // string categoriesJson = File.ReadAllText("./../../../Datasets/categories.json");
            // res = ImportCategories(context, categoriesJson);

            // task 4
            // string categoriesProductsJson = File
            //    .ReadAllText("./../../../Datasets/categories-products.json");

            // res = ImportCategoryProducts(context, categoriesProductsJson);

            // task 5
            // res = GetProductsInRange(context);

            // task 6
            // res = GetSoldProducts(context);

            // task 7
            res = GetCategoriesByProductsCount(context);
            // task 8
            //res = GetUsersWithProducts(context);
            Console.WriteLine(res);
            ;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var filteredUsers = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .OrderByDescending(u => u.ProductsSold.Count(ps => ps.Buyer != null))
                .Select(u => new
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new
                    {
                        Count = u.ProductsSold.Count(ps => ps.Buyer != null),
                        Products = u.ProductsSold
                        .Where(ps => ps.Buyer != null)
                        .Select(ps => new
                        {
                            Name = ps.Name,
                            Price = ps.Price
                        })
                        .ToArray()
                    }
                })
                .ToArray();
            var result = new
            {
                UsersCount = filteredUsers.Length,
                Users = filteredUsers
            };

            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var json = JsonConvert.SerializeObject(result,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = contractResolver,
                    NullValueHandling = NullValueHandling.Ignore
                });

            return json;

        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories

                .OrderByDescending(c => c.CategoryProducts.Count)
                //.Include(c => c.CategoryProducts)
                //.ThenInclude(cp => cp.Product)
                .Select(c => new
                {
                    Category = c.Name,
                    ProductsCount = c.CategoryProducts.Count,
                    AveragePrice = $"{c.CategoryProducts.Average(cp => cp.Product.Price):F2}",
                    TotalRevenue = $"{c.CategoryProducts.Sum(cp => cp.Product.Price)}"
                })
                .ToArray();
            ;
            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var json = JsonConvert.SerializeObject(categories,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = contractResolver,
                    NullValueHandling = NullValueHandling.Ignore

                });

            return json;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var filteredUsers = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold
                    // .Where(ps => ps.Buyer != null)
                    .Select(ps => new
                    {
                        Name = ps.Name,
                        Price = ps.Price,
                        BuyerFirstName = ps.Buyer.FirstName,
                        BuyerLastName = ps.Buyer.LastName
                    })
                    .ToArray()
                })
               .ToArray();

            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var json = JsonConvert.SerializeObject(filteredUsers,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = contractResolver
                });

            return json;
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new ProductDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    Seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                })
                .OrderBy(p => p.Price)
                .ToArray();


            var result = JsonConvert.SerializeObject(products, Formatting.Indented);

            return result;
            ;
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {

            var users = JsonConvert.DeserializeObject<User[]>(inputJson)
                .Where(u => u.LastName != null && u.LastName.Length >= 3)
                .ToArray();

            context.Users.AddRange(users);

            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson)
                .Where(p => !string.IsNullOrEmpty(p.Name) &&
                    p.Name.Length >= 3)
                .ToArray();

            context.Products.AddRange(products);

            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<Category[]>(inputJson)
                .Where(c => !string.IsNullOrEmpty(c.Name) &&
                    c.Name.Length >= 3 && c.Name.Length <= 15)
                .ToArray();

            context.Categories.AddRange(categories);

            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var validCategoryIds = context.Categories
                .Select(c => c.Id)
                .ToHashSet<int>();

            var validProductIds = context.Products
                .Select(p => p.Id)
                .ToHashSet<int>();

            var categoryProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson)
                .Where(c => validCategoryIds.Contains(c.CategoryId) &&
                    validProductIds.Contains(c.ProductId))
                .ToArray();

            context.CategoryProducts.AddRange(categoryProducts);

            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Length}";

        }
    }
}
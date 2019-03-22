namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                // Init database
                // DbInitializer.ResetDatabase(db);

                // task 1
                // var result = GetBooksByAgeRestriction(db, "teen");

                // task 2
                // var result = GetGoldenBooks(db);

                // task 3
                // var result = GetBooksByPrice(db);

                // task 4
                // var result = GetBooksNotReleasedIn(db, 2000);

                // task 5
                // var result = GetBooksByCategory(db, "horror mystery drama");

                // task 6
                // var result = GetBooksReleasedBefore(db, "12-04-1992");

                // task 7
                 var result = GetAuthorNamesEndingIn(db, "E");

                // task 8
                // var result = GetBookTitlesContaining(db, "sK");

                // task 9
                // var result = GetBooksByAuthor(db, "r");

                // task 10
                // var result = CountBooks(db, 12);

                // task 11
                // var result = CountCopiesByAuthor(db);

                // task 12
                // var result = GetTotalProfitByCategory(db);

                // task 13
                // var result = GetMostRecentBooks(db);

                // task 14
                // IncreasePrices(db);

                //task 15
                //int removedCount = RemoveBooks(db);
                //Console.WriteLine(removedCount);

                Console.WriteLine(result);

                ;
            }
        }

        // Task 14
        public static void IncreasePrices(BookShopContext context)
        {
            // //requires Z.Entityframework.plus.efcore
            // var books = context.Books
            // .Where(x => x.ReleaseDate.Value.Year < 2010)
            // .Update(x => new Book() { Price = x.Price + 5 });

            var books = context.Books
                .Where(x => x.ReleaseDate.Value.Year < 2010)
                .ToList();


            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        // Task 15
        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            var count = books.Count();

            context.Books.RemoveRange(books);
            context.SaveChanges();
            return count;
        }

        // Task 1
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            var books = context.Books
                .Where(a => a.AgeRestriction == ageRestriction)
                .Select(t => t.Title)
                .OrderBy(x => x)
                .ToList();

            var result = String.Join(Environment.NewLine, books);

            return result;
        }


        // Task 2
        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.EditionType == Enum.Parse<EditionType>("Gold"))
                .Where(b => b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            var result = String.Join(Environment.NewLine, books);

            return result;
        }


        // Task 3
        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new
                {
                    Title = b.Title,
                    Price = b.Price
                })
                .ToList();

            var result = String.Join(Environment.NewLine, books.Select(b => $"{b.Title} - ${b.Price:F2}"));

            return result;
        }


        // Task 4
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            var result = String.Join(Environment.NewLine, books);

            return result;
        }


        // Task 5
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input
                .ToLower()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var books = context.Books
                .Where(bc => bc.BookCategories.Any(c => categories.Contains(c.Category.Name.ToLower())))
                .Select(t => t.Title)
                .OrderBy(t => t)
                .ToList();

            var result = String.Join(Environment.NewLine, books);
            return result;
        }


        // Task 6
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var targetDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var books = context.Books
                .Where(b => b.ReleaseDate.Value < targetDate)
                .OrderByDescending(r => r.ReleaseDate.Value)
                .Select(x => new
                {
                    x.Title,
                    x.EditionType,
                    x.Price
                })
                .ToList();

            var result = String.Join(Environment.NewLine,
                books.Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:f2}"));

            return result;
        }


        // Task 7
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(a => EF.Functions.Like(a.FirstName.ToLower(), $"%{input.ToLower()}"))
                //a.FirstName.EndsWith(input, true, CultureInfo.InvariantCulture))
                .OrderBy(b => b.FirstName)
                .ThenBy(b => b.LastName)
                .Select(a => $"{a.FirstName} {a.LastName}")
                .ToList();


            var result = String.Join(Environment.NewLine, authors);
            return result;
        }


        // Task 8
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => EF.Functions.Like(b.Title, $"%{input}%"))
                .OrderBy(b => b.Title)
                .Select(b => b.Title);

            var result = String.Join(Environment.NewLine, books);

            return result;
        }


        // Task 9
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(a => EF.Functions.Like(a.Author.LastName, $"{input}%"))
                .OrderBy(b => b.BookId)
                .Select(x => new
                {
                    x.Title,
                    x.Author.FirstName,
                    x.Author.LastName
                })
                .ToList();

            var result = string.Join(Environment.NewLine,
                books.Select(x => $"{x.Title} ({x.FirstName} {x.LastName})"));

            return result;
        }


        // Task 10
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books
                .Count(b => b.Title.Length > lengthCheck);
        }


        // Task 11
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    BooksCount = x.Books.Sum(c => c.Copies)
                })
            .OrderByDescending(b => b.BooksCount)
            .ToList();

            var result = String.Join(Environment.NewLine,
                authors.Select(x => $"{x.FirstName} {x.LastName} - {x.BooksCount}"));

            return result;
        }


        // Task 12
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    Name = c.Name,
                    Total = c.CategoryBooks.Sum(b => b.Book.Price * b.Book.Copies)
                })
                .OrderByDescending(x => x.Total)
                .ThenBy(x => x.Name)
                .ToList();


            var result = String.Join(Environment.NewLine,
                categories.Select(x => $"{x.Name} ${x.Total}"));

            return result;
        }


        // Task 13
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                //.Include(x => x.CategoryBooks)
                .Select(x => new
                {
                    CategoryName = x.Name,
                    Books = x.CategoryBooks.Select(e => new
                    {
                        e.Book.Title,
                        e.Book.ReleaseDate
                    })
                    .OrderByDescending(e => e.ReleaseDate)
                    .Take(3)
                    .ToList()
                })
                .OrderBy(c => c.CategoryName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.CategoryName}");
                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }

            }
            return sb.ToString();
        }

    }
}

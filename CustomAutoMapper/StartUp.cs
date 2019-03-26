namespace CustomAutoMapper
{
    using Newtonsoft.Json;
    using System;

   public class StartUp
    {
       public static void Main(string[] args)
        {
            var person = new Person()
            {
                FirstName = "Pesho",
                LastName = "Ivanov",
                Age = 33
            };

            var mapper = new Mapper();

            var student = mapper.Map<Student>(person);

            Console.WriteLine(JsonConvert.SerializeObject(student));
            ;
        }
    }
}

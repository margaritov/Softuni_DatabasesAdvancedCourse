using System;
using System.Collections.Generic;
using System.Linq;
namespace Sandbox
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            Person person = new Person("Pesho","Ivanov");

            List<Person> persons = new List<Person>();

            Console.WriteLine(person.GetType().Name);

            Console.WriteLine(persons.GetType().GetGenericArguments().First());

            ;

        }
    }
}

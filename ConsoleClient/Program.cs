using Bogus;
using Otus.Teaching.Concurrency.Import.DataGenerator.Generators;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ConsoleClient
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            client.BaseAddress = new Uri("https://localhost:44351/");
            client.Timeout = TimeSpan.FromSeconds(20);

            while (true)
            {
                Console.WriteLine("1 - to find an existing customer");
                Console.WriteLine("2 - to generate and save a new customer");
                Console.WriteLine("e - exit");

                var k = Console.ReadLine();

                if (k == "1")
                {
                    Console.WriteLine("Input customer id:");

                    if (int.TryParse(Console.ReadLine(), out var id))
                    {
                        try
                        {
                            Console.WriteLine($"Customer is found: {(await client.GetFromJsonAsync($"/customers/{id}", typeof(Customer))).ToString()}");
                        }
                        catch (HttpRequestException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Id is incorrect");
                    }
                }
                else if (k == "2")
                {

                    var newCustomer = (Customer) new Faker<Customer>()
                        .CustomInstantiator(f => new Customer()
                        {
                            Id = 0
                        })
                        .RuleFor(u => u.FullName, (f, u) => f.Name.FullName())
                        .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FullName))
                        .RuleFor(u => u.Phone, (f, u) => f.Phone.PhoneNumber("1-###-###-####"));

                    try
                    {
                        (await client.PostAsJsonAsync("/customers", newCustomer)).EnsureSuccessStatusCode();
                        Console.WriteLine($"Customer is created: {newCustomer.ToString()}");
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                else if (k == "e")
                {
                    break;
                }
            }
        }
    }
}

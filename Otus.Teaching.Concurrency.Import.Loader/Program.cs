using System;
using System.Diagnostics;
using System.IO;
using Otus.Teaching.Concurrency.Import.Core.Loaders;
using Otus.Teaching.Concurrency.Import.DataGenerator.Generators;
using Otus.Teaching.Concurrency.Import.DataAccess.Parsers;
using Otus.Teaching.Concurrency.Import.DataAccess.Repositories;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using System.Collections.Generic;

namespace Otus.Teaching.Concurrency.Import.Loader
{
    class Program
    {
        private static string _dataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Customers");
        private const string GeneratorApp = @"C:\Users\kai\source\repos\Otus.Teaching.Concurrency.Import/Otus.Teaching.Concurrency.Import.DataGenerator.App/bin/Debug/netcoreapp3.1/Otus.Teaching.Concurrency.Import.DataGenerator.App.exe";
        private const int FileLength = 1000000;
        private const bool UseXML = false;

        static void Main(string[] args)
        {
            if (args != null && args.Length == 1)
            {
                _dataFilePath = args[0];
            }

            Console.WriteLine($"Loader started with process Id {Process.GetCurrentProcess().Id}...");

            var stopWatch = new Stopwatch();
            List<Customer> customers;

            if (UseXML)
            {
                _dataFilePath += ".xml";
                var withProcess = "";

                while (withProcess != "y" && withProcess != "n")
                {
                    Console.WriteLine("File is generated with method. Change it to process (y/n)?");
                    withProcess = Console.ReadLine();
                }

                Console.WriteLine("Generating XML...");
                stopWatch.Start();
                GenerateCustomersDataFile(withProcess);
                stopWatch.Stop();
                Console.WriteLine($"Generation XML: {stopWatch.Elapsed.TotalSeconds}");

                Console.WriteLine("Parsing XML...");
                stopWatch.Restart();
                customers = new XmlParser(_dataFilePath).Parse();
                stopWatch.Stop();
                Console.WriteLine($"Parsing XML: {stopWatch.Elapsed.TotalSeconds}");
            }
            else
            {
                _dataFilePath += ".csv";
                Console.WriteLine("Generating CSV...");
                stopWatch.Start();
                var csvGenerator = new CsvGenerator(_dataFilePath, FileLength);
                csvGenerator.Generate();
                stopWatch.Stop();
                Console.WriteLine($"Generation CSV: {stopWatch.Elapsed.TotalSeconds}");

                Console.WriteLine("Parsing CSV...");
                stopWatch.Restart();
                customers = new CsvParser(_dataFilePath).Parse();
                stopWatch.Stop();
                Console.WriteLine($"Parsing CSV: {stopWatch.Elapsed.TotalSeconds}");
            }

            var customerRepository = new CustomerRepository();
            Console.WriteLine("Reseting repository...");
            customerRepository.Reset();

            Console.WriteLine($"Loading {FileLength} records. {Environment.ProcessorCount} threads");

            Console.WriteLine("Loading with threads...");
            stopWatch.Restart();
            new DataLoader(customers).LoadData();
            stopWatch.Stop();
            Console.WriteLine($"Loading with threads: {stopWatch.Elapsed.TotalSeconds}");

            Console.WriteLine("Reseting repository...");
            customerRepository.Reset();

            Console.WriteLine("Loading with thread pools...");
            stopWatch.Restart();            
            new DataLoaderThreadPool(customers).LoadData();
            stopWatch.Stop();
            Console.WriteLine($"Loading with thread pools: {stopWatch.Elapsed.TotalSeconds}");
        }

        static void GenerateCustomersDataFile(string withProcess)
        {
            switch (withProcess)
            {
                case "y":
                    var process = Process.Start(GeneratorApp, $"{_dataFilePath} {FileLength.ToString()}");
                    process.WaitForExit();
                    break;
                case "n":
                    var xmlGenerator = new XmlGenerator(_dataFilePath, FileLength);
                    xmlGenerator.Generate();
                    break;
            }
        }
    }
}
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Otus.Teaching.Concurrency.Import.Core.Parsers;
using Otus.Teaching.Concurrency.Import.DataGenerator.Dto;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using ServiceStack.Text;

namespace Otus.Teaching.Concurrency.Import.DataAccess.Parsers
{
    public class CsvParser
        : IDataParser<List<Customer>>
    {
        private readonly string _dataFilePath;
        public CsvParser(string dataFilePath)
        {
            _dataFilePath = dataFilePath;
        }
        public List<Customer> Parse()
        {
           using var stream = File.Open(_dataFilePath, FileMode.Open);

            return CsvSerializer.DeserializeFromStream<CustomersList>(stream).Customers;

        }
    }
}
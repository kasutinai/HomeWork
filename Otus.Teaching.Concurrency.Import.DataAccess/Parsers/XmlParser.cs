using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Otus.Teaching.Concurrency.Import.Core.Parsers;
using Otus.Teaching.Concurrency.Import.Handler.Entities;

namespace Otus.Teaching.Concurrency.Import.DataAccess.Parsers
{
    public class XmlParser
        : IDataParser<List<Customer>>
    {
        private readonly string _dataFilePath;
        public XmlParser(string dataFilePath)
        {
            _dataFilePath = dataFilePath;
        }
        public List<Customer> Parse()
        {
            var xdoc = XDocument.Load(_dataFilePath);
            var xElements = xdoc.Root.Element("Customers").Elements("Customer");

            return xElements.Select(
                xElement => new Customer
                {
                    Id = int.Parse(xElement.Element("Id").Value),
                    FullName = xElement.Element("FullName")?.Value,
                    Email = xElement.Element("Email")?.Value,
                    Phone = xElement.Element("Phone")?.Value
                }).ToList();
        }
    }
}
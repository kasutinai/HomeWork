using System.ComponentModel.DataAnnotations.Schema;

namespace Otus.Teaching.Concurrency.Import.Handler.Entities
{
    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public override string ToString()
        {
            var id = Id != 0 ? $"Id: {Id}; " : "";

            return $"{id}Full name: {FullName}; Email: {Email}; Phone: {Phone}";
        }
    }
}
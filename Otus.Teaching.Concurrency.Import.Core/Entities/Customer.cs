using System.ComponentModel.DataAnnotations.Schema;

namespace Otus.Teaching.Concurrency.Import.Handler.Entities
{
    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplication12.Models
{
    public class BookOutOfStorage : ModelBase
    {
        public Person Person { get; set; }
        public Book Book { get; set; }
        public DateTime BookTakeDate { get; set; }
        public DateTime BookReturnDate { get; set; }


    }
}

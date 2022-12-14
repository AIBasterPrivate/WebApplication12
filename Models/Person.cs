namespace WebApplication12.Models
{
    public class Person:ModelBase
    {
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}

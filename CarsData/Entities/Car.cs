namespace CarsData.Entities
{
    public class Car : BaseEntity
    {
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
        public int Year { get; set; }
        public decimal Volume { get; set; }
    }
}

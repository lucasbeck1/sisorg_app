namespace api.Models
{
    public class Country
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string Color { get; set; }
        public Country() { }
        public Country(string name, decimal value, string color)
        {
            this.Name = name;
            this.Value = value;
            this.Color = color;
        }
        public override string ToString()
        {
            string CountryObject = $"Name: {Name}, Value: {Value}, Color: {Color}";
            return "{ " + CountryObject + " }";
        }
    }
}

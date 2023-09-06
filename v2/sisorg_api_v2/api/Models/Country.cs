namespace api.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public decimal Value { get; set; }
        
        public Country() { }
        public Country(string name, decimal value, string color)
        {
            this.Name = name;
            this.Value = value;
            this.Color = color;
        }
        public override string ToString()
        {
            // Same code == return $"{this.Id} {this.Name} {this.Value} {this.Color}";
            return string.Format("{0} {1} {2} {3}", this.Id, this.Name, this.Value, this.Color);
        }
    }
}

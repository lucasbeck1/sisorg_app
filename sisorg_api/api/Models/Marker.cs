namespace api.Models
{
    public class Marker
    {
        public int ID { get; set; }
        public int Count { get; set; }
        public DateTime Timestamp { get; set; }
        public List<Country> Rows { get; set; }
        public Marker() { }
        public Marker(int id, int count, DateTime timeStamp, List<Country> countries)
        {
            this.ID = id;
            this.Count = count;
            this.Timestamp = timeStamp;
            this.Rows = countries;
        }
    }
}

namespace api.Models
{
    public class Marker
    {
        public int ID { get; set; }
        public int Count { get; set; }
        public DateTime Timestamp { get; set; }
        public List<Country>? Rows { get; set; }

        public Marker() { }
        public Marker(int count, DateTime timeStamp, List<Country> countries)
        {
            this.Count = count;
            this.Timestamp = timeStamp;
            this.Rows = countries;
        }
        public override string ToString()
        {
            if (this.Rows?.Count > 0)
            {   
                string rowsToPrint = "[";
                foreach (var row in this.Rows)
                {
                    rowsToPrint += row.ToString() + ", ";
                }
                // rowsToPrint.
                rowsToPrint = rowsToPrint.Remove(rowsToPrint.Length -2);
                rowsToPrint += "]";
                return string.Format("{0} {1} {2} {3}", this.ID, this.Count, this.Timestamp, rowsToPrint);
            }
            else
            {
                return string.Format("{0} {1} {2} {3}", this.ID, this.Count, this.Timestamp, "[]");
            }
        }
    }
}
                
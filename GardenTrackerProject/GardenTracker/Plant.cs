namespace GardenTracker.Models
{
    public class Plant
    {
        public string Name { get; set; }

        public Plant(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

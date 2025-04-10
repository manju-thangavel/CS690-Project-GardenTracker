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

        public override bool Equals(object obj)
        {
            if (obj is Plant otherPlant)
            {
                return string.Equals(Name, otherPlant.Name, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Name?.ToLowerInvariant().GetHashCode() ?? 0;
        }
    }
}

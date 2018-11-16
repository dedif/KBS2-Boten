namespace WpfApp6
{
    public class Boat
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int Weight { get; set; }

        public int AmountOfRowers { get; set; }

        public bool Steerman { get; set; }

        public Boat(int id, string name, string type, int weight, int amountOfRowers, bool steerman)
        {
            Id = id;
            Name = name;
            Type = type;
            Weight = weight;
            AmountOfRowers = amountOfRowers;
            Steerman = steerman;
        }
    }
}
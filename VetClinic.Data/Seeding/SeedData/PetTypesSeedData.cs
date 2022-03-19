namespace VetClinic.Data.Seeding.SeedData
{
    public class PetTypesSeedData
    {
        public PetTypesSeedData()
        {
            PetTypes = new List<string>()
            {
                "Dog",
                "Cat",
                "Bird",
                "Rodent",
                "Reptile",
                "Exotic",
                "Fish",
                "Other",
            };
        }

        public static ICollection<string> PetTypes { get; private set; }
    }
}

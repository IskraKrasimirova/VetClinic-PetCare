namespace VetClinic.Data.Seeding.Contracts
{
    public interface ISeeder
    {
        void Seed(VetClinicDbContext data, IServiceProvider serviceProvider);
    }
}

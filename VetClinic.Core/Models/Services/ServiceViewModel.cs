namespace VetClinic.Core.Models.Services
{
    public class ServiceViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int DepartmentId { get; set; }

        public string Department { get; set; }
    }
}

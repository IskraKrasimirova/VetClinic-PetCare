using VetClinic.Data.Models;
using VetClinic.Data.Seeding.Contracts;
using VetClinic.Data.Seeding.SeedData;

namespace VetClinic.Data.Seeding
{
    internal class DepartmentsSeeder : ISeeder
    {
        public void Seed(VetClinicDbContext data, IServiceProvider serviceProvider)
        {
            if (!data.Departments.Any())
            {
                var allDepartments = new List<Department>();

                var examinationAndConsultation = new Department()
                {
                    Name = DepartmentsSeedData.ExaminationAndConsultation.Name,
                    Description = DepartmentsSeedData.ExaminationAndConsultation.Description,
                };
                allDepartments.Add(examinationAndConsultation);

                var internalMedicine = new Department()
                {
                    Name = DepartmentsSeedData.InternalMedicine.Name,
                    Description = DepartmentsSeedData.InternalMedicine.Description,
                };
                allDepartments.Add(internalMedicine);

                var surgery = new Department()
                {
                    Name = DepartmentsSeedData.Surgery.Name,
                    Description = DepartmentsSeedData.Surgery.Description,
                };
                allDepartments.Add(surgery);

                var orthopedicsAndTraumatology = new Department()
                {
                    Name = DepartmentsSeedData.OrthopedicsAndTraumatology.Name,
                    Description = DepartmentsSeedData.OrthopedicsAndTraumatology.Description,
                };
                allDepartments.Add(orthopedicsAndTraumatology);

                var dermatology = new Department()
                {
                    Name = DepartmentsSeedData.Dermatology.Name,
                    Description = DepartmentsSeedData.Dermatology.Description,
                };
                allDepartments.Add(dermatology);

                var emergencyMedicine = new Department()
                {
                    Name = DepartmentsSeedData.EmergencyMedicine.Name,
                    Description = DepartmentsSeedData.EmergencyMedicine.Description,
                };
                allDepartments.Add(emergencyMedicine);

                var grooming = new Department()
                {
                    Name = DepartmentsSeedData.Grooming.Name,
                    Description = DepartmentsSeedData.Grooming.Description,
                };
                allDepartments.Add(grooming);

                var clinicalLaboratory = new Department()
                {
                    Name = DepartmentsSeedData.ClinicalLaboratory.Name,
                    Description = DepartmentsSeedData.ClinicalLaboratory.Description,
                };
                allDepartments.Add(clinicalLaboratory);

                data.Departments.AddRange(allDepartments);
                data.SaveChanges();
            }
        }
    }
}

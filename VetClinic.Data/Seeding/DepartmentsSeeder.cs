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
                    Image = DepartmentsSeedData.ExaminationAndConsultation.Image,
                    Description = DepartmentsSeedData.ExaminationAndConsultation.Description,
                };
                allDepartments.Add(examinationAndConsultation);

                var internalMedicine = new Department()
                {
                    Name = DepartmentsSeedData.InternalMedicine.Name,
                    Image = DepartmentsSeedData.InternalMedicine.Image,
                    Description = DepartmentsSeedData.InternalMedicine.Description,
                };
                allDepartments.Add(internalMedicine);

                var surgery = new Department()
                {
                    Name = DepartmentsSeedData.Surgery.Name,
                    Image = DepartmentsSeedData.Surgery.Image,
                    Description = DepartmentsSeedData.Surgery.Description,
                };
                allDepartments.Add(surgery);

                var orthopedicsAndTraumatology = new Department()
                {
                    Name = DepartmentsSeedData.OrthopedicsAndTraumatology.Name,
                    Image = DepartmentsSeedData.OrthopedicsAndTraumatology.Image,
                    Description = DepartmentsSeedData.OrthopedicsAndTraumatology.Description,
                };
                allDepartments.Add(orthopedicsAndTraumatology);

                var dermatology = new Department()
                {
                    Name = DepartmentsSeedData.Dermatology.Name,
                    Image = DepartmentsSeedData.Dermatology.Image,
                    Description = DepartmentsSeedData.Dermatology.Description,
                };
                allDepartments.Add(dermatology);

                var emergencyMedicine = new Department()
                {
                    Name = DepartmentsSeedData.EmergencyMedicine.Name,
                    Image = DepartmentsSeedData.EmergencyMedicine.Image,
                    Description = DepartmentsSeedData.EmergencyMedicine.Description,
                };
                allDepartments.Add(emergencyMedicine);

                var grooming = new Department()
                {
                    Name = DepartmentsSeedData.Grooming.Name,
                    Image = DepartmentsSeedData.Grooming.Image,
                    Description = DepartmentsSeedData.Grooming.Description,
                };
                allDepartments.Add(grooming);

                var clinicalLaboratory = new Department()
                {
                    Name = DepartmentsSeedData.ClinicalLaboratory.Name,
                    Image = DepartmentsSeedData.ClinicalLaboratory.Image,
                    Description = DepartmentsSeedData.ClinicalLaboratory.Description,
                };
                allDepartments.Add(clinicalLaboratory);

                data.Departments.AddRange(allDepartments);
                data.SaveChanges();
            }
        }
    }
}

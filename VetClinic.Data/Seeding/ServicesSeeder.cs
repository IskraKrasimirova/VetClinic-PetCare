using VetClinic.Data.Models;
using VetClinic.Data.Seeding.Contracts;

namespace VetClinic.Data.Seeding
{
    public class ServicesSeeder : ISeeder
    {
        public void Seed(VetClinicDbContext data, IServiceProvider serviceProvider)
        {
            if (!data.Services.Any())
            {
                var services = new Service[]
                {
                    // 1. Examination & Consultation
                    new Service
                    {
                        Name = "Primary Examination",
                        Description = "The standard check-up includes measuring internal body temperature, measuring the time for filling the capillaries;palpation of superficial lymph nodes,color of visible mucous membranes,test for hydration of the patient,auscultation of the trachea, lungs and heart,measuring pulse,abdominal palpation,neurological status, orthopedic status, skin and coat status, eye and ear status.",
                        DepartmentId = 1,
                        Price = 25.0M
                    },
                    new Service
                    {
                        Name = "Secondary Examination",
                        Description = "Standard check-up",
                        DepartmentId = 1,
                        Price = 20.0M
                    },
                    new Service
                    {
                        Name = "Annual preventive examination",
                        Description = "Check-designed especially for the youngest and oldest patients, including laboratory screening, external and internal parasite prevention, immunizations, healthy eating and diets.",
                        DepartmentId = 1,
                        Price = 50.00M
                    },
                    new Service
                    {
                        Name = "Vaccine Pure Vax RCP - cat",
                        Description = "For cats",
                        DepartmentId = 1,
                        Price = 45.00M
                    },
                    new Service
                    {
                        Name = "Vaccine Pure Vax FeLV - cat",
                        Description = "For cats",
                        DepartmentId = 1,
                        Price = 45.00M
                    },
                    new Service
                    {
                        Name = "Vaccine Pure Vax Rabies - cat",
                        Description = "For cats",
                        DepartmentId = 1,
                        Price = 45.00M
                    },
                    new Service
                    {
                        Name = "Vaccine Pure Vax RCPCHFeLV - cat",
                        Description = "For cats (viral rhinotracheitis, calicivirus, chlamydia, panleukopenia and feline leukemia)",
                        DepartmentId = 1,
                        Price = 65.00M
                    },
                    new Service
                    {
                        Name = "Vaccine Pestorin - rabbit",
                        Description = "For rabbits",
                        DepartmentId = 1,
                        Price = 17.00M
                    },
                    new Service
                    {
                        Name = "Monovalent vaccine - rabies",
                        Description = "For all types of pets",
                        DepartmentId = 1,
                        Price = 30.00M
                    },
                    new Service
                    {
                        Name = "Monovalent vaccine - leptospirosis",
                        Description = "For all types of pets",
                        DepartmentId = 1,
                        Price = 30.00M
                    },
                    new Service
                    {
                        Name = "Vaccine Nobivac DP+ - dog",
                        Description = "For dogs",
                        DepartmentId = 1,
                        Price = 38.00M
                    },
                    new Service
                    {
                        Name = "Vaccine Nobivac Puppy DP",
                        Description = "Combined live, lyophilized vaccine for puppies against ghana and parvovirus",
                        DepartmentId = 1,
                        Price = 20.00M
                    },
                    new Service
                    {
                        Name = "Issuance of an international passport",
                        Description = "For all types of pets",
                        DepartmentId = 1,
                        Price = 45.00M
                    },
                    new Service
                    {
                        Name = "Issuance of a health card",
                        Description = "For all types of pets",
                        DepartmentId = 1,
                        Price = 15.00M
                    },
                    new Service
                    {
                        Name = "Placing a microchip",
                        Description = "For all types of pets",
                        DepartmentId = 1,
                        Price = 60.00M
                    },
                    new Service
                    {
                        Name = "Internal deworming",
                        Description = "For all types of pets, the price of the medicine product is not included",
                        DepartmentId = 1,
                        Price = 5.00M
                    },
                    new Service
                    {
                        Name = "Tick removal",
                        Description = "For all types of pets",
                        DepartmentId = 1,
                        Price = 10.00M
                    },
                    // 2. Internal Medicine
                    new Service 
                    {
                        Name = "Primary Examination by a specialist",
                        Description = "Thorough medical examination in the field of Gastroenterology, Nephrology and Urology, Allergology, Respiratory diseases",
                        DepartmentId = 2,
                        Price = 50.0M
                    },
                    new Service
                    {
                        Name = "Secondary Examination by a specialist",
                        Description = "Мonitoring the development of the disease",
                        DepartmentId = 2,
                        Price = 30.0M
                    },
                    new Service
                    {
                        Name = "Ultrasound examination of abdominal organs",
                        Description = "For all types of pets",
                        DepartmentId = 2,
                        Price = 50.0M
                    },
                    new Service
                    {
                        Name = "Echocardiography",
                        Description = "For all types of pets",
                        DepartmentId = 2,
                        Price = 100.0M
                    },
                     // 3. Surgery
                    new Service
                    {
                        Name = "Primary Examination by a specialist",
                        Description = "Thorough medical examination",
                        DepartmentId = 3,
                        Price = 50.0M
                    },
                    new Service
                    {
                        Name = "Secondary Examination by a specialist",
                        Description = "Мonitoring the development of the treatment",
                        DepartmentId = 3,
                        Price = 30.0M
                    },
                    new Service
                    {
                        Name = "Castration of a male dog - Up to 20 kg",
                        Description = "Up to 20 kg",
                        DepartmentId = 3,
                        Price = 80.0M
                    },
                    new Service
                    {
                        Name = "Castration of a male dog - Over 20 kg",
                        Description = "Over 20 kg",
                        DepartmentId = 3,
                        Price = 120.0M
                    },
                    new Service
                    {
                        Name = "Castration of a female dog - Up to 20 kg",
                        Description = "Up to 20 kg",
                        DepartmentId = 3,
                        Price = 150.0M
                    },
                    new Service
                    {
                        Name = "Castration of a female dog - Over 20 kg",
                        Description = "Over 20 kg",
                        DepartmentId = 3,
                        Price = 200.0M
                    },
                    new Service
                    {
                        Name = "Castration of a female cat",
                        Description = "The Price does not include anesthesia",
                        DepartmentId = 3,
                        Price = 100.0M
                    },
                    new Service
                    {
                        Name = "Castration of a male cat",
                        Description = "The Price does not include anesthesia",
                        DepartmentId = 3,
                        Price = 60.0M
                    },
                    new Service
                    {
                        Name = "Castration of male rabbits / rodents",
                        Description = "The Price does not include anesthesia",
                        DepartmentId = 3,
                        Price = 80.0M
                    },
                    new Service
                    {
                        Name = "Castration of female rabbits / rodents",
                        Description = "The Price does not include anesthesia",
                        DepartmentId = 3,
                        Price = 120.0M
                    },
                    new Service
                    {
                        Name = "Castration of leeks + extirpation of anal glands",
                        Description = "The Price does not include anesthesia",
                        DepartmentId = 3,
                        Price = 250.0M
                    },
                    new Service
                    {
                        Name = "Cesarean section - dog",
                        Description = "The Price does not include anesthesia, analgesia and venous",
                        DepartmentId = 3,
                        Price = 300.0M
                    },
                    new Service
                    {
                        Name = "Cesarean section - cat",
                        Description = "The Price does not include anesthesia, analgesia and venous",
                        DepartmentId = 3,
                        Price = 200.0M
                    },
                    // 4. Orthopedics and Traumatology
                    new Service
                    {
                        Name = "Primary Examination by a specialist",
                        Description = "Thorough medical examination",
                        DepartmentId = 4,
                        Price = 50.0M
                    },
                    new Service
                    {
                        Name = "Secondary Examination by a specialist",
                        Description = "Мonitoring the development of the treatment",
                        DepartmentId = 4,
                        Price = 30.0M
                    },
                    // 5. Dermatology
                    new Service
                    {
                        Name = "Primary Examination by a dermatologist",
                        Description = "Thorough medical examination",
                        DepartmentId = 5,
                        Price = 50.0M
                    },
                    new Service
                    {
                        Name = "Secondary Examination by a dermatologist",
                        Description = "Мonitoring the development of the treatment",
                        DepartmentId = 5,
                        Price = 25.0M
                    },
                    // 7. Grooming
                    new Service
                    {
                        Name = "Sanitary clipping - cats",
                        Description = "Аlso applies to other pets up to 10 kg",
                        DepartmentId = 7,
                        Price = 35.0M
                    },
                    new Service
                    {
                        Name = "Sanitary clipping - dogs, up to 25 kg",
                        Description = "Аlso applies to other pets up to 25 kg",
                        DepartmentId = 7,
                        Price = 45.0M
                    },
                    new Service
                    {
                        Name = "Sanitary clipping - dogs, over 25 kg",
                        Description = "Аlso applies to other pets over 25 kg",
                        DepartmentId = 7,
                        Price = 70.0M
                    },
                    new Service
                    {
                        Name = "Whole clipping",
                        Description = "For cats/dogs and other pets",
                        DepartmentId = 7,
                        Price = 70.0M
                    },
                    new Service
                    {
                        Name = "Bathing and drying - up to 10 kg",
                        Description = "For cats/dogs and other pets up to 10 kg",
                        DepartmentId = 7,
                        Price = 30.0M
                    },
                    new Service
                    {
                        Name = "Bathing and drying - dogs from 11 to 30 kg",
                        Description = "For dogs from 11 to 30 kg",
                        DepartmentId = 7,
                        Price = 40.0M
                    },
                    new Service
                    {
                        Name = "Bathing and drying - dogs more than 30 kg",
                        Description = "For dogs more than 30 kg",
                        DepartmentId = 7,
                        Price = 60.0M
                    },
                    new Service
                    {
                        Name = "Nail clipping",
                        Description = "For all types of pets",
                        DepartmentId = 7,
                        Price = 10.0M
                    },
                    new Service
                    {
                        Name = "Ear cleaning",
                        Description = "For all types of pets",
                        DepartmentId = 7,
                        Price = 10.0M
                    },
                    new Service
                    {
                        Name = "Trimming ears and paws",
                        Description = "For all types of pets",
                        DepartmentId = 7,
                        Price = 15.0M
                    },
                    // 8. Clinical Laboratory
                    new Service
                    {
                        Name = "Complete blood count",
                        Description = "For all types of pets",
                        DepartmentId = 8,
                        Price = 18.0M
                    },
                    new Service
                    {
                        Name = "Cross-match in case of blood transfusion",
                        Description = "For all types of pets",
                        DepartmentId = 8,
                        Price = 40.0M
                    },
                    new Service
                    {
                        Name = "Urine examination - Test strip",
                        Description = "For all types of pets",
                        DepartmentId = 8,
                        Price = 12.0M
                    },
                    new Service
                    {
                        Name = "Complete urine test",
                        Description = "For all types of pets",
                        DepartmentId = 8,
                        Price = 40.0M
                    },
                    new Service
                    {
                        Name = "Each sample",
                        Description = "For all types of pets",
                        DepartmentId = 8,
                        Price = 20.0M
                    },
                    new Service
                    {
                        Name = "Tissue biopsy",
                        Description = "For all types of pets",
                        DepartmentId = 8,
                        Price = 70.0M
                    },
                    new Service
                    {
                        Name = "X-ray",
                        Description = "For all types of pets",
                        DepartmentId = 8,
                        Price = 30.0M
                    },
                    new Service
                    {
                        Name = "X-ray examination of the digestive system with contrast",
                        Description = "For all types of pets",
                        DepartmentId = 8,
                        Price = 100.0M
                    },
                    new Service
                    {
                        Name = "X-ray for hip/elbow dysplasia",
                        Description = "For all types of pets",
                        DepartmentId = 8,
                        Price = 130.0M
                    },
                    
                };

                data.Services.AddRange(services);
                data.SaveChanges();
            }
        }
    }
}

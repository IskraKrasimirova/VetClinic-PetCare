using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinic.Data.Seeding.SeedData
{
    public class DoctorsSeedData
    {
        public const string DefaultPassword = "123456";

        public class ConsultationDoctor1
        {
            public const string Email = "ivanov@vetcare.com";
            public const string FullName = "Ivan Ivanov";
            public const string Username = "ivanov@vetcare.com";
            public const string PhoneNumber = "0888111111";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "~/img/drIvanov";
            public const string Description = "Examination & Consultation";
            public const int DepartmentId = 1;
        }

        public class ConsultationDoctor2
        {
            public const string Email = "nikolova@vetcare.com";
            public const string FullName = "Anna Nikolova";
            public const string Username = "nikolova@vetcare.com";
            public const string PhoneNumber = "0888111112";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "~/img/drNikolova";
            public const string Description = "Examination & Consultation";
            public const int DepartmentId = 1;
        }

        public class ConsultationDoctor3
        {
            public const string Email = "petrova@vetcare.com";
            public const string FullName = "Maria Petrova";
            public const string Username = "petrova@vetcare.com";
            public const string PhoneNumber = "0888111113";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "~/img/drPetrova";
            public const string Description = "Examination & Consultation";
            public const int DepartmentId = 1;
        }

        public class InternalDoctor1
        {
            public const string Email = "georgiev@vetcare.com";
            public const string FullName = "Petar Georgiev";
            public const string Username = "georgiev@vetcare.com";
            public const string PhoneNumber = "0888111121";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "~/img/drGeorgiev";
            public const string Description = "Internal Medicine, Gastroenterology";
            public const int DepartmentId = 2;
        }

        public class InternalDoctor2
        {
            public const string Email = "vasilev@vetcare.com";
            public const string FullName = "Vasil Vasilev";
            public const string Username = "vasilev@vetcare.com";
            public const string PhoneNumber = "0888111122";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "~/img/drVasilev";
            public const string Description = "Internal Medicine, Nephrology and Urology";
            public const int DepartmentId = 2;
        }

        public class InternalDoctor3
        {
            public const string Email = "dimova@vetcare.com";
            public const string FullName = "Radoslava Dimova";
            public const string Username = "dimova@vetcare.com";
            public const string PhoneNumber = "0888111123";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "~/img/drDimova";
            public const string Description = "Internal Medicine, Allergology, Respiratory diseases";
            public const int DepartmentId = 2;
        }

        public class SurgeryDoctor1
        {
            public const string Email = "asenov@vetcare.com";
            public const string FullName = "Asen Asenov";
            public const string Username = "asenov@vetcare.com";
            public const string PhoneNumber = "0888111131";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "~/img/drAsenov";
            public const string Description = "Surgery";
            public const int DepartmentId = 3;
        }

        public class SurgeryDoctor2
        {
            public const string Email = "stoyanov@vetcare.com";
            public const string FullName = "Dimo Stoyanov";
            public const string Username = "stoyanov@vetcare.com";
            public const string PhoneNumber = "0888111132";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "~/img/drStoyanov";
            public const string Description = "Surgery";
            public const int DepartmentId = 3;
        }

        public class OrthopedicsDoctor1
        {
            public const string Email = "nikolov@vetcare.com";
            public const string FullName = "Ivo Nikolov";
            public const string Username = "nikolov@vetcare.com";
            public const string PhoneNumber = "0888111141";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "~/img/drNikolov";
            public const string Description = "Orthopedics and Traumatology";
            public const int DepartmentId = 4;
        }

        public class DermatologyDoctor1
        {
            public const string Email = "radeva@vetcare.com";
            public const string FullName = "Ema Radeva";
            public const string Username = "radeva@vetcare.com";
            public const string PhoneNumber = "0888111151";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "~/img/drRadeva";
            public const string Description = "Dermatology";
            public const int DepartmentId = 5;
        }

        public class EmergencyDoctor1
        {
            public const string Email = "stefanov@vetcare.com";
            public const string FullName = "Stefan Stefanov";
            public const string Username = "stefanov@vetcare.com";
            public const string PhoneNumber = "0888111161";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "~/img/drStefanov";
            public const string Description = "Emergency Medicine";
            public const int DepartmentId = 6;
        }

        public class EmergencyDoctor2
        {
            public const string Email = "veleva@vetcare.com";
            public const string FullName = "Zoya Veleva";
            public const string Username = "veleva@vetcare.com";
            public const string PhoneNumber = "0888111162";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "~/img/drVeleva";
            public const string Description = "Emergency Medicine";
            public const int DepartmentId = 6;
        }

        public class GroomingDoctor1
        {
            public const string Email = "petkova@vetcare.com";
            public const string FullName = "Iva Petkova";
            public const string Username = "petkova@vetcare.com";
            public const string PhoneNumber = "0888111171";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "~/img/drPetkova";
            public const string Description = "Grooming, Exotic animals";
            public const int DepartmentId = 7;
        }
    }
}
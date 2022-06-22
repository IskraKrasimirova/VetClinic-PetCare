namespace VetClinic.Data.Seeding.SeedData
{
    public class DoctorsSeedData
    {
        public const string DefaultPassword = "123456";
        public const string DefaultDescription1 = " Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";
        public const string DefaultDescription2 = " Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur sed nisi turpis. Sed vulputate lectus nulla, ac ultrices enim sollicitudin ac.";
        public const string DefaultDescription3 = " Mauris bibendum pellentesque cursus. In ut augue sagittis, lacinia nulla id, lobortis nunc. Nam ultrices diam ligula, ut ullamcorper magna semper et.";
        public class ConsultationDoctor1
        {
            public const string Email = "ivanov@petcare.com";
            public const string FullName = "Ivan Ivanov";
            public const string Username = "ivanov@petcare.com";
            public const string PhoneNumber = "0888111111";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "https://res.cloudinary.com/dqnorpdaj/image/upload/v1648707675/VetClinic/drIvanov_kkxrqa.png";
            public const string Description = "Examination & Consultation." + DefaultDescription1;
            public const int DepartmentId = 1;
        }

        public class ConsultationDoctor2
        {
            public const string Email = "nikolova@petcare.com";
            public const string FullName = "Anna Nikolova";
            public const string Username = "nikolova@petcare.com";
            public const string PhoneNumber = "0888111112";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "https://res.cloudinary.com/dqnorpdaj/image/upload/v1648707675/VetClinic/drNikolova_ag9dpb.png";
            public const string Description = "Examination & Consultation." + DefaultDescription2;
            public const int DepartmentId = 1;
        }

        public class ConsultationDoctor3
        {
            public const string Email = "petrova@petcare.com";
            public const string FullName = "Maria Petrova";
            public const string Username = "petrova@petcare.com";
            public const string PhoneNumber = "0888111113";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "https://res.cloudinary.com/dqnorpdaj/image/upload/v1648707676/VetClinic/drPetrova_khxori.png";
            public const string Description = "Examination & Consultation." + DefaultDescription3;
            public const int DepartmentId = 1;
        }

        public class InternalDoctor1
        {
            public const string Email = "georgiev@petcare.com";
            public const string FullName = "Petar Georgiev";
            public const string Username = "georgiev@petcare.com";
            public const string PhoneNumber = "0888111121";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "https://res.cloudinary.com/dqnorpdaj/image/upload/v1648707675/VetClinic/drGeorgiev_hqlx4c.jpg";
            public const string Description = "Internal Medicine, Gastroenterology." + DefaultDescription1;
            public const int DepartmentId = 2;
        }

        public class InternalDoctor2
        {
            public const string Email = "vasilev@petcare.com";
            public const string FullName = "Vasil Vasilev";
            public const string Username = "vasilev@petcare.com";
            public const string PhoneNumber = "0888111122";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "https://res.cloudinary.com/dqnorpdaj/image/upload/v1648707676/VetClinic/drVasilev_oaip6u.jpg";
            public const string Description = "Internal Medicine, Nephrology and Urology." + DefaultDescription2;
            public const int DepartmentId = 2;
        }

        public class InternalDoctor3
        {
            public const string Email = "dimova@petcare.com";
            public const string FullName = "Radoslava Dimova";
            public const string Username = "dimova@petcare.com";
            public const string PhoneNumber = "0888111123";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "https://res.cloudinary.com/dqnorpdaj/image/upload/v1648707675/VetClinic/drDimova_zdbhmy.jpg";
            public const string Description = "Internal Medicine, Allergology, Respiratory diseases." + DefaultDescription3;
            public const int DepartmentId = 2;
        }

        public class SurgeryDoctor1
        {
            public const string Email = "asenov@petcare.com";
            public const string FullName = "Asen Asenov";
            public const string Username = "asenov@petcare.com";
            public const string PhoneNumber = "0888111131";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "https://res.cloudinary.com/dqnorpdaj/image/upload/v1648707675/VetClinic/drAsenov_areb9j.jpg";
            public const string Description = "Surgery." + DefaultDescription1;
            public const int DepartmentId = 3;
        }

        public class SurgeryDoctor2
        {
            public const string Email = "stoyanov@petcare.com";
            public const string FullName = "Dimo Stoyanov";
            public const string Username = "stoyanov@petcare.com";
            public const string PhoneNumber = "0888111132";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "https://res.cloudinary.com/dqnorpdaj/image/upload/v1648707676/VetClinic/drStoyanov_ti5yxo.jpg";
            public const string Description = "Surgery." + DefaultDescription2;
            public const int DepartmentId = 3;
        }

        public class OrthopedicsDoctor1
        {
            public const string Email = "nikolov@petcare.com";
            public const string FullName = "Ivo Nikolov";
            public const string Username = "nikolov@petcare.com";
            public const string PhoneNumber = "0888111141";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "https://res.cloudinary.com/dqnorpdaj/image/upload/v1648707675/VetClinic/drNikolov_aovh0z.jpg";
            public const string Description = "Orthopedics and Traumatology." + DefaultDescription1;
            public const int DepartmentId = 4;
        }

        public class DermatologyDoctor1
        {
            public const string Email = "radeva@petcare.com";
            public const string FullName = "Ema Radeva";
            public const string Username = "radeva@petcare.com";
            public const string PhoneNumber = "0888111151";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "https://res.cloudinary.com/dqnorpdaj/image/upload/v1648707676/VetClinic/drRadeva_qwfszz.jpg";
            public const string Description = "Dermatology." + DefaultDescription1;
            public const int DepartmentId = 5;
        }

        public class EmergencyDoctor1
        {
            public const string Email = "stefanov@petcare.com";
            public const string FullName = "Stefan Stefanov";
            public const string Username = "stefanov@petcare.com";
            public const string PhoneNumber = "0888111161";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "https://res.cloudinary.com/dqnorpdaj/image/upload/v1648707676/VetClinic/drStefanov_wzdwwa.jpg";
            public const string Description = "Emergency Medicine." + DefaultDescription1;
            public const int DepartmentId = 6;
        }

        public class EmergencyDoctor2
        {
            public const string Email = "veleva@petcare.com";
            public const string FullName = "Zoya Veleva";
            public const string Username = "veleva@petcare.com";
            public const string PhoneNumber = "0888111162";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "https://res.cloudinary.com/dqnorpdaj/image/upload/v1648707676/VetClinic/drVeleva_p8uzst.jpg";
            public const string Description = "Emergency Medicine." + DefaultDescription2;
            public const int DepartmentId = 6;
        }

        public class GroomingDoctor1
        {
            public const string Email = "petkova@petcare.com";
            public const string FullName = "Iva Petkova";
            public const string Username = "petkova@petcare.com";
            public const string PhoneNumber = "0888111171";
            public const string Password = DefaultPassword;
            public const string ProfileImage = "https://res.cloudinary.com/dqnorpdaj/image/upload/v1648707675/VetClinic/drPetkova_urtsvq.jpg";
            public const string Description = "Grooming, Exotic animals." + DefaultDescription1;
            public const int DepartmentId = 7;
        }
    }
}
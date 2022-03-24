namespace VetClinic.Data.Seeding.SeedData
{
    public class UsersSeedData
    {
        public const string DefaultPassword = "123456";

        public class Doctor
        {
            public const string Email = "doctor@vetclinic.com";
            public const string FullName = "DoctorFullName";
            public const string Username = "doctor@vetclinic.com";
            public const string PhoneNumber = "0888888888";
            public const string Password = DefaultPassword;
            public const string Description = "Vet Clinic Consultations";
            public const int DepartmentId = 1;
        }

        public class Client
        {
            public const string Email = "client@client.com";
            public const string FullName = "ClientFullName";
            public const string Username = "client@client.com";
            public const string PhoneNumber = "0777777777";
            public const string Password = DefaultPassword;
        }

        public class Admin
        {
            public const string Email = "admin@vetclinic.com";
            public const string Username = "admin@vetclinic.com";
            public const string FullName = "Administrator";
            public const string PhoneNumber = "0999999999";
            public const string Password = DefaultPassword;
        }
    }
}

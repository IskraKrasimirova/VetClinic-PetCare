namespace VetClinic.Data
{
    public class ModelConstants
    {
        public class User
        {
            public const int FullNameMinLength = 3;
            public const int FullNameMaxLength = 150;
            public const int EmailAdressMinLength = 3;
            public const int EmailAdressMaxLength = 100;
            //public const string UserEmailRegularExpression =
            //    @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            public const string PhoneNumberRegex = @"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$";
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;
            public const int PhoneNumberMinLength = 6;
            public const int PhoneNumberMaxLength = 30;
        }

        public class Doctor
        {
            public const int FullNameMinLength = 3;
            public const int FullNameMaxLength = 150;
            public const int EmailAdressMinLenght = 3;
            public const int EmailAdressMaxLenght = 100;
            public const string PhoneNumberRegex = @"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$";
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;
            public const int PhoneNumberMinLength = 6;
            public const int PhoneNumberMaxLength = 30;
            public const int DescriptionMinLength = 5;
            public const int DescriptionMaxLength = 500;
        }

        public class Client
        {
            public const int FullNameMinLength = 3;
            public const int FullNameMaxLength = 150;
            public const int EmailAdressMinLength = 3;
            public const int EmailAdressMaxLength = 100;
            public const string PhoneNumberRegex = @"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$";
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;
            public const int PhoneNumberMinLength = 6;
            public const int PhoneNumberMaxLength = 30;
        }

        public class Pet
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;
            public const int BreedMinLength = 3;
            public const int BreedMaxLength = 50;
            public const int DescriptionMinLength = 2;
            public const int DescriptionMaxLength = 300;
        }

        public class Departmenet
        {
            public const int NameMinLength = 5;
            public const int NameMaxLength = 50;
            public const int DescriptionMinLength = 5;
            public const int DescriptionMaxLength = 1000;
        }

        public class Service
        {
            public const int NameMinLength = 5;
            public const int NameMaxLength = 100;
            public const int DescriptionMinLength = 5;
            public const int DescriptionMaxLength = 1000;
        }

        public class PetType
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;
        }

        public class Prescription
        {
            public const int DescriptionMinLength = 5;
            public const int DescriptionMaxLength = 500;
        }
    }
}

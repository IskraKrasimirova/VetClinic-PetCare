﻿namespace VetClinic.Data.Seeding.SeedData
{
    public class UsersSeedData
    {
        public const string DefaultPassword = "123456";
        public const string AdminPassword = "admin12";

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
            public const string Email = "admin@petcare.com";
            public const string Username = "admin@petcare.com";
            public const string FullName = "Administrator";
            public const string PhoneNumber = "0999999999";
            public const string Password = AdminPassword;
        }
    }
}

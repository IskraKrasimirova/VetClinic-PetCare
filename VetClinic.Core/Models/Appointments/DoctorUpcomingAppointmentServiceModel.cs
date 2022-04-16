﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinic.Core.Models.Appointments
{
    public class DoctorUpcomingAppointmentServiceModel
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string Hour { get; set; }

        public string ClientFullName { get; set; }

        public string ClientPhoneNumber { get; set; }

        public string ServiceName { get; set; }

        public string PetName { get; set; }

        public string PetType { get; set; }
    }
}

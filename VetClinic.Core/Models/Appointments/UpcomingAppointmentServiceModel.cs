﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinic.Core.Models.Appointments
{
    public class UpcomingAppointmentServiceModel
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string Hour { get; set; }

        public string DoctorFullName { get; set; }

        public string ServiceName { get; set; }

        public string PetName { get; set; }
    }
}

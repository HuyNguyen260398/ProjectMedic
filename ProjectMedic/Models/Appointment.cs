using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectMedic.Models
{
    public class Appointment
    {
        MediPlusEntities db = new MediPlusEntities();

        public int Appointment_ID { get; set; }
        public int MS_ID { get; set; }
        public string Medical_Specialty { get; set; }
        public string Date { get; set; }
        public string Doctor { get; set; }
        public string Room { get; set; }
        public double Price { get; set; }

        public Appointment(int id)
        {
            Appointment_ID = id;
            Working_Schedule ws = db.Working_Schedule.Single(a => a.WorkingSchedule_ID == id);
            Medical_Specialty = ws.Doctor.Medical_Specialty.MedicalSpecialty_Name;
            MS_ID = ws.Doctor.MedicalSpecialty_ID;
            Date = String.Concat(ws.Start.Value.ToShortDateString()," ", ws.Working_Time.WoringTime_Period);
            Doctor = String.Concat(ws.Doctor.Academic_Title.AcademicTitle_Code, " ", ws.Doctor.Doctor_Name);
            Room = ws.Working_Room.WorkingRoom_Number;
            Price = Double.Parse(ws.Doctor.Medical_Specialty.MedicalSpecialty_ServicePrice.ToString());
        }
    }
}
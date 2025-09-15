using System;
using HospitalManagementSystem.Utilities;

namespace HospitalManagementSystem.Models
{
    /// <summary>
    /// Appointment class to link Doctor and Patient
    /// </summary>
    public class Appointment
    {
        // Properties
        public int ID { get; private set; }
        public int DoctorID { get; set; }
        public int PatientID { get; set; }
        public string Description { get; set; }
        public DateTime AppointmentDate { get; set; }  // For future enhancement

        // Default constructor
        public Appointment()
        {
            ID = IDGenerator.GenerateID();
            AppointmentDate = DateTime.Now;
        }

        // Parameterized constructor
        public Appointment(int doctorId, int patientId, string description)
        {
            ID = IDGenerator.GenerateID();
            DoctorID = doctorId;
            PatientID = patientId;
            Description = description;
            AppointmentDate = DateTime.Now;
        }

        // Constructor for loading from file (with existing ID)
        public Appointment(int id, int doctorId, int patientId, string description, DateTime appointmentDate)
        {
            ID = id;
            DoctorID = doctorId;
            PatientID = patientId;
            Description = description;
            AppointmentDate = appointmentDate;
        }

        // ToString method for display
        public override string ToString()
        {
            return $"Appointment ID: {ID} | Doctor ID: {DoctorID} | Patient ID: {PatientID} | Description: {Description}";
        }

        // Method to convert to file format for saving
        public string ToFileFormat()
        {
            return $"{ID}|{DoctorID}|{PatientID}|{Description}|{AppointmentDate:yyyy-MM-dd HH:mm:ss}";
        }

        // Static method to create from file format
        public static Appointment FromFileFormat(string line)
        {
            try
            {
                string[] parts = line.Split('|');
                if (parts.Length < 5) return null;

                int id = int.Parse(parts[0]);
                int doctorId = int.Parse(parts[1]);
                int patientId = int.Parse(parts[2]);
                string description = parts[3];
                DateTime appointmentDate = DateTime.Parse(parts[4]);

                return new Appointment(id, doctorId, patientId, description, appointmentDate);
            }
            catch
            {
                return null;
            }
        }
    }
}
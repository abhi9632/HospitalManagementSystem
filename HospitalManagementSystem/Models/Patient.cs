using System;
using System.Collections.Generic;
using System.Linq;
using HospitalManagementSystem.Services;
using HospitalManagementSystem.Utilities;

namespace HospitalManagementSystem.Models
{
    /// <summary>
    /// Patient class inheriting from Person
    /// </summary>
    public class Patient : Person
    {
        // Additional Patient-specific properties
        public int? RegisteredDoctorID { get; set; }
        public string MedicalHistory { get; set; }

        // Default constructor
        public Patient() : base()
        {
            ID = IDGenerator.GenerateID();
        }

        // Parameterized constructor - demonstrates constructor overloading
        public Patient(string name, string password, string phone, string address, string email, string medicalHistory = "")
            : base(name, password, phone, address, email)
        {
            ID = IDGenerator.GenerateID();
            MedicalHistory = medicalHistory;
            RegisteredDoctorID = null;
        }

        // Constructor for loading from file (with existing ID)
        public Patient(int id, string name, string password, string phone, string address, string email,
                      int? doctorId, string medicalHistory)
            : base(name, password, phone, address, email)
        {
            ID = id;
            RegisteredDoctorID = doctorId;
            MedicalHistory = medicalHistory;
        }

        // Override ToString method - demonstrates method overriding
        public override string ToString()
        {
            string doctorInfo = RegisteredDoctorID.HasValue ? $"Doctor ID: {RegisteredDoctorID}" : "No Doctor Assigned";
            return $"Patient | ID: {ID} | Name: {Name} | Phone: {Phone} | {doctorInfo}";
        }

        // Override DisplayDetails
        public override void DisplayDetails()
        {
            Console.WriteLine("\n========== PATIENT DETAILS ==========");
            base.DisplayDetails();
            Console.WriteLine($"Medical History: {(string.IsNullOrEmpty(MedicalHistory) ? "None" : MedicalHistory)}");
            Console.WriteLine($"Registered Doctor ID: {(RegisteredDoctorID.HasValue ? RegisteredDoctorID.ToString() : "Not Registered")}");
            Console.WriteLine("=====================================\n");
        }

        // Implementation of abstract method
        public override void DisplayMenu()
        {
            bool logout = false;
            while (!logout)
            {
                Console.Clear();
                Console.WriteLine("\n========== PATIENT MENU ==========");
                Console.WriteLine($"Welcome {Name} (ID: {ID})");
                Console.WriteLine("==================================");
                Console.WriteLine("1. List Patient Details");
                Console.WriteLine("2. List My Doctor Details");
                Console.WriteLine("3. List All Appointments");
                Console.WriteLine("4. Book Appointment");
                Console.WriteLine("5. Logout");
                Console.WriteLine("6. Exit");
                Console.WriteLine("==================================");
                Console.Write("Please select an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListPatientDetails();
                        break;
                    case "2":
                        ListDoctorDetails();
                        break;
                    case "3":
                        ListAppointments();
                        break;
                    case "4":
                        BookAppointment();
                        break;
                    case "5":
                        logout = true;
                        Console.WriteLine("Logging out...");
                        System.Threading.Thread.Sleep(1000);
                        break;
                    case "6":
                        Console.WriteLine("Exiting application...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Patient menu methods
        private void ListPatientDetails()
        {
            Console.Clear();
            DisplayDetails();
            Console.WriteLine("Press any key to return to menu...");
            Console.ReadKey();
        }

        private void ListDoctorDetails()
        {
            Console.Clear();
            Console.WriteLine("\n========== MY DOCTOR DETAILS ==========");

            if (!RegisteredDoctorID.HasValue)
            {
                Console.WriteLine("You are not registered with any doctor.");
            }
            else
            {
                var doctor = DataManager.GetDoctorById(RegisteredDoctorID.Value);
                if (doctor != null)
                {
                    doctor.DisplayDetails();
                }
                else
                {
                    Console.WriteLine("Doctor information not found.");
                }
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        private void ListAppointments()
        {
            Console.Clear();
            Console.WriteLine("\n========== MY APPOINTMENTS ==========");

            var appointments = DataManager.GetAppointmentsByPatientId(ID);

            if (appointments.Count == 0)
            {
                Console.WriteLine("You have no appointments.");
            }
            else
            {
                foreach (var appointment in appointments)
                {
                    Console.WriteLine(appointment.ToString());
                }
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        private void BookAppointment()
        {
            Console.Clear();
            Console.WriteLine("\n========== BOOK APPOINTMENT ==========");

            // Check if patient has a registered doctor
            if (!RegisteredDoctorID.HasValue)
            {
                Console.WriteLine("You need to register with a doctor first.");
                Console.WriteLine("\nAvailable Doctors:");

                var doctors = DataManager.GetAllDoctors();
                if (doctors.Count == 0)
                {
                    Console.WriteLine("No doctors available.");
                    Console.WriteLine("\nPress any key to return to menu...");
                    Console.ReadKey();
                    return;
                }

                for (int i = 0; i < doctors.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {doctors[i].ToString()}");
                }

                Console.Write("\nSelect a doctor (enter number): ");
                if (int.TryParse(Console.ReadLine(), out int selection) && selection > 0 && selection <= doctors.Count)
                {
                    RegisteredDoctorID = doctors[selection - 1].ID;
                    DataManager.UpdatePatient(this);
                    Console.WriteLine($"Successfully registered with Dr. {doctors[selection - 1].Name}");
                }
                else
                {
                    Console.WriteLine("Invalid selection.");
                    Console.WriteLine("\nPress any key to return to menu...");
                    Console.ReadKey();
                    return;
                }
            }

            // Book appointment with registered doctor
            Console.Write("\nEnter appointment description: ");
            string description = Console.ReadLine();

            var appointment = new Appointment(RegisteredDoctorID.Value, ID, description);
            DataManager.AddAppointment(appointment);

            Console.WriteLine("\nAppointment booked successfully!");
            Console.WriteLine($"Appointment ID: {appointment.ID}");

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        // Method to convert to file format for saving
        public string ToFileFormat()
        {
            return $"{ID}|{Name}|{Password}|{Phone}|{Address}|{Email}|{RegisteredDoctorID?.ToString() ?? ""}|{MedicalHistory}";
        }

        // Static method to create from file format
        public static Patient FromFileFormat(string line)
        {
            try
            {
                string[] parts = line.Split('|');
                if (parts.Length < 8) return null;

                int id = int.Parse(parts[0]);
                string name = parts[1];
                string password = parts[2];
                string phone = parts[3];
                string address = parts[4];
                string email = parts[5];
                int? doctorId = null;
                if (!string.IsNullOrEmpty(parts[6]))
                {
                    doctorId = int.Parse(parts[6]);
                }
                string medicalHistory = parts[7];

                return new Patient(id, name, password, phone, address, email, doctorId, medicalHistory);
            }
            catch
            {
                return null;
            }
        }
    }
}
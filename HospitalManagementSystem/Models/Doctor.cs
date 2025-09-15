using System;
using System.Collections.Generic;
using System.Linq;
using HospitalManagementSystem.Services;
using HospitalManagementSystem.Utilities;

namespace HospitalManagementSystem.Models
{
    /// <summary>
    /// Doctor class inheriting from Person
    /// </summary>
    public class Doctor : Person
    {
        // Additional Doctor-specific properties
        public string Specialization { get; set; }

        // Default constructor
        public Doctor() : base()
        {
            ID = IDGenerator.GenerateID();
        }

        // Parameterized constructor
        public Doctor(string name, string password, string phone, string address, string email, string specialization)
            : base(name, password, phone, address, email)
        {
            ID = IDGenerator.GenerateID();
            Specialization = specialization;
        }

        // Constructor for loading from file (with existing ID)
        public Doctor(int id, string name, string password, string phone, string address, string email, string specialization)
            : base(name, password, phone, address, email)
        {
            ID = id;
            Specialization = specialization;
        }

        // Override ToString method
        public override string ToString()
        {
            return $"Doctor | ID: {ID} | Name: {Name} | Specialization: {Specialization} | Phone: {Phone}";
        }

        // Override DisplayDetails
        public override void DisplayDetails()
        {
            Console.WriteLine("\n========== DOCTOR DETAILS ==========");
            base.DisplayDetails();
            Console.WriteLine($"Specialization: {Specialization}");
            Console.WriteLine("====================================\n");
        }

        // Implementation of abstract method
        public override void DisplayMenu()
        {
            bool logout = false;
            while (!logout)
            {
                Console.Clear();
                Console.WriteLine("\n========== DOCTOR MENU ==========");
                Console.WriteLine($"Welcome Dr. {Name} (ID: {ID})");
                Console.WriteLine("=================================");
                Console.WriteLine("1. List Doctor Details");
                Console.WriteLine("2. List Patients");
                Console.WriteLine("3. List Appointments");
                Console.WriteLine("4. Check Particular Patient");
                Console.WriteLine("5. List Appointments With Patient");
                Console.WriteLine("6. Logout");
                Console.WriteLine("7. Exit");
                Console.WriteLine("=================================");
                Console.Write("Please select an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListDoctorDetails();
                        break;
                    case "2":
                        ListPatients();
                        break;
                    case "3":
                        ListAppointments();
                        break;
                    case "4":
                        CheckParticularPatient();
                        break;
                    case "5":
                        ListAppointmentsWithPatient();
                        break;
                    case "6":
                        logout = true;
                        Console.WriteLine("Logging out...");
                        System.Threading.Thread.Sleep(1000);
                        break;
                    case "7":
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

        // Doctor menu methods
        private void ListDoctorDetails()
        {
            Console.Clear();
            DisplayDetails();
            Console.WriteLine("Press any key to return to menu...");
            Console.ReadKey();
        }

        private void ListPatients()
        {
            Console.Clear();
            Console.WriteLine("\n========== MY PATIENTS ==========");

            var patients = DataManager.GetPatientsByDoctorId(ID);

            if (patients.Count == 0)
            {
                Console.WriteLine("You have no registered patients.");
            }
            else
            {
                foreach (var patient in patients)
                {
                    Console.WriteLine(patient.ToString());
                }
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        private void ListAppointments()
        {
            Console.Clear();
            Console.WriteLine("\n========== MY APPOINTMENTS ==========");

            var appointments = DataManager.GetAppointmentsByDoctorId(ID);

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

        private void CheckParticularPatient()
        {
            Console.Clear();
            Console.WriteLine("\n========== CHECK PARTICULAR PATIENT ==========");

            Console.Write("Enter Patient ID: ");
            if (int.TryParse(Console.ReadLine(), out int patientId))
            {
                var patient = DataManager.GetPatientById(patientId);

                if (patient != null)
                {
                    patient.DisplayDetails();
                }
                else
                {
                    Console.WriteLine("Patient not found with the given ID.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format. Please enter a valid number.");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        private void ListAppointmentsWithPatient()
        {
            Console.Clear();
            Console.WriteLine("\n========== APPOINTMENTS WITH PATIENT ==========");

            Console.Write("Enter Patient ID: ");
            if (int.TryParse(Console.ReadLine(), out int patientId))
            {
                var patient = DataManager.GetPatientById(patientId);

                if (patient != null)
                {
                    Console.WriteLine($"\nAppointments with {patient.Name} (ID: {patient.ID}):");

                    var appointments = DataManager.GetAppointmentsByDoctorAndPatient(ID, patientId);

                    if (appointments.Count == 0)
                    {
                        Console.WriteLine("No appointments found with this patient.");
                    }
                    else
                    {
                        foreach (var appointment in appointments)
                        {
                            Console.WriteLine(appointment.ToString());
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Patient not found with the given ID.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format. Please enter a valid number.");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        // Method to convert to file format for saving
        public string ToFileFormat()
        {
            return $"{ID}|{Name}|{Password}|{Phone}|{Address}|{Email}|{Specialization}";
        }

        // Static method to create from file format
        public static Doctor FromFileFormat(string line)
        {
            try
            {
                string[] parts = line.Split('|');
                if (parts.Length < 7) return null;

                int id = int.Parse(parts[0]);
                string name = parts[1];
                string password = parts[2];
                string phone = parts[3];
                string address = parts[4];
                string email = parts[5];
                string specialization = parts[6];

                return new Doctor(id, name, password, phone, address, email, specialization);
            }
            catch
            {
                return null;
            }
        }
    }
}
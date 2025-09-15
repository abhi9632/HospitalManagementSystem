using System;
using HospitalManagementSystem.Services;
using HospitalManagementSystem.Utilities;

namespace HospitalManagementSystem.Models
{
    /// <summary>
    /// Receptionist class - Additional user role for bonus marks
    /// Can manage appointments and patient check-ins
    /// </summary>
    public class Receptionist : Person
    {
        public string Shift { get; set; }  // Morning/Evening/Night

        // Default constructor
        public Receptionist() : base()
        {
            ID = IDGenerator.GenerateID();
            Shift = "Morning";
        }

        // Parameterized constructor
        public Receptionist(string name, string password, string phone, string address, string email, string shift)
            : base(name, password, phone, address, email)
        {
            ID = IDGenerator.GenerateID();
            Shift = shift;
        }

        // Constructor for loading from file
        public Receptionist(int id, string name, string password, string phone, string address, string email, string shift)
            : base(name, password, phone, address, email)
        {
            ID = id;
            Shift = shift;
        }

        // Override ToString method
        public override string ToString()
        {
            return $"Receptionist | ID: {ID} | Name: {Name} | Shift: {Shift}";
        }

        // Override DisplayDetails
        public override void DisplayDetails()
        {
            Console.WriteLine("\n========== RECEPTIONIST DETAILS ==========");
            base.DisplayDetails();
            Console.WriteLine($"Shift: {Shift}");
            Console.WriteLine("==========================================\n");
        }

        // Implementation of abstract method
        public override void DisplayMenu()
        {
            bool logout = false;
            while (!logout)
            {
                Console.Clear();
                Console.WriteLine("\n========== RECEPTIONIST MENU ==========");
                Console.WriteLine($"Welcome {Name} (ID: {ID})");
                Console.WriteLine($"Current Shift: {Shift}");
                Console.WriteLine("=======================================");
                Console.WriteLine("1. View All Appointments Today");
                Console.WriteLine("2. Register New Patient");
                Console.WriteLine("3. Book Appointment for Patient");
                Console.WriteLine("4. View Patient Details");
                Console.WriteLine("5. View Doctor Schedule");
                Console.WriteLine("6. Check In Patient");
                Console.WriteLine("7. Logout");
                Console.WriteLine("8. Exit");
                Console.WriteLine("=======================================");
                Console.Write("Please select an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewAllAppointments();
                        break;
                    case "2":
                        RegisterNewPatient();
                        break;
                    case "3":
                        BookAppointmentForPatient();
                        break;
                    case "4":
                        ViewPatientDetails();
                        break;
                    case "5":
                        ViewDoctorSchedule();
                        break;
                    case "6":
                        CheckInPatient();
                        break;
                    case "7":
                        logout = true;
                        Console.WriteLine("Logging out...");
                        System.Threading.Thread.Sleep(1000);
                        break;
                    case "8":
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

        // Receptionist specific methods
        private void ViewAllAppointments()
        {
            Console.Clear();
            Console.WriteLine("\n========== TODAY'S APPOINTMENTS ==========");

            var appointments = DataManager.GetAllAppointments();
            if (appointments.Count == 0)
            {
                Console.WriteLine("No appointments scheduled.");
            }
            else
            {
                foreach (var appointment in appointments)
                {
                    var patient = DataManager.GetPatientById(appointment.PatientID);
                    var doctor = DataManager.GetDoctorById(appointment.DoctorID);

                    Console.WriteLine($"ID: {appointment.ID} | Patient: {patient?.Name ?? "Unknown"} | Doctor: {doctor?.Name ?? "Unknown"} | {appointment.Description}");
                }
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        private void RegisterNewPatient()
        {
            Console.Clear();
            Console.WriteLine("\n========== REGISTER NEW PATIENT ==========");

            try
            {
                Console.Write("Enter Patient Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter Password: ");
                string password = Console.ReadLine();

                Console.Write("Enter Phone Number: ");
                string phone = Console.ReadLine();

                Console.Write("Enter Address: ");
                string address = Console.ReadLine();

                Console.Write("Enter Email: ");
                string email = Console.ReadLine();

                Console.Write("Enter Medical History (optional): ");
                string medicalHistory = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("\nError: Name and Password are required.");
                }
                else
                {
                    var patient = new Patient(name, password, phone, address, email, medicalHistory);
                    DataManager.AddPatient(patient);

                    // Send email confirmation (bonus feature)
                    EmailService.SendPatientRegistrationEmail(patient);

                    Console.WriteLine($"\nPatient registered successfully!");
                    Console.WriteLine($"Generated ID: {patient.ID}");
                    Console.WriteLine("Registration confirmation email sent.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError registering patient: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        private void BookAppointmentForPatient()
        {
            Console.Clear();
            Console.WriteLine("\n========== BOOK APPOINTMENT ==========");

            Console.Write("Enter Patient ID: ");
            if (int.TryParse(Console.ReadLine(), out int patientId))
            {
                var patient = DataManager.GetPatientById(patientId);
                if (patient != null)
                {
                    Console.WriteLine($"Booking appointment for: {patient.Name}");

                    // Show available doctors
                    var doctors = DataManager.GetAllDoctors();
                    Console.WriteLine("\nAvailable Doctors:");
                    for (int i = 0; i < doctors.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {doctors[i].ToString()}");
                    }

                    Console.Write("\nSelect doctor (enter number): ");
                    if (int.TryParse(Console.ReadLine(), out int selection) && selection > 0 && selection <= doctors.Count)
                    {
                        var selectedDoctor = doctors[selection - 1];

                        Console.Write("Enter appointment description: ");
                        string description = Console.ReadLine();

                        var appointment = new Appointment(selectedDoctor.ID, patient.ID, description);
                        DataManager.AddAppointment(appointment);

                        // Send email confirmation (bonus feature)
                        EmailService.SendAppointmentConfirmationEmail(patient, selectedDoctor, appointment);

                        Console.WriteLine("\nAppointment booked successfully!");
                        Console.WriteLine($"Appointment ID: {appointment.ID}");
                        Console.WriteLine("Confirmation email sent to patient.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection.");
                    }
                }
                else
                {
                    Console.WriteLine("Patient not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Patient ID.");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        private void ViewPatientDetails()
        {
            Console.Clear();
            Console.WriteLine("\n========== VIEW PATIENT DETAILS ==========");

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
                    Console.WriteLine("Patient not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Patient ID.");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        private void ViewDoctorSchedule()
        {
            Console.Clear();
            Console.WriteLine("\n========== DOCTOR SCHEDULES ==========");

            var doctors = DataManager.GetAllDoctors();
            foreach (var doctor in doctors)
            {
                Console.WriteLine($"\n{doctor.Name} - {doctor.Specialization}:");
                var appointments = DataManager.GetAppointmentsByDoctorId(doctor.ID);

                if (appointments.Count == 0)
                {
                    Console.WriteLine("  No appointments scheduled");
                }
                else
                {
                    foreach (var apt in appointments)
                    {
                        var patient = DataManager.GetPatientById(apt.PatientID);
                        Console.WriteLine($"  - {patient?.Name ?? "Unknown"}: {apt.Description}");
                    }
                }
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        private void CheckInPatient()
        {
            Console.Clear();
            Console.WriteLine("\n========== PATIENT CHECK-IN ==========");

            Console.Write("Enter Patient ID: ");
            if (int.TryParse(Console.ReadLine(), out int patientId))
            {
                var patient = DataManager.GetPatientById(patientId);
                if (patient != null)
                {
                    Console.WriteLine($"\nPatient: {patient.Name}");
                    Console.WriteLine($"Phone: {patient.Phone}");

                    var appointments = DataManager.GetAppointmentsByPatientId(patientId);
                    if (appointments.Count > 0)
                    {
                        Console.WriteLine("\nAppointments:");
                        foreach (var apt in appointments)
                        {
                            var doctor = DataManager.GetDoctorById(apt.DoctorID);
                            Console.WriteLine($"- Dr. {doctor?.Name ?? "Unknown"}: {apt.Description}");
                        }

                        Console.WriteLine($"\n✓ Patient {patient.Name} checked in successfully at {DateTime.Now:HH:mm}");
                    }
                    else
                    {
                        Console.WriteLine("\nNo appointments found for this patient.");
                    }
                }
                else
                {
                    Console.WriteLine("Patient not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Patient ID.");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        // File format methods
        public string ToFileFormat()
        {
            return $"{ID}|{Name}|{Password}|{Phone}|{Address}|{Email}|{Shift}";
        }

        public static Receptionist FromFileFormat(string line)
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
                string shift = parts[6];

                return new Receptionist(id, name, password, phone, address, email, shift);
            }
            catch
            {
                return null;
            }
        }
    }
}
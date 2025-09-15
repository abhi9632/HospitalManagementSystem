using System;
using HospitalManagementSystem.Services;
using HospitalManagementSystem.Utilities;

namespace HospitalManagementSystem.Models
{
    /// <summary>
    /// Administrator class inheriting from Person
    /// </summary>
    public class Administrator : Person
    {
        // Additional Admin-specific properties
        public string Role { get; set; }

        // Default constructor
        public Administrator() : base()
        {
            ID = IDGenerator.GenerateID();
            Role = "Administrator";
        }

        // Parameterized constructor
        public Administrator(string name, string password, string phone, string address, string email)
            : base(name, password, phone, address, email)
        {
            ID = IDGenerator.GenerateID();
            Role = "Administrator";
        }

        // Constructor for loading from file (with existing ID)
        public Administrator(int id, string name, string password, string phone, string address, string email, string role)
            : base(name, password, phone, address, email)
        {
            ID = id;
            Role = role;
        }

        // Override ToString method
        public override string ToString()
        {
            return $"Admin | ID: {ID} | Name: {Name} | Role: {Role}";
        }

        // Override DisplayDetails
        public override void DisplayDetails()
        {
            Console.WriteLine("\n========== ADMINISTRATOR DETAILS ==========");
            base.DisplayDetails();
            Console.WriteLine($"Role: {Role}");
            Console.WriteLine("==========================================\n");
        }

        // Implementation of abstract method
        public override void DisplayMenu()
        {
            bool logout = false;
            while (!logout)
            {
                Console.Clear();
                Console.WriteLine("\n========== ADMINISTRATOR MENU ==========");
                Console.WriteLine($"Welcome {Name} (ID: {ID})");
                Console.WriteLine("========================================");
                Console.WriteLine("1. List All Doctors");
                Console.WriteLine("2. Check Doctor Details");
                Console.WriteLine("3. List All Patients");
                Console.WriteLine("4. Check Patient Details");
                Console.WriteLine("5. Add Doctor");
                Console.WriteLine("6. Add Patient");
                Console.WriteLine("7. Logout");
                Console.WriteLine("8. Exit");
                Console.WriteLine("========================================");
                Console.Write("Please select an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListAllDoctors();
                        break;
                    case "2":
                        CheckDoctorDetails();
                        break;
                    case "3":
                        ListAllPatients();
                        break;
                    case "4":
                        CheckPatientDetails();
                        break;
                    case "5":
                        AddDoctor();
                        break;
                    case "6":
                        AddPatient();
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

        // Administrator menu methods
        private void ListAllDoctors()
        {
            Console.Clear();
            Console.WriteLine("\n========== ALL DOCTORS ==========");

            var doctors = DataManager.GetAllDoctors();

            if (doctors.Count == 0)
            {
                Console.WriteLine("No doctors in the system.");
            }
            else
            {
                foreach (var doctor in doctors)
                {
                    Console.WriteLine(doctor.ToString());
                }
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        private void AddReceptionist()
        {
            Console.Clear();
            Console.WriteLine("\n========== ADD NEW RECEPTIONIST ==========");

            try
            {
                Console.Write("Enter Receptionist Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter Password: ");
                string password = Console.ReadLine();

                Console.Write("Enter Phone Number: ");
                string phone = Console.ReadLine();

                Console.Write("Enter Address: ");
                string address = Console.ReadLine();

                Console.Write("Enter Email: ");
                string email = Console.ReadLine();

                Console.Write("Enter Shift (Morning/Evening/Night): ");
                string shift = Console.ReadLine();

                // Validate inputs
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("\nError: Name and Password are required fields.");
                }
                else
                {
                    var receptionist = new Receptionist(name, password, phone, address, email, shift);
                    DataManager.AddReceptionist(receptionist);

                    Console.WriteLine($"\nReceptionist added successfully!");
                    Console.WriteLine($"Generated ID: {receptionist.ID}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError adding receptionist: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        private void CheckDoctorDetails()
        {
            Console.Clear();
            Console.WriteLine("\n========== CHECK DOCTOR DETAILS ==========");

            Console.Write("Enter Doctor ID: ");
            if (int.TryParse(Console.ReadLine(), out int doctorId))
            {
                var doctor = DataManager.GetDoctorById(doctorId);

                if (doctor != null)
                {
                    doctor.DisplayDetails();
                }
                else
                {
                    Console.WriteLine("Doctor not found with the given ID.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format. Please enter a valid number.");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        private void ListAllPatients()
        {
            Console.Clear();
            Console.WriteLine("\n========== ALL PATIENTS ==========");

            var patients = DataManager.GetAllPatients();

            if (patients.Count == 0)
            {
                Console.WriteLine("No patients in the system.");
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

        private void CheckPatientDetails()
        {
            Console.Clear();
            Console.WriteLine("\n========== CHECK PATIENT DETAILS ==========");

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

        private void AddDoctor()
        {
            Console.Clear();
            Console.WriteLine("\n========== ADD NEW DOCTOR ==========");

            try
            {
                Console.Write("Enter Doctor Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter Password: ");
                string password = Console.ReadLine();

                Console.Write("Enter Phone Number: ");
                string phone = Console.ReadLine();

                Console.Write("Enter Address: ");
                string address = Console.ReadLine();

                Console.Write("Enter Email: ");
                string email = Console.ReadLine();

                Console.Write("Enter Specialization: ");
                string specialization = Console.ReadLine();

                // Validate inputs
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("\nError: Name and Password are required fields.");
                }
                else
                {
                    var doctor = new Doctor(name, password, phone, address, email, specialization);
                    DataManager.AddDoctor(doctor);

                    Console.WriteLine($"\nDoctor added successfully!");
                    Console.WriteLine($"Generated ID: {doctor.ID}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError adding doctor: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        private void AddPatient()
        {
            Console.Clear();
            Console.WriteLine("\n========== ADD NEW PATIENT ==========");

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

                // Validate inputs
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("\nError: Name and Password are required fields.");
                }
                else
                {
                    var patient = new Patient(name, password, phone, address, email, medicalHistory);
                    DataManager.AddPatient(patient);

                    Console.WriteLine($"\nPatient added successfully!");
                    Console.WriteLine($"Generated ID: {patient.ID}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError adding patient: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        // Method to convert to file format for saving
        public string ToFileFormat()
        {
            return $"{ID}|{Name}|{Password}|{Phone}|{Address}|{Email}|{Role}";
        }

        // Static method to create from file format
        public static Administrator FromFileFormat(string line)
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
                string role = parts[6];

                return new Administrator(id, name, password, phone, address, email, role);
            }
            catch
            {
                return null;
            }
        }
    }
}
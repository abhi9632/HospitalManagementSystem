using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Utilities;

namespace HospitalManagementSystem.Services
{
    /// <summary>
    /// Static class for managing data persistence with text files
    /// </summary>
    public static class DataManager
    {
        // Simple file paths - just use "Data" folder
        private static readonly string DataDirectory = "Data";
        private static readonly string PatientsFile = Path.Combine(DataDirectory, "patients.txt");
        private static readonly string DoctorsFile = Path.Combine(DataDirectory, "doctors.txt");
        private static readonly string AdminsFile = Path.Combine(DataDirectory, "admins.txt");
        private static readonly string AppointmentsFile = Path.Combine(DataDirectory, "appointments.txt");
        private static readonly string ReceptionistsFile = Path.Combine(DataDirectory, "receptionists.txt");

        // In-memory collections
        private static List<Patient> patients = new List<Patient>();
        private static List<Doctor> doctors = new List<Doctor>();
        private static List<Administrator> administrators = new List<Administrator>();
        private static List<Appointment> appointments = new List<Appointment>();
        private static List<Receptionist> receptionists = new List<Receptionist>();

        /// <summary>
        /// Initialize the data manager and load all data
        /// </summary>
        public static void Initialize()
        {
            try
            {
                // Create data directory if it doesn't exist
                if (!Directory.Exists(DataDirectory))
                {
                    Directory.CreateDirectory(DataDirectory);
                }

                // Load all data from files
                LoadPatients();
                LoadDoctors();
                LoadAdministrators();
                LoadReceptionists();
                LoadAppointments();

                // If no data exists, create default admin
                if (administrators.Count == 0)
                {
                    CreateDefaultAdmin();
                }

                // Create sample data if needed
                if (doctors.Count == 0 && patients.Count == 0)
                {
                    CreateSampleData();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing data: {ex.Message}");
            }
        }

        #region Patient Operations

        public static List<Patient> GetAllPatients()
        {
            return patients;
        }

        public static Patient GetPatientById(int id)
        {
            foreach (var patient in patients)
            {
                if (patient.ID == id)
                    return patient;
            }
            return null;
        }

        public static List<Patient> GetPatientsByDoctorId(int doctorId)
        {
            List<Patient> result = new List<Patient>();
            foreach (var patient in patients)
            {
                if (patient.RegisteredDoctorID == doctorId)
                    result.Add(patient);
            }
            return result;
        }

        public static void AddPatient(Patient patient)
        {
            patients.Add(patient);
            SavePatients();

            // Send registration email (bonus feature)
            EmailService.SendPatientRegistrationEmail(patient);
        }

        public static void UpdatePatient(Patient patient)
        {
            for (int i = 0; i < patients.Count; i++)
            {
                if (patients[i].ID == patient.ID)
                {
                    patients[i] = patient;
                    SavePatients();
                    break;
                }
            }
        }

        private static void LoadPatients()
        {
            try
            {
                if (File.Exists(PatientsFile))
                {
                    string[] lines = File.ReadAllLines(PatientsFile);
                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            Patient patient = Patient.FromFileFormat(line);
                            if (patient != null)
                            {
                                patients.Add(patient);
                                IDGenerator.RegisterID(patient.ID);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading patients: {ex.Message}");
            }
        }

        private static void SavePatients()
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (var patient in patients)
                {
                    lines.Add(patient.ToFileFormat());
                }
                File.WriteAllLines(PatientsFile, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving patients: {ex.Message}");
            }
        }

        #endregion

        #region Doctor Operations

        public static List<Doctor> GetAllDoctors()
        {
            return doctors;
        }

        public static Doctor GetDoctorById(int id)
        {
            foreach (var doctor in doctors)
            {
                if (doctor.ID == id)
                    return doctor;
            }
            return null;
        }

        public static void AddDoctor(Doctor doctor)
        {
            doctors.Add(doctor);
            SaveDoctors();
        }

        private static void LoadDoctors()
        {
            try
            {
                if (File.Exists(DoctorsFile))
                {
                    string[] lines = File.ReadAllLines(DoctorsFile);
                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            Doctor doctor = Doctor.FromFileFormat(line);
                            if (doctor != null)
                            {
                                doctors.Add(doctor);
                                IDGenerator.RegisterID(doctor.ID);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading doctors: {ex.Message}");
            }
        }

        private static void SaveDoctors()
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (var doctor in doctors)
                {
                    lines.Add(doctor.ToFileFormat());
                }
                File.WriteAllLines(DoctorsFile, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving doctors: {ex.Message}");
            }
        }

        #endregion

        #region Administrator Operations

        public static List<Administrator> GetAllAdministrators()
        {
            return administrators;
        }

        public static Administrator GetAdministratorById(int id)
        {
            foreach (var admin in administrators)
            {
                if (admin.ID == id)
                    return admin;
            }
            return null;
        }

        private static void LoadAdministrators()
        {
            try
            {
                if (File.Exists(AdminsFile))
                {
                    string[] lines = File.ReadAllLines(AdminsFile);
                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            Administrator admin = Administrator.FromFileFormat(line);
                            if (admin != null)
                            {
                                administrators.Add(admin);
                                IDGenerator.RegisterID(admin.ID);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading administrators: {ex.Message}");
            }
        }

        private static void SaveAdministrators()
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (var admin in administrators)
                {
                    lines.Add(admin.ToFileFormat());
                }
                File.WriteAllLines(AdminsFile, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving administrators: {ex.Message}");
            }
        }

        #endregion

        #region Receptionist Operations (Bonus Feature)

        public static List<Receptionist> GetAllReceptionists()
        {
            return receptionists;
        }

        public static void AddReceptionist(Receptionist receptionist)
        {
            receptionists.Add(receptionist);
            SaveReceptionists();
        }

        private static void LoadReceptionists()
        {
            try
            {
                if (File.Exists(ReceptionistsFile))
                {
                    string[] lines = File.ReadAllLines(ReceptionistsFile);
                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            Receptionist receptionist = Receptionist.FromFileFormat(line);
                            if (receptionist != null)
                            {
                                receptionists.Add(receptionist);
                                IDGenerator.RegisterID(receptionist.ID);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading receptionists: {ex.Message}");
            }
        }

        private static void SaveReceptionists()
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (var receptionist in receptionists)
                {
                    lines.Add(receptionist.ToFileFormat());
                }
                File.WriteAllLines(ReceptionistsFile, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving receptionists: {ex.Message}");
            }
        }

        #endregion

        #region Appointment Operations

        public static List<Appointment> GetAllAppointments()
        {
            return appointments;
        }

        public static List<Appointment> GetAppointmentsByPatientId(int patientId)
        {
            List<Appointment> result = new List<Appointment>();
            foreach (var appointment in appointments)
            {
                if (appointment.PatientID == patientId)
                    result.Add(appointment);
            }
            return result;
        }

        public static List<Appointment> GetAppointmentsByDoctorId(int doctorId)
        {
            List<Appointment> result = new List<Appointment>();
            foreach (var appointment in appointments)
            {
                if (appointment.DoctorID == doctorId)
                    result.Add(appointment);
            }
            return result;
        }

        public static List<Appointment> GetAppointmentsByDoctorAndPatient(int doctorId, int patientId)
        {
            List<Appointment> result = new List<Appointment>();
            foreach (var appointment in appointments)
            {
                if (appointment.DoctorID == doctorId && appointment.PatientID == patientId)
                    result.Add(appointment);
            }
            return result;
        }

        public static void AddAppointment(Appointment appointment)
        {
            appointments.Add(appointment);
            SaveAppointments();

            // Send appointment confirmation email (bonus feature)
            var patient = GetPatientById(appointment.PatientID);
            var doctor = GetDoctorById(appointment.DoctorID);
            if (patient != null && doctor != null)
            {
                EmailService.SendAppointmentConfirmationEmail(patient, doctor, appointment);
            }
        }

        private static void LoadAppointments()
        {
            try
            {
                if (File.Exists(AppointmentsFile))
                {
                    string[] lines = File.ReadAllLines(AppointmentsFile);
                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            Appointment appointment = Appointment.FromFileFormat(line);
                            if (appointment != null)
                            {
                                appointments.Add(appointment);
                                IDGenerator.RegisterID(appointment.ID);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading appointments: {ex.Message}");
            }
        }

        private static void SaveAppointments()
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (var appointment in appointments)
                {
                    lines.Add(appointment.ToFileFormat());
                }
                File.WriteAllLines(AppointmentsFile, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving appointments: {ex.Message}");
            }
        }

        #endregion

        #region Authentication

        public static Person Authenticate(int id, string password)
        {
            // Check patients
            foreach (var patient in patients)
            {
                if (patient.ID == id && patient.Password == password)
                    return patient;
            }

            // Check doctors
            foreach (var doctor in doctors)
            {
                if (doctor.ID == id && doctor.Password == password)
                    return doctor;
            }

            // Check administrators
            foreach (var admin in administrators)
            {
                if (admin.ID == id && admin.Password == password)
                    return admin;
            }

            // Check receptionists (bonus feature)
            foreach (var receptionist in receptionists)
            {
                if (receptionist.ID == id && receptionist.Password == password)
                    return receptionist;
            }

            return null;
        }

        #endregion

        #region Default Data Creation

        private static void CreateDefaultAdmin()
        {
            Administrator admin = new Administrator("Admin", "admin123", "555-0001", "123 Admin St", "admin@hospital.com");
            administrators.Add(admin);
            SaveAdministrators();

            Console.WriteLine("\n===== DEFAULT ADMIN CREATED =====");
            Console.WriteLine($"ID: {admin.ID}");
            Console.WriteLine("Password: admin123");
            Console.WriteLine("=================================\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void CreateSampleData()
        {
            // Create sample doctors
            Doctor doctor1 = new Doctor("Dr. Smith", "pass123", "555-1001", "456 Medical Ave", "smith@hospital.com", "Cardiology");
            Doctor doctor2 = new Doctor("Dr. Johnson", "pass123", "555-1002", "789 Health Blvd", "johnson@hospital.com", "Pediatrics");

            doctors.Add(doctor1);
            doctors.Add(doctor2);
            SaveDoctors();

            // Create sample patients
            Patient patient1 = new Patient("John Doe", "pass123", "555-2001", "321 Patient St", "john@email.com", "No major health issues");
            Patient patient2 = new Patient("Jane Smith", "pass123", "555-2002", "654 Patient Ave", "jane@email.com", "Allergic to penicillin");

            patient1.RegisteredDoctorID = doctor1.ID;
            patient2.RegisteredDoctorID = doctor2.ID;

            patients.Add(patient1);
            patients.Add(patient2);
            SavePatients();

            // Create sample receptionist (bonus feature)
            Receptionist receptionist1 = new Receptionist("Sarah Wilson", "pass123", "555-3001", "789 Reception St", "sarah@hospital.com", "Morning");
            receptionists.Add(receptionist1);
            SaveReceptionists();

            // Create sample appointments
            Appointment appointment1 = new Appointment(doctor1.ID, patient1.ID, "Regular checkup");
            Appointment appointment2 = new Appointment(doctor2.ID, patient2.ID, "Vaccination");

            appointments.Add(appointment1);
            appointments.Add(appointment2);
            SaveAppointments();

            Console.WriteLine("\n===== SAMPLE DATA CREATED =====");
            Console.WriteLine($"Doctor 1 - ID: {doctor1.ID}, Password: pass123");
            Console.WriteLine($"Doctor 2 - ID: {doctor2.ID}, Password: pass123");
            Console.WriteLine($"Patient 1 - ID: {patient1.ID}, Password: pass123");
            Console.WriteLine($"Patient 2 - ID: {patient2.ID}, Password: pass123");
            Console.WriteLine($"Receptionist - ID: {receptionist1.ID}, Password: pass123");
            Console.WriteLine("================================\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        #endregion
    }
}
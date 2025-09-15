using System;
using System.IO;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Services
{
    /// Simulates email functionality by writing to a file
    /// For bonus marks - Email functionality
    public static class EmailService
    {
        private static readonly string EmailLogFile = Path.Combine("Data", "email_log.txt");

        /// Send confirmation email when patient registers
        public static void SendPatientRegistrationEmail(Patient patient)
        {
            string emailContent = $@"
===== EMAIL NOTIFICATION =====
To: {patient.Email}
From: noreply@hospital.com
Subject: Registration Confirmation
Date: {DateTime.Now}

Dear {patient.Name},

Your registration at our hospital has been confirmed.
Your Patient ID is: {patient.ID}

Please keep this ID safe as you will need it to login.

Best regards,
Hospital Management System
==============================
";

            // Simulate sending email by writing to console and file
            Console.WriteLine("\n[EMAIL SENT] Registration confirmation sent to " + patient.Email);
            LogEmail(emailContent);
        }

        /// <summary>
        /// Send confirmation email when appointment is booked
        /// </summary>
        public static void SendAppointmentConfirmationEmail(Patient patient, Doctor doctor, Appointment appointment)
        {
            string emailContent = $@"
===== EMAIL NOTIFICATION =====
To: {patient.Email}
From: noreply@hospital.com
Subject: Appointment Confirmation
Date: {DateTime.Now}

Dear {patient.Name},

Your appointment has been successfully booked.

Appointment Details:
- Appointment ID: {appointment.ID}
- Doctor: {doctor.Name} ({doctor.Specialization})
- Description: {appointment.Description}
- Date: {appointment.AppointmentDate}

Please arrive 15 minutes before your appointment.

Best regards,
Hospital Management System
==============================
";

            // Simulate sending email
            Console.WriteLine("\n[EMAIL SENT] Appointment confirmation sent to " + patient.Email);
            LogEmail(emailContent);
        }

        /// <summary>
        /// Log email to file (simulating email sending)
        /// </summary>
        private static void LogEmail(string content)
        {
            try
            {
                // Ensure Data directory exists
                if (!Directory.Exists("Data"))
                {
                    Directory.CreateDirectory("Data");
                }

                // Append email to log file
                File.AppendAllText(EmailLogFile, content + "\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging email: {ex.Message}");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;
using HospitalManagementSystem.Utilities;

namespace HospitalManagementSystem
{
    /// <summary>
    /// Main program class - Entry point of the application
    /// </summary>
    class Program
    {
        // Demonstrates delegate for bonus marks
        public delegate void MenuAction();

        // Demonstrates anonymous method for bonus marks
        private static Action<string> logAction = delegate (string message)
        {
            Console.WriteLine($"[LOG] {DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}");
        };

        static void Main(string[] args)
        {
            try
            {
                // Set console title
                Console.Title = "Hospital Management System";

                // Initialize data manager
                ConsoleHelper.DisplayHeader("HOSPITAL MANAGEMENT SYSTEM");
                Console.WriteLine("Initializing system...");
                DataManager.Initialize();

                // Log initialization (demonstrates anonymous method)
                logAction("System initialized successfully");

                // Main application loop
                RunApplication();
            }
            catch (Exception ex)
            {
                ConsoleHelper.DisplayError($"Fatal error: {ex.Message}");
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
            finally
            {
                // Demonstrates garbage collection
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// Main application loop
        /// </summary>
        private static void RunApplication()
        {
            bool exitApplication = false;

            while (!exitApplication)
            {
                try
                {
                    DisplayLoginMenu();

                    // Get login credentials
                    Console.Write("Enter ID: ");
                    string idInput = Console.ReadLine();

                    if (!int.TryParse(idInput, out int id))
                    {
                        ConsoleHelper.DisplayError("Invalid ID format. Please enter a valid number.");
                        ConsoleHelper.PressAnyKeyToContinue();
                        continue;
                    }

                    Console.Write("Enter Password: ");
                    string password = ConsoleHelper.ReadPassword();

                    // Authenticate user
                    Person user = DataManager.Authenticate(id, password);

                    if (user != null)
                    {
                        ConsoleHelper.DisplaySuccess($"Login successful! Welcome {user.Name}");
                        logAction($"User {user.ID} logged in");
                        System.Threading.Thread.Sleep(1500);

                        // Display appropriate menu based on user type
                        user.DisplayMenu();
                    }
                    else
                    {
                        ConsoleHelper.DisplayError("Invalid ID or Password. Please try again.");
                        ConsoleHelper.PressAnyKeyToContinue();
                    }
                }
                catch (Exception ex)
                {
                    ConsoleHelper.DisplayError($"An error occurred: {ex.Message}");
                    ConsoleHelper.PressAnyKeyToContinue();
                }
            }
        }

        /// <summary>
        /// Display the login menu
        /// </summary>
        private static void DisplayLoginMenu()
        {
            Console.Clear();
            Console.WriteLine("\n╔════════════════════════════════════════╗");
            Console.WriteLine("║    HOSPITAL MANAGEMENT SYSTEM          ║");
            Console.WriteLine("║           LOGIN PORTAL                 ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");
            Console.WriteLine("Please enter your credentials to continue.\n");
        }
    }

    #region Bonus Features

    /// <summary>
    /// Generic class example for bonus marks
    /// </summary>
    public class GenericRepository<T> where T : class
    {
        private List<T> items = new List<T>();

        public void Add(T item)
        {
            items.Add(item);
        }

        public List<T> GetAll()
        {
            return new List<T>(items);
        }

        public T GetById(Func<T, bool> predicate)
        {
            return items.FirstOrDefault(predicate);
        }
    }

    #endregion
}
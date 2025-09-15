using System;

namespace HospitalManagementSystem.Models
{
    /// <summary>
    /// Base class for all people in the system (Patient, Doctor, Admin)
    /// Demonstrates inheritance principle
    /// </summary>
    public abstract class Person
    {
        // Properties
        public int ID { get; protected set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        // Constructor - demonstrates constructor principle
        public Person()
        {
            // ID will be set by derived classes
        }

        // Constructor overloading - demonstrates method overloading
        public Person(string name, string password, string phone, string address, string email)
        {
            Name = name;
            Password = password;
            Phone = phone;
            Address = address;
            Email = email;
        }

        // Abstract method - must be implemented by derived classes
        public abstract void DisplayMenu();

        // Virtual method - can be overridden by derived classes
        public virtual string ToString()
        {
            return $"ID: {ID} | Name: {Name} | Phone: {Phone}";
        }

        // Method for displaying details
        public virtual void DisplayDetails()
        {
            Console.WriteLine($"ID: {ID}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Phone: {Phone}");
            Console.WriteLine($"Address: {Address}");
            Console.WriteLine($"Email: {Email}");
        }
    }
}
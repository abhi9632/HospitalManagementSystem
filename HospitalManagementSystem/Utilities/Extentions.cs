using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalManagementSystem.Utilities
{
    /// <summary>
    /// Extension methods for the application
    /// Demonstrates extension method principle for marking criteria
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Extension method to check if a string is a valid email
        /// </summary>
        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Extension method to check if a string contains only digits
        /// </summary>
        public static bool IsNumeric(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;

            return str.All(char.IsDigit);
        }

        /// <summary>
        /// Extension method to truncate a string to specified length
        /// </summary>
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
        }

        /// <summary>
        /// Extension method for IEnumerable to check if collection is null or empty
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        /// <summary>
        /// Extension method to format phone numbers
        /// </summary>
        public static string FormatPhoneNumber(this string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return phone;

            // Remove all non-numeric characters
            string cleaned = new string(phone.Where(char.IsDigit).ToArray());

            // Format as XXX-XXX-XXXX if 10 digits
            if (cleaned.Length == 10)
            {
                return $"{cleaned.Substring(0, 3)}-{cleaned.Substring(3, 3)}-{cleaned.Substring(6)}";
            }

            return phone;
        }
    }
}
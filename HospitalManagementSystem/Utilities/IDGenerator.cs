using System;
using System.Collections.Generic;

namespace HospitalManagementSystem.Utilities
{
    /// <summary>
    /// Utility class for generating unique IDs
    /// </summary>
    public static class IDGenerator
    {
        private static Random random = new Random();
        private static HashSet<int> usedIDs = new HashSet<int>();

        /// <summary>
        /// Generates a unique ID between 10000 and 99999999 (5-8 digits)
        /// </summary>
        public static int GenerateID()
        {
            int id;
            do
            {
                // Generate a random number between 10000 (5 digits) and 99999999 (8 digits)
                id = random.Next(10000, 100000000);
            } while (usedIDs.Contains(id));

            usedIDs.Add(id);
            return id;
        }

        /// <summary>
        /// Register an ID as used (for loading from files)
        /// </summary>
        public static void RegisterID(int id)
        {
            usedIDs.Add(id);
        }

        /// <summary>
        /// Clear all registered IDs (useful for testing)
        /// </summary>
        public static void ClearRegisteredIDs()
        {
            usedIDs.Clear();
        }
    }
}
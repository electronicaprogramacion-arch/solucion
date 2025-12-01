using System;
using System.Collections.Generic;
using System.Linq;

namespace Helpers
{
    /// <summary>
    /// Utility class for sorting Dictionary<string, string> by values
    /// </summary>
    public static class DictionarySortingUtilities
    {
        /// <summary>
        /// Sorts a Dictionary<string, string> by values in ascending order (case-sensitive)
        /// Returns a List of KeyValuePair for guaranteed order preservation
        /// </summary>
        /// <param name="dictionary">The dictionary to sort</param>
        /// <returns>List of KeyValuePair sorted by values</returns>
        public static List<KeyValuePair<string, string>> SortByValues(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
                return new List<KeyValuePair<string, string>>();

            return dictionary
                .OrderBy(kvp => kvp.Value)
                .ToList();
        }

        /// <summary>
        /// Sorts a Dictionary<string, string> by values in ascending order (case-insensitive)
        /// Returns a List of KeyValuePair for guaranteed order preservation
        /// </summary>
        /// <param name="dictionary">The dictionary to sort</param>
        /// <returns>List of KeyValuePair sorted by values (case-insensitive)</returns>
        public static List<KeyValuePair<string, string>> SortByValuesIgnoreCase(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
                return new List<KeyValuePair<string, string>>();

            return dictionary
                .OrderBy(kvp => kvp.Value, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        /// <summary>
        /// Sorts a Dictionary<string, string> by values in descending order
        /// Returns a List of KeyValuePair for guaranteed order preservation
        /// </summary>
        /// <param name="dictionary">The dictionary to sort</param>
        /// <param name="ignoreCase">Whether to ignore case when sorting</param>
        /// <returns>List of KeyValuePair sorted by values in descending order</returns>
        public static List<KeyValuePair<string, string>> SortByValuesDescending(
            Dictionary<string, string> dictionary, 
            bool ignoreCase = false)
        {
            if (dictionary == null)
                return new List<KeyValuePair<string, string>>();

            var comparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
            
            return dictionary
                .OrderByDescending(kvp => kvp.Value, comparer)
                .ToList();
        }

        /// <summary>
        /// Sorts a Dictionary<string, string> by values and returns as IEnumerable
        /// Useful for LINQ operations and deferred execution
        /// </summary>
        /// <param name="dictionary">The dictionary to sort</param>
        /// <param name="ignoreCase">Whether to ignore case when sorting</param>
        /// <returns>IEnumerable of KeyValuePair sorted by values</returns>
        public static IEnumerable<KeyValuePair<string, string>> SortByValuesAsEnumerable(
            Dictionary<string, string> dictionary, 
            bool ignoreCase = false)
        {
            if (dictionary == null)
                return Enumerable.Empty<KeyValuePair<string, string>>();

            var comparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
            
            return dictionary.OrderBy(kvp => kvp.Value, comparer);
        }

        /// <summary>
        /// Sorts a Dictionary<string, string> by values and returns a new Dictionary
        /// Note: Regular Dictionary doesn't guarantee order, but insertion order is preserved in .NET Core 2.1+
        /// For guaranteed order preservation, consider using SortedDictionary or OrderedDictionary
        /// </summary>
        /// <param name="dictionary">The dictionary to sort</param>
        /// <param name="ignoreCase">Whether to ignore case when sorting</param>
        /// <returns>New Dictionary with entries sorted by values</returns>
        public static Dictionary<string, string> SortByValuesAsDictionary(
            Dictionary<string, string> dictionary, 
            bool ignoreCase = false)
        {
            if (dictionary == null)
                return new Dictionary<string, string>();

            var comparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
            
            return dictionary
                .OrderBy(kvp => kvp.Value, comparer)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        /// <summary>
        /// Sorts a Dictionary<string, string> by values and returns a SortedDictionary
        /// Note: SortedDictionary sorts by KEY, so this creates a new dictionary with sorted entries
        /// but the SortedDictionary itself will be sorted by the keys of the result
        /// </summary>
        /// <param name="dictionary">The dictionary to sort</param>
        /// <param name="ignoreCase">Whether to ignore case when sorting</param>
        /// <returns>Dictionary with entries sorted by values, then by keys</returns>
        public static Dictionary<string, string> SortByValuesThenByKeys(
            Dictionary<string, string> dictionary, 
            bool ignoreCase = false)
        {
            if (dictionary == null)
                return new Dictionary<string, string>();

            var comparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
            
            return dictionary
                .OrderBy(kvp => kvp.Value, comparer)
                .ThenBy(kvp => kvp.Key, comparer)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        /// <summary>
        /// Sorts multiple inner dictionaries within a nested dictionary structure
        /// Specifically designed for the grouped dictionaries from CreateGroupedDictionaryFromAppState
        /// </summary>
        /// <param name="groupedDictionary">The nested dictionary to sort</param>
        /// <param name="ignoreCase">Whether to ignore case when sorting</param>
        /// <returns>New nested dictionary with inner dictionaries sorted by values</returns>
        public static Dictionary<string, Dictionary<string, string>> SortGroupedDictionaryByValues(
            Dictionary<string, Dictionary<string, string>> groupedDictionary,
            bool ignoreCase = false)
        {
            if (groupedDictionary == null)
                return new Dictionary<string, Dictionary<string, string>>();

            var result = new Dictionary<string, Dictionary<string, string>>();

            foreach (var group in groupedDictionary)
            {
                result[group.Key] = SortByValuesAsDictionary(group.Value, ignoreCase);
            }

            return result;
        }

        /// <summary>
        /// Sorts both the outer dictionary (by group keys) and inner dictionaries (by values)
        /// </summary>
        /// <param name="groupedDictionary">The nested dictionary to sort</param>
        /// <param name="ignoreCase">Whether to ignore case when sorting</param>
        /// <returns>Fully sorted nested dictionary</returns>
        public static Dictionary<string, Dictionary<string, string>> SortGroupedDictionaryCompletely(
            Dictionary<string, Dictionary<string, string>> groupedDictionary,
            bool ignoreCase = false)
        {
            if (groupedDictionary == null)
                return new Dictionary<string, Dictionary<string, string>>();

            var comparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
            var result = new Dictionary<string, Dictionary<string, string>>();

            // Sort outer dictionary by keys, then sort inner dictionaries by values
            foreach (var group in groupedDictionary.OrderBy(kvp => kvp.Key, comparer))
            {
                result[group.Key] = SortByValuesAsDictionary(group.Value, ignoreCase);
            }

            return result;
        }
    }
}

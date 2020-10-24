using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public static class StringGrammar
    {
        private static string vowels = "aeiou";
        public static string BeginsWithVowel(string stringToCheck, bool appendToString = false)
        {
            string result = "";
            if (appendToString) result = $" {stringToCheck}";

            return vowels.ToCharArray().Any(
                v => v.ToString() == stringToCheck.Substring(0, 1).ToLower()) ? $"an{result}" : $"a{result}";
        }

        public static string BeginsWithVowelAndIsPlural(string stringToCheck, bool appendToString = false)
        {
            string result = "";
            if (appendToString) 
                if (stringToCheck.Substring(stringToCheck.Length - 1, 1) == "s")
                    return $"some{result}";

            return vowels.ToCharArray().Any(
                v => v.ToString() == stringToCheck.Substring(0, 1).ToLower()) ? $"an{result}" : $"a{result}";
        }

        public static string MakePlural(string stringToCheck, bool appendToString = false)
        {
            string result = "";
            if (appendToString) result = stringToCheck;
            return (stringToCheck.Substring(stringToCheck.Length - 1, 1) == "s") ? result : $"{result}s";
        }

        public static string CheckIfPlural(string stringToCheck, bool appendToString = false)
        {
            string result = "";
            if (appendToString) result = stringToCheck;
            return (stringToCheck.Substring(stringToCheck.Length - 1, 1) == "s") ? $"some {result}" : BeginsWithVowel(result,true);
        }
    }
}

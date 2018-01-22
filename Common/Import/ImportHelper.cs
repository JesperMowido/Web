using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Import
{
    public static class ImportHelper
    {
        public static decimal GetDecimalFromString(string input)
        {
            decimal result = 0m;

            if (!string.IsNullOrEmpty(input))
            {
                string inputResult = Regex.Replace(input, @"[^\d]", "");

                decimal.TryParse(inputResult, out result);
            }

            return result;
        }

        public static string GetPartOfString(string input, char splitChar, int index)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(input))
            {
                if (splitChar.ToString().Length > 0 && input.Contains(splitChar))
                {
                    result = input.Split(splitChar)[index].Trim().ToLower();
                    result = result.First().ToString().ToUpper() + result.Substring(1);
                }
                else
                {
                    result = input.First().ToString().ToUpper() + input.Substring(1).ToLower();
                }
            }

            return result;
        }

        public static string GetPartOfString(string input, string splitString, int index)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(input))
            {
                if (splitString.ToString().Length > 0 && input.Contains(splitString))
                {
                    result = input.Split( new string[] { splitString }, StringSplitOptions.None)[index].Trim().ToLower();
                    result = result.First().ToString().ToUpper() + result.Substring(1);
                }
                else
                {
                    result = input.First().ToString().ToUpper() + input.Substring(1).ToLower();
                }
            }

            return result;
        }

        public static int GetFirstIntFromString(string input)
        {
            int result = 0;

            if (!string.IsNullOrEmpty(input))
            {
                Regex reg = new Regex(@"([?:\d*])+");
                var matches = reg.Matches(input);
                string inputResult = "0";

                if (matches.Count > 0)
                {
                    inputResult = matches[0].Value;
                }

                int.TryParse(inputResult, out result);
            }

            return result;
        }

        public static string GetFirstString(string input)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(input))
            {
                Regex reg = new Regex(@"([a-zA-Z- ])+");
                var matches = reg.Matches(input);

                if (matches.Count > 0)
                {
                    result = matches[0].Value;
                }
            }

            return result;
        }

        public static int GetLivingSpace(string input)
        {
            int result = 0;

            if (!string.IsNullOrEmpty(input))
            {
                Regex reg = new Regex(@"([\d\*])+( m²)");
                var matches = reg.Matches(input);
                string inputResult = "0";

                if (matches.Count > 0)
                {
                    inputResult = ImportHelper.GetFirstIntFromString(matches[0].Value).ToString();
                }

                int.TryParse(inputResult, out result);
            }

            return result;
        }

        public static string GetUrlFromString(string input)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(input))
            {
                Regex reg = new Regex(@"\'.*?\'");
                var matches = reg.Matches(input);

                if (matches.Count > 0)
                {
                    result = matches[0].Value;
                }
            }

            return result;
        }
    }
}

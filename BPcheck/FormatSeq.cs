using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPcheck
{
    class FormatSeq
    {
        static private string Capitalize(char input) // Capitalize. Exceptions not thrown here
        {
            return Convert.ToChar((int)input - 32).ToString();
        }

        static public List<string> DelSpaceAndCap(string input) // Empty the spaces using Replace and capitalize
        {
            string temp = input.ToString().Replace(" ", string.Empty);
            List<string> result = new List<string>();
            for(int i = 0; i < temp.Length; i++)
            {
                if (temp[i]>=97 && temp[i]<=122)  // if a-z
                {
                    result.Add(Capitalize(temp[i]));
                }
                else
                {
                    result.Add(temp[i].ToString()); 
                }
                
            }
            return result;
        }

    }
}

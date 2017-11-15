using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPCheck
{
    class SeqParse
    {
        static public List<int> Parse(List<string> input) // Parse nucleotide into number
        {
            List<int> result = new List<int>(input.Count);
            for (int i =0; i < input.Count; i++)
            {
                switch (input[i])
                {
                    case "A":
                        result.Add(-1);
                        break;
                    case "T":
                        result.Add(1);
                        break;
                    case "C":
                        result.Add(-2);
                        break;
                    case "G":
                        result.Add(2);
                        break;
                    default:
                        result.Add(10);
                        break;
                        // zero is later used for breaking point **** caution
                }
            }
            return result;
        }

        static public List<int> Parse(char[] input) {
            List<int> result = new List<int>(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case 'A':
                        result.Add(-1);
                        break;
                    case 'T':
                        result.Add(1);
                        break;
                    case 'C':
                        result.Add(-2);
                        break;
                    case 'G':
                        result.Add(2);
                        break;
                    default:
                        result.Add(10);
                        break;
                        // zero is later used for breaking point **** caution
                }
            }
            return result;
        }
           
            

        static public string UnParse(List<int> input) // The inverse of Parse()
        {
            char[] result = new char[input.Count];

            for(int i = 0; i < input.Count; i++)
            {
                switch (input[i])
                {
                    case -1:
                        result[i] = 'A';
                        break;
                    case 1:
                        result[i] = 'T';
                        break;
                    case -2:
                        result[i] = 'C';
                        break;
                    case 2:
                        result[i] = 'G';
                        break;
                    case 0:
                        result[i] = '^';      // cutting point
                        break;
                    default:
                        result[i] = '?';      // unkown sequence entrypoint
                        break;
                }
            }
            return new string(result);

        }
    }
}

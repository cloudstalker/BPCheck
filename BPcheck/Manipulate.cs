using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPCheck
{
    /// <summary>
    /// A static class for manipulating ssDNA's
    /// </summary>
    public class Manipulate
    {

        /// <summary>
        /// Getting the reverse complementary of a given strand
        /// </summary>
        /// <param name="input">List of a ssDNA strand</param>
        /// <returns></returns>
        static public List<int> ReverseComp(List<int> input) // Reverse complementary
        {
            int L = input.Count;
            List<int> Result = new List<int>(L);
            for (int i = 0; i < L; i++)
            {
                Result.Add(-input[L-i-1]);
            }
            return Result;
            // List.Reverse() reverses whatever lists located in that shared memory
        }
        /// <summary>
        /// Cutting a strand into two at a given entry point (count from left)
        /// </summary>
        /// <param name="input">List of a ssDNA strand</param>
        /// <param name="entry">Entry point(from left)</param>
        /// <returns></returns>
        static public List<int>[] PickAndCut(List<int> input, int entry)
        {
            List<int>[] result = new List<int>[2];
            result[0] = input.GetRange(0, entry);
            result[1] = input.GetRange(entry, input.Count - entry);
            return result;
        }
        /// <summary>
        /// Cutting a strand into two at a given entry point (count from right)
        /// </summary>
        /// <param name="input">List of a ssDNA strand</param>
        /// <param name="entry">Entry point(from right)</param>
        /// <param name="option">Arbitrary string just for overloading</param>
        /// <returns></returns>
        static public List<int>[] PickAndCut(List<int> input, int entry, string option)
        {
            List<int>[] result = new List<int>[2];
            result[0] = input.GetRange(0, input.Count - entry);
            result[1] = input.GetRange(input.Count - entry, entry);
            return result;
        }
        /// <summary>
        /// Cutting a strand into three at two given entry points (count from left)
        /// </summary>
        /// <param name="input">List of a ssDNA strand</param>
        /// <param name="entry1">First entry point</param>
        /// <param name="entry2">Second entry point</param>
        /// <returns></returns>
        static public List<int>[] PickAndCut(List<int> input, int entry1, int entry2)
        {
            List<int>[] result = new List<int>[3];
            result[0] = input.GetRange(0, entry1);
            result[1] = input.GetRange(entry1, entry2 - entry1);
            result[2] = input.GetRange(entry2, input.Count - entry2);
            return result; 
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPcheck
{
    public class DsDna                 // requiring the two strand to be equal length. can extend in future ***
                                       // no exception thrown here
    {
#region Fields
        public bool IsComplementary;   // if fully self-complementary
        public bool IsDs;              // if a double stranded structure
        public bool IsSameLength;
            
        public float ComplemPercent;          // percentage of complementarity
        public List<int> MisMatch;                // indexes of mismatches
        public SsDna[] Strand                 // can be extended to triplex, but only duplex is inplemented here ***
        {
            get; set;
        }

#endregion

#region Constructors
        /// <summary>
        /// Contructor for a DsDNA
        /// </summary>
        /// <param name="strand">a length-2 ssDNA array</param>
        public DsDna(SsDna[] strand)
        {
            Strand = strand;
            IsDs = Strand.Length == 2;
            MisMatch = new List<int>();
            IsSameLength = strand[0].Sequence.Count == strand[1].Sequence.Count;
            CompCheck();
        }
#endregion

        private void CompCheck()              // set IsComplementary, ComplemPercent and Mismatch indices
        {
            SsDna[] temp = Strand;
            int L = temp[1].Sequence.Count;
            for (int i = 0; i < L; i++)
            {
                if (temp[0].Sequence[i] + temp[1].Sequence[L-1-i] != 0) // when not complementary
                {
                    MisMatch.Add(i);
                }
            }
            int countemp = MisMatch.Count;
            IsComplementary = countemp == 0;
            ComplemPercent = 1f - (float)countemp / temp[0].Sequence.Count;

        }
    }
}

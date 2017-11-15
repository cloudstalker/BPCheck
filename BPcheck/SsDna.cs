using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BPcheck
{
    /// <summary>
    /// ssDNA data type
    /// </summary>
    public class SsDna:INotifyPropertyChanged
    {
        /// <summary>
        /// For monitoring changing property
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private void NotifyPropertyChanged(string propertyName, string option)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
#region Fields
        private bool _IsChecked = false;
        private List<int> _Sequence = null;
        private string _Name = null;
        private float _GCcontent = -1f;
        private float _MW = 0f;
        private string _StrSequence = null;
        private int _Length;
        #endregion
#region Properties
        /// <summary>
        /// Whether or not the strand is checked in the list
        /// </summary>
        public bool IsChecked
        {
            get
            {
                return _IsChecked;
            }
            set
            {
                if (value != this._IsChecked)
                {
                    this._IsChecked = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Store the integer sequence of a strand
        /// </summary>
        public List<int> Sequence
        {
            get
            {
                return _Sequence;
            }
            set
            {
                if (value != this._Sequence)
                {
                    this._Sequence = value;
                    GCcontent = CalcGCcontent(Sequence);
                    MW = CalcMW(Sequence);
                    StrSequence = BPCheck.SeqParse.UnParse(Sequence);
                    Length = Sequence.Count;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// the length of the strand
        /// </summary>
        public int Length
        {
            get
            {
                return _Length;
            }
            set
            {
                _Length = value;
            }
        }

        /// <summary>
        /// the name of the strand
        /// </summary>
        public string Name
        {
            get {
                return this._Name;
            }
            set
            {
                if (value != this._Name)
                {
                    this._Name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// calculated GC percentage of the strand
        /// </summary>
        public float GCcontent
        {
            get
            {
                return this._GCcontent;
            }
            set
            {
                _GCcontent = value;
            }
        }

        /// <summary>
        /// calculated molecular weight of the strand
        /// </summary>
        public float MW
        {
            get
            {
                return this._MW;
            }
            set
            {
                _MW = value;
            }
        }

        /// <summary>
        /// the readable sequence for user
        /// </summary>
        public string StrSequence
        {
            get
            {
                return this._StrSequence;
            }
            set
            {
                if (value != this._StrSequence)
                {
                    this._StrSequence = value;
                    Sequence = BPCheck.SeqParse.Parse(StrSequence.ToCharArray());
                    _MW = CalcMW(Sequence);
                    _GCcontent = CalcGCcontent(Sequence);
                    _Length = Sequence.Count;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("MW", "");
                    NotifyPropertyChanged("GCcontent", "");
                    NotifyPropertyChanged("Length", "");
                }
            }
        }
        #endregion
#region Constructors
        /// <summary>
        /// Constructs a SsDNA object for later use
        /// </summary>
        /// <param name="sequence">A sequence list</param>
        public SsDna(List<int> sequence)
        {
            Sequence = sequence;
            _GCcontent = CalcGCcontent(Sequence);
            _MW = CalcMW(Sequence);
            _Length = Sequence.Count;
            StrSequence = BPCheck.SeqParse.UnParse(Sequence);
        }

#endregion
        private float CalcGCcontent(List<int> input)
        {
            int sum = 0;
            int L = input.Count;
            for(int i = 0; i < L; i++)
            {
                sum += Math.Abs(input[i]);
            }
            return (float)sum / L - 1f;
        }

        private float CalcMW(List<int> input) //  (An x 313.2) + (Tn x 304.2) + (Cn x 289.2) + (Gn x 329.2), in Dalton
        {
            return input.FindAll(ind => ind == -1).Count * 313.2f +
                input.FindAll(ind => ind == 1).Count * 304.2f +
                input.FindAll(ind => ind == -2).Count * 289.2f +
                input.FindAll(ind => ind == 2).Count * 329.2f;
        }
        /// <summary>
        /// Searching a specific subsequence inside the strand
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Returns the first index of the matching sequence(s)</returns>
        public List<int> Search(List<int> input)
        {
            int L = input.Count;
            List<int> result = new List<int>();
            for(int i=0; i<=this.Sequence.Count-L;i++)
            {
                if(this.Sequence.GetRange(i, L).SequenceEqual(input))
                {
                    result.Add(i);
                }
            }
            if (result.Count == 0)
            {
                result = null;
            }
            return result;
        }
    }
}

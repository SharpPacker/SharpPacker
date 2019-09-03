using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
//using System.Linq;

namespace SharpPacker.Models
{
    public class BoxList : IEnumerable<Box>
    {
        private readonly List<Box> _list = new List<Box>();
        private bool _isSorted = false;

        public void Insert(Box b)
        {
            _list.Add(b);
            _isSorted = false;
        }

        public IEnumerator<Box> GetEnumerator()
        {
            Sort();
            var listCopy = new List<Box>(_list);

            return listCopy.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void Sort()
        {
            if (!_isSorted)
            {
                _list.Sort(Compare);
                _isSorted = true;
            }
        }

        private int Compare(Box boxA, Box boxB)
        {
            // try smallest box first
            var volumeDecider = boxA.InnerVolume.CompareTo(boxB.InnerVolume);
            if (volumeDecider != 0)
            {
                return volumeDecider;
            }

            // with smallest empty weight
            var emptyWeightDecider = boxB.EmptyWeight.CompareTo(boxA.EmptyWeight);
            if(emptyWeightDecider != 0)
            {
                return emptyWeightDecider;
            }
            
            // maximum weight capacity as fallback decider
            return boxA.WeightCapacity.CompareTo(boxB.WeightCapacity);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
//using System.Linq;

namespace SharpPacker.Models
{
    public class BoxList : IEnumerable<Box>
    {
        private readonly List<Box> bList = new List<Box>();
        private bool isSoreted = false;

        public void Insert(Box b)
        {
            bList.Add(b);
            isSoreted = false;
        }

        public IEnumerator<Box> GetEnumerator()
        {
            if (!isSoreted)
            {
                bList.Sort(Compare);
            };

            var listCopy = new List<Box>(bList);

            return listCopy.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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

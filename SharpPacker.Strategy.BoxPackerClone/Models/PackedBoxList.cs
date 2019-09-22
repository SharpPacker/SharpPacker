using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Strategy.BoxPackerClone.Models
{
    public class PackedBoxList : IEnumerable<PackedBox>
    {
        private readonly List<PackedBox> _list = new List<PackedBox>();
        private bool _isSorted = false;

        public int Count => _list.Count;

        public void Insert(PackedBox pBox)
        {
            _list.Add(pBox);
            _isSorted = false;
        }

        public void InsertFromArray(PackedBox[] pBoxArray)
        {
            foreach(var pBox in pBoxArray)
            {
                Insert(pBox);
            }
        }

        public PackedBox Top()
        {
            Sort();
            if(Count > 0)
            {
                return _list[0];
            }

            return null;
        }

        /// <summary>
        /// Calculate the average (mean) weight of the boxes.
        /// </summary>
        /// <returns></returns>
        public float GetMeanWeight()
        {
            if(Count == 0)
            {
                return 0;
            }

            var totalWeight = 0;
            foreach(var pBox in _list)
            {
                totalWeight += pBox.TotalWeight;
            }

            return (float)totalWeight / Count;
        }

        /// <summary>
        /// Calculate the variance in weight between these boxes.
        /// </summary>
        /// <returns></returns>
        public double GetWeightVariance()
        {
            if(Count == 0)
            {
                return 0f;
            }

            var mean = GetMeanWeight();
            var weightVariance = 0d;
            foreach(var pBox in _list)
            {
                weightVariance += Math.Pow(pBox.TotalWeight - mean, 2);
            }

            return Math.Round(weightVariance / Count, 1);
        }

        /// <summary>
        /// Get volume utilisation of the set of packed boxes.
        /// </summary>
        /// <returns></returns>
        public float GetVolumeUtilisation()
        {
            var itemVolume = 0f;
            var boxVolume = 0f;

            foreach(var pBox in _list)
            {
                boxVolume += pBox.InnerVolume;
                foreach(var pItem in pBox.PackedItems)
                {
                    itemVolume += pItem.Volume;
                }
            }

            if(boxVolume == 0)
            {
                return 0f;
            }

            return (float)Math.Round((double)itemVolume / boxVolume * 100, 1);
        }

        private void Sort()
        {
            if (!_isSorted)
            {
                _list.Sort(Compare);
                _isSorted = true;
            }
        }

        private int Compare(PackedBox pBoxA, PackedBox pBoxB)
        {
            var choice = pBoxB.PackedItems.Count.CompareTo(pBoxA.PackedItems.Count);

            if(choice == 0)
            {
                choice = pBoxB.InnerVolume.CompareTo(pBoxA.InnerVolume);
            }
            if(choice == 0)
            {
                choice = pBoxA.TotalWeight.CompareTo(pBoxB.TotalWeight);
            }

            return choice;
        }

        public List<PackedBox> ToList()
        {
            Sort();
            var listCopy = new List<PackedBox>(_list);

            return listCopy;
        }

        public IEnumerator<PackedBox> GetEnumerator()
        {
            Sort();
            var listCopy = new List<PackedBox>(_list);

            return listCopy.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Models
{
    /// <summary>
    /// List of packed items, ordered by volume.
    /// </summary>
    public class PackedItemList : IEnumerable<PackedItem>
    {
        private readonly List<PackedItem> _list = new List<PackedItem>();
        private bool _isSorted = false;

        public int Count => _list.Count;

        public void Insert(PackedItem pItem)
        {
            _list.Add(pItem);
        }

        public List<Item> AsItemList()
        {
            var resultList = new List<Item>();
            foreach(var pItem in _list)
            {
                resultList.Add(pItem.Item);
            }

            return resultList;
        }

        private void Sort()
        {
            if (!_isSorted)
            {
                _list.Sort(Compare);
                _isSorted = true;
            }
        }

        private int Compare(PackedItem itemA, PackedItem itemB)
        {
            var volumeDecider = itemB.Volume.CompareTo(itemA.Volume);
            if (volumeDecider != 0)
            {
                return volumeDecider;
            }

            return itemB.Weight.CompareTo(itemA.Weight);
        }

        public IEnumerator<PackedItem> GetEnumerator()
        {
            Sort();
            var listCopy = new List<PackedItem>(_list);

            return listCopy.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}


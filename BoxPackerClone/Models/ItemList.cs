using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BoxPackerClone.Models
{
    public class ItemList : IEnumerable<Item>
    {
        private readonly List<Item> _list = new List<Item>();
        private bool _isSorted = false;

        public int Count => _list.Count;

        public void Insert(Item item)
        {
            _list.Add(item);
            _isSorted = false;
        }

        public void Remove(Item item)
        {
            if (_list.Contains(item))
            {
                _list.Remove(item);
                _isSorted = false;
            }
        }

        public ItemList Clone()
        {
            var cil = new ItemList();
            foreach(var i in _list)
            {
                cil.Insert(i);
            }

            return cil;
        }

        public Item Extract()
        {
            Sort();

            var lastIndex = _list.Count - 1;
            if(lastIndex >= 0)
            {
                var result = _list[lastIndex];
                _list.RemoveAt(lastIndex);

                return result;
            }
            else
            {
                return null;
            }
        }

        public Item Top()
        {
            Sort();

            var lastIndex = _list.Count - 1;
            if(lastIndex >= 0)
            {
                return _list[lastIndex];
            }

            return null;
        }

        public ItemList TopN(int n)
        {
            Sort();

            var lastIndex = _list.Count - 1;

            var result = new ItemList();
            for(var i = 0; i < n; i++)
            {
                var c = lastIndex - i;
                if(c < 0)
                {
                    break;
                }
                result.Insert(_list[c]);
            }

            return result;
        }

        public IEnumerator<Item> GetEnumerator()
        {
            Sort();
            var listCopy = new List<Item>(_list);
            listCopy.Reverse();

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

        private int Compare(Item itemA, Item itemB)
        {
            var volumeDecider = itemA.Volume.CompareTo(itemB.Volume);
            if (volumeDecider != 0)
            {
                return volumeDecider;
            }

            var weightDecider = itemA.Weight.CompareTo(itemB.Weight);
            if (weightDecider != 0)
            {
                return weightDecider;
            }

            return itemB.Description.CompareTo(itemA.Description);
        }
    }
}

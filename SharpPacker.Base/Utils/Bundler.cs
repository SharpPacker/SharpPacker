using SharpPacker.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SharpPacker.Base.Utils
{
    public static class Bundler
    {
        public static IEnumerable<ItemsBundle> GroupToBundles(IEnumerable<Item> itemsList)
        {
            return itemsList.GroupBy(item => item).Select(g => new ItemsBundle(g.Key, (uint)g.Count()));
        }
        public static IEnumerable<Item> UngroupBundles(IEnumerable<ItemsBundle> bundles)
        {
            var list = new List<Item>();

            foreach(var bundle in bundles)
            {
                for(var i = 0; i < bundle.Quantity; i++)
                {
                    list.Add(bundle.Item);
                }
            }

            return list;
        }
    }
}

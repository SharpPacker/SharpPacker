using SharpPacker.Models;
using System;
using System.Collections.Generic;

namespace SharpPacker
{
    public class Packer : IPacker<Item4d, Box4d, PackedBox4d>
    {
        public int MaxBoxesToBalanceWeight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void AddBox(Box4d box)
        {
            throw new NotImplementedException();
        }

        public void AddItem(Item4d item, int quantity = 1)
        {
            throw new NotImplementedException();
        }

        public List<PackedBox4d> DoVolumePacking()
        {
            throw new NotImplementedException();
        }

        public List<PackedBox4d> Pack()
        {
            throw new NotImplementedException();
        }

        public void SetBoxes(IEnumerable<Box4d> boxCollection)
        {
            throw new NotImplementedException();
        }

        public void SetItems(IEnumerable<Item4d> itemCollection)
        {
            throw new NotImplementedException();
        }
    }
}
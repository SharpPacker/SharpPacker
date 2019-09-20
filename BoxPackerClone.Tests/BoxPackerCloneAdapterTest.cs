using SharpPacker.Base.Models;
using SharpPacker.Base.DataTypes;
using System;
using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace BoxPackerClone.Adapter.Tests
{
    public class BoxPackerCloneAdapterTest
    {
        /// <summary>
        /// Where 2 perfectly filled boxes are a choice, need to ensure we pick the larger one or there is a cascading
        /// failure of many small boxes instead of a few larger ones.
        /// </summary>
        [Fact]
        public void TestIssue38()
        {
            var boxList = new List<BoxType>();
            var itemsList = new List<Item>();
            
            boxList.Add(new BoxType("Box1")
            {
                OuterDimensions = new Dimensions(2, 2, 2),
                EmptyWeight = 0,
                InnerDimensions = new Dimensions(2, 2, 2),
                MaxWeight = 1000,
            });

            boxList.Add(new BoxType("Box2")
            {
                OuterDimensions = new Dimensions(4, 4, 4),
                EmptyWeight = 0,
                InnerDimensions = new Dimensions(4, 4, 4),
                MaxWeight = 1000,
            });


            itemsList.Add(new Item("Item1")
            {
                Dimensions = new Dimensions(1, 1, 1),
                Weight = 100,
                AllowedRotations = RotationFlags.AllRotations,
            });
            itemsList.Add(new Item("Item2")
            {
                Dimensions = new Dimensions(1, 1, 1),
                Weight = 100,
                AllowedRotations = RotationFlags.AllRotations,
            });
            itemsList.Add(new Item("Item3")
            {
                Dimensions = new Dimensions(1, 1, 1),
                Weight = 100,
                AllowedRotations = RotationFlags.AllRotations,
            });
            itemsList.Add(new Item("Item4")
            {
                Dimensions = new Dimensions(1, 1, 1),
                Weight = 100,
                AllowedRotations = RotationFlags.AllRotations,
            });


            itemsList.Add(new Item("Item5")
            {
                Dimensions = new Dimensions(2, 2, 2),
                Weight = 100,
                AllowedRotations = RotationFlags.AllRotations,
            });
            itemsList.Add(new Item("Item6")
            {
                Dimensions = new Dimensions(2, 2, 2),
                Weight = 100,
                AllowedRotations = RotationFlags.AllRotations,
            });
            itemsList.Add(new Item("Item7")
            {
                Dimensions = new Dimensions(2, 2, 2),
                Weight = 100,
                AllowedRotations = RotationFlags.AllRotations,
            });
            itemsList.Add(new Item("Item8")
            {
                Dimensions = new Dimensions(2, 2, 2),
                Weight = 100,
                AllowedRotations = RotationFlags.AllRotations,
            });


            itemsList.Add(new Item("Item9")
            {
                Dimensions = new Dimensions(4, 4, 4),
                Weight = 100,
                AllowedRotations = RotationFlags.AllRotations,
            });

            var request = new BoxPackerRequest
            {
                Boxes = boxList,
                Items = itemsList
            };

            var packer = new BoxPackerCloneAdapter();
            packer.options.MaxBoxesToBalanceWeight = 12;
            var result = packer.Pack(request);

            Assert.Equal(2, result.PackedBoxes.Count());
        }
    }
}

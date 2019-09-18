using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpPacker.Base.DataTypes;
using sharp = SharpPacker.Base.Models;
using clone = BoxPackerClone.Models;
using SharpPacker.Base.Utils;

namespace BoxPackerCloneAdapter
{
    static class Convertors
    {
        public static clone.Box BoxToBox(sharp.BoxType bt)
        {
            return new clone.Box
                {
                    Reference = bt.Name,

                    EmptyWeight = bt.EmptyWeight,
                    MaxWeight = bt.MaxWeight,

                    InnerWidth = bt.InnerDimensions.sizeX,
                    InnerLength = bt.InnerDimensions.sizeY,
                    InnerDepth = bt.InnerDimensions.sizeZ,

                    OuterWidth = bt.OuterDimensions.sizeX,
                    OuterLength = bt.OuterDimensions.sizeY,
                    OuterDepth = bt.OuterDimensions.sizeZ,
                };
        }

        public static sharp.BoxType BoxToBox(clone.Box bt)
        {
            return new sharp.BoxType(bt.Reference)
            {
                MinItemsCount = uint.MinValue,
                MaxItemsCount = uint.MaxValue,

                InnerDimensions = new Dimensions(bt.InnerWidth, bt.InnerLength, bt.InnerDepth),
                OuterDimensions = new Dimensions(bt.OuterWidth, bt.OuterLength, bt.OuterDepth),

                EmptyWeight = bt.EmptyWeight,
                MaxWeight = bt.MaxWeight,
            };
        }


        public static clone.Item ItemToItem(sharp.Item item)
        {
            return new clone.Item
            {
                Description = item.Name,

                Width = item.Dimensions.sizeX,
                Length = item.Dimensions.sizeY,
                Depth = item.Dimensions.sizeZ,

                Weight = item.Weight,

                KeepFlat = (item.AllowedRotations == RotationFlags.DoNotTurnOver) || (item.AllowedRotations == RotationFlags.XYZ_to_XYZ),
            };
        }

        public static sharp.Item ItemToItem(clone.Item item)
        {
            return new sharp.Item(item.Description)
            {
                Dimensions = new Dimensions(item.Width, item.Length, item.Depth),
                Weight = item.Weight,
                AllowedRotations = item.KeepFlat ? RotationFlags.DoNotTurnOver : RotationFlags.AllRotations
            };
        }

        public static sharp.PackedItem PackedItemToPackedItem(clone.PackedItem pItem)
        {
            var originalDimensions = new Dimensions(pItem.Item.Width, pItem.Item.Length, pItem.Item.Depth);
            var rotatedDimensions = new Dimensions(pItem.Width, pItem.Length, pItem.Depth);

            return new sharp.PackedItem(ItemToItem(pItem.Item))
            {
                Position = new Position(pItem.X, pItem.Y, pItem.Z),
                Rotation = Rotator.GetRotation(originalDimensions, rotatedDimensions),
            };
        }
    }
}

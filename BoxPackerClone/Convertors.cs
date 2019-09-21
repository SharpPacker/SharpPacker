using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpPacker.Base.DataTypes;
using sharp = SharpPacker.Base.Models;
using clone = BoxPackerClone.Models;
using SharpPacker.Base.Utils;

namespace BoxPackerClone.Adapter
{
    static class Convertors
    { 
        public static int UintToInt(uint u) => (u > int.MaxValue) ? int.MaxValue : (int)u;

        public static clone.Box BoxToBox(sharp.BoxType bt)
        {
            return new clone.Box
                {
                    Reference = bt.Name,

                    EmptyWeight = UintToInt(bt.EmptyWeight),
                    MaxWeight = UintToInt(bt.MaxWeight),

                    InnerWidth = UintToInt(bt.InnerDimensions.sizeX),
                    InnerLength = UintToInt(bt.InnerDimensions.sizeY),
                    InnerDepth = UintToInt(bt.InnerDimensions.sizeZ),

                    OuterWidth = UintToInt(bt.OuterDimensions.sizeX),
                    OuterLength = UintToInt(bt.OuterDimensions.sizeY),
                    OuterDepth = UintToInt(bt.OuterDimensions.sizeZ),
                };
        }

        public static sharp.BoxType BoxToBox(clone.Box bt)
        {
            return new sharp.BoxType(bt.Reference)
            {
                MinItemsCount = uint.MinValue,
                MaxItemsCount = uint.MaxValue,

                InnerDimensions = new Dimensions((uint)bt.InnerWidth, (uint)bt.InnerLength, (uint)bt.InnerDepth),
                OuterDimensions = new Dimensions((uint)bt.OuterWidth, (uint)bt.OuterLength, (uint)bt.OuterDepth),

                EmptyWeight = (uint)bt.EmptyWeight,
                MaxWeight = (uint)bt.MaxWeight,
            };
        }


        public static clone.Item ItemToItem(sharp.Item item)
        {
            return new clone.Item
            {
                Description = item.Name,

                Width = UintToInt(item.Dimensions.sizeX),
                Length = UintToInt(item.Dimensions.sizeY),
                Depth = UintToInt(item.Dimensions.sizeZ),

                Weight = UintToInt(item.Weight),

                KeepFlat = (item.AllowedRotations == RotationFlags.DoNotTurnOver) || (item.AllowedRotations == RotationFlags.XYZ_to_XYZ),
            };
        }

        public static sharp.Item ItemToItem(clone.Item item)
        {
            return new sharp.Item(item.Description)
            {
                Dimensions = new Dimensions((uint)item.Width, (uint)item.Length, (uint)item.Depth),
                Weight = (uint)item.Weight,
                AllowedRotations = item.KeepFlat ? RotationFlags.DoNotTurnOver : RotationFlags.AllRotations
            };
        }

        public static sharp.PackedItem PackedItemToPackedItem(clone.PackedItem pItem)
        {
            var originalDimensions = new Dimensions((uint)pItem.Item.Width, (uint)pItem.Item.Length, (uint)pItem.Item.Depth);
            var rotatedDimensions = new Dimensions((uint)pItem.Width, (uint)pItem.Length, (uint)pItem.Depth);

            return new sharp.PackedItem(ItemToItem(pItem.Item))
            {
                Position = new Position(pItem.X, pItem.Y, pItem.Z),
                Rotation = Rotator.GetRotation(originalDimensions, rotatedDimensions),
            };
        }
    }
}

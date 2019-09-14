using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpPacker.Core.DataTypes;
using sharp = SharpPacker.Core.Models;
using clone = BoxPackerClone.Models;

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

                KeepFlat = (item.AllowedRotations.HasFlag(RotationFlags.XYZ_to_XYZ) || item.AllowedRotations.HasFlag(RotationFlags.XYZ_to_YXZ))
                            && !item.AllowedRotations.HasFlag(RotationFlags.XYZ_to_XZY)
                            && !item.AllowedRotations.HasFlag(RotationFlags.XYZ_to_YZX)
                            && !item.AllowedRotations.HasFlag(RotationFlags.XYZ_to_ZXY)
                            && !item.AllowedRotations.HasFlag(RotationFlags.XYZ_to_ZYX),
            };
        }

        public static sharp.Item ItemToItem(clone.Item item)
        {
            throw new NotImplementedException();

            return new sharp.Item(item.Description)
            {
                Dimensions = new Dimensions(item.Width, item.Length, item.Depth),
                Weight = item.Weight,
            };
        }

        public static sharp.PackedItem PackedItemToPackedItem(clone.PackedItem pItem)
        {
            throw new NotImplementedException();

            return new sharp.PackedItem(ItemToItem(pItem.Item))
            {
            };
        }
    }
}

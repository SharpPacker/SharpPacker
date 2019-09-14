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

        public static clone.Item ItemToItem(sharp.Item item)
        {
            return new clone.Item
            {
                Description = item.Name,

                Width = item.Dimensions.sizeX,
                Length = item.Dimensions.sizeY,
                Depth = item.Dimensions.sizeZ,

                Weight = item.Weight,

                KeepFlat = (item.AllowedRotations.Length == 1 && item.AllowedRotations[0] == Rotation.XYZ_to_XYZ)
                            || (item.AllowedRotations.Length == 2
                                && item.AllowedRotations.Contains(Rotation.XYZ_to_XYZ)
                                && item.AllowedRotations.Contains(Rotation.XYZ_to_YXZ)),
            };
        }
    }
}

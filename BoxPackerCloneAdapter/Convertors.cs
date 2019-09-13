using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpPacker.Core.DataTypes;

namespace BoxPackerCloneAdapter
{
    static class Convertors
    {
        public static BoxPackerClone.Models.Box BoxToBox(SharpPacker.Core.Models.BoxType bt)
        {
            return new BoxPackerClone.Models.Box
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

        public static BoxPackerClone.Models.Item ItemToItem(SharpPacker.Core.Models.Item item)
        {
            return new BoxPackerClone.Models.Item
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

using SharpPacker.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SharpPacker.Services
{
    /// <summary>
    /// Applies load stability to generated result.
    /// </summary>
    class LayerStabiliser
    {
        public List<PackedLayer> Stabilise(IEnumerable<PackedLayer> packedLayers)
        {
            // first re-order according to footprint
            var stabilizedLayers = new List<PackedLayer>();

            packedLayers.OrderBy(l => l);
            var currentZ = 0;
            foreach(var oldZLayer in packedLayers)
            {
                var oldZStart = oldZLayer.GetStartDepth();

                var newZLayer = new PackedLayer();
                foreach(var oldZItem in oldZLayer.Items)
                {
                    var newZ = oldZItem.Z - oldZStart + currentZ;
                    var newZItem = new PackedItem4d()
                    {
                        Item = oldZItem.Item,
                        X = oldZItem.X,
                        Y = oldZItem.Y,
                        Z = oldZItem.Z,
                    };
                }
                stabilizedLayers.Add(newZLayer);
                currentZ += newZLayer.GetDepth();
            }

            return stabilizedLayers;
        }
    }
}

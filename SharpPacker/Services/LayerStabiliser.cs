using SharpPacker.Models;
using System.Collections.Generic;
using System.Linq;

namespace SharpPacker.Services
{
    /// <summary>
    /// Applies load stability to generated result.
    /// </summary>
    internal class LayerStabiliser
    {
        public List<PackedLayer> Stabilise(IEnumerable<PackedLayer> packedLayers)
        {
            // first re-order according to footprint
            var stabilizedLayers = new List<PackedLayer>();

            packedLayers.OrderBy(l => l);

            var currentZ = 0;
            foreach (var oldZLayer in packedLayers)
            {
                var oldZStart = oldZLayer.GetStartDepth();
                var newZLayer = new PackedLayer();

                foreach (var oldZItem in oldZLayer.Items)
                {
                    var newZ = oldZItem.Z - oldZStart + currentZ;
                    var newZItem = new PackedItem()
                    {
                        Item = oldZItem.Item,
                        Width = oldZItem.Width,
                        Length = oldZItem.Length,
                        Depth = oldZItem.Depth,
                        X = oldZItem.X,
                        Y = oldZItem.Y,
                        Z = newZ,
                    };
                    newZLayer.Items.Add(newZItem);
                }
                stabilizedLayers.Add(newZLayer);
                currentZ += newZLayer.GetDepth();
            }

            return stabilizedLayers;
        }
    }
}

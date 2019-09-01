using SharpPacker.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpPacker.Helpers
{
    internal static class PackedBoxListHelpers
    {
        /// <summary>
        /// Calculate the average (mean) weight of the boxes.
        /// </summary>
        /// <param name="pBoxList"></param>
        /// <returns></returns>
        public static double GetMeanWeight(IEnumerable<PackedBox> pBoxList)
        {
            return pBoxList.Average(b => b?.TotalWeight) ?? 0d;
        }

        /// <summary>
        /// Get volume utilisation of the set of packed boxes.
        /// </summary>
        /// <param name="pBoxList"></param>
        /// <returns></returns>
        public static double GetVolumeUtilizationPercent(IEnumerable<PackedBox> pBoxList)
        {
            var itemsVolume = 0f;
            var boxesVolume = 0f;

            foreach (var pBox in pBoxList)
            {
                boxesVolume += pBox.InnerVolume;
                foreach (var pItem in pBox.PackedItems)
                {
                    itemsVolume += pItem.Volume;
                }
            }

            if (boxesVolume == 0)
            {
                return 0;
            }

            return (100 * (double)itemsVolume / boxesVolume);
        }

        /// <summary>
        /// Calculate the variance in weight between these boxes.
        /// </summary>
        /// <param name="pBoxList"></param>
        /// <returns></returns>
        public static double GetWeightVariance(IEnumerable<PackedBox> pBoxList)
        {
            var boxCount = pBoxList.Count();

            if (boxCount == 0)
            {
                return 0;
            }

            var mean = GetMeanWeight(pBoxList);
            var variance = pBoxList.Sum(pBox => Math.Pow(pBox.TotalWeight - mean, 2));

            var meanVariance = variance / boxCount;

            return meanVariance;
        }
    }
}

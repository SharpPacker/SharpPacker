using SharpPacker.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SharpPacker.Helpers
{
    static class PackedBoxListHelpers
    {
        /// <summary>
        /// Calculate the average (mean) weight of the boxes.
        /// </summary>
        /// <param name="pBoxList"></param>
        /// <returns></returns>
        public static double GetMeanWeight(IEnumerable<PackedBox4d> pBoxList)
        {
            return pBoxList.Average(b => b?.TotalWeight) ?? 0d;
        }

        /// <summary>
        /// Calculate the variance in weight between these boxes.
        /// </summary>
        /// <param name="pBoxList"></param>
        /// <returns></returns>
        public static double GetWeightVariance(IEnumerable<PackedBox4d> pBoxList)
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

        /// <summary>
        /// Get volume utilisation of the set of packed boxes.
        /// </summary>
        /// <param name="pBoxList"></param>
        /// <returns></returns>
        public static double GetVolumeUtilizationPercent(IEnumerable<PackedBox4d> pBoxList)
        {
            var itemsVolume = 0;
            var boxesVolume = 0;

            foreach(var pBox in pBoxList)
            {
                boxesVolume += pBox.InnerVolume;
                foreach(var pItem in pBox.PackedItems)
                {
                    itemsVolume += pItem.Volume;
                }
            }

            if(boxesVolume == 0)
            {
                return 0;
            }

            return (100 * (double)itemsVolume / boxesVolume);
        }
    }
}

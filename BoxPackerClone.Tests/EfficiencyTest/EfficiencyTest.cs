using BoxPackerClone.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using System.Linq;
using System.Globalization;

namespace BoxPackerClone.Tests.EfficiencyTest
{
    public class Expected
    {
        public int boxes;
        public double weightVariance;
        public float utilisation;
    }

    public class ItemData
    {
        public int qty;
        public string name;
        public int width;
        public int length;
        public int depth;
        public int weight;
    }

    public class EfficiencyTest
    {
        [Theory]
        [MemberData(nameof(DataGenerator.GetCases_TestCanPackRepresentativeLargerSamples), MemberType = typeof(DataGenerator))]
        public void TestCanPackRepresentativeLargerSamples2D(string testId, BoxList boxes, List<ItemData> itemsData, Expected expected2D, Expected expected3D)
        {
            var expectedItemCount = 0;

            var packer = new Packer();

            foreach(var box in boxes)
            {
                packer.AddBox(box);
            }

            foreach (var iData in itemsData)
            {
                expectedItemCount += iData.qty;

                packer.AddItem(Factory.CreateItem(
                            iData.name,
                            iData.width,
                            iData.length,
                            iData.depth,
                            iData.weight,
                            true
                        ), iData.qty
                    );
            }

            var packedBoxes = packer.Pack();

            var packedItemCount = packedBoxes.ToList().Sum(pb => pb?.PackedItems?.Count ?? 0);

            Assert.Equal(expected2D.boxes, packedBoxes.Count);
            Assert.Equal(expectedItemCount, packedItemCount);
            Assert.Equal(expected2D.utilisation, packedBoxes.GetVolumeUtilisation());
            Assert.Equal(expected2D.weightVariance, packedBoxes.GetWeightVariance());
        }

        [Theory]
        [MemberData(nameof(DataGenerator.GetCases_TestCanPackRepresentativeLargerSamples), MemberType = typeof(DataGenerator))]
        public void TestCanPackRepresentativeLargerSamples3D(string testId, BoxList boxes, List<ItemData> itemsData, Expected expected2D, Expected expected3D)
        {
            var expectedItemCount = 0;

            var packer = new Packer();

            foreach (var box in boxes)
            {
                packer.AddBox(box);
            }

            foreach (var iData in itemsData)
            {
                expectedItemCount += iData.qty;

                packer.AddItem(Factory.CreateItem(
                            iData.name,
                            iData.width,
                            iData.length,
                            iData.depth,
                            iData.weight,
                            true
                        ), iData.qty
                    );
            }

            var packedBoxes3D = packer.Pack();

            var packedItemCount3D = packedBoxes3D.ToList().Sum(pb => pb?.PackedItems?.Count ?? 0);

            Assert.Equal(expected3D.boxes, packedBoxes3D.Count);
            Assert.Equal(expectedItemCount, packedItemCount3D);
            Assert.Equal(expected3D.utilisation, packedBoxes3D.GetVolumeUtilisation());
            Assert.Equal(expected3D.weightVariance, packedBoxes3D.GetWeightVariance());
        }
    }

    public class DataGenerator
    {
        public static IEnumerable<object[]> GetCases_TestCanPackRepresentativeLargerSamples()
        {
            var expected2DDict = new Dictionary<string, Expected>();
            var expected3DDict = new Dictionary<string, Expected>();

            var boxes = new BoxList();

            var itemsDict = new Dictionary<string, List<ItemData>>();

            string[] expectedLines = File.ReadAllLines("./EfficiencyTest/Data/expected.csv");
            string[] boxesLines = File.ReadAllLines("./EfficiencyTest/Data/boxes.csv");
            string[] itemsLines = File.ReadAllLines("./EfficiencyTest/Data/items.csv");

            foreach (var expectedLine in expectedLines)
            {
                string[] data = expectedLine.Split(",");
                var key = data[0];
                expected2DDict.Add(key, new Expected
                {
                    boxes = int.Parse(data[1]),
                    weightVariance = double.Parse(data[2], CultureInfo.InvariantCulture),
                    utilisation = float.Parse(data[3], CultureInfo.InvariantCulture)
                }
                                );
                expected3DDict.Add(key, new Expected
                {
                    boxes = int.Parse(data[4]),
                    weightVariance = double.Parse(data[5], CultureInfo.InvariantCulture),
                    utilisation = float.Parse(data[6], CultureInfo.InvariantCulture)
                }
                );
            }

            foreach (var boxLine in boxesLines)
            {
                string[] data = boxLine.Split(",");

                boxes.Insert(Factory.CreateBox(
                        data[0],
                        int.Parse(data[1]),
                        int.Parse(data[2]),
                        int.Parse(data[3]),
                        int.Parse(data[4]),
                        int.Parse(data[5]),
                        int.Parse(data[6]),
                        int.Parse(data[7]),
                        int.Parse(data[8])
                    )
                );
            }

            foreach (var itemLine in itemsLines)
            {
                string[] data = itemLine.Split(",");
                var key = data[0];

                if (!itemsDict.ContainsKey(key))
                {
                    itemsDict.Add(key, new List<ItemData>());
                }

                itemsDict[key].Add(new ItemData
                {
                    qty = int.Parse(data[1]),
                    name = data[2],
                    width = int.Parse(data[3]),
                    length = int.Parse(data[4]),
                    depth = int.Parse(data[5]),
                    weight = int.Parse(data[6])
                }
                );
            }

            foreach (var kv in itemsDict)
            {
                yield return new object[] {
                    kv.Key,
                    boxes,
                    itemsDict[kv.Key],
                    expected2DDict[kv.Key],
                    expected3DDict[kv.Key]
                };
            }
        }
    }
}

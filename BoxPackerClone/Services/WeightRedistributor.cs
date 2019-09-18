using BoxPackerClone.Models;
using System.Collections.Generic;
using System.Linq;

namespace BoxPackerClone.Services
{
    internal class WeightRedistributor
    {
        private readonly BoxList boxes;

        public WeightRedistributor(BoxList _boxes)
        {
            boxes = _boxes;
        }

        /// <summary>
        /// Given a solution set of packed boxes, repack them to achieve optimum weight distribution.
        /// </summary>
        /// <param name="originalBoxes"></param>
        /// <returns></returns>
        public PackedBoxList RedistributeWeight(PackedBoxList originalBoxes)
        {
            var targetWeight = originalBoxes.GetMeanWeight();

            var redistrebutedBoxes = originalBoxes.ToList();
            redistrebutedBoxes.OrderByDescending(box => box.TotalWeight);

            var iterationSuccessful = false;

            do
            {
                iterationSuccessful = false;

                var a = redistrebutedBoxes.Count;
                while(a - 1 >= 0)
                {
                    a -= 1;
                    var boxA = redistrebutedBoxes[a];

                    var b = redistrebutedBoxes.Count;
                    while (b - 1 >= 0)
                    {
                        b -= 1;
                        var boxB = redistrebutedBoxes[b];

                        if(b <= a || boxA.TotalWeight == boxB.TotalWeight)
                        {
                            continue; //no need to evaluate
                        }

                        iterationSuccessful = EqualiseWeight(ref boxA, ref boxB, targetWeight);
                        redistrebutedBoxes[a] = boxA;
                        redistrebutedBoxes[b] = boxB;

                        if (iterationSuccessful)
                        {
                            //remove any now-empty boxes from the list
                            redistrebutedBoxes = redistrebutedBoxes
                                .Where(box => box != null)
                                .OrderByDescending(box => box.TotalWeight)
                                .ToList();

                            goto LEAVE_LOOPS;
                        }
                    }
                }

            LEAVE_LOOPS:; 
            } while (iterationSuccessful);

            //Combine back into a single list
            var packedBoxes = new PackedBoxList();
            packedBoxes.InsertFromArray(redistrebutedBoxes.ToArray());

            return packedBoxes;
        }

        /// <summary>
        /// Not every attempted repack is actually helpful - sometimes moving an item between two
        /// otherwise identical boxes, or sometimes the box used for the now lighter set of items
        /// actually weighs more when empty causing an increase in total weight.
        /// </summary>
        /// <returns></returns>
        private bool DidRepackActuallyHelp(PackedBox oldBoxA, PackedBox oldBoxB, PackedBox newBoxA, PackedBox newBoxB)
        {
            PackedBoxList oldList = new PackedBoxList();
            oldList.InsertFromArray(new PackedBox[] { oldBoxA, oldBoxB});

            PackedBoxList newList = new PackedBoxList();
            newList.InsertFromArray(new PackedBox[] { newBoxA, newBoxB });

            return newList.GetWeightVariance() < oldList.GetWeightVariance();
        }

        /// <summary>
        /// Do a volume repack of a set of items.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private PackedBoxList DoVolumeRepack(IEnumerable<Item> items)
        {
            return DoVolumeRepack(items, null);
        }

        /// <summary>
        /// Do a volume repack of a set + one of items.
        /// </summary>
        /// <param name="originalItems"></param>
        /// <param name="plusOneItem"></param>
        /// <returns></returns>
        private PackedBoxList DoVolumeRepack(IEnumerable<Item> originalItems, Item plusOneItem)
        {
            var packer = new Packer();
            packer.SetBoxes(this.boxes);
            packer.SetItems(originalItems);
            if (plusOneItem != null)
            {
                packer.AddItem(plusOneItem);
            }

            return packer.DoVolumePacking();
        }

        /// <summary>
        /// Attempt to equalise weight distribution between 2 boxes
        /// </summary>
        /// <param name="boxA"></param>
        /// <param name="boxB"></param>
        /// <param name="targetWeight"></param>
        /// <returns>was the weight rebalanced?</returns>
        private bool EqualiseWeight(ref PackedBox boxA, ref PackedBox boxB, double targetWeight)
        {
            var anyIterationSuccessful = false;

            var overWeightBox = (boxA.TotalWeight > boxB.TotalWeight) ? boxA : boxB;
            var underWeightBox = (boxA.TotalWeight > boxB.TotalWeight) ? boxB : boxA;

            var overWeightBoxItems = overWeightBox.PackedItems.AsItemList();
            var underWeightBoxItems = underWeightBox.PackedItems.AsItemList();

            var key = overWeightBoxItems.Count;
            while(key - 1 >= 0)
            {
                key--;
                var overWeightItem = overWeightBoxItems[key];

                //TODO: check algorithm logic - why there is direct boxB using instead of over/underWeightBox?
                if (overWeightItem.Weight + boxB.TotalWeight > targetWeight)
                {
                    // moving this item would harm more than help
                    continue;
                }
                var newLighterBoxes = DoVolumeRepack(underWeightBoxItems, overWeightItem);
                if(newLighterBoxes.Count != 1)
                {
                    //only want to move this item if it still fits in a single box
                    continue;
                }

                underWeightBoxItems.Add(overWeightItem);
                if(overWeightBoxItems.Count == 1)
                {
                    //sometimes a repack can be efficient enough to eliminate a box
                    boxB = newLighterBoxes.Top();
                    boxA = null;

                    return true;
                }

                overWeightBoxItems.RemoveAt(key);
                var newHeavierBoxes = DoVolumeRepack(overWeightBoxItems);
                if(newHeavierBoxes.Count != 1)
                {
                    continue;
                }

                if (DidRepackActuallyHelp(boxA, boxB, newHeavierBoxes.Top(), newLighterBoxes.Top()))
                {
                    boxB = newLighterBoxes.Top();
                    boxA = newHeavierBoxes.Top();
                    anyIterationSuccessful = true;
                }
            }

            return anyIterationSuccessful;
        }
    }
}

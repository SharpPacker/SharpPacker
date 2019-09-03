using SharpPacker.Models;
using System.Collections.Generic;

namespace SharpPacker
{
    internal interface IPacker
    {
        /// <summary>
        /// Number of boxes at which balancing weight is deemed not worth the extra computation time.
        /// </summary>
        int MaxBoxesToBalanceWeight { get; set; }

        /// <summary>
        /// Add box size.
        /// </summary>
        /// <param name="box"></param>
        void AddBox(Box box);

        /// <summary>
        /// Add a pre-prepared set of boxes all at once.
        /// </summary>
        /// <param name="boxCollection"></param>
        void SetBoxes(BoxList boxCollection);

        /// <summary>
        /// Add item to be packed.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        void AddItem(Item item, int quantity = 1);
        
        /// <summary>
        /// Set a list of items all at once.
        /// </summary>
        /// <param name="itemCollection"></param>
        void SetItems(ItemList itemCollection);

        /// <summary>
        /// Pack items into boxes using the principle of largest volume item first.
        /// </summary>
        /// <returns></returns>
        PackedBoxList DoVolumePacking();

        /// <summary>
        /// Pack items into boxes.
        /// </summary>
        /// <returns></returns>
        PackedBoxList Pack();
        
    }
}

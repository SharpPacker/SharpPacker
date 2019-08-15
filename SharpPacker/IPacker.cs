using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker
{
    interface IPacker<TItem, TBox, TPackedBox>
    {
        /// <summary>
        /// Add item to be packed.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        void AddItem(TItem item, int quantity = 1);

        /// <summary>
        /// Set a list of items all at once.
        /// </summary>
        /// <param name="itemCollection"></param>
        void SetItems(IEnumerable<TItem> itemCollection);

        /// <summary>
        /// Add box size.
        /// </summary>
        /// <param name="box"></param>
        void AddBox(TBox box);

        /// <summary>
        /// Add a pre-prepared set of boxes all at once.
        /// </summary>
        /// <param name="boxCollection"></param>
        void SetBoxes(IEnumerable<TBox> boxCollection);

        /// <summary>
        /// Number of boxes at which balancing weight is deemed not worth the extra computation time.
        /// </summary>
        int MaxBoxesToBalanceWeight { get; set; }

        /// <summary>
        /// Pack items into boxes.
        /// </summary>
        /// <returns></returns>
        List<TPackedBox> Pack();

        /// <summary>
        /// Pack items into boxes using the principle of largest volume item first.
        /// </summary>
        /// <returns></returns>
        List<TPackedBox> DoVolumePacking();
    }
}

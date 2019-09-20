using System;
using System.Collections.Generic;
using System.Text;
using BoxPackerClone.Exceptions;
using BoxPackerClone.Models;

namespace BoxPackerClone
{
    /// <summary>
    /// A version of the packer that swallows internal exceptions.
    /// </summary>
    class InfalliblePacker : Packer
    {
        public readonly ItemList _unpackedItems;

        public InfalliblePacker() : base()
        {
            _unpackedItems = new ItemList();
        }

        public override PackedBoxList Pack()
        {
            var il = this._items.Clone();
            while(true){
                try
                {
                    return base.Pack();
                } catch(ItemTooLargeException ex)
                {
                    var item = (Item)ex.Data["item"];
                    _unpackedItems.Insert(item);
                    _items.Remove(item);
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}

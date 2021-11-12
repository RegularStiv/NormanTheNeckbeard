using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NormanTheNeckbeard
{
    class InventoryManager
    {
        // Fields

        private List<Item> items;



        // Constructor

        public InventoryManager()
        {
            items = new List<Item>();
        }



        // Methods

        public void AddItem(Item newItem)
        {
            items.Add(newItem);
        }

        public bool UseItem(string itemName)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Name == itemName)
                {
                    //found and removes item
                    //obviously we need the item to give its effects to the player but that'll need a whole system
                    items.RemoveAt(i);
                    return true;
                }
            }

            //it didn't find an item with that name
            return false;
        }
        
    }
}

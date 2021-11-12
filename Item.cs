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
    class Item : GameObject
    {
        // Fields

        private string name;



        // Properties

        public string Name
        {
            get { return name; }
        }



        // Constructor

        public Item(Rectangle position, Rectangle hitBox, Texture2D texture, string name)
            : base(position, hitBox, texture)
        {
            this.name = name;
        }



        // Methods

        public void Update()
        {
            // These don't move and are "collected" elsewhere so nothing to see here
        }

    }
}

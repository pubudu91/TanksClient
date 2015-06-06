using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Core;

namespace GameEntity
{
    class BrickCell : Cell
    {
        private DamageLevel damage;

        public DamageLevel damageLevel
        {
            get
            {
                return damage;
            }

            set
            {
                switch (value)
                {
                    case DamageLevel.DAMAGE0:
                        texture = Util.Content.Load<Texture2D>("brick_1");
                        break;
                    case DamageLevel.DAMAGE25:
                        texture = Util.Content.Load<Texture2D>("brick_1_25");
                        break;
                    case DamageLevel.DAMAGE50:
                        texture = Util.Content.Load<Texture2D>("brick_1_50");
                        break;
                    case DamageLevel.DAMAGE75:
                        texture = Util.Content.Load<Texture2D>("brick_1_75");
                        break;
                    case DamageLevel.DAMAGE100:
                        texture = Util.Content.Load<Texture2D>("brick_1_100"); // TODO: add the texture of a blank cell here
                        break;
                }

                damage = value;
            }
        }

        public BrickCell()
        {
            isPassable = false;
            texture = Util.Content.Load<Texture2D>("brick_1");
        }

        public override void Draw()
        {

        }
    }
}

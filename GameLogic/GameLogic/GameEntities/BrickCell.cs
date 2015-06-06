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
                /*switch (value)
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
                }*/

                damage = value;
            }
        }

        public BrickCell()
        {
            F = -1;
            G = 0;
            H = 0;
            priority = 1;
            isPassable = false;
            texture = Util.Content.Load<Texture2D>("brick_1");
        }

        public override void Draw()
        {
            //Util.sprite.Draw(texture, new Vector2(10 + (x * Util.scale * 64), 10 + (y * Util.scale * 64)), null, Color.White, 0.0f, new Vector2(0, 0), Util.scale, SpriteEffects.None, 0);
            Vector2 position = Vector2.Zero;
            Vector2 texturePosition = new Vector2(x * 30, y * 30) + position;

            //Here you would typically index to a Texture based on the textureId.
            Util.sprite.Draw(texture, texturePosition, null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0f); 
        }
    }
}

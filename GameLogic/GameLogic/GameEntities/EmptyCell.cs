using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Core;

namespace GameEntity
{
    public class EmptyCell : Cell
    {
        public EmptyCell()
        {
            texture = Util.Content.Load<Texture2D>("sand");
            isPassable = false;
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

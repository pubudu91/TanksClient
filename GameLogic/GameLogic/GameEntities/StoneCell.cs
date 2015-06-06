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
    public class StoneCell : Cell
    {
        public StoneCell()
        {
            F = -1;
            G = 0;
            H = 0;
            priority = 2;
            texture = Util.Content.Load<Texture2D>("stone");
            isPassable = false;
        }

        public override void Draw()
        {
            Vector2 position = Vector2.Zero;
            Vector2 texturePosition = new Vector2(x * 30, y * 30) + position;

            //Here you would typically index to a Texture based on the textureId.
            Util.sprite.Draw(texture, texturePosition, null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0f); 
        }
    }
}

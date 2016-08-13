using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Green
{
    class Goo : Sprite
    {
        private float speed;

        public Goo(Texture2D texture, Vector2 position, Vector2 scale)
            : base(texture, position, scale)
        {
            speed = 150f;
        }

        public void Update(GameTime time)
        {
            float elapsed = (float)time.ElapsedGameTime.TotalSeconds;
            Position += new Vector2(0, speed * elapsed);
        }

        public void Kill()
        {
            IsAlive = false;
        }
    }
}

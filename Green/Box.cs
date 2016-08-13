using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Green
{
    class Box : Sprite
    {
        private float speed;
        public bool IsBigBox { get; private set; }
        

        public Box(Texture2D texture, Vector2 position,bool isBigBox, Vector2 scale)
            : base(texture,position,scale)
        {
            speed = 80f;
            this.IsBigBox = isBigBox;
        }

        public void Update(GameTime time)
        {
            float elapsed = (float)time.ElapsedGameTime.TotalSeconds;
            Position += new Vector2(speed * elapsed, 0);
        }

        public void Kill()
        {
            IsAlive = false;
        }
    }
}

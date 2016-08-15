using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Green
{
    class Box : Sprite
    {
        private float speed;
        public bool IsBigBox { get; private set; }
        public bool Filled { get; set; }

        public Rectangle HitBox
        {
            get
            {
                return new Rectangle((int)Position.X + (3 * (int)scale.X), 
                    (int)Position.Y + (3 * (int)scale.Y), 
                    10 * (int)scale.X,
                    6 * (int)scale.Y);
            }
        }


        public Box(Texture2D texture, Vector2 position,bool isBigBox, Vector2 scale)
            : base(texture,position,scale)
        {
            speed = 50f;
            IsBigBox = isBigBox;
            Filled = false;
        }

        public new void Update(GameTime time)
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

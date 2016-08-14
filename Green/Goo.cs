using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Green
{
    class Goo : Sprite
    {
        private float speed;
        public int Charges { get; private set; }

        public Goo(Texture2D texture, Vector2 position, Vector2 scale, int charges)
            : base(texture, position, scale)
        {
            speed = 150f;
            this.Charges = charges;
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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Green
{
    class Human : Sprite
    {
        private float speed;
        private Vector2 direction;

        public Human(Texture2D texture, Vector2 position, Vector2 scale)
            : base(texture,position,scale)
        {
            speed = 20f;
            direction = new Vector2(1, 0);
        }

        public new void Update(GameTime time)
        {
            float elapsed = (float)time.ElapsedGameTime.TotalSeconds;
            if (Position.X > 192)
            {
                direction = new Vector2(0, 1);
                speed = 100f;
            }
            Position += speed * direction * elapsed;
        }

        public void Kill()
        {
            IsAlive = false;
        }
    }
}

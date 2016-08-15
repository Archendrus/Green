using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Green
{
    // Base sprite class
    // Has a texture position, width, and height
    // IsAlive flag if active
    // Multiple draw methods
    class Sprite
    {
        private Texture2D texture;

        public Vector2 Position { get; set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        // Animated sprite
        private float frameSpeed;
        private float time;
        private int frames;
        private int frameIndex;
        private int frameWidth;
        private int frameHeight;
        private Rectangle sourceRect;

        protected Vector2 scale;

        // Calculated Vector2 for center of sprite
        public Vector2 Center
        {
            get
            {
                return new Vector2(Position.X + (Width / 2), Position.Y + (Height / 2));
            }
        }

        public bool IsAlive { get; set; }

        // Rectangle set at size of texture
        public Rectangle BoundingRect
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }

        // Animated Sprite
        public Sprite(Texture2D texture, Vector2 position, Vector2 scale,  int frames, int frameWidth, int frameHeight, float frameSpeed)
        {
            this.texture = texture;
            this.scale = scale; 
            Position = position * scale;

            this.frames = frames;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.frameSpeed = frameSpeed;
            frameIndex = 0;

            // Calculate actual sprite size
            Height = texture.Height * (int)scale.Y;
            Width = texture.Width * (int)scale.X;

            IsAlive = true;          
        }

        // Create sprite with texture
        // Calculate sprite dimensions based on scale
        // set to alive on creation
        // sprite will have no position and will not be drawn
        public Sprite(Texture2D texture, Vector2 scale)
        {
            this.texture = texture;
            this.scale = scale;
            Position = Vector2.Zero;
            Width = texture.Width * (int)scale.X;
            Height = texture.Height * (int)scale.Y;
            IsAlive = true;
        }

        public Sprite(Texture2D texture, Vector2 position, Vector2 scale)
        {
            this.texture = texture;
            this.scale = scale;
            Position = position * scale;

            // Calculate actual sprite size
            Height = texture.Height * (int)scale.Y;
            Width = texture.Width * (int)scale.X;

            IsAlive = true;
        }

        public void ChangeTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (time > frameSpeed)
            {
                frameIndex++;
                time = 0f;
            }


            if (frameIndex > frames - 1)
            {
                frameIndex = 0;
            }
            sourceRect = new Rectangle(frameIndex * frameWidth, 0, frameWidth, frameHeight);


        }


        // Draw sprite at position at Game1.Scale
        public void Draw(SpriteBatch spriteBatch)
        {
            // Only draw if Sprite.IsAlive
            if (IsAlive)
            {
                if(frames == 0)
                {
                    spriteBatch.Draw(
                        texture,
                        new Vector2((int)Position.X, (int)Position.Y),
                        null,
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        scale,
                        SpriteEffects.None,
                        0.0f);
                }
                else
                {
                    spriteBatch.Draw(
                        texture,
                        new Vector2((int)Position.X, (int)Position.Y),
                        sourceRect,
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        scale,
                        SpriteEffects.None,
                        0.0f);
                }

            }

        }

        // Draw sprite at position, tint with color
        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            // Only draw if Sprite.IsAlive
            if (IsAlive)
            {
                spriteBatch.Draw(
                    texture,
                    new Vector2((int)Position.X, (int)Position.Y),
                    null,
                    color,
                    0.0f,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    0.0f);
            }
            
        }

        // Draw sprite at position with alpha
        public void Draw(SpriteBatch spriteBatch, float alpha)
        {
            // Only draw if Sprite.IsAlive
            if (IsAlive)
            {
                spriteBatch.Draw(
                    texture,
                    new Vector2((int)Position.X, (int)Position.Y),
                    null,
                    Color.White * alpha,
                    0.0f,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    0.0f);
            }
        }
        
    }
}

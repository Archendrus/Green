using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Green
{
    class HumanManager
    {
        public List<Human> Humans { get; private set; } // List of humans
        Texture2D humanTexture;

        float humanTimerElapsed;

        public HumanManager(ContentManager content)
        {
            Humans = new List<Human>();
            humanTexture = content.Load<Texture2D>("Human");

        }

        public void Update(GameTime time)
        {
            MakeHumans(time);
            for (int i = 0; i < Humans.Count; i++)
            {
                Humans[i].Update(time);
                if (Humans[i].Position.Y > 192)
                {
                    Humans[i].Kill();
                    Humans.Remove(Humans[i]);
                }
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Humans.Count; i++)
            {
                Humans[i].Draw(spriteBatch);
            }
        }

        public void MakeHumans(GameTime time)
        {
            float spawnRate = 2f;
            humanTimerElapsed += (float)time.ElapsedGameTime.TotalSeconds;
            if (humanTimerElapsed > spawnRate)
            {
                Humans.Add(new Human(humanTexture, new Vector2(0,96), new Vector2(1, 1)));

                humanTimerElapsed = 0;
            }
        }
    }
}

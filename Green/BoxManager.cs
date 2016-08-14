using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Green
{
    class BoxManager
    {
        public List<Box> Boxes { get; private set; } // List of boxes

        Texture2D smallBoxTexture;
        Texture2D smallBoxFull;
        Texture2D smallBoxHalf;
        Texture2D smallBoxOver;

        Texture2D bigBoxTexture;
        Texture2D bigBoxFull;
        Texture2D bigBoxHalf;
        Texture2D bigBoxOver;

        Rectangle screen;

        Random randomGen;

        Vector2 scale;

        float boxTimerElapsed;

        public BoxManager(ContentManager content, Vector2 scale, Rectangle screen)
        {
            Boxes = new List<Box>();
            this.screen = screen;
            this.scale = scale;

            randomGen = new Random();

            smallBoxTexture = content.Load<Texture2D>("smallBox");
            smallBoxFull = content.Load<Texture2D>("smallBoxFull");
            smallBoxHalf = content.Load<Texture2D>("smallBoxHalf");
            smallBoxOver = content.Load<Texture2D>("smallBoxOver");

            bigBoxTexture = content.Load<Texture2D>("bigBox");
            bigBoxFull = content.Load<Texture2D>("BigBoxFull");
            bigBoxHalf = content.Load<Texture2D>("BigBoxHalf");
            bigBoxOver = content.Load<Texture2D>("BigBoxOver");

        }

        public void Update(GameTime time, ref int score)
        {
            MakeBoxes(time);
            for (int i = 0; i < Boxes.Count; i++)
            {
                Boxes[i].Update(time);
                if (Boxes[i].Position.X > screen.Right)
                {
                    if (Boxes[i].Filled)
                    {
                        score++;
                    }
                    Boxes[i].Kill();
                    Boxes.Remove(Boxes[i]);
                }

            }

        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            for (int i = 0; i < Boxes.Count; i++)
            {
                Boxes[i].Draw(spriteBatch);
                // DEBUG
                //spriteBatch.Draw(pixel, Boxes[i].HitBox, Color.White);
            }
        }

        private void MakeBoxes(GameTime time)
        {
            float boxRate = 2f;
            boxTimerElapsed += (float)time.ElapsedGameTime.TotalSeconds;

            if (boxTimerElapsed > boxRate)
            {
                bool isBigBox;
                Texture2D newTexture;
                Vector2 newPosition;
                float boxChance = .5f;
                float roll = (float)randomGen.NextDouble();
                if (roll < boxChance)
                {
                    isBigBox = false;
                    newTexture = smallBoxTexture;
                    newPosition = new Vector2(0, 144);
                }
                else
                {
                    isBigBox = true;
                    newTexture = bigBoxTexture;
                    newPosition = new Vector2(0, 128);
                }

                Boxes.Add(new Box(newTexture, newPosition, isBigBox, scale));
                boxTimerElapsed = 0;
            }

        }

        public void FillBox(Box box, int chargeCount)
        {
            if (box.IsBigBox)
            {
                if (chargeCount == 4)
                {
                    box.ChangeTexture(bigBoxFull);
                    box.Filled = true;
                }
                else if (chargeCount < 4)
                {
                    box.ChangeTexture(bigBoxHalf);
                }
                else // chargeCount > 4
                {
                    box.ChangeTexture(bigBoxOver);
                }
            }
            else
            {
                if (chargeCount == 2)
                {
                    box.ChangeTexture(smallBoxFull);
                    box.Filled = true;
                }
                else if (chargeCount < 2)
                {
                    box.ChangeTexture(smallBoxHalf);
                }
                else // chargeCount > 2
                {
                    box.ChangeTexture(smallBoxOver);
                }
            }

        }
    }
}

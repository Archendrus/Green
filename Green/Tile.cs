﻿using Microsoft.Xna.Framework;

namespace Green
{
    // Single tile
    // Has width, height, and IsSolid flag
    // Stores source rectangle to draw from tile sheet
    class Tile
    {
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        public Rectangle SourceRectangle { get; private set; } // Source rectangle from tile sheet
        public Vector2 Position { get; set; }
        public bool IsSolid { get; private set; }
        Vector2 scale;

        // Rectangle set at size of texture
        public Rectangle BoundingRect
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }

        // Create new tile using texture at sourceRectangle of tile sheet,
        // set at position, set solid state to isSolid
        public Tile(Rectangle sourceRectangle, Vector2 position, Vector2 scale, bool isSolid)
        {
            SourceRectangle = sourceRectangle;
            Position = position;
            IsSolid = isSolid;
            this.scale = scale;

            // Tile size is always 16 (size of actual asset) * scale
            Width = 16 * (int)scale.X; 
            Height = 16 * (int)scale.Y;
        }
    }
}

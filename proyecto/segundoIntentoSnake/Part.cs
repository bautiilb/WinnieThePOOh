using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace segundoIntentoSnake
{
    internal class Part
    {
        SnakePartType type;
        Vector2 position;
        char direction = 'T';

        public SnakePartType Type { get { return type; } set { type = value; } }
        public Vector2 Position { get { return position; } set { position = value; } }
        public char Direction { get { return direction; } set { direction = value; } }
        public enum SnakePartType
        {
            HeadHorizontalRight,
            HeadVerticalUp,
            HeadHorizontalLeft,
            HeadVerticalDown,
            BodyHorizontal,
            BodyVertical,
            BodyCornerTopRight,
            BodyCornerTopLeft,
            BodyCornerBottomRight,
            BodyCornerBottomLeft,
            TailHorizontalRight,
            TailVerticalUp,
            TailHorizontalLeft,
            TailVerticalDown
        }
        Dictionary<SnakePartType, Rectangle> snakeParts = new Dictionary<SnakePartType, Rectangle>
        {
            { SnakePartType.HeadHorizontalRight, new Rectangle(256, 0, 64, 64) },
            { SnakePartType.HeadVerticalUp, new Rectangle(192, 0, 64, 64) },
            { SnakePartType.HeadHorizontalLeft, new Rectangle(192, 64, 64, 64) },
            { SnakePartType.HeadVerticalDown, new Rectangle(256, 64, 64, 64) },
            { SnakePartType.BodyHorizontal, new Rectangle(64, 0, 64, 64) },
            { SnakePartType.BodyVertical, new Rectangle(128, 64, 64, 64) },
            { SnakePartType.BodyCornerTopRight, new Rectangle(128, 0, 64, 64) },
            { SnakePartType.BodyCornerTopLeft, new Rectangle(0, 0, 64, 64) },
            { SnakePartType.BodyCornerBottomRight, new Rectangle(128, 128, 64, 64) },
            { SnakePartType.BodyCornerBottomLeft, new Rectangle(0, 64, 64, 64) },
            { SnakePartType.TailHorizontalRight, new Rectangle(256, 128, 64, 64) },
            { SnakePartType.TailVerticalUp, new Rectangle(192, 128, 64, 64) },
            { SnakePartType.TailHorizontalLeft, new Rectangle(192, 192, 64, 64) },
            { SnakePartType.TailVerticalDown, new Rectangle(256, 192, 64, 64) },
        };

        public Rectangle RectanglePart()
        {
            if (snakeParts.TryGetValue(type, out Rectangle rectangle))
                return rectangle;
            else
                throw new ArgumentOutOfRangeException();
        }
    }
}

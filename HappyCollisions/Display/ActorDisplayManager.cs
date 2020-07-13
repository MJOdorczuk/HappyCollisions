using HappyCollisions.Actors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace HappyCollisions.Display
{
    class ActorDisplayManager
    {
        public void Display(IActor actor, Graphics graphics, PointF focus, float dx, float dy)
        {
            var relativeX = (actor.X - focus.X) / dx;
            var relativeY = (actor.Y - focus.Y) / dy;
            switch (actor)
            {
                case PointActor pointActor:

                    break;
            }
            graphics.FillEllipse(new SolidBrush(Color.DarkRed), 
                                 (int)relativeX - 5, 
                                 (int)relativeY - 5, 
                                 10, 
                                 10);
        }
    }
}

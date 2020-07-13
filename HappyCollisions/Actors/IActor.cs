using System;
using System.Collections.Generic;
using System.Text;

namespace HappyCollisions.Actors
{
    public interface IActor
    {
        double X { get; set; }
        double Y { get; set; }
        double VX { get; set; }
        double VY { get; set; }
    }
}

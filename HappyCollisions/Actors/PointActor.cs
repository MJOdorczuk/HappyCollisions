using System;
using System.Collections.Generic;
using System.Text;

namespace HappyCollisions.Actors
{
    class PointActor : IActor
    {
        private double x;
        private double y;
        private double vx;
        private double vy;

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public double VX { get => vx; set => vx = value; }
        public double VY { get => vy; set => vy = value; }

        public PointActor(double x, double y, double vx, double vy)
        {
            this.x = x;
            this.y = y;
            this.vx = vx;
            this.vy = vy;
        }
    }
}

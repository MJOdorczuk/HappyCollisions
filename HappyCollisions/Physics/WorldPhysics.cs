using HappyCollisions.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HappyCollisions.Physics
{
    class WorldPhysics
    {
        private static readonly double G_CONSTANT = 10.0f;

        private readonly List<IActor> actors = new List<IActor>();

        public void Tick(double dt = 0.01)
        {
            MoveActors(dt);
            ApplyGlobalAcceleration(dt);
        }

        public void AddActor(IActor actor)
        {
            this.actors.Add(actor);
        }

        public List<IActor> Actors { get => actors; }

        private void ApplyGlobalAcceleration(double dt)
        {
            actors.ForEach(actor => actor.VY += G_CONSTANT * dt);
        }

        private void MoveActors(double dt)
        {
            actors.ForEach(actor => { actor.X += actor.VX * dt; actor.Y += actor.VY * dt; });
        }
    }
}

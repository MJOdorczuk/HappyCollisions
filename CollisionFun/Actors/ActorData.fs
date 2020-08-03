module Actors.ActorData

open OpenTK
open Interfaces

type ActorData(position : Vector2d, velocity : Vector2d) =
    let mutable position : Vector2d = position
    let mutable velocity : Vector2d = velocity
    new(position : Vector2d) = ActorData(position, Vector2d(0.0, 0.0))
    interface IActorData with
        member __.Accelerate(delta: Vector2d): unit = 
            velocity <- velocity + delta
        member __.Move(delta: Vector2d): unit = 
            position <- position + delta
        member __.Position: Vector2d = 
            position
        member __.Velocity: Vector2d = 
            velocity
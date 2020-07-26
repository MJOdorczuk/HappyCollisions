module Actors.PointActor

open OpenTK
open Interfaces

type public PointActor (position : Vector2d, velocity : Vector2d) =
    let mutable position : Vector2d = position
    let mutable velocity : Vector2d = velocity
    interface IActor with
        member __.Position: Vector2d = 
            position
        member __.Velocity: Vector2d = 
            velocity
        member __.Accelerate(a: Vector2d): unit = 
            velocity <- velocity + a
        member __.Move (delta : Vector2d) : unit =
            position <- position + delta
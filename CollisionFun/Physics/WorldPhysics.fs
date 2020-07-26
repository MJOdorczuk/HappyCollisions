module Physics.WorldPhysics

open OpenTK
open Interfaces
open System

type CustomWorldPhysics () =
    let mutable actors : IActor list = []
    let GRAVITY_CONSTANT : Vector2d = Vector2d(0.0, -0.0001)
    let moveActors (delta : float) : unit =
        actors
        |> List.map (fun actor -> actor.Move (actor.Velocity * delta))
        |> ignore
    let applyGravity (delta : float) : unit =
        actors
        |> List.map (fun actor -> actor.Accelerate (GRAVITY_CONSTANT * delta))
        |> ignore
    interface IWorldPhysics with
        member __.AddActor(actor: IActor): unit = 
            actors <- actor::actors
        member __.RemoveActor(actor: IActor): unit = 
            actors <- actors
                      |> List.filter (fun a -> a <> actor)
        member __.Tick(delta: float): unit = 
            applyGravity delta
            moveActors delta
        member __.Actors : IActor list =
            actors
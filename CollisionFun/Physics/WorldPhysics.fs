module Physics.WorldPhysics

open OpenTK
open Interfaces
open Actors.Operations

type CustomWorldPhysics () =
    let mutable actors : Actor list = []
    let GRAVITY_CONSTANT : Vector2d = Vector2d(0.0, -0.01)
    let moveActors (delta : float) : unit =
        actors
        |> List.map ActorsData
        |> List.map (fun data -> data.Move (data.Velocity * delta))
        |> ignore
    let applyGravity (delta : float) : unit =
        actors
        |> List.map (AccelerateActor (GRAVITY_CONSTANT * delta))
        |> ignore
    interface IWorldPhysics with
        member __.AddActor (actor : Actor): unit = 
            actors <- actor::actors
        member __.RemoveActor (actor: Actor): unit = 
            actors <- actors
                      |> List.filter (fun a -> a <> actor)
        member __.Tick(delta: float): unit = 
            applyGravity delta
            moveActors delta
        member __.Actors : Actor list =
            actors
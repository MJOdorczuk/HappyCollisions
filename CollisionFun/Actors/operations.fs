module Actors.Operations

open Interfaces
open OpenTK
open ActorData

let CreatePointActor (velocity : Vector2d) (position : Vector2d) : Actor =
    (position, velocity)
    |> ActorData
    :> IActorData
    |> PointActor

let CreateTriangleActor (velocity : Vector2d) (a : Vector2d) (b : Vector2d) (c : Vector2d) : Actor =
    let midpoint = (a + b + c) / 3.0
    let data =  
        (midpoint, velocity)
        |> ActorData
        :> IActorData
    TriangleActor (data, a - midpoint, b - midpoint, c - midpoint)

let ActorsData (actor : Actor) : IActorData =
    match actor with
    | PointActor data -> data
    | TriangleActor (data, _, _, _) -> data

let MoveActor (delta : Vector2d) (actor : Actor) : unit =
    let data = ActorsData actor in
    data.Move delta

let AccelerateActor (delta : Vector2d) (actor : Actor) : unit =
    let data = ActorsData actor in
    data.Accelerate delta

let PositionOfActor (actor : Actor) : Vector2d =
    let data = ActorsData actor in
    data.Position

let VelocityOfActor (actor : Actor) : Vector2d =
    let data = ActorsData actor in
    data.Velocity
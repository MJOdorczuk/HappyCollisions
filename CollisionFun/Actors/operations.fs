module Actors.Operations

open Interfaces
open OpenTK
open ActorData

let CreatePointActor (velocity : Vector2d) (position : Vector2d) : Actor =
    (position, velocity)
    |> ActorData
    :> IActorData
    |> PointActor

let CreatePolygonActor (velocity : Vector2d) (points : Vector2d list) : Actor =
    let midpoint = 
        points
        |> List.fold (+) (Vector2d(0.0, 0.0))
        |> (*) (1.0 / float points.Length)
    let data =
        (midpoint, velocity)
        |> ActorData
        :> IActorData
    PolygonActor (data, points |> List.map (fun point -> point - midpoint))

let ActorsData (actor : Actor) : IActorData =
    match actor with
    | PointActor data -> data
    | PolygonActor (data, _) -> data

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
module Drawing.ActorDrawing

open OpenTK
open Interfaces
open Drawing.Graphics
open Utils
open Actors.Operations

let (>>=) = useAndPass in

let DrawActor (camera : ICamera) (actor : Actor) : unit =
    actor
    |> Points
    |> List.map WorldPoint
    |> List.map camera.ProjectToDisplay
    >>= (List.map (FillCircle Color.Green 5.0))
    |> DrawLineLoop Color.Green
module Drawing.ActorDrawing

open OpenTK
open Interfaces
open Drawing.Graphics
open Utils

let (>>=) = useAndPass in

type CustomActorDisplayer (camera : ICamera) =
    interface IActorDisplayer with
        member __.Draw(actor: Actor): unit = 
            match actor with
            | PointActor data ->
                data.Position
                |> WorldPoint
                |> camera.ProjectToDisplay
                |> FillCircle Color.Red 5.0
            | TriangleActor (data, a, b, c) ->
                [a; b; c]
                |> List.map (fun point -> point + data.Position)
                |> List.map WorldPoint
                |> List.map camera.ProjectToDisplay
                >>= (List.map (FillCircle Color.Green 5.0))
                |> DrawLineLoop Color.Green
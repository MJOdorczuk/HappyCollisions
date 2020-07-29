module Drawing.ActorDrawing

open OpenTK
open Interfaces
open Actors.PointActor
open OpenTK.Graphics.OpenGL
open Drawing.Graphics

type CustomActorDisplayer (camera : ICamera) =
    interface IActorDisplayer with
        member __.Draw(actor: IActor): unit = 
            match actor with
            | :? PointActor -> 
                actor.Position
                |> WorldPoint
                |> camera.ProjectToDisplay
                |> FillCircle Color.Red <| 500.0
            | _ -> ()
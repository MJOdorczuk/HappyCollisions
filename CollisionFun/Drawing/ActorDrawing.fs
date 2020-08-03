module Drawing.ActorDrawing

open OpenTK
open Interfaces
open Drawing.Graphics

type CustomActorDisplayer (camera : ICamera) =
    interface IActorDisplayer with
        member __.Draw(actor: Actor): unit = 
            match actor with
            | PointActor data ->
                data.Position
                |> WorldPoint
                |> camera.ProjectToDisplay
                |> FillCircle Color.Red 500.0
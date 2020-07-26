module Drawing.ActorDrawing

open OpenTK
open Interfaces
open Actors.PointActor
open OpenTK.Graphics.OpenGL

type CustomActorDisplayer (camera : ICamera) =
    interface IActorDisplayer with
        member __.Draw(actor: IActor): unit = 
            match actor with
            | :? PointActor -> 
                do GL.Color4 Color.Red
                do GL.PointSize 5.0f
                do GL.Enable EnableCap.PointSmooth
                do GL.Begin PrimitiveType.Points
                actor.Position
                |> WorldPoint
                |> camera.ProjectToDisplay
                |> GL.Vertex2
                do GL.End ()
            | _ -> ()
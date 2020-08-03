module IOManager.Input.MouseInput

open OpenTK
open Interfaces
open Actors.ActorData

let mapMouseOnDisplay (data : IApplicationData) (point : Point) : Vector2d =
    let width = float data.WindowBoundaries.Width
    let height = float data.WindowBoundaries.Height
    let x = 2.0 * float point.X / width - 1.0
    let y = 1.0 - 2.0 * float point.Y / height
    Vector2d(x, y)

let middleMouseButtonDown (data : IApplicationData) (point : Vector2d) : unit =
    match data.InputMode with
    | StandardControl ->
        do point
           |> DisplayPoint
           |> data.Camera.SaveAnchor
        data.InputMode <- CameraPan
    | _ -> ()

let middleMouseButtonUp (data : IApplicationData) (point : Vector2d) : unit =
    match data.InputMode with
    | CameraPan -> 
        data.InputMode <- StandardControl
    | _ -> ()

let leftMouseButtonDown (data : IApplicationData) (point : Vector2d) : unit =
    match data.InputMode with
    | StandardControl ->
        point
        |> DisplayPoint
        |> data.Camera.ProjectToWorld
        |> ActorData
        :> IActorData
        |> PointActor
        |> data.Physics.AddActor
    | _ -> ()

let mouseMove (data : IApplicationData) (point : Vector2d) : unit =
    match data.InputMode with
    | CameraPan ->
        do point
           |> DisplayPoint
           |> data.Camera.Pan
    | _ -> ()

let wheelMove (data : IApplicationData) (point : Vector2d) (factor : float) : unit =
    match data.InputMode with
    | StandardControl ->
        data.Camera.Zoom factor (WorldPoint point)
    | _ -> ()
module IOManager.Input.MouseInput

open OpenTK
open Interfaces
open Actors.ActorData
open Actors.Operations

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
    let point = point
                |> DisplayPoint
                |> data.Camera.ProjectToWorld
    match data.InputMode with
    | StandardControl ->
        let zeroVelocity = Vector2d(0.0, 0.0)
        match data.BuildMode with
        | Point -> 
            point
            |> CreatePointActor zeroVelocity
            |> data.Physics.AddActor
        | Triangle -> data.BuildMode <- Triangle1 point
        | Triangle1 a -> data.BuildMode <- Triangle2 (a, point)
        | Triangle2 (a, b) -> 
            (a, b, point)
            |||> CreateTriangleActor zeroVelocity
            |> data.Physics.AddActor
            data.BuildMode <- Triangle
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
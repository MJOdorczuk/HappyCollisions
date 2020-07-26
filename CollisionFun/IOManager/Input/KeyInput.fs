module IOManager.Input.KeyInput

open OpenTK
open Interfaces

let aKeyDown (data : IApplicationData) (angle : int) : unit =
    match data.Mode with
    | RotateLeft
    | CameraControl -> 
        data.Camera.Rotate angle
        data.Mode <- RotateLeft
    | _ -> ()

let aKeyUp (data : IApplicationData) : unit =
    match data.Mode with
    | RotateLeft -> data.Mode <- CameraControl
    | _ -> ()

let dKeyDown (data : IApplicationData) (angle : int) : unit =
    match data.Mode with
    | RotateRight
    | CameraControl ->
        data.Camera.Rotate (- angle)
        data.Mode <-RotateRight
    | _ -> ()

let dKeyUp (data : IApplicationData) : unit =
    match data.Mode with
    | RotateRight -> data.Mode <- CameraControl
    | _ -> ()
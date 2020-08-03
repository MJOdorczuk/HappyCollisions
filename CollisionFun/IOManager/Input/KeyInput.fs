module IOManager.Input.KeyInput

open OpenTK
open Interfaces

let aKeyDown (data : IApplicationData) (angle : int) : unit =
    match data.InputMode with
    | RotateLeft
    | StandardControl -> 
        data.Camera.Rotate angle
        data.InputMode <- RotateLeft
    | _ -> ()

let aKeyUp (data : IApplicationData) : unit =
    match data.InputMode with
    | RotateLeft -> data.InputMode <- StandardControl
    | _ -> ()

let dKeyDown (data : IApplicationData) (angle : int) : unit =
    match data.InputMode with
    | RotateRight
    | StandardControl ->
        data.Camera.Rotate (- angle)
        data.InputMode <-RotateRight
    | _ -> ()

let dKeyUp (data : IApplicationData) : unit =
    match data.InputMode with
    | RotateRight -> data.InputMode <- StandardControl
    | _ -> ()
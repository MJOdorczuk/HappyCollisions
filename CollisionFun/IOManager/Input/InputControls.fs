module IOManager.Input.InputControls

open Interfaces
open OpenTK.Input
open MouseInput
open KeyInput

let OnMouseButtonDown (data : IApplicationData) (e : MouseButtonEventArgs) : unit =
    let point = 
        e.Position
        |> mapMouseOnDisplay data
    match e.Button with
    | MouseButton.Middle ->
        point
        |> middleMouseButtonDown data
    | MouseButton.Left ->
        point
        |> leftMouseButtonDown data
    | MouseButton.Right ->
        point
        |> rightMouseButtonDown data
    | _ -> ()

let OnMouseButtonUp (data : IApplicationData) (e : MouseButtonEventArgs) : unit =
    match e.Button with
    | MouseButton.Middle -> 
        e.Position
        |> mapMouseOnDisplay data
        |> middleMouseButtonUp data
    | _ -> ()

let OnMouseMove (data : IApplicationData) (e : MouseMoveEventArgs) : unit =
    e.Position
    |> mapMouseOnDisplay data
    |> mouseMove data

let OnWheelMove (data : IApplicationData) (e : MouseWheelEventArgs) : unit =
    let point = mapMouseOnDisplay data e.Position
    let factor = if e.Delta < 0 then 0.9 else 1.0 / 0.9
    wheelMove data point factor
                   
let OnKeyDown (data : IApplicationData) (e : KeyboardKeyEventArgs) : unit =
    match e.Key with
    | Key.A -> aKeyDown data 10
    | Key.D -> dKeyDown data 10
    | _ -> ()

let OnKeyUp (data : IApplicationData) (e : KeyboardKeyEventArgs) : unit =
    match e.Key with
    | Key.A -> aKeyUp data
    | Key.D -> dKeyUp data
    | _ -> ()
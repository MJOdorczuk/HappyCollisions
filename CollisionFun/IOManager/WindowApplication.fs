module IOManager.WindowApplication

open OpenTK
open OpenTK.Graphics.OpenGL
open System
open System.ComponentModel
open Interfaces
open OpenTK.Input
open Drawing.Background
open IOManager.Input.InputControls
open IOManager.ApplicationData
open Drawing.ActorDrawing
open Actors.Operations

let zeroVector = Vector2d(0.0, 0.0)

let WindowLoad (e : EventArgs) : unit =
    ()

let WindowUpdateFrame (e : FrameEventArgs) : unit =
    ()

let WindowClosing (e : CancelEventArgs) : unit =
    ()

let MouseButtonDown (e : MouseButtonEventArgs) : unit =
    ()

type public Application(window : GameWindow) =
    do WindowLoad |> window.Load.Add
    do WindowUpdateFrame |> window.UpdateFrame.Add
    do WindowClosing |> window.Closing.Add
    let data = ApplicationData(window.Bounds) :> IApplicationData
    let backgroundPainter = BackgroundPainter(5, 3, Color.FromArgb(255, 5, 5, 25), Color.White)

    let WindowRenderFrame (e : FrameEventArgs) : unit =
        data.Physics.Tick e.Time
        backgroundPainter.ClearBackground()
        let l = [Vector2d(-1.0, -1.0); Vector2d(-1.0, 1.0); Vector2d(1.0, 1.0); Vector2d(1.0, -1.0)]
        let delimiters = l |> List.map (DisplayPoint >> data.Camera.ProjectToWorld)
        backgroundPainter.DrawMesh data.Camera.Focus delimiters (WorldPoint >> data.Camera.ProjectToDisplay)
        data.Physics.Actors
        |> List.map (DrawActor data.Camera)
        |> ignore
        let target = data.Camera.ProjectToWorld data.LastTargetPoint
        match data.InputMode with
        | StandardControl ->
            let points =
                match data.BuildMode with
                | Polygon points -> points
                | _ -> []
            target::points
            |> CreatePolygonActor zeroVector
            |> DrawActor data.Camera
        | _ -> ()
        GL.Flush()
        window.SwapBuffers()

    let MouseButtonDown (e : MouseButtonEventArgs) : unit =
        OnMouseButtonDown data e
    let MouseButtonUp (e : MouseButtonEventArgs) : unit =
        OnMouseButtonUp data e
    let MouseMove (e : MouseMoveEventArgs) : unit =
        OnMouseMove data e
    let MouseWheelMove (e : MouseWheelEventArgs) : unit =
        OnWheelMove data e
    let KeyDown (e : KeyboardKeyEventArgs) : unit =
        OnKeyDown data e
    let KeyUp (e : KeyboardKeyEventArgs) : unit =
        OnKeyUp data e
    do window.RenderFrame.Add WindowRenderFrame
    do window.MouseDown.Add MouseButtonDown
    do window.MouseUp.Add MouseButtonUp
    do window.MouseMove.Add MouseMove
    do window.MouseWheel.Add MouseWheelMove
    do window.KeyDown.Add KeyDown
    do window.KeyUp.Add KeyUp
    member __.Run : unit -> unit =
        window.Run
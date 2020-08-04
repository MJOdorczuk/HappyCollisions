module Drawing.Graphics

open OpenTK
open OpenTK.Graphics.OpenGL
open System
open Utils

let DrawLine (color : Color) (a : Vector2d) (b : Vector2d) : unit =
    GL.Begin PrimitiveType.Lines
    GL.Color4 color
    GL.Vertex2 a
    GL.Vertex2 b
    GL.End ()

let DrawLines (color : Color) (ends : (Vector2d * Vector2d) list) : unit =
    GL.Begin PrimitiveType.Lines
    GL.Color4 color
    ends
    |> List.map (fun (a, b) -> do GL.Vertex2 a
                               do GL.Vertex2 b)
    |> ignore
    GL.End ()

let DrawLineLoop (color : Color) (points : Vector2d list) : unit =
    GL.Begin PrimitiveType.LineLoop
    GL.Color4 color
    points
    |> List.map GL.Vertex2
    |> ignore
    GL.End ()

let FillLineLoop (color : Color) (points : Vector2d list) : unit =
    GL.Begin PrimitiveType.TriangleFan
    GL.PolygonMode (MaterialFace.FrontAndBack, PolygonMode.Fill)
    GL.Color4 color
    points
    |> List.map GL.Vertex2
    |> ignore
    GL.End ()

let DrawCircle (color : Color) (radius : float) (center : Vector2d) : unit =
    let resolution = 100
    [1..resolution]
    |> List.map float
    |> List.map (fun i -> i * Math.PI * 2.0 / float resolution)
    |> List.map fromAngle
    |> List.map (fun v -> v * radius)
    |> List.map (fun v -> v + center)
    |> DrawLineLoop color

let FillCircle (color : Color) (radius : float) (center : Vector2d) : unit =
    let resolution = 100
    [1..resolution]
    |> List.map float
    |> List.map (fun i -> i * Math.PI * 2.0 / float resolution)
    |> List.map fromAngle
    |> List.map (fun v -> v * radius)
    |> List.map (fun v -> v + center)
    |> FillLineLoop color
module Drawing.Graphics

open OpenTK
open OpenTK.Graphics.OpenGL

let FillCircle (color : Color) (radius : float) (center : Vector2d) : unit =
    GL.Enable EnableCap.PointSmooth
    radius
    |> (*) 2.0
    |> float32
    |> GL.PointSize
    
    GL.Begin PrimitiveType.Points
    GL.Color4 color
    GL.Vertex2 center
    GL.End ()

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

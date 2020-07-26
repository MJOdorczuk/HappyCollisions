module Drawing.Background

open OpenTK
open OpenTK.Graphics.OpenGL
open System

let maxDimension (delimiters : Vector2d list) : float =
    let xs = delimiters |> List.map (fun v -> v.X) in
    let ys = delimiters |> List.map (fun v -> v.Y) in
    let dx = (List.max xs) - (List.min xs) in
    let dy = (List.max ys) - (List.min ys) in
    Math.Max(dx, dy)

let baseMeshSize (step : int) (maxD : float) : float =
    let n = Math.Ceiling(Math.Log(maxD, float step)) in
    Math.Pow(float step, n)

let centralMeshPoint (focus : Vector2d) (baseSize : float) : Vector2d =
    let x = (focus.X / baseSize)
            |> Math.Round
            |> (*) baseSize in
    let y = (focus.Y / baseSize)
            |> Math.Round
            |> (*) baseSize in
    Vector2d(x, y)

let calculateMeshLineAlphaFactor (meshSize : float) (maxD : float) : float =
    (meshSize / maxD)
    |> Math.Sqrt


let drawLine (color : Color) (a : Vector2d) (b : Vector2d) : unit =
    do GL.Color4 color
    do GL.Vertex2 a
    do GL.Vertex2 b

type BackgroundPainter(step : int, depth : int, background : Color, mesh : Color) =
    let mutable backgroundColor : Color = background
    let mutable meshColor : Color = mesh
    member public __.ClearBackground () =
        do GL.Enable EnableCap.Blend
        do GL.BlendFunc(0, BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusDstAlpha)
        do background
           |> GL.ClearColor
        do ClearBufferMask.ColorBufferBit
           |> GL.Clear
    member public __.DrawMesh (focus : Vector2d) 
                              (delimiters : Vector2d list) 
                              (toDisplay : Vector2d -> Vector2d) =
        
        do GL.LineWidth 2.0f
        do GL.Begin PrimitiveType.Lines
        let xs = delimiters |> List.map (fun v -> v.X)
        let ys = delimiters |> List.map (fun v -> v.Y)
        let minX, maxX = List.min xs, List.max xs
        let minY, maxY = List.min ys, List.max ys
        let maxD = Math.Max (maxX - minX, maxY - minY)
        let baseSize = baseMeshSize step maxD
        let centerPoint = centralMeshPoint focus baseSize
        let rec drawVerticalLines (oneSideCount : int) (diff : float) (depth : int) =
            match depth with
            | 0 -> ()
            | _ -> 
                let alpha = calculateMeshLineAlphaFactor diff maxD
                let color = Color.FromArgb(int (alpha * float meshColor.A), 
                                           int meshColor.R, 
                                           int meshColor.G, 
                                           int meshColor.B)
                do [-oneSideCount..oneSideCount] 
                   |> List.filter (fun i -> i % step <> 0)
                   |> List.map float
                   |> List.map (fun i -> centerPoint.X + i * diff)
                   |> List.filter (fun x -> x >= minX && x <= maxX)
                   |> List.map (fun x -> Vector2d(x, minY), Vector2d(x, maxY))
                   |> List.map (fun (u, v) -> toDisplay u, toDisplay v)
                   |> List.map (fun (u, v) -> drawLine color u v)
                   |> ignore
                do drawVerticalLines (oneSideCount * step) (diff / float step) (depth - 1)
        let rec drawHorizontalLines (oneSideCount : int) (diff : float) (depth : int) =
            match depth with
            | 0 -> ()
            | _ ->
                let alpha = calculateMeshLineAlphaFactor diff maxD
                let color = Color.FromArgb(int (alpha * float meshColor.A), 
                                           int meshColor.R, 
                                           int meshColor.G, 
                                           int meshColor.B)
                do [-oneSideCount..oneSideCount] 
                   |> List.filter (fun i -> i % step <> 0)
                   |> List.map float
                   |> List.map (fun i -> centerPoint.Y + i * diff)
                   |> List.filter (fun y -> y >= minY && y <= maxY)
                   |> List.map (fun y -> Vector2d(minX, y), Vector2d(maxX, y))
                   |> List.map (fun (u, v) -> toDisplay u, toDisplay v)
                   |> List.map (fun (u, v) -> drawLine color u v)
                   |> ignore
                do drawHorizontalLines (oneSideCount * step) (diff / float step) (depth - 1)
        do drawVerticalLines step (baseSize / float step) (depth)
        do drawHorizontalLines step (baseSize / float step) (depth)
        let a, b = toDisplay (Vector2d(centerPoint.X, minY)), toDisplay (Vector2d(centerPoint.X, maxY))
        do drawLine meshColor a b
        let a, b = toDisplay (Vector2d(minX, centerPoint.Y)), toDisplay (Vector2d(maxX, centerPoint.Y))
        do drawLine meshColor a b
        do GL.End ()
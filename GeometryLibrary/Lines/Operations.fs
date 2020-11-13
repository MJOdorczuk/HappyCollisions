module GeometryLibrary.Lines.Operations

open OpenTK
open GeometryLibrary.Vectors.Operations
open GeometryLibrary.Types
open GeometryLibrary.Utils

module Vecops = GeometryLibrary.Vectors.Operations

let Equals ((a, b) : Line) ((c, d) : Line) : bool =
    let delta = 0.000000000001
    let v1 = a - c
    let v2 = a - d
    let v3 = b - c
    abs (CrossProduct v1 v2) < delta && abs (CrossProduct v2 v3) < delta

let MidPoint ((a, b) : Line) : Vector2d =
    (a + b) * 0.5

let LineTo (_to : Vector2d) (_from : Vector2d) : Line =
    _from, _to

let Difference ((a, b) : Line) : Vector2d =
    b - a

let Length : Line -> float =
    Difference
    >> Vecops.Length
    
let Direction : Line -> Vector2d =
    Difference
    >> Vecops.Normalize

let Reverse ((a, b) : Line) : Line =
    b, a

let Bisector (line : Line) : Line =
    let midPoint = line |> MidPoint in
    line
    |> Direction
    |> Vecops.PerpendicularRight
    |> (+) midPoint
    |> LineTo midPoint
    |> Reverse

let DistanceTo (line : Line) (point : Vector2d) : float =
    line
    |> fst
    |> (-) point
    |> Vecops.CrossProduct (Direction line)
    |> abs

let ProjectOn ((a, b) : Line) (point : Vector2d) : Vector2d =
    let dir = Direction (a, b) in
    point
    |> (+) -a
    |> Vecops.DotProduct dir
    |> (*) dir
    |> (+) a

let CrossPoint ((a, b) : Line) (l2 : Line) : Vector2d =
    let h = ProjectOn l2 a - a
    let dir = Direction (a, b)
    let hls = h.LengthSquared
    hls / (Vecops.DotProduct h dir) * dir + a

let ProjectsInside ((a, b) : Line) (point : Vector2d) : bool =
    let projected = ProjectOn (a, b) point
    (projected.X |> Between a.X b.X) && (projected.Y |> Between a.Y b.Y)

let ApplyToPoints (modification : Vector2d -> Vector2d) ((a, b) : Line) : Line =
    modification a, modification b

let Translate : Vector2d -> Line -> Line =
    (+)
    >> ApplyToPoints

let RotateAround (axis : Vector2d) : float -> Line -> Line =
    axis
    |> Vecops.RotateAround
    >> ApplyToPoints
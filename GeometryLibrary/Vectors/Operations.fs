module GeometryLibrary.Vectors.Operations

open OpenTK
open System

let Angle (v : Vector2d) : float =
    GeometryLibrary.Utils.Angle v.X v.Y

let Normalize (v : Vector2d) : Vector2d =
    v.Normalized ()

let LengthSquared (v : Vector2d) : float =
    v.LengthSquared

let Length (v : Vector2d) : float =
    v.Length

let PerpendicularRight (v : Vector2d) : Vector2d =
    v.PerpendicularRight

let PerpendicularLeft (v : Vector2d) : Vector2d =
    v.PerpendicularLeft

let ComplexProduct (v1 : Vector2d) (v2 : Vector2d) : Vector2d =
    Vector2d (v1.X * v2.X - v1.Y * v2.Y, v1.X * v2.Y + v1.Y * v2.X)

let CrossProduct (v1 : Vector2d) (v2 : Vector2d) : float =
    v1.X * v2.Y - v1.Y * v2.X

let CrossProductWith (v2 : Vector2d) (v1 : Vector2d) : float =
    CrossProduct v1 v2

let DotProduct (v1 : Vector2d) (v2 : Vector2d) : float =
    v1.X * v2.X + v1.Y * v2.Y

let FromAngle (angle : float) : Vector2d =
    Vector2d (Math.Cos angle, Math.Sin angle)

let Rotate : float -> Vector2d -> Vector2d =
    FromAngle
    >> ComplexProduct

let RotateAround (axis : Vector2d) (angle : double) (v : Vector2d) : Vector2d =
    v - axis
    |> Rotate angle
    |> (+) axis

let CrossProductOfTurn (_from : Vector2d) (first : Vector2d) (second : Vector2d) : float =
    (first - _from)
    |> Normalize
    |> CrossProductWith (Normalize (second - _from))

let MidPoint (points : Vector2d list) : Vector2d =
    points
    |> List.reduce (+)
    |> (*) (1.0 / float points.Length)

let AngularSort (points : Vector2d list) : Vector2d list =
    let mid = MidPoint points
    points
    |> List.sortBy (fun point -> Angle (point - mid))
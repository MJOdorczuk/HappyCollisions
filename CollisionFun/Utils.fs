module Utils

open OpenTK
open System

let useAndPass (param : 'a) (usedIn : 'a -> 'b) : 'a =
    param
    |> usedIn
    |> ignore
    param

let fromAngle (angle : float) : Vector2d =
    let x = Math.Cos angle
    let y = Math.Sin angle
    Vector2d(x, y)
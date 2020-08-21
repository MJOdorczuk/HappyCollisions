module GeometryLibrary.Utils

open System

let Angle (x : double) (y : double) : double =
    match x with
    | x when x = 0.0 -> 
        match y with
        | y when y > 0.0 -> Math.PI * 0.5
        | y when y = 0.0 -> nan
        | _ -> Math.PI * 1.5
    | x when x > 0.0 -> 
        match y with
        | y when y = 0.0 -> 0.0
        | y when y > 0.0 -> atan(y / x)
        | _ -> atan(y / x) + 2.0 * Math.PI
    | _ -> 
        match y with
        | y when y = 0.0 -> Math.PI
        | _ -> atan(y / x) + Math.PI

let Limit (_value : double) (_range : double) =
    _value - Math.Floor(_value / _range) * _range

let LimitAngle =
    fun (angle : double) -> Limit angle (Math.PI * 2.0)

let Between (a : float) (b : float) (value : float) : bool =
    (a > value && value > b) || (a < value && value < b)
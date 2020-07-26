module Display.Camera

open Interfaces
open OpenTK
open System

type Camera() =
    let mutable focus : Vector2d = Vector2d(0.0, 0.0)
    let mutable zoom : float = 1.0
    let mutable angle : int = 0
    let mutable anchor : Vector2d = Vector2d(0.0, 0.0)
    let add (v1 : Vector2d) (v2 : Vector2d) : Vector2d =
        Vector2d.Add(v1, v2)
    let reverse (v : Vector2d) : Vector2d =
        Vector2d(- v.X, - v.Y)
    let scale (factor : float) (v : Vector2d) : Vector2d =
        Vector2d.Multiply(v, factor)
    let rotate (angle : int) (v : Vector2d) : Vector2d =
        let radians = (angle |> float) * Math.PI / 180.0 in
        let s = radians |> Math.Sin in
        let c = radians |> Math.Cos in
        Vector2d(v.X * c - v.Y * s, v.Y * c + v.X * s)   
    let projectToWorld (point : AppPoint) (angle : int) (zoom : float) (focus : Vector2d) : Vector2d =
        match point with
        | WorldPoint v -> v
        | DisplayPoint v -> 
            v
            |> rotate (- angle)
            |> scale (1.0 / zoom)
            |> add focus   
    let projectToDisplay (point : AppPoint) (angle : int) (zoom : float) (focus : Vector2d) : Vector2d =
        match point with
        | DisplayPoint v -> v
        | WorldPoint v ->
            focus
            |> reverse
            |> add v
            |> scale zoom
            |> rotate angle
    interface ICamera with
        member __.Pan (point: AppPoint): unit = 
            let diff = anchor - (projectToWorld point angle zoom focus) in
            focus <- focus + diff
        member __.SaveAnchor (point: AppPoint): unit = 
            anchor <- projectToWorld point angle zoom focus
        member __.Focus: Vector2d = 
            focus
        member __.Rotate (da: int): unit = 
            angle <- angle + da
        member __.ScaleFactor: float = 
            zoom
        member __.Translate (target: AppPoint): unit = 
            let target = projectToWorld target angle zoom focus in
            focus <- Vector2d.Add(focus, target)
        member __.Zoom (factor: float) (target: AppPoint): unit = 
            let target = projectToWorld target angle zoom focus in
            focus <- target
                     |> reverse
                     |> add focus
                     |> scale (1.0 / factor)
                     |> add target
            zoom <- zoom * factor
        member __.ProjectToDisplay (point : AppPoint) : Vector2d =
            projectToDisplay point angle zoom focus
        member __.ProjectToWorld (point : AppPoint) : Vector2d =
            projectToWorld point angle zoom focus
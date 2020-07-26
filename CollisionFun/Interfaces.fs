module Interfaces

open OpenTK
open System.ComponentModel
open System

type InputMode =
    | CameraControl
    | CameraPan
    | RotateLeft
    | RotateRight
    | None

type public AppPoint =
    | WorldPoint of Vector2d
    | DisplayPoint of Vector2d

type public IActor =
    abstract member Move : Vector2d -> unit
    abstract member Accelerate : Vector2d -> unit
    abstract member Position : Vector2d
    abstract member Velocity : Vector2d

type public IActorDisplayer =
    abstract member Draw : IActor -> unit

type public IWorldPhysics =
    abstract member Tick : float -> unit
    abstract member AddActor : IActor -> unit
    abstract member RemoveActor : IActor -> unit
    abstract member Actors : IActor list

type public ICamera =
    abstract member ScaleFactor : float
    abstract member Focus : Vector2d
    abstract member Zoom : float -> AppPoint -> unit
    abstract member Translate : AppPoint -> unit
    abstract member Rotate : int -> unit
    abstract member SaveAnchor : AppPoint -> unit
    abstract member Pan : AppPoint -> unit
    abstract member ProjectToDisplay : AppPoint -> Vector2d
    abstract member ProjectToWorld : AppPoint -> Vector2d

type public IApplicationData =
    abstract member Camera : ICamera
    abstract member Physics : IWorldPhysics
    abstract member ActorDisplayer : IActorDisplayer
    abstract member Mode : InputMode with get, set
    abstract member WindowBoundaries : Rectangle with get, set

type public IApplication =
    abstract member Camera : ICamera

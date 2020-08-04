module IOManager.ApplicationData

open Interfaces
open Display.Camera
open OpenTK
open Physics.WorldPhysics
open Drawing.ActorDrawing

type ApplicationData(windowBoundaries : Rectangle) =
    let camera : ICamera = Camera() :> ICamera
    let physics : IWorldPhysics = CustomWorldPhysics() :> IWorldPhysics
    let displayer : IActorDisplayer = CustomActorDisplayer camera :> IActorDisplayer
    interface IApplicationData with
        member __.Camera: ICamera = 
            camera
        member __.Physics : IWorldPhysics =
            physics
        member __.ActorDisplayer : IActorDisplayer =
            displayer
        member val InputMode = StandardControl with get, set
        member val WindowBoundaries = windowBoundaries with get, set
        member val BuildMode : BuildMode = BuildMode.Triangle with get, set
        member val CachedActor : Actor option = None with get, set
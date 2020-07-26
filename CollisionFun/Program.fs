open System
open OpenTK
open IOManager.WindowApplication
open OpenTK.Graphics

let WIDTH : int = 1280 in
let HEIGHT : int = 720 in
let TITLE : string = "Collision fun" in

[<EntryPoint>]
let main _ =
    let window = new GameWindow(WIDTH, HEIGHT, GraphicsMode.Default, TITLE, GameWindowFlags.FixedWindow) in
    let app = Application(window) in
    window.Run()
    0

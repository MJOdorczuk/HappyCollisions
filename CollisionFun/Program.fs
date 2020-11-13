open System
open OpenTK
open IOManager.WindowApplication
open OpenTK.Graphics

let WIDTH : int = 1280
let HEIGHT : int = 720
let TITLE : string = "Collision fun"

[<EntryPoint>]
let main _ =
    let window = new GameWindow(WIDTH, HEIGHT, GraphicsMode.Default, TITLE, GameWindowFlags.FixedWindow)
    let app = Application(window)
    app.Run()
    0

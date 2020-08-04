module Utils

let useAndPass (param : 'a) (usedIn : 'a -> 'b) : 'a =
    param
    |> usedIn
    |> ignore
    param

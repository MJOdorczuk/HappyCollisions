module GeometryLibraryTests.Utils

open Microsoft.VisualStudio.TestTools.UnitTesting
open System

let (>>=) (param : 'a) (usedIn : 'a -> 'b) : 'a =
    param
    |> usedIn
    |> ignore
    param

let propertyTest (generator : int -> 'a list) (presenter : 'a -> string) (count : int) (properties : (('a -> bool) * string) list) : unit =
    let data = generator count
    let response (property : ('a -> bool) * string) : unit =
        let rule, name = property
        let assertion (sample : 'a) : unit =
            let result = rule sample
            Assert.IsTrue(result, "The " + name + " property is violated for: " + presenter sample)
        data
        |> List.map assertion
        |> ignore
    properties
    |> List.map response
    |> ignore

let pairPropertyTest (generator : int -> ('a * 'a) list) (presenter : 'a -> string) (count : int) (properties : (('a * 'a -> bool) * string) list) : unit =
    let data = generator count
    let response (property : ('a * 'a -> bool) * string) : unit =
        let rule, name = property
        let assertion (sample : 'a * 'a) : unit =
            let a, b = sample
            let stringA, stringB = presenter a, presenter b
            let result = rule sample
            Assert.IsTrue(result, "The " + name + " property is violated for: " + stringA + " " + stringB)
        data
        |> List.map assertion
        |> ignore
    properties
    |> List.map response
    |> ignore

let rnd = Random ()

let rndDouble () = rnd.NextDouble () * 4.0 - 2.0

let rec applyNTimes (count : int) (modifier : 'a -> 'a) (value : 'a) : 'a =
    match count with
    | 0 -> value
    | _ -> applyNTimes (count - 1) modifier (modifier value)
namespace GeometryLibraryTests.Vectors

open System
open OpenTK
open GeometryLibrary.Vectors.Operations
open Microsoft.VisualStudio.TestTools.UnitTesting
open GeometryLibraryTests.Utils

[<TestClass>]
type OperationsTests () =
    let delta = 0.00000000001
    let rndVector () : Vector2d =
        Vector2d (rndDouble (), rndDouble ())
    let rndVectors (count : int) : Vector2d list = 
        [1..count]
        |> List.map (fun _ -> rndVector())
    let randomVectorPairs (count : int) : (Vector2d * Vector2d) list =
        [1..count]
        |> List.map (fun _ -> rndVector (), rndVector ())
    let proxime (u : Vector2d) (v : Vector2d) : bool =
        Length (u - v) < delta
    let test = pairPropertyTest randomVectorPairs string
    let standardTest = test 1000

    [<TestMethod>]
    member __.AngleTest () : unit =
        let reversability ((u, _) : Vector2d * 'a) : bool =
            let angle = Angle u
            let v = Vector2d (cos angle, sin angle) * Length u
            proxime u v
        let properties = [
            reversability, "Reversability"]
        standardTest properties

    [<TestMethod>]
    member __.NormalizeTest () : unit =
        let reversability ((u, _) : Vector2d * 'a) : bool =
            let v = 
                u
                |> Normalize
                |> (*) (Length u)
            proxime v u
        let properties = [
            reversability, "Reversability"]
        standardTest properties

    [<TestMethod>]
    member __.LengthSquaredTest () : unit =
        let multiplicationSquare ((u, _) : Vector2d * 'a) : bool =
            let c = rndDouble ()
            let l1 = LengthSquared u
            let l2 = LengthSquared (u * c)
            abs (l1 * c * c - l2) < delta
        let directionIndependency _ : bool =
            let a = rndDouble ()
            let c = rndDouble ()
            let u = Vector2d (cos a, sin a) * c
            abs (LengthSquared u - c * c) < delta
        let properties = [
            multiplicationSquare, "Multiplication square";
            directionIndependency, "Direction independency"]
        standardTest properties

    [<TestMethod>]
    member __.LengthTest () : unit =
        let multiplicationAssociativity ((u, _) : Vector2d * 'a) : bool =
            let c = abs (rndDouble ())
            let l1 = Length u
            let l2 = Length (u * c)
            abs (l1 * c - l2) < delta
        let directionIndependency _ : bool =
            let c = abs (rndDouble ())
            let a = rndDouble ()
            let u = Vector2d (cos a, sin a) * c
            abs (Length u - c) < delta
        let properties = [
            multiplicationAssociativity, "Multiplication associativity";
            directionIndependency, "Direction independency"]
        standardTest properties

    [<TestMethod>]
    member __.PerpendicularRightTest () : unit =
        let lengthConservation ((u, _) : Vector2d * 'a) : bool =
            let v = PerpendicularRight u
            abs (Length u - Length v) < delta
        let composableToReverse ((u, _) : Vector2d * 'a) : bool =
            let v = applyNTimes 2 PerpendicularRight u
            proxime u -v
        let turning90DegreeRight ((u, _) : Vector2d * 'a) : bool =
            let a = Angle u + Math.PI * 0.5
            let v = 
                (cos a, sin a)
                |> Vector2d
                |> PerpendicularRight
            abs (Angle u - Angle v) < delta
        let properties = [
            lengthConservation, "Length conservation";
            composableToReverse, "Composable to reverse";
            turning90DegreeRight, "Turning 90 degree right"]
        standardTest properties

    [<TestMethod>]
    member __.PerpendicularLeftTest () : unit =
        let lengthConservation ((u, _) : Vector2d * 'a) : bool =
            let v = PerpendicularLeft u
            abs (Length u - Length v) < delta
        let composableToReverse ((u, _) : Vector2d * 'a) : bool =
            let v = applyNTimes 2 PerpendicularLeft u
            proxime u -v
        let turning90DegreeLeft ((u, _) : Vector2d * 'a) : bool =
            let a = Angle u - Math.PI * 0.5
            let v = 
                (cos a, sin a)
                |> Vector2d
                |> PerpendicularLeft
            abs (Angle u - Angle v) < delta
        let properties = [
            lengthConservation, "Length conservation";
            composableToReverse, "Composable to reverse";
            turning90DegreeLeft, "Turning 90 degree left"]
        standardTest properties

    [<TestMethod>]
    member __.ComplexProductTest () : unit =
        let equivalencyToSquareForLength ((u, v) : Vector2d * Vector2d) : bool =
            let w = ComplexProduct u v
            abs (Length w - Length v * Length u) < delta
        let commutative ((u, v) : Vector2d * Vector2d) : bool =
            let c1 = ComplexProduct u v
            let c2 = ComplexProduct v u
            proxime c1 c2
        let neutralityOfXAxis ((u, _) : Vector2d * 'a) : bool =
            let x = u.X
            let v = Vector2d (x, 0.0)
            let w = ComplexProduct v v
            abs w.Y < delta
        let neutralElement ((u, _) : Vector2d * 'a) : bool =
            let v = Vector2d (1.0, 0.0)
            let w = ComplexProduct u v
            proxime u w
        let equivalencyToAdditionForAngle ((u, v) : Vector2d * Vector2d) : bool =
            let w = ComplexProduct u v
            let w = w / Length w
            let a = Angle u + Angle v
            let uv = Vector2d (cos a, sin a)
            proxime w uv
        let absorbingElement ((u, _) : Vector2d * 'a) : bool =
            let v = Vector2d (0.0, 0.0)
            let w = ComplexProduct u v
            proxime v w
        let properties = [
            equivalencyToSquareForLength, "Equivalency to square for length";
            commutative, "Commutative";
            neutralityOfXAxis, "Neutrality of X axis";
            neutralElement, "Neutral element";
            equivalencyToAdditionForAngle, "Equivalency to addition for angle";
            absorbingElement, "Absorbing element"]
        standardTest properties

    [<TestMethod>]
    member __.CrossProductTest () : unit =
        let selfAbsorbtion ((u, _) : Vector2d * 'a) : bool =
            let c = CrossProduct u u 
            abs c < delta
        let reverseCommutative ((u, v) : Vector2d * Vector2d) : bool =
            let c1 = CrossProduct u v
            let c2 = CrossProduct v u
            abs (c1 + c2) < delta
        let consistencyForOrtogonalVersors ((u, _) : Vector2d * 'a) : bool =
            let u = u / Length u
            let v = u.PerpendicularRight
            let c = CrossProduct u v
            abs (c + 1.0) < delta
        let lengthDistributive ((u, v) : Vector2d * Vector2d) : bool =
            let c1 = CrossProduct u v
            let c2 = CrossProduct (u / Length u) (v / Length v) * Length u * Length v
            abs (c1 - c2) < delta
        let parallelAbsorbtive ((u, _) : Vector2d * 'a) : bool =
            let v = u * rndDouble ()
            let c = CrossProduct u v
            abs c < delta
        let properties = [
            selfAbsorbtion, "Self absorbtion";
            reverseCommutative, "Reverse commutative";
            consistencyForOrtogonalVersors, "Consistency for ortogonal versors";
            lengthDistributive, "Length distributive";
            parallelAbsorbtive, "Parallel absorbtive"]
        standardTest properties

    [<TestMethod>]
    member __.CrossProductWithTest () : unit =
        let reverseArgumentOrder ((u, v) : Vector2d * Vector2d) : bool =
            let c1 = CrossProduct u v
            let c2 = CrossProductWith v u
            abs (c1 - c2) < delta
        let properties = [
            reverseArgumentOrder, "Reverse argument order"]
        standardTest properties

    [<TestMethod>]
    member __.DotProductTest () : unit =
        let selfLengthSquare ((u, _) : Vector2d * 'a) : bool =
            let c = DotProduct u u
            abs (c - LengthSquared u) < delta
        let commutative ((u, v) : Vector2d * Vector2d) : bool =
            let c1 = DotProduct u v
            let c2 = DotProduct v u
            abs (c1 - c2) < delta
        let ortogonalAbsorbtive ((u, _) : Vector2d * 'a) : bool =
            let c = DotProduct u u.PerpendicularRight
            abs c < delta
        let lengthDistributive ((u, v) : Vector2d * Vector2d) : bool =
            let c1 = DotProduct u v
            let c2 = DotProduct (u / Length u) (v / Length v) * Length u * Length v
            abs (c1 - c2) < delta
        let multiplicationDistributive ((u, v) : Vector2d * Vector2d) : bool =
            let c = rndDouble ()
            let w = v * c
            let c1 = DotProduct u v
            let c2 = DotProduct u w
            abs (c1 * c - c2) < delta
        let properties = [
            selfLengthSquare, "Self length square";
            commutative, "Commutative";
            ortogonalAbsorbtive, "Ortogonal absorbtive";
            lengthDistributive, "Length distributive";
            multiplicationDistributive, "multiplication distributive"]
        standardTest properties

    [<TestMethod>]
    member __.FromAngleTest () : unit =
        let inverseToAngle ((u, _) : Vector2d * 'a) : bool =
            let v = FromAngle (Angle u) * Length u
            proxime u v
        let properties = [
            inverseToAngle, "Inverse to Angle"]
        standardTest properties

    [<TestMethod>]
    member __.RotateTest () : unit =
        let inversable ((u, _) : Vector2d * 'a) : bool =
            let angle = rndDouble ()
            let v =
                u
                |> Rotate angle
                |> Rotate -angle
            proxime u v
        let commutative((u, _) : Vector2d * 'a) : bool =
            let a1 = rndDouble ()
            let a2 = rndDouble ()
            let v = 
                u
                |> Rotate a1
                |> Rotate a2
            let w =
                u
                |> Rotate a2
                |> Rotate a1
            proxime v w
        let lengthNeutral ((u, _) : Vector2d * 'a) : bool =
            let angle = rndDouble ()
            let v = Rotate angle u
            abs (Length u - Length v) < delta
        let equivalentToAdditionForAngle ((u, _) : Vector2d * 'a) : bool =
            let a1 = Angle u
            let a2 = rndDouble ()
            let v = Rotate a2 u
            let w = FromAngle (a1 + a2) * Length u
            proxime v w
        let properties = [
            inversable, "Inversable";
            commutative, "Commutative";
            lengthNeutral, "Length neutral";
            equivalentToAdditionForAngle, "Equivalent to addition for angle"]
        standardTest properties

    [<TestMethod>]
    member __.RotateAroundTest () : unit =
        let selfNeutral ((u, _) : Vector2d * 'a) : bool =
            let angle = rndDouble ()
            let v = RotateAround u angle u
            proxime u v
        let neutralElement ((u, _) : Vector2d * 'a) : bool =
            let zero = Vector2d (0.0, 0.0)
            let angle = rndDouble ()
            let v = RotateAround zero angle u
            let w = Rotate angle u
            proxime v w
        let inversable ((u, v) : Vector2d * Vector2d) : bool =
            let angle = rndDouble ()
            let w =
                u
                |> RotateAround v angle
                |> RotateAround v -angle
            proxime u w
        let distanceConservation ((u, v) : Vector2d * Vector2d) : bool =
            let angle = rndDouble ()
            let w = RotateAround v angle u
            let d1 = Length (u - v)
            let d2 = Length (w - v)
            abs (d1 - d2) < delta
        let angleAdditive ((u, v) : Vector2d * Vector2d) : bool =
            let a1 = rndDouble ()
            let a2 = rndDouble ()
            let w1 = 
                u
                |> RotateAround v a1
                |> RotateAround v a2
            let w2 = RotateAround v (a1 + a2) u
            proxime w1 w2
        let equivalentToRelativeAngleAddition ((u, v) : Vector2d * Vector2d) : bool =
            let angle = rndDouble ()
            let w1 = RotateAround v angle u
            let w2 = 
                (u - v)
                |> Rotate angle
                |> (+) v
            proxime w1 w2
        let properties = [
            selfNeutral, "Self neutral";
            neutralElement, "Neutral element";
            inversable, "Inversable";
            distanceConservation, "Distance conservation";
            angleAdditive, "Angle additive";
            equivalentToRelativeAngleAddition, "Equivalent to relative angle addition"]
        standardTest properties

    [<TestMethod>] // TO BE EXTENDED
    member __.CrossProductOfTurnTest () : unit =
        let crossProductAssociation ((u, v) : Vector2d * Vector2d) : bool =
            let w = rndVector ()
            let a = (u - w)
            let b = (v - w)
            let res1 = CrossProduct (Normalize a) (Normalize b)
            let res2 = CrossProductOfTurn w u v
            abs (res1 - res2) < delta
        let properties = [
            crossProductAssociation, "Cross product association"]
        standardTest properties

    [<TestMethod>]
    member __.MidPointTest () : unit =
        let selfIdentity ((u, _) : Vector2d * 'a) : bool =
            let mid = MidPoint (u::[])
            proxime u mid
        let argumentOrderNeutral _ : bool =
            let vectors = rndVectors 1000
            let shuffled = List.sortBy (fun _ -> rndDouble ()) vectors
            let mid1 = MidPoint vectors
            let mid2 = MidPoint shuffled
            proxime mid1 mid2
        let proportionallyDistributive _ : bool =
            let i = rnd.Next (10, 100)
            let j = rnd.Next (10, 100)
            let vectors1 = rndVectors i
            let vectors2 = rndVectors j
            let m1 = MidPoint vectors1
            let m2 = MidPoint vectors2
            let m3 = MidPoint (vectors1 @ vectors2)
            let m12 = (m1 * float i + m2 * float j) / float (i + j)
            proxime m3 m12
        let properties = [
            selfIdentity, "Self identity";
            argumentOrderNeutral, "Argument order neutral";
            proportionallyDistributive, "Proportionally distributive"]
        standardTest properties

    [<TestMethod>]
    member __.AngularSortTest () : unit =
        let elementConsistency _ : bool =
            let vectors = 
                (10, 100)
                |> rnd.Next
                |> rndVectors
                |> List.distinct
            let sorted = AngularSort vectors
            vectors
            |> List.map (fun v -> List.contains v sorted)
            |> List.reduce (&&)
            |> (&&) (vectors.Length = sorted.Length)
        let angularOrder _ : bool =
            let sorted =
                (10, 100)
                |> rnd.Next
                |> rndVectors
                |> AngularSort
            let mid = MidPoint sorted
            (List.last sorted, sorted.Head)::(List.pairwise sorted)
            |> List.map (fun (u, v) -> CrossProductOfTurn mid u v >= 0.0)
            |> List.reduce (&&)
        let properties = [
            elementConsistency, "Element consistency";
            angularOrder, "Angular order"]
        standardTest properties
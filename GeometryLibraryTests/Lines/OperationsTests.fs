namespace GeometryLibraryTests.Lines

open System
open OpenTK
open GeometryLibrary.Types
open GeometryLibrary.Lines.Operations
open Microsoft.VisualStudio.TestTools.UnitTesting
open GeometryLibraryTests.Utils
module Vecops = GeometryLibrary.Vectors.Operations

[<TestClass>]
type OperationsTests () =
    let delta = 0.00000000001
    let rndVector () : Vector2d =
        Vector2d (rndDouble (), rndDouble ())
    let rndVectors (count : int) : Vector2d list = 
        [1..count]
        |> List.map (fun _ -> rndVector())
    let randomLinePairs (count : int) : (Line * Line) list =
        [1..count]
        |> List.map (fun _ -> (rndVector (), rndVector ()), (rndVector (), rndVector ()))
    let test = pairPropertyTest randomLinePairs string
    let standardTest = test 1000
    let proxime (u : Vector2d) (v : Vector2d) : bool =
        Vecops.Length (u - v) < delta

    [<TestMethod>]
    member __.EqualsTest () : unit =
        let selfEquality ((line, _) : Line * 'a) : bool =
            Equals line line
        let selfReverseEquality (((a, b), _) : Line * 'a) : bool =
            Equals (a, b) (b, a)
        let collinearEquality (((a, b), _) : Line * 'a) : bool =
            let factor1, factor2 = rndDouble (), rndDouble ()
            let difference = b - a
            let c, d = a + difference * factor1, b + difference * factor2
            Equals (a, b) (c, d)
        let parallelTranslationInequality (((a, b), _) : Line * 'a) : bool =
            let factor = rndDouble()
            let perpendicular =
                (b - a)
                |> Vecops.PerpendicularLeft
                |> Vecops.Normalize
                |> (*) factor
            abs factor < delta || not (Equals (a, b) (a + perpendicular, b + perpendicular))
        let rotationInequality (((a, b), _) : Line * 'a) : bool =
            let angle = rndDouble ()
            let rotatedB = Vecops.RotateAround a angle b
            abs angle < delta || not (Equals (a, b) (a, rotatedB))
        let properties = [
            selfEquality, "Self equality";
            selfReverseEquality, "Self reverse equality";
            collinearEquality, "Collinear equality";
            parallelTranslationInequality, "Parallel translation inequality";
            rotationInequality, "Rotation inequality"]
        standardTest properties

    [<TestMethod>]
    member __.MidPointTest () : unit =
        let translationConsistency (((a, b), _) : Line * 'a) : bool =
            let v = rndVector ()
            let c, d = a + v, b + v
            proxime (MidPoint (a, b) + v) (MidPoint (c, d))
        let lineAdditionConsistency (((a, b), (c, d)) : Line * Line) : bool =
            let mid1 = MidPoint (a, b)
            let mid2 = MidPoint (c, d)
            let line = a + c, b + d
            proxime (MidPoint line) (mid1 + mid2)
        let balancedAdditionIndependency (((a, b), _) : Line * 'a) : bool =
            let v = rndVector ()
            let c, d = a + v, b - v
            proxime (MidPoint (a, b)) (MidPoint (c, d))
        let rotationConsistency (((a, b), _) : Line * 'a) : bool =
            let axis = rndVector ()
            let angle = rndDouble ()
            let c, d = Vecops.RotateAround axis angle a, Vecops.RotateAround axis angle b
            let mid1 = MidPoint (a, b)
            let mid2 = Vecops.RotateAround axis angle mid1
            proxime mid2 (MidPoint (c, d))
        let collinearity (((a, b), _) : Line * 'a) : bool =
            let mid = MidPoint (a, b)
            Equals (a, mid) (mid, b) && Equals (a, b) (a, mid)
        let midway (((a, b), _) : Line * 'a) : bool =
            let mid = MidPoint (a, b)
            proxime (mid - a) (b - mid)
        let properties = [
            translationConsistency, "Translation consistency";
            lineAdditionConsistency, "Line addition consistency";
            balancedAdditionIndependency, "Balanced addition independency";
            rotationConsistency, "Rotation consistency";
            collinearity, "Collinearity";
            midway, "Midway"]
        standardTest properties

    [<TestMethod>]
    member __.LineToTest () : unit =
        let consistency (((a, b), _) : Line * 'a) : bool =
            let line = a |> LineTo b
            let c, d = line
            proxime a c && proxime b d
        let properties = [
            consistency, "Consistency"]
        standardTest properties

    [<TestMethod>]
    member __.Difference () : unit =
        let consistency (((a, b), _) : Line * 'a) : bool =
            let difference = Difference (a, b)
            proxime (a + difference) b
        let properties = [
            consistency, "Consistency"]
        standardTest properties

    [<TestMethod>]
    member __.Length () : unit =
        let reversalIndependency (((a, b), _) : Line * 'a) : bool =
            abs (Length (a, b) - Length (b, a)) < delta
        let translationIndependency (((a, b), _) : Line * 'a) : bool =
            let v = rndVector ()
            let c, d = a + v, b + v
            abs (Length (a, b) - Length (c, d)) < delta
        let rotationIndependency (((a, b), _) : Line * 'a) : bool =
            let axis = rndVector ()
            let angle = rndDouble ()
            let c, d = Vecops.RotateAround axis angle a, Vecops.RotateAround axis angle b
            abs (Length (a, b) - Length (c, d)) < delta
        let multiplicationConsistency (((a, b), _) : Line * 'a) : bool =
            let c = rndDouble ()
            let diff = Difference (a, b)
            let d = a + diff * c
            abs (Length (a, b) * abs c - Length (a, d)) < delta
        let unitLengthOfVersor (((a, _), _) : Line * 'a) : bool =
            let v = rndVector () |> Vecops.Normalize
            abs (Length (a, a + v) - 1.0) < delta
        let zeroLength (((a, _), _) : Line * 'a) : bool =
            abs (Length (a, a)) < delta
        let cartesianLength (((a, b), _) : Line * 'a) : bool =
            let factor = rndDouble ()
            let diff = Difference (a, b)
            let perpendicular = 
                diff 
                |> Vecops.PerpendicularLeft 
                |> Vecops.Normalize 
                |> (*) factor
            let length1 = Length (a, b)
            let length2 = Length (a, b + perpendicular)
            abs (length2 - sqrt (length1 * length1 + factor * factor)) < delta
        let properties = [
            reversalIndependency, "Reversal independency";
            translationIndependency, "Translation independency";
            rotationIndependency, "Rotation independency";
            multiplicationConsistency, "Multiplication consistency";
            unitLengthOfVersor, "Unit length of versor";
            zeroLength, "Zero length";
            cartesianLength, "Cartesian length"]
        standardTest properties

    [<TestMethod>]
    member __.Direction () : unit =
        let collinearity (((a, b), _) : Line * 'a) : bool =
            let dir = Direction (a, b)
            let c = a + dir
            Equals (a, b) (a, c)
        let unitLength ((line, _) : Line * 'a) : bool =
            let dir = Direction line
            abs (Vecops.Length dir - 1.0) < delta
        let turnConsistency (((a, b), _) : Line * 'a) : bool =
            let dir = Direction (a, b)
            let length = Length (a, b)
            let c = a + dir * length
            proxime b c
        let properties = [
            collinearity, "Collinearity";
            unitLength, "Unit length";
            turnConsistency, "Turn consistency"]
        standardTest properties

    [<TestMethod>]
    member __.ReverseTest () : unit =
        let consistency (((a, b), _) : Line * 'a) : bool =
            let c, d = Reverse (a, b)
            proxime a d && proxime b c
        let properties = [
            consistency, "Consistency"]
        standardTest properties
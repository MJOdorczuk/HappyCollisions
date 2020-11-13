namespace GeometryLibraryTests.Lines

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

    [<TestMethod>]
    member __.BisectorTest () : unit =
        let orthogonality ((line, _) : Line * 'a) : bool =
            let bisector = Bisector line
            abs (Vecops.DotProduct (Direction line) (Direction bisector)) < delta
        let equidistance (((a, b), _) : Line * 'a) : bool =
            let c, d = Bisector (a, b)
            let diff1 = Vecops.Length (a - c) - Vecops.Length (b - c)
            let diff2 = Vecops.Length (a - d) - Vecops.Length (b - d)
            abs diff1 < delta && abs diff2 < delta
        let properties = [
            orthogonality, "Orthogonality";
            equidistance, "Equidistance"]
        standardTest properties

    [<TestMethod>]
    member __.DistanceToTest () : unit =
        let selfProximity (((a, b), _) : Line * 'a) : bool =
            abs (DistanceTo (a, b) a) < delta && abs (DistanceTo (a, b) b) < delta
        let rhombusHeight (((a, b), (c, _)) : Line * Line) : bool =
            let h = DistanceTo (a, b) c
            let l = Length (a, b)
            let s1 = b - a
            let s2 = c - a
            let cross = abs (Vecops.CrossProduct s1 s2)
            let surface = h * l
            abs (surface - cross) < delta
        let positiveResult (((a, b), (c, _)) : Line * Line) : bool =
            DistanceTo (a, b) c >= 0.0
        let properties = [
            selfProximity, "Self proximity";
            rhombusHeight, "Rhombus height";
            positiveResult, "Positive result"]
        standardTest properties

    [<TestMethod>]
    member __.ProjectOnTest () : unit =
        let selfProximity (((a, b), _) : Line * 'a) : bool =
            proxime (ProjectOn (a, b) a) a
        let collinearity (((a, b), (c, _)) : Line * Line) : bool =
            let d = ProjectOn (a, b) c
            let diff1 = d - a
            let diff2 = b - d
            proxime diff1 (diff2 * diff1.X / diff2.X)
        let orthogonality (((a, b), (c, _)) : Line * Line) : bool =
            let d = ProjectOn (a, b) c
            let diff1, diff2 = a - b, c - d
            let dot = Vecops.DotProduct diff1 diff2 
            abs dot < delta
        let minimalDistance (((a, b), (c, _)) : Line * Line) : bool =
            let d = ProjectOn (a, b) c
            let l1 = Vecops.Length (d - c)
            let l2 = DistanceTo (a, b) c
            abs (l1 - l2) < delta
        let properties = [
            selfProximity, "Self proximity";
            collinearity, "Collinearity";
            orthogonality, "Orthogonality";
            minimalDistance, "Minimal distance"]
        standardTest properties

    [<TestMethod>]
    member __.CrossPointTest () : unit =
        let resultContainment ((l1, l2) : Line * Line) : bool =
            let x = CrossPoint l1 l2
            let projected1 = ProjectOn l1 x
            let projected2 = ProjectOn l2 x
            proxime projected1 projected2
        let properties = [
            resultContainment, "Result containment"]
        standardTest properties

    [<TestMethod>]
    member __.ProjectsInsideTest () : unit =
        let consistency (((a, b), (point, _)) : Line * Line) : bool =
            let c = ProjectOn (a, b) point
            let acbx = a.X <= c.X && c.X <= b.X
            let bcax = b.X <= c.X && c.X <= a.X
            let acby = a.Y <= c.Y && c.Y <= b.Y
            let bcay = b.Y <= c.Y && c.Y <= a.Y
            ((acbx || bcax) && (acby || bcay)) = ProjectsInside (a, b) point
        let properties = [
            consistency, "Consistency"]
        standardTest properties
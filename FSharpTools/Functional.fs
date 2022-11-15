namespace FSharpTools

module Functional =
    open System

    type Resetter() = 
        let mutable action: (unit->unit) option = None

        member this.SetResetAction resetAction = 
            action <- Some resetAction

        member this.Reset () = 
            if action.IsSome then action.Value()
        
    type RefCell<'a> = {
        mutable Value: 'a option
        mutable Valid: bool
    }

    /// <summary>
    /// Memoization. A function result is memoized if called for the first time. 
    /// Subsequent calls always returns return the memoized value, not calling the function any more.
    /// </summary>
    /// <param name="f">function with one input parameter 'a returning 'b</param>
    /// <returns>Memoized function with the same signature</returns>
    let memoize<'a, 'b> (func: 'a->'b) =
        let memoization = System.Collections.Concurrent.ConcurrentDictionary<'a, 'b>()
        fun key -> memoization.GetOrAdd (key, func)

    /// <summary>
    /// Memoization. A function result is memoized if called for the first time. 
    /// Subsequent calls always returns return the memoized value, not calling the function any more.
    /// </summary>
    /// <param name="f">function with no (unit) input parameter 'a returning 'b</param>
    /// <returns>Memoized function with the same signature</returns>
    let memoizeSingle funToMemoize =
        let memoized = funToMemoize ()
        fun () -> memoized

    /// <summary>
    /// Memoization. A function result is memoized if called for the first time. 
    /// Subsequent calls always returns return the memoized value, not calling the function any more.
    /// </summary>
    /// <param name="funToMemoize">function with no (unit) input parameter 'a returning 'b</param>
    /// <param name="resetter">A Resetter to reset the cached value</param>
    /// <returns>Memoized function with the same signature</returns>
    let memoizeSingleReset<'a> (funToMemoize: unit->'a) (resetter: Resetter) =
        let refCell = { 
            Value = None
            Valid = false   
        }
        resetter.SetResetAction (fun () -> refCell.Valid <- false)
        let locker = Object()
        fun () -> 
            if refCell.Valid then 
                refCell.Value.Value
            else
                lock locker (fun () ->
                    if refCell.Valid then 
                        refCell.Value.Value
                    else 
                        refCell.Value <- Some <| funToMemoize ()
                        refCell.Valid <- true
                        refCell.Value.Value
                )
            
    /// <summary>
    /// Helper function for composing functions (Railway Oriented Programming). 
    /// Slot for  dead end function (Inject side effects in function coposition pipeline)
    /// </summary>
    /// <param name="f">function with one input parameter 'a. This is the dead end function</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>'a</returns>
    let sideEffect f x =
        f x |> ignore
        x                   
    
    /// <summary>
    /// Takes the first element of a tuple disgarding the second
    /// </summary>
    /// <param name="a, _">Tuple of two elements</param>
    /// <returns>The first tuple element a</returns>
    let takeFirstTupleElem (a, _) = a
    
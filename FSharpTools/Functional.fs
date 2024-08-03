namespace FSharpTools

module Functional =
    open System

    type Resetter private (autoResetTimeSpan: option<TimeSpan>) = 
        let mutable action: (unit->unit) option = None

        let reset () = 
            if action.IsSome then action.Value()

        let createTimer (span: TimeSpan) = 
            let timer = new Timers.Timer (span)
            let tick _ = 
                printfn "Resette........................................"
                reset ()
            timer.Elapsed.Add tick
            timer

        let timer = 
            autoResetTimeSpan
            |> Option.map createTimer

        new () = Resetter(None)
        new (autoResetTimeSpan: TimeSpan) = Resetter(Some autoResetTimeSpan)

        member this.SetResetAction resetAction = 
            action <- Some resetAction

        member this.Reset = reset
        
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

    let private memoizeSingleOptionReset<'a> (funToMemoize: unit->'a) (resetter: Option<Resetter>) =
        let refCell = { 
            Value = None
            Valid = false   
        }
        resetter
        |> Option.iter (fun r -> r.SetResetAction (fun () -> refCell.Valid <- false))
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
    /// Memoization. A function result is memoized if called for the first time. 
    /// Subsequent calls always returns return the memoized value, not calling the function any more.
    /// </summary>
    /// <param name="f">function with no (unit) input parameter 'a returning 'b</param>
    /// <returns>Memoized function with the same signature</returns>
    let memoizeSingle funToMemoize = memoizeSingleOptionReset funToMemoize None

    /// <summary>
    /// Memoization. A function result is memoized if called for the first time. 
    /// Subsequent calls always returns return the memoized value, not calling the function any more.
    /// </summary>
    /// <param name="funToMemoize">function with no (unit) input parameter 'a returning 'b</param>
    /// <param name="resetter">A Resetter to reset the cached value</param>
    /// <returns>Memoized function with the same signature</returns>
    let memoizeSingleReset<'a> (funToMemoize: unit->'a) (resetter: Resetter) =
        memoizeSingleOptionReset funToMemoize (Some resetter)
            
    /// <summary>
    /// Helper function for composing functions (Railway Oriented Programming). 
    /// Slot for dead end function (Inject side effects in function coposition pipeline)
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
    

    // type Map =

    //     static member Map (x : option<_>      , f : 'T->'U, [<Optional>]_mthd : Map) = Option.map  f x
    //     static member Map (x : optiontask<_>      , f : 'T->'U, [<Optional>]_mthd : Map) = OptionTask.map  f x
    //     static member Map (x : Task<_>      , f : 'T->'U, [<Optional>]_mthd : Map) = Task.map  f x
        
    //     static member inline Invoke (mapping :'T->'U) (source : '``Functor<'T>``) : '``Functor<'U>`` = 
    //             let inline call (mthd : ^M, source : ^I, _output : ^R) = ((^M or ^I or ^R) : (static member Map: _*_*_ -> _) source, mapping, mthd)
    //             call (Unchecked.defaultof<Map>, source, Unchecked.defaultof<'``Functor<'U>``>)

    // type Bind =
    //     static member Bind (source, f : 'T -> _) = Option.bind f source : option<'U>
    //     static member Bind (source, f : 'T -> _) = OptionTask.bind f source : optiontask<'U>
    //     static member Bind (source, f : 'T -> _) = Task.bind f source : Task<'U>

    //     static member inline Invoke (source : '``Monad<'T>``) (binder : 'T -> '``Monad<'U>``) : '``Monad<'U>`` =
    //         let inline call (_mthd : 'M, input : 'I, _output : 'R, f) = ((^M or ^I or ^R) : (static member Bind: _*_ -> _) input, f)
    //         call (Unchecked.defaultof<Bind>, source, Unchecked.defaultof<'``Monad<'U>``>, binder)

    // let inline (>=>) (f:'T->'``Monad<'U>``) (g:'U->'``Monad<'V>``) (x:'T) : '``Monad<'V>`` = Bind.Invoke (f x) g        
    // let inline (>>=) x f = Bind.Invoke x f
    // let inline (|>>)  (x:'``Functor<'T>``) (f:'T->'U) :'``Functor<'U>`` = Map.Invoke f x
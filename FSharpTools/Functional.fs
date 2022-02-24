namespace FSharpTools
module Functional = 

    /// <summary>
    /// Functional alternative to exception handling.
    /// Instead of exception handling a Response object is used (Railway Oriented Programming).async
    /// There is an option of two possible values: Ok&lt;'a&gt; and Err&lt;Exception&gt;
    /// </summary>
    type Response<'a> = 
        | Ok  of 'a
        | Err of System.Exception

    /// <summary>
    /// Fish operator (Kleisli Category) for composing functions returning Option values (Railway Oriented Programming).
    /// </summary>
    /// <param name="switch1">function with one input parameter 'a returning an option&lt;'b&gt;</param>
    /// <param name="switch2">function with one input parameter 'b returning an option&lt;'c&gt;</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>function with one input parameter 'a returning an option&lt;'c&gt;</returns>
    let (>=>?) switch1 switch2 x =
        match switch1 x with
        | Some s -> switch2 s
        | None   -> None

    /// <summary>
    /// Fish operator (Kleisli Category) for composing functions returning Response values (Railway Oriented Programming).
    /// </summary>
    /// <param name="switch1">function with one input parameter 'a returning a Response&lt;'b&gt;</param>
    /// <param name="switch2">function with one input parameter 'b returning a Response&lt;'c&gt;</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>function with one input parameter 'a returning a Response&lt;'c&gt;</returns>
    let (>=>!) switch1 switch2 x =
        match switch1 x with
        | Ok s -> switch2 s
        | Err e   -> Err e

    /// <summary>
    /// Helper function for composing functions with Fish operator with option (Railway Oriented Programming)
    /// </summary>
    /// <param name="f">function with one input parameter 'a returning 'b</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>option&lt;'b&gt;</returns>
    let switch f x = f x |> Some 

    /// <summary>
    /// Helper function for composing functions with Fish operator with Response values (Railway Oriented Programming)
    /// </summary>
    /// <param name="f">function with one input parameter 'a returning 'b</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>Response&lt;'b&gt;</returns>
    let switchResponse f x = f x |> Ok 

    /// <summary>
    /// Helper function for composing functions (Railway Oriented Programming). 
    /// Slot for  dead end function (Inject side effects in function coposition pipeline)
    /// </summary>
    /// <param name="f">function with one input parameter 'a. This is the dead end function</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>'a</returns>
    let tee f x =
        f x |> ignore
        x       

    let OptionFrom2Options a b = 
        match a, b with
        | Some a, Some b -> Some (a, b)
        | _              -> None

    let withInputVar switch x = 
        match switch x with
        | Some s -> Some(x, s)
        | None   -> None

    let omitInputVar (_, b)  = Some(b)

    /// <summary>
    /// Memoization. A function result is memoized if called for the first time. 
    /// Subsequent calls always returns return the memoized value, not calling the function any more.
    /// </summary>
    /// <param name="f">function with one input parameter 'a returning 'b</param>
    /// <returns>Memoized function with the same signature</returns>
    let memoize func =
        let memoization = System.Collections.Generic.Dictionary<_, _>()
        fun key ->
            match memoization.TryGetValue key with
            | true, value -> value
            | _           -> 
                             let value = func key  
                             memoization.Add(key, value)
                             value

    let exceptionToOption func =
        try
            match func () with
            | res when res <> null -> Some(res) 
            | _                    -> None
        with
        | _ -> None

    let exceptionToResponse func =
        try
            Ok(func ()) 
        with
        | e -> Err(e)
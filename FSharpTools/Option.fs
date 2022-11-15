namespace FSharpTools

module Option =
    /// <summary>
    /// Bind operator for composing functions returning Option values (Railway Oriented Programming).
    /// </summary>
    /// <param name="binder">function with one input parameter 'a returning an option&lt;'b&gt;</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>option&lt;'b&gt;</returns>
    let (>>=) x binder = Option.bind binder x

    /// <summary>
    /// Fish operator (Kleisli Category) for composing functions returning Option values (Railway Oriented Programming).
    /// </summary>
    /// <param name="f1">function with one input parameter 'a returning an option&lt;'b&gt;</param>
    /// <param name="f2">function with one input parameter 'b returning an option&lt;'c&gt;</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>function with one input parameter 'a returning an option&lt;'c&gt;</returns>
    let (>=>) f1 f2 x = f1 x >>= f2

    /// <summary>
    /// Helper function for composing functions with Fish operator with option (Railway Oriented Programming)
    /// </summary>
    /// <param name="f">function with one input parameter 'a returning 'b</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>option&lt;'b&gt;</returns>
    let switch f x = f x |> Some         

    let OptionFrom2Options a b = 
        match a, b with
        | Some a, Some b -> Some (a, b)
        | _              -> None

    /// <summary>
    /// Maps the Error value of a Result to an option, discarding the Ok value
    /// </summary>
    /// <param name="result">Result from which the error should be taken</param>
    let mapOnlyError result = 
        match result with
        | Ok    _ -> None
        | Error u -> Some u

    let withInputVar switch x = 
        match switch x with
        | Some s -> Some(x, s)
        | None   -> None

    let omitInputVar (_, b)  = Some(b)

    let exceptionToOption func =
        try
            match func () with
            | res when res <> null -> Some(res) 
            | _                    -> None
        with
        | _ -> None

    module Asnyc =

        /// <summary>
        /// Fish operator (Kleisli Category) for composing functions returning Option values (Railway Oriented Programming).
        /// Asynchronous version
        /// </summary>
        /// <param name="f1">function with one input parameter 'a returning an option&lt;'b&gt;</param>
        /// <param name="f2">function with one input parameter 'b returning an option&lt;'c&gt;</param>
        /// <param name="x">input parameter 'a</param>
        /// <returns>function with one input parameter 'a returning an option&lt;'c&gt;</returns>
        let (>=>) f1 f2 x = async {
            match! f1 x with
            | Some s -> return! f2 s
            | None   -> return None
        }

        /// <summary>
        /// Maps the Some value by  calling function f, leaving the None value
        /// Asynchronous version
        /// <param name="f">function with one input parameter 'a returning 'b</param>
        /// <param name="x">input parameter option&lt;'a&gt;</param>
        /// <returns>option&lt;'b&gt;</returns>
        let map f x = async {
            match! x with
            | Some y ->
                let! s = f y
                return Some s
            | None -> return None
        }
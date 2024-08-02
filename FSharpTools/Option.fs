namespace FSharpTools

module Option =

    /// <summary>
    /// Invoking a function an catching the exception, returning either Some 'a or None, and also mapping null to None
    /// </summary>    
    /// <param name="func">function returning 'a</param>
    /// <returns>option&lt;'a&gt;</returns>
    let catch func =
        try
            match func () with
            | res when res <> null -> Some(res) 
            | _ -> None
        with
        | _ -> None

    /// <summary>
    /// Helper function for composing functions with Fish operator with option (Railway Oriented Programming)
    /// </summary>
    /// <param name="f">function with one input parameter 'a returning 'b</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>option&lt;'b&gt;</returns>
    let switch f x = f x |> Some         

    let withInputVar switch x = 
        match switch x with
        | Some s -> Some(x, s)
        | None   -> None

    let omitInputVar (_, b)  = Some(b)

    let iterAsync action option = async {
        match option with
        | Some o -> do! action o
        | None -> ()
    }

//     let OptionFrom2Options a b = 
//         match a, b with
//         | Some a, Some b -> Some (a, b)
//         | _              -> None
//     /// <summary>
//     /// Bind operator for composing functions returning Option values (Railway Oriented Programming).
//     /// </summary>
//     /// <param name="binder">function with one input parameter 'a returning an option&lt;'b&gt;</param>
//     /// <param name="x">input parameter 'a</param>
//     /// <returns>option&lt;'b&gt;</returns>
//     let (>>=) x binder = Option.bind binder x

//     /// <summary>
//     /// Fish operator (Kleisli Category) for composing functions returning Option values (Railway Oriented Programming).
//     /// </summary>
//     /// <param name="f1">function with one input parameter 'a returning an option&lt;'b&gt;</param>
//     /// <param name="f2">function with one input parameter 'b returning an option&lt;'c&gt;</param>
//     /// <param name="x">input parameter 'a</param>
//     /// <returns>function with one input parameter 'a returning an option&lt;'c&gt;</returns>
//     let (>=>) f1 f2 x = f1 x >>= f2

//     /// <summary>
//     /// Map operator for composing functions returning Option values (Railway Oriented Programming).
//     /// </summary>
//     /// <param name="f">function with one input parameter 'a returning an Option&lt;'b&gt;</param>
//     /// <param name="x">input parameter 'a</param>
//     /// <returns>Option&lt;'b&gt;</returns>
//     let (|>>) x f = Option.map f x


//     /// <summary>
//     /// Maps the Error value of a Result to an option, discarding the Ok value
//     /// </summary>
//     /// <param name="result">Result from which the error should be taken</param>
//     let mapOnlyError result = 
//         match result with
//         | Ok    _ -> None
//         | Error u -> Some u


//     let exceptionToOption func =
//         try
//             match func () with
//             | res when res <> null -> Some(res) 
//             | _                    -> None
//         with
//         | _ -> None


namespace FSharpTools

module Result =
    /// <summary>
    /// Bind operator for composing functions returning Result values (Railway Oriented Programming).
    /// </summary>
    /// <param name="binder">function with one input parameter 'a returning an Result&lt;'b&gt;</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>Result&lt;'b&gt;</returns>
    let (>>=) x binder = Result.bind binder x

    /// <summary>
    /// Fish operator (Kleisli Category) for composing functions returning Result values (Railway Oriented Programming).
    /// </summary>
    /// <param name="f1">function with one input parameter 'a returning a Result&lt;'b&gt;</param>
    /// <param name="f2">function with one input parameter 'b returning a Result&lt;'c&gt;</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>function with one input parameter 'a returning a Result&lt;'c&gt;</returns>
    let (>=>) f1 f2 x = f1 x >>= f2

    /// <summary>
    /// Map operator for composing functions returning Result values (Railway Oriented Programming).
    /// </summary>
    /// <param name="binder">function with one input parameter 'a returning an Result&lt;'b&gt;</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>Result&lt;'b&gt;</returns>
    let (|>>) x f = Result.map f x

    /// <summary>
    /// Helper function for composing functions with Fish operator with Result values (Railway Oriented Programming)
    /// </summary>
    /// <param name="f">function with one input parameter 'a returning 'b</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>Result&lt;'b&gt;</returns>
    let switch f x = f x |> Ok 

    /// <summary>
    /// Monad which takes a function which can throw an exception. The exception 
    /// or the result is wrapped in an Result<'a, exn>
    /// </summary>
    /// <param name="f">function with no input parameter () returning 'a</param>
    /// <returns>Result&lt;'a, exn&gt;</returns>
    let exceptionToResult func =
        try
            Ok(func ()) 
        with
        | e -> Error e

    /// <summary>
    /// Raising an exception from a Result containing an error value, 
    /// otherwise returning the Ok value
    /// </summary>
    /// <param name="result">Result from which the error should be thrown as exception</param>
    let throw result = 
        match result with
        | Ok value  -> value
        | Error exn -> raise exn

    /// <summary>
    /// Maps an error value in Result to another error type
    /// </summary>
    let mapError f x = 
        match x with
        | Ok ok   -> Ok ok
        | Error e -> Error <| f e
            
    /// <summary>
    /// Running function returning 'a and converting 'a' to Result&lt;'a,exn&gt;. If an exception occurs, it will be put in the Result
    /// </summary>
    let catch (func: Unit->'a) = 
        try
            Ok <| func ()
        with         
            exn -> Error exn 
    
    /// <summary>
    /// Runs funToRun one time, repeated run on Error up to 'count' times after wait peroid
    /// </summary>
    /// <param name="waitTime">Time to wait between runs</param>
    /// <param name="count">Max count the function is to be performed</param>
    /// <param name="funToRun">Function is to be executed returning Result</param>
    /// <returns>Result of the last executed function</returns>
    let rec repeatOnError (waitTime: System.TimeSpan) count funToRun = 
        match count, funToRun () with
        | 1, res -> res
        | _, Ok ok         -> Ok ok
        | _, Error _            -> 
            System.Threading.Thread.Sleep waitTime
            repeatOnError waitTime (count-1) funToRun

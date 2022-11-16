namespace FSharpTools

module AsyncResult =
    /// <summary>
    /// Binds the Ok value by calling function f, leaving the Err value
    /// <param name="f">function with one input parameter 'a returning 'b</param>
    /// <param name="x">input parameter Result&lt;'a&gt;</param>
    /// <returns>Result&lt;'b&gt;</returns>
    let bind f x = async {
        match! x with
        | Ok s    -> return! f s
        | Error e -> return Error e
    }

    /// <summary>
    /// Bind operator for composing functions returning Result values (Railway Oriented Programming).
    /// </summary>
    /// <param name="binder">function with one input parameter 'a returning an Result&lt;'b&gt;</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>Result&lt;'b&gt;</returns>
    let (>>=) x binder = bind binder x

    /// <summary>
    /// Maps the Ok value by  calling function f, leaving the Err value
    /// Asynchronous version
    /// <param name="f">function with one input parameter 'a returning 'b</param>
    /// <param name="x">input parameter Result&lt;'a&gt;</param>
    /// <returns>Result&lt;'b&gt;</returns>
    let map f x = async {
        match! x with
        | Ok y ->
            let! s = f y
            return Ok s
        | Error e -> return Error e
    }

    /// <summary>
    /// Map operator for composing functions returning Result values (Railway Oriented Programming).
    /// </summary>
    /// <param name="binder">function with one input parameter 'a returning an Result&lt;'b&gt;</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>Result&lt;'b&gt;</returns>
    let (|>>) x f = map f x
    
    /// <summary>
    /// Fish operator (Kleisli Category) for composing functions returning Result values (Railway Oriented Programming).
    /// </summary>
    /// <param name="f1">function with one input parameter 'a returning a Result&lt;'b&gt;</param>
    /// <param name="f2">function with one input parameter 'b returning a Result&lt;'c&gt;</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>function with one input parameter 'a returning a Result&lt;'c&gt;</returns>
    let (>=>) f1 f2 x = f1 x >>= f2

    /// <summary>
    /// Monad which takes a function which can throw an exception. The exception 
    /// or the result is wrapped in an Result<'a, exn>
    /// </summary>
    /// <param name="f">function with no input parameter () returning 'a</param>
    /// <returns>Result&lt;'a, exn&gt;</returns>
    let exceptionToResult = Result.exceptionToResult

    /// <summary>
    /// Raising an exception from a Result containing an error value, 
    /// otherwise returning the Ok value
    /// </summary>
    /// <param name="result">Result from which the error should be thrown as exception</param>
    let throw = Result.throw

    /// <summary>
    /// Maps an error value in Result to another error type
    /// </summary>
    let mapError f x = async {
        match! x with
        | Ok ok -> return Ok ok
        | Error e -> 
            let err = f e
            return Error err
    }


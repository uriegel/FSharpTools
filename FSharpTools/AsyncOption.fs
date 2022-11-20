namespace FSharpTools

module AsyncOption =
    /// <summary>
    /// Binds the Some value by calling function f
    /// </summary>
    /// <param name="f">function with one input parameter 'a returning an option&lt;'b&gt;</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>option&lt;'b&gt;</returns>
    let bind f x = async {
        match! x with
        | Some s    -> return! f s
        | None -> return None
    }

    /// <summary>
    /// Maps the Some value by calling function f, leaving the None value
    /// Asynchronous version
    /// </summary>
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

    /// <summary>
    /// Map operator for composing functions returning Option values (Railway Oriented Programming).
    /// </summary>
    /// <param name="f">function with one input parameter 'a returning an Option&lt;'b&gt;</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>Option&lt;'b&gt;</returns>
    let (|>>) x f = map f x

    /// <summary>
    /// Bind operator for composing functions returning Option values (Railway Oriented Programming).
    /// </summary>
    /// <param name="binder">function with one input parameter 'a returning an option&lt;'b&gt;</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>option&lt;'b&gt;</returns>
    let (>>=) x binder = bind binder x

    /// <summary>
    /// Fish operator (Kleisli Category) for composing functions returning Option values (Railway Oriented Programming).
    /// </summary>
    /// <param name="f1">function with one input parameter 'a returning an option&lt;'b&gt;</param>
    /// <param name="f2">function with one input parameter 'b returning an option&lt;'c&gt;</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>function with one input parameter 'a returning an option&lt;'c&gt;</returns>
    let (>=>) f1 f2 x = f1 x >>= f2

    /// <summary>
    /// Maps the Error value of a Result to an option, discarding the Ok value
    /// </summary>
    /// <param name="result">Result from which the error should be taken</param>
    let mapOnlyError = Option.mapOnlyError

    let exceptionToOption = Option.exceptionToOption



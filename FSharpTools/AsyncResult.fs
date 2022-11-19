namespace FSharpTools

open System
open System.Threading.Tasks

open Async

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
    /// <param name="binder">function with one input parameter 'a returning an Result&lt;'c, 'b&gt;</param>
    /// <param name="x">input parameter 'a</param>
    /// <returns>Result&lt;'b&gt;</returns>
    let (>>=) x binder = bind binder x

    /// <summary>
    /// Maps the Ok value by  calling function f, leaving the Err value.
    /// Asynchronous version
    /// </summary>
    /// <param name="f">function with one input parameter 'a returning Async&lt;'b&gt;</param>
    /// <param name="x">input parameter Async&lt;Result&lt;'a, 'c&gt;&gt;</param>
    /// <returns>Async&lt;Result&lt;'b, 'c&gt;&gt;</returns>
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

    /// <summary>
    /// Running function returning Task&lt;'a&gt; and converting 'a' to Result&lt;'a,exn&gt;. If an exception occurs, it will be put in the Result
    /// </summary>
    let catch (asyncFunc: Unit->Task<'a>) = 
        let toResult (task: Task<'a>) = 
            let continueFrom ((ok: Result<'a, exn> -> Unit), _, _) = 
                let continueWith (task: Task<'a>) =
                    let result = 
                        if task.IsCompletedSuccessfully then
                            Ok task.Result
                        elif task.IsFaulted && task.Exception.InnerException <> null then
                            Error task.Exception.InnerException
                        elif task.IsFaulted then
                            Error task.Exception
                        elif task.IsCanceled then
                            Error <| TaskCanceledException ()
                        else
                            Error <| Exception ()
                    ok result
                task.ContinueWith continueWith 
                |> ignore

            Async.FromContinuations continueFrom
        try
            asyncFunc ()
            |> toResult

        with         
            exn -> Error exn |> toAsync

    /// <summary>
    /// Runs funToRun one time, repeated run on Error up to 'count' times after wait peroid
    /// </summary>
    /// <param name="waitTime">Time to wait between runs</param>
    /// <param name="count">Max count the function is to be performed</param>
    /// <param name="funToRun">Function is to be executed returning Result</param>
    /// <returns>Asynchron result of the last executed function</returns>
    let rec repeatOnError (waitTime: TimeSpan) count funToRun = async {
        let! res = funToRun ()
        match count, res with
        | 1, res -> return res
        | _, Ok ok         -> return Ok ok
        | _, Error _            -> 
            do! Async.Sleep (int waitTime.TotalMilliseconds)
            return! repeatOnError waitTime (count-1) funToRun
    }

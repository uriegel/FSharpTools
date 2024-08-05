namespace FSharpTools
open System.Threading.Tasks
open System

module AsyncResult =

    type AsyncResult<'a, 'e> = {
        value: Async<Result<'a, 'e>>
    }

    let toAsyncResultAwait (ar: Async<Result<'a, 'e>>): AsyncResult<'a, 'e> = {value = ar }
    let toAsyncResult (r: Result<'a, 'e>): AsyncResult<'a, 'e> = {value = async { return r } }
    let toResult a = a.value

    let catch (func: unit -> Task<'a>) : AsyncResult<'a, Exception> =  
        async {
            let! res = func ()  |> Async.AwaitTask |> Async.Catch
            return 
                match res with
                | Choice1Of2 r -> Ok r
                | Choice2Of2 e -> Error e
        } |> toAsyncResultAwait


    type AsyncResult<'a, 'e> with
        static member bind (f: 'a -> AsyncResult<'b, 'e>) (ar: AsyncResult<'a, 'e>) : AsyncResult<'b, 'e> = 
            async {
                let! x = ar.value
                let x = match x with
                                | Ok ok -> f ok
                                | Error err ->  Error err |> toAsyncResult
                return! x.value
            } |> toAsyncResultAwait
        
        static member map (f: 'a -> 'b) (ar: AsyncResult<'a, 'e>) : AsyncResult<'b, 'e> = 
            async {
                let! x = ar.value
                let x = match x with
                                | Ok ok -> Ok <| f ok
                                | Error err ->  Error err
                return x
            } |> toAsyncResultAwait

        static member mapError (f: 'e -> 'e1) (ar: AsyncResult<'a, 'e>) : AsyncResult<'a, 'e1> = 
            async {
                let! x = ar.value
                let x = match x with
                                | Ok ok -> Ok ok
                                | Error err ->  Error <| f err
                return x
            } |> toAsyncResultAwait

        static member bindToError (f: 'a -> Result<'a, 'e>) (ar: AsyncResult<'a, 'e>) : AsyncResult<'a, 'e> = 
            async {
                let! x = ar.value
                let x = match x with
                                | Ok ok -> f ok
                                | Error err ->  Error err
                return x
            } |> toAsyncResultAwait

        static member bindToOk (f: 'e -> Result<'a, 'e>) (ar: AsyncResult<'a, 'e>) : AsyncResult<'a, 'e> = 
            async {
                let! x = ar.value
                let x = match x with
                                | Ok ok -> Ok ok
                                | Error err ->  f err
                return x
            } |> toAsyncResultAwait

        static member (>>=) ((ar: AsyncResult<'a, 'e>), (f: 'a -> AsyncResult<'b, 'e>)) = 
            ar |> (fun x ->  AsyncResult.bind f x)


    //     /// <summary>
    //     /// Bind operator for composing functions returning Result values (Railway Oriented Programming).
    //     /// </summary>
    //     /// <param name="binder">function with one input parameter 'a returning an Result&lt;'c, 'b&gt;</param>
    //     /// <param name="x">input parameter 'a</param>
    //     /// <returns>Result&lt;'b&gt;</returns>
    //     let (>>=) x binder = bind binder x

    //     /// <summary>
    //     /// Map operator for composing functions returning Result values (Railway Oriented Programming).
    //     /// </summary>
    //     /// <param name="binder">function with one input parameter 'a returning an Result&lt;'b&gt;</param>
    //     /// <param name="x">input parameter 'a</param>
    //     /// <returns>Result&lt;'b&gt;</returns>
    //     let (|>>) x f = map f x
        
    //     /// <summary>
    //     /// Fish operator (Kleisli Category) for composing functions returning Result values (Railway Oriented Programming).
    //     /// </summary>
    //     /// <param name="f1">function with one input parameter 'a returning a Result&lt;'b&gt;</param>
    //     /// <param name="f2">function with one input parameter 'b returning a Result&lt;'c&gt;</param>
    //     /// <param name="x">input parameter 'a</param>
    //     /// <returns>function with one input parameter 'a returning a Result&lt;'c&gt;</returns>
    //     let (>=>) f1 f2 x = f1 x >>= f2

    //     /// <summary>
    //     /// Monad which takes a function which can throw an exception. The exception 
    //     /// or the result is wrapped in an Result<'a, exn>
    //     /// </summary>
    //     /// <param name="f">function with no input parameter () returning 'a</param>
    //     /// <returns>Result&lt;'a, exn&gt;</returns>
    //     let exceptionToResult = Result.exceptionToResult

    //     /// <summary>
    //     /// Raising an exception from a Result containing an error value, 
    //     /// otherwise returning the Ok value
    //     /// </summary>
    //     /// <param name="result">Result from which the error should be thrown as exception</param>
    //     let throw = Result.throw

    //     /// <summary>
    //     /// Maps an error value in Result to another error type
    //     /// </summary>
    //     let mapError f x = async {
    //         match! x with
    //         | Ok ok -> return Ok ok
    //         | Error e -> 
    //             let err = f e
    //             return Error err
    //     }

    //     /// <summary>
    //     /// Running function returning Task&lt;'a&gt; and converting 'a' to Result&lt;'a,exn&gt;. If an exception occurs, it will be put in the Result
    //     /// </summary>
    //     let catch (asyncFunc: Unit->Task<'a>) = 
    //         let toResult (task: Task<'a>) = 
    //             let continueFrom ((ok: Result<'a, exn> -> Unit), _, _) = 
    //                 let continueWith (task: Task<'a>) =
    //                     let result = 
    //                         if task.IsCompletedSuccessfully then
    //                             Ok task.Result
    //                         elif task.IsFaulted && task.Exception.InnerException <> null then
    //                             Error task.Exception.InnerException
    //                         elif task.IsFaulted then
    //                             Error task.Exception
    //                         elif task.IsCanceled then
    //                             Error <| TaskCanceledException ()
    //                         else
    //                             Error <| Exception ()
    //                     ok result
    //                 task.ContinueWith continueWith 
    //                 |> ignore

    //             Async.FromContinuations continueFrom
    //         try
    //             asyncFunc ()
    //             |> toResult

    //         with         
    //             exn -> Error exn |> toAsync

    //     /// <summary>
    //     /// Runs funToRun one time, repeated run on Error up to 'count' times after wait peroid
    //     /// </summary>
    //     /// <param name="waitTime">Time to wait between runs</param>
    //     /// <param name="count">Max count the function is to be performed</param>
    //     /// <param name="funToRun">Function is to be executed returning Result</param>
    //     /// <returns>Asynchron result of the last executed function</returns>
    //     let rec repeatOnError (waitTime: TimeSpan) count funToRun = async {
    //         let! res = funToRun ()
    //         match count, res with
    //         | 1, res -> return res
    //         | _, Ok ok         -> return Ok ok
    //         | _, Error _            -> 
    //             do! Async.Sleep (int waitTime.TotalMilliseconds)
    //             return! repeatOnError waitTime (count-1) funToRun
    //     }

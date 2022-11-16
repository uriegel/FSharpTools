namespace FSharpTools

open System
open System.Threading.Tasks

module Async = 

    /// <summary>
    /// Function Compostion for 2 async functions
    /// </summary>
    /// <param name="f">function with one input parameter 'a returning asyc result 'b</param>
    /// <param name="f">function with one input parameter 'b returning asyc result 'c</param>
    /// <returns>function with input parameter 'a returning asyc result 'c</returns>
    let (>>) f g x = async {
        let! y = f x
        let! e = g y
        return e
    }

    /// <summary>
    /// Anamorph function lifting 'a to Async&lt;'a&gt;
    /// </summary>
    /// <param name="a">value to lift</param>
    /// <returns>returns lifted value Async&lt;'a&gt;</returns>
    let toAsync a = async {
        return a
    }

    /// <summary>
    /// Running Task&lt;'a&gt; and converting result to Result&lt;'a,exn&gt;. If an exception occurs, it will be put in the Result
    /// </summary>
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

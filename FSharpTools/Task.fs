namespace FSharpTools

module Task =
    open System.Threading.Tasks

    /// <summary>
    /// Converts a Task without result to a Task&lt;Unit&gt;
    /// </summary>
    /// <param name="task">a Task without result</param>
    /// <returns>The converted Task&lt;Unit&gt;</returns>
    let toUnit (task: Task) = 
        task.ContinueWith<Unit> (fun _ -> ())

    /// <summary></summary>
    /// <param name="selector"></param>
    /// <param name="task"></param>
    /// <typeparam name="'a"></typeparam>
    /// <typeparam name="'b"></typeparam>
    /// <returns></returns>
    let map (selector: 'a->'b) (task: Task<'a>) : Task<'b> = 
        task.ContinueWith (fun (ta: Task<'a>) -> selector (ta.Result))

    let bind (selector: 'a->Task<'b>) (task: Task<'a>) = 
        task.ContinueWith<Task<'b>> (fun (ta: Task<'a>) -> selector ta.Result)    
        |> (fun t -> t.Unwrap())
        
    let iter (selector: 'a->unit) (task: Task<'a>) = 
        task.ContinueWith (fun (ta: Task<'a>) -> selector ta.Result)
        |> ignore

    // /// <summary>
    // /// Bind operator for composing functions returning Tasks (Railway Oriented Programming).
    // /// </summary>
    // /// <param name="binder">function with one input parameter 'a returning a Task&lt;'b&gt;</param>
    // /// <param name="x">input parameter 'a</param>
    // /// <returns>Task&lt;'b&gt;</returns>
    // let (>>=) x binder = bind binder x

    // /// <summary>
    // /// Fish operator (Kleisli Category) for composing functions returning Tasks (Railway Oriented Programming).
    // /// </summary>
    // /// <param name="f1">function with one input parameter 'a returning a Task&lt;'b&gt;</param>
    // /// <param name="f2">function with one input parameter 'b returning a Task&lt;'c&gt;</param>
    // /// <param name="x">input parameter 'a</param>
    // /// <returns>function with one input parameter 'a returning a Task&lt;'c&gt;</returns>
    // let (>=>) f1 f2 x = f1 x >>= f2

    // /// <summary>
    // /// Map operator for composing functions returning Tasks (Railway Oriented Programming).
    // /// </summary>
    // /// <param name="f">function with one input parameter 'a returning a Task&lt;'b&gt;</param>
    // /// <param name="x">input parameter 'a</param>
    // /// <returns>Task&lt;'b&gt;</returns>
    // let (|>>) x f = map f x

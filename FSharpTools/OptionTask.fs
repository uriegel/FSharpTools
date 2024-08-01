namespace FSharpTools

// module OptionTask =
//     open System.Threading.Tasks

//     type optiontask<'a> = Task<Option<'a>>

//     let map (selector: 'a->'b) (opt: optiontask<'a>) : optiontask<'b> = 
//         if opt.IsCompleted then
//             Task.FromResult (opt.Result
//                 |> Option.map selector)
//         else            
//             opt.ContinueWith (fun (ta: optiontask<'a>) -> 
//                 ta.Result
//                 |> Option.map selector)

//     let bindNoTask (selector: 'a->Option<'b>) (opt: optiontask<'a>) : optiontask<'b> = 
//         if opt.IsCompleted then
//             Task.FromResult (opt.Result
//                 |> Option.bind selector)
//         else            
//             opt.ContinueWith (fun (ta: optiontask<'a>) -> 
//                 ta.Result 
//                 |> Option.bind selector)    
        
//     let bind (selector: 'a->optiontask<'b>) (opt: optiontask<'a>) : optiontask<'b> = 
//         // TODO task.isCompleted
//         let cont (ta: optiontask<'a>): optiontask<'b> = 

//             let a = ta.Result
//             match a with
//             | Some n -> selector n
//             | None -> Task.FromResult(None)

//         let e = opt.ContinueWith cont
//         e.Unwrap()

//     let iter (selector: 'a->unit) (opt: optiontask<'a>) = 
//         if opt.IsCompleted then
//             Option.iter selector opt.Result
//             |> ignore
//         else
//             opt.ContinueWith (fun (ta: optiontask<'a>) -> ta.Result |> Option.iter selector)
//             |> ignore

//     /// <summary>
//     /// Bind operator for composing functions returning OptionTasks (Railway Oriented Programming).
//     /// </summary>
//     /// <param name="binder">function with one input parameter 'a returning an OptionTask&lt;'b&gt;</param>
//     /// <param name="x">input parameter 'a</param>
//     /// <returns>OptionTask&lt;'b&gt;</returns>
//     let (>>=) x binder = bind binder x

//     /// <summary>
//     /// Fish operator (Kleisli Category) for composing functions returning OptionTasks (Railway Oriented Programming).
//     /// </summary>
//     /// <param name="f1">function with one input parameter 'a returning an OptionTask&lt;'b&gt;</param>
//     /// <param name="f2">function with one input parameter 'b returning an OptionTask&lt;'c&gt;</param>
//     /// <param name="x">input parameter 'a</param>
//     /// <returns>function with one input parameter 'a returning an OptionTask&lt;'c&gt;</returns>
//     let (>=>) f1 f2 x = f1 x >>= f2

//     /// <summary>
//     /// Map operator for composing functions returning OptionTasks (Railway Oriented Programming).
//     /// </summary>
//     /// <param name="f">function with one input parameter 'a returning an OptionTask&lt;'b&gt;</param>
//     /// <param name="x">input parameter 'a</param>
//     /// <returns>OptionTask&lt;'b&gt;</returns>
//     let (|>>) x f = map f x

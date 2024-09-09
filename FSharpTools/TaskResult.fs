namespace FSharpTools
open System.Threading.Tasks

module TaskResult =

    type TaskResult<'a, 'e> = {
        value: Task<Result<'a, 'e>>
    }

    let toTaskResultAwait (ar: Task<Result<'a, 'e>>): TaskResult<'a, 'e> = {value = ar }
    let toTaskResult (r: Result<'a, 'e>): TaskResult<'a, 'e> = {value = task { return r } }
    let toResult a = a.value

    type TaskResult<'a, 'e> with
        static member bind (f: 'a -> TaskResult<'b, 'e>) (ar: TaskResult<'a, 'e>) : TaskResult<'b, 'e> = 
            task {
                let! x = ar.value
                let x = match x with
                                | Ok ok -> f ok
                                | Error err ->  Error err |> toTaskResult
                return! x.value
            } |> toTaskResultAwait
        
        static member map (f: 'a -> 'b) (ar: TaskResult<'a, 'e>) : TaskResult<'b, 'e> = 
            task {
                let! x = ar.value
                let x = match x with
                                | Ok ok -> Ok <| f ok
                                | Error err ->  Error err
                return x
            } |> toTaskResultAwait

        static member mapError (f: 'e -> 'e1) (ar: TaskResult<'a, 'e>) : TaskResult<'a, 'e1> = 
            task {
                let! x = ar.value
                let x = match x with
                                | Ok ok -> Ok ok
                                | Error err ->  Error <| f err
                return x
            } |> toTaskResultAwait

        static member bindToError (f: 'a -> Result<'a, 'e>) (ar: TaskResult<'a, 'e>) : TaskResult<'a, 'e> = 
            task {
                let! x = ar.value
                let x = match x with
                                | Ok ok -> f ok
                                | Error err ->  Error err
                return x
            } |> toTaskResultAwait

        static member bindToOk (f: 'e -> Result<'a, 'e>) (ar: TaskResult<'a, 'e>) : TaskResult<'a, 'e> = 
            task {
                let! x = ar.value
                let x = match x with
                                | Ok ok -> Ok ok
                                | Error err ->  f err
                return x
            } |> toTaskResultAwait

        static member (>>=) ((ar: TaskResult<'a, 'e>), (f: 'a -> TaskResult<'b, 'e>)) = 
            ar |> (fun x ->  TaskResult.bind f x)


    
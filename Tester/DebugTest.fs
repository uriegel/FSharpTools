module DebugTest

open System.Diagnostics
open FSharpTools
open Task
open System.Threading.Tasks
open Option
open OptionTask
open Functional

let runTests () = 
    let getString (text: string) = 
        Task.Delay 10000
        |> toUnit 
        |>> fun () -> text

    // getString "Hello"
    // |>> fun b -> b + " Welt"
    // |> iter (printfn "%s") 

    let addStringAsync (ts: Task<string>) (str: string) = 
        ts 
        |>> fun s -> str + " " + s
    
    getString "Hello"
        |> Task.bind (addStringAsync (getString "Wörld"))
        |> Task.iter (printfn "%s") 

    getString "Hello"
        >>= addStringAsync (getString "Wörld👍")
        |> Task.iter (printfn "%s") 

    let getNullableString isNotNull = 
        if isNotNull then "Not null" else null

    let getOptionString = getNullableString >> ofObj

    getOptionString true
        |>> fun s -> "Ein string: " + s
        |> iter (printfn "%s") 

    // getOptionString false
    //     |>> fun s -> "Ein string: " + s
    //     |> iter (printfn "%s") 

    let getOptionTaskString b = 
        Task.Delay 10000
        |> toUnit 
        |>> fun () -> (getNullableString b) |> ofObj

    getOptionTaskString true
        |> map (fun s -> "Ein string (Task): " + s)
        |> iter (printfn "Optiontask: %s") 

let run () = async {
    runTests ()
    System.Console.ReadLine ()
    let! result  = Process.asyncRun "ls" "-la"
    let! result2 = Process.asyncRun "lsf" "-la"
    let! result3 = Process.asyncRun "ls" "-Wrong"
    ()

    let urls = [ 
        "/"
        "/index.html"
        "/web/index.html"
        "/web/styles/style.css"
        "/methods/query"
        "/app/methods/query" 
        "/app/methods/query?param=test&param2=67" ]

    let test url =
        let query = UrlQueryComponents.create url
        printfn "url: %s method: %O" url query

    urls |> List.iter test

    let performaceTest url = 
        printfn "Starting performance test: %s" url
        let stopwatch = Stopwatch()
        stopwatch.Start ()

        for i in [1 .. 1_000_000 ] do
            let query = UrlQueryComponents.create url
            ()

        let elapsed = stopwatch.Elapsed
        printfn "Duration: %O" elapsed

    urls |> List.iter performaceTest
}

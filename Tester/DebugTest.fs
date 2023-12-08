module DebugTest

open System.Diagnostics
open FSharpTools
open Option 

type EvenInteger = option<int>

let fromEven i = 
    if (i % 2) = 0 then
        Some i
    else
        None

let runTests () =
    let a = fromEven 23
    let b = fromEven 24

    b 
    |>> (+) 17
    |> defaultValue 0
    |> printf "%d" 
    ()

let run () = async {
    runTests ()
    System.Console.ReadLine () |> ignore
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

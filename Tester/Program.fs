// Learn more about F# at http://fsharp.org

open System
open System.Diagnostics

[<EntryPoint>]
let main argv =

    let urls = [ 
        "http://localhost"
        "http://localhost/index.html"
        "http://localhost/web/index.html"
        "http://localhost/web/styles/style.css"
        "http://localhost/methods/query"
        "http://localhost/app/methods/query" 
        "http://localhost/app/methods/query?param=test&param2=67" ]

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
    
    0 // return an integer exit code

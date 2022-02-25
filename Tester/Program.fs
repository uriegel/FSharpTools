open System.Diagnostics
open FSharpTools

async {
    let! result  = Process.run "ls" "-la"
    let! result2 = Process.run "lsf" "-la"
    let! result3 = Process.run "ls" "-Wrong"
    ()
} |> Async.RunSynchronously


JsonTest.deserializeTest ()    

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


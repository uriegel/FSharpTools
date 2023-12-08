module DebugTest

open System.Diagnostics
open System.Threading.Tasks

open FSharpTools

let fromEven i = 
    if (i % 2) = 0 then
        Some i
    else
        None

open Option 

let runOptionTests () =
    let a = fromEven 23
    let b = fromEven 24
    let addToEven (a: int) (b: int) = 
        fromEven (a + b)

    let affe = addToEven 56

    let rwo value = 
        value 
        |>> (+) 17
        |>> sprintf "%d"
        |>> (+) "Als Text: "
        |> defaultValue "0"

    printfn "%s" <| rwo a
    printfn "%s" <| rwo b

    let erg1 = 
        fromEven 40
        >>= addToEven 90
    let erg2 = 
        fromEven 40
        >>= addToEven 91
    let erg3 = 
        fromEven 41
        >>= addToEven 91

    let fromEvenAddToEven a = fromEven >=> addToEven a
    let erg3 = fromEvenAddToEven 24 88
    let erg4 = fromEvenAddToEven 24 83
            
    ()

open Task
// TODO Railway oriented concatination of functions returning Tasks 

let runTaskTests () =
    let getString (text: string) = 
        Task.Delay 5000
        |> toUnit 
        |>> fun () -> text

    getString "test"

// TODO Railway oriented concatination of optiontasks 

let run () = async {
    runOptionTests ()
    let! res = runTaskTests () |> Async.AwaitTask
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

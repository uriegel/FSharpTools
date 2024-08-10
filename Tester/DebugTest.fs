module DebugTest

open System.Diagnostics
open System.Threading.Tasks

open FSharpPlus
open FSharpTools
open System

type Kontakt = {
    Name: string
    Number: int
}

let kontakt = {
    Name = "Uwe Riegel"
    Number = 89
}

let fromEven i = 
    if (i % 2) = 0 then
        Some i
    else
        None
let runOptionTests () =
    let a = fromEven 23
    let b = fromEven 24
    let addToEven (a: int) (b: int) = 
        fromEven (a + b)

    let rwo value = 
        value 
        |>> (+) 17
        |>> sprintf "%d"
        |>> (+) "Als Text: "
        |> Option.defaultValue "0"

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

let runTaskTests () =
    let getString (text: string) = 
        Task.Delay 5000
        |> Task.toUnit 
        |>> fun () -> text

    let addString (a: string) =
        Task.FromResult a 
        |>> (fun a -> a + " is extended") 

    let erg = 
        getString "test"
        |>> (fun a -> a + " + addition")
        >>= addString

    let getStringAdded = getString >=> addString
    let erg1 = getStringAdded "Begin"

    erg1
    
// TODO Railway oriented concatination of optiontasks 

let run () = async {

    let x = TextJson.serialize kontakt
    let k: Kontakt = TextJson.deserialize x


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

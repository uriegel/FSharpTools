module DebugTest

open System.Diagnostics
open FSharpTools
open Task
open System.Threading.Tasks
open Option 
open OptionTask
open Functional
    
// type Test< ^T when ^T : (static member Add : ^T -> ^T -> ^T)> = 
//     static member Add (a: string) (b: string) =
//         a + b
//     static member Add (a: int) (b: int) =
//         a + b
    
//     static member inline Add1 (a: ^T) (b: ^T) =
//         Add a b

// TODO options operator |>> >=> >>=
// TODO Task operator |>> >=> >>=


type Ext = Ext
    with
        static member Bar (ext: Ext, flt: float) = 1.0 + flt
        static member Bar (ext: Ext, i: int) = 1 + i

let inline bar (x: ^a) = 
    ((^b or ^a) : (static member Bar : ^b * ^a -> ^a) (Ext, x))

type Affe(x: int) =
    member this.x = x
    static member Addiere (a: Affe, b: Affe) =
        a.x + b.x

type Schwein(x: int) =
    member this.x = x
    static member Addiere2 (a: Schwein) (b: Schwein) =
        a.x + b.x
    static member Addiere (a: Schwein, b: Schwein) =
        a.x - b.x

let inline Addiere< ^T when ^T : 
    (static member Addiere : ^T * ^T -> int)> (a:^T, b:^T) : int =
    printfn "%s" "Bin drinne"
    (^T : (static member Addiere : ^T * ^T -> int) ( a, b))
    
let inline Addiere2< ^T when ^T :
    (static member Addiere : ^T -> ^T -> int)> (a:^T)  (b:^T) : int =
    printfn "%s" "Bin drinne"
    (^T : (static member Addiere : ^T * ^T -> int) ( a, b))

let inline check< ^T when ^T : 
    (static member IsInfinity : ^T -> bool)> (num:^T) : option< ^T > =
    printfn "%s" "Bin drinne"
    if (^T : (static member IsInfinity : ^T -> bool) (num)) then None
    else Some num

let inline (+++) a b = Addiere2 a b
let inline (~%) a = bar a 
// let inline (~+++)< ^T when ^T : 
//     (static member IsInfinity : ^T -> bool)> (num:^T) : option< ^T > =
//     printfn "%s" "Bin drinne +++"
//     if (^T : (static member IsInfinity : ^T -> bool) (num)) then None
//     else Some num

//let (+++) = check

let runTests () =
    let a = Affe(3)
    let b = Affe(4)
    let c = Schwein(3)
    let d = Schwein(4)
    let erg = Addiere (a, b)
    let erg2 = Addiere2 a b
    let erg3 = a +++ b
    let erg4 = c +++ d

    let erg5 = bar 7.0
    let erg6 = bar 45
    //let a = check 42.0 
    let erg7 = % 4
    let erg8 = %3.5
    
    //val it : float option = Some(42)
    //let b = +++(1.0f / 0.0f) 
    //val it : float32 option = null
    //let c = check (1 / 2)
    () 
    // let getString (text: string) = 
    //     Task.Delay 10000
    //     |> toUnit 
    //     |>> fun () -> text

    // // getString "Hello"
    // // |>> fun b -> b + " Welt"
    // // |> iter (printfn "%s") 

    // let addStringAsync (ts: Task<string>) (str: string) = 
    //     ts 
    //     |>> fun s -> str + " " + s
    
    // getString "Hello"
    //     |> Task.bind (addStringAsync (getString "Wörld"))
    //     |> Task.iter (printfn "%s") 

    // getString "Hello"
    //     >>= addStringAsync (getString "Wörld👍")
    //     |> Task.iter (printfn "%s") 

    // let getNullableString isNotNull = 
    //     if isNotNull then "Not null" else null

    // let getOptionString = getNullableString >> ofObj

    // getOptionString true
    //     |>> fun s -> "Ein string: " + s
    //     |> iter (printfn "%s") 

    // // getOptionString false
    // //     |>> fun s -> "Ein string: " + s
    // //     |> iter (printfn "%s") 

    // let getOptionTaskString b = 
    //     Task.Delay 10000
    //     |> toUnit 
    //     |>> fun () -> (getNullableString b) |> ofObj

    // getOptionTaskString true
    //     |> map (fun s -> "Ein string (Task): " + s)
    //     |> iter (printfn "Optiontask: %s") 

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

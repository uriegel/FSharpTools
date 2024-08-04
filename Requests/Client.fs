namespace FSharpTools.Requests
open System
open System.Net.Http
open FSharpTools.Functional

module Client =
    let mutable private maxConnectionsVal = 8
    let mutable private timeoutVal = TimeSpan.FromSeconds 100

    let init maxConnections =
        maxConnectionsVal <- maxConnections

    let initWithTimeout maxConnections timeout =
        maxConnectionsVal <- maxConnections
        timeoutVal <- timeout


    let getClient = 
        let getClient () =
            new HttpClient(new HttpClientHandler(
                MaxConnectionsPerServer = maxConnectionsVal),
                Timeout = timeoutVal)
        memoizeSingle getClient  



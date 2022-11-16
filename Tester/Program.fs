module Program  

let [<EntryPoint>] main _ = 
    DebugTest.run () 
    |> Async.RunSynchronously
    0


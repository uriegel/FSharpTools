namespace FSharpTools

module Process = 
    open FSharpRailway
    open System
    open System.Threading

    type ProcessResult = {
        Output: string option
        Error: string option
        ExitCode: int option
        Exception: Exception option
    }

    let run fileName args = 
        async {
            try 
                use proc = new Diagnostics.Process() 
                proc.StartInfo <- Diagnostics.ProcessStartInfo()
                proc.StartInfo.RedirectStandardOutput <- true
                proc.StartInfo.RedirectStandardError <- true
                proc.StartInfo.FileName <- fileName
                proc.StartInfo.Arguments <- args
                proc.StartInfo.CreateNoWindow <- true

                proc.Start() |> ignore
                do! proc.WaitForExitAsync CancellationToken.None |> Async.AwaitTask
                let! responseString = proc.StandardOutput.ReadToEndAsync () |> Async.AwaitTask
                let! errorString = proc.StandardError.ReadToEndAsync () |> Async.AwaitTask
                return {
                    Output = if responseString.Length > 0 then Some responseString else None
                    Error = if errorString.Length > 0 then Some errorString else None
                    ExitCode = Some proc.ExitCode
                    Exception = None
                }
            with
                | e -> return {
                        Output = None
                        Error = None
                        ExitCode = None
                        Exception = Some e
                    }
        }

    open Async

    /// <summary>
    /// Runs a cmd returning a string
    /// </summary>
    /// <param name="cmd">Command (process name)</param>
    /// <param name="args">Arguemtn list</param> 
    /// <returns>Returned msg as string</returns>
    let runCmd cmd args = 
        let getStringFromResult (result: ProcessResult) = async { return result.Output.Value } 
        let runCmd () = run cmd args
        runCmd >> getStringFromResult

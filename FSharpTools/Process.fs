namespace FSharpTools
module Process = 

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
                | :? Exception as e -> return {
                        Output = None
                        Error = None
                        ExitCode = None
                        Exception = Some e
                    }
                | _ as e -> return {
                        Output = None
                        Error = None
                        ExitCode = None
                        Exception = Some (Exception(e.ToString()))
                    }
        }
        
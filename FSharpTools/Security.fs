module Security
open System
open System.Runtime.InteropServices

let readPasswd () =
    let rec readKey charList =
        match (Console.ReadKey true).KeyChar with
        | '\r' | '\n' -> 
            Console.WriteLine ()
            charList
        | '\b' -> 
            match charList with
            | head :: tail -> 
                Console.Write "\b \b"
                readKey <| tail 
            | [] -> readKey []        
        | chr -> 
            Console.Write '*'
            readKey <| chr :: charList
    let secstr = new Security.SecureString ()
    readKey []
    |> List.rev
    |> List.iter secstr.AppendChar
    secstr

let readSecureString (secstr: Security.SecureString) =
    let mutable valuePtr = IntPtr.Zero
    try 
        valuePtr <- Marshal.SecureStringToGlobalAllocUnicode secstr
        Marshal.PtrToStringUni valuePtr
    finally 
        Marshal.ZeroFreeGlobalAllocUnicode valuePtr

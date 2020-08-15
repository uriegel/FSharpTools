namespace FSharpTools
module Ipv6Listener = 

    open System.Net.Sockets
    open System.Net

    open SocketExtensions

    type Ipv6Listener = {
        Listener: TcpListener 
        Ipv6: bool 
    }

    let Create port = 
        try
            let listener = TcpListener(IPAddress.IPv6Any, port)
            listener.Server.SetDualMode ()
            { Listener = listener; Ipv6 = true }            
        with 
        | :? SocketException as se when se.SocketErrorCode = SocketError.AddressFamilyNotSupported
            ->  let listener = TcpListener(IPAddress.Any, port) 
                {
                    Listener = listener
                    Ipv6 = false
                }    
        | :? SocketException as se when se.SocketErrorCode <> SocketError.AddressFamilyNotSupported 
            -> raise se
        | :? SocketException as se        
            ->
                let listener = TcpListener(IPAddress.Any, port)
                { Listener = listener; Ipv6 = false }            
            






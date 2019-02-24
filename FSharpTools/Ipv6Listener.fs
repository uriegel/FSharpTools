module Ipv6Listener

open System.Net.Sockets
open System.Net

open SocketExtensions

type Ipv6Listener = {
    Listener: TcpListener 
    Ipv6: bool 
}

let Create port = 
    try
        let listener = new TcpListener(IPAddress.IPv6Any, port)
        listener.Server.SetDualMode ()
        { Listener = listener; Ipv6 = true }            
    with 
    | :? SocketException as se ->
        if se.SocketErrorCode <> SocketError.AddressFamilyNotSupported then raise se
        let listener = new TcpListener(IPAddress.Any, port)
        { Listener = listener; Ipv6 = false }            
        






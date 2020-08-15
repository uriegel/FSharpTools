namespace FSharpTools
module SocketExtensions = 

    open System.Net.Sockets

    type Socket with
        /// <summary>
        /// Setting DualMode, support for either IPv6 or IPv4.
        /// <list type="bullet">
        /// <listheader>
        /// <description>Setting for TcpServer:</description>
        /// </listheader>
        /// <item>
        /// <description>tcpServer.Server.SetDualMode()</description>
        /// </item>
        /// </list>
        /// <list type="bullet">
        /// <listheader>
        /// <description>Setting for TcpClient:</description>
        /// </listheader>
        /// <item>
        /// <description>tcpClient.Client.SetDualMode()</description>
        /// </item>
        /// </list>
        /// <remarks>
        /// Setting for TcpServer: "IPAddress.IPv6Any", for TcpClient "AddressFamily.InterNetworkV6".
        /// </remarks>
        /// </summary>
        member this.SetDualMode () = this.SetSocketOption(SocketOptionLevel.IPv6, enum<SocketOptionName>(27), 0)
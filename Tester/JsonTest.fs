module JsonTest
open System
open System.IO

type Contact = {
    Name: string
    SurName: string
    EMail: string
    Nr: int
}

type Folder = {
    Name: string
    Path: string
    Size: int64
    Date: DateTime
}

type Drive = {
    Name: string
    Description: string
    Size: int64
    Mountpoint: string
}

type MethodType = | Contact = 1 | Folder = 2 | Drive = 3 

type Message = {
    Method: MethodType
    Contact: Contact option
    Folder: Folder option
    Drive: Drive option
}

type Init = string
type SetPath = string

type MessageOf = 
    | Contact of Contact
    | Folder of Folder
    | Drive of Drive
    | Init of Init
    | SetPath of SetPath

let deserializeTest () = 
    let msg = Folder {
        Name = "OfficeDocuments"
        Path = "/home/uwe/Documents"
        Size = 232434L
        Date = DateTime.Now 
    }
//    let msg = Init "folder"

    let testUnion () = 
        use ms = new MemoryStream ()
        Json.serializeStream ms msg

        ms.Capacity <- int ms.Length
        let text = System.Text.Encoding.Default.GetString (ms.GetBuffer ())

        ms.Position <- 0L
        let msg = Json.deserializeStream<Message> ms 
        ()

    testUnion ()
    printfn "Starting union"
    for i in [1..1_000_000] do
        testUnion ()
    printfn "finished union"
    ()

    let msg = {
        Method = MethodType.Contact
        Folder = Some {
            Name = "OfficeDocuments"
            Path = "/home/uwe/Documents"
            Size = 232434L
            Date = DateTime.Now 
        }
        Drive = None
        Contact = None
    }

    use ms = new MemoryStream ()
    Json.serializeStreamWithOptions ms msg
    
    ms.Position <- 0L
    let msg = Json.deserializeStreamWithOptions<Message> ms

    printfn "Starting"
    for i in [1..1_000_000] do
        ms.Position <- 0L
        let msg = Json.deserializeStreamWithOptions ms
        ()
    printfn "finished"
    ()

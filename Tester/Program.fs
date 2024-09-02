module Program  
open FSharpTools

let [<EntryPoint>] main _ = 


    // let exif = ExifReader.getExif "/daten/Bilder/Fotos/2024/20240117_165954.jpg" 
    // if exif.IsSome then
    //     let lat = exif.Value.getTagValue<double> ExifReader.ExifTag.GPSLatitude
    //     let lon = exif.Value.getTagValue<double> ExifReader.ExifTag.GPSLongitude
    //     ()


    DebugTest.run () 
    |> Async.RunSynchronously
    0


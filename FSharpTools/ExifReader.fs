namespace FSharpTools
module ExifReader =

    type ExifTag = 
        // IFD0 items
        ImageWidth = 0x100
        | ImageLength = 0x101
        | BitsPerSample = 0x102
        | Compression = 0x103
        | PhotometricInterpretation = 0x106
        | ImageDescription = 0x10E
        | Make = 0x10F
        | Model = 0x110
        | StripOffsets = 0x111
        | Orientation = 0x112
        | SamplesPerPixel = 0x115
        | RowsPerStrip = 0x116
        | StripByteCounts = 0x117
        | XResolution = 0x11A
        | YResolution = 0x11B
        | PlanarConfiguration = 0x11C
        | ResolutionUnit = 0x128
        | TransferFunction = 0x12D
        | Software = 0x131
        | DateTime = 0x132
        | Artist = 0x13B
        | WhitePoint = 0x13E
        | PrimaryChromaticities = 0x13F
        | JPEGInterchangeFormat = 0x201
        | JPEGInterchangeFormatLength = 0x202
        | YCbCrCoefficients = 0x211
        | YCbCrSubSampling = 0x212
        | YCbCrPositioning = 0x213
        | ReferenceBlackWhite = 0x214
        | Copyright = 0x8298
     
         // SubIFD items
        | ExposureTime = 0x829A
        | FNumber = 0x829D
        | ExposureProgram = 0x8822
        | SpectralSensitivity = 0x8824
        | ISOSpeedRatings = 0x8827
        | OECF = 0x8828
        | ExifVersion = 0x9000
        | DateTimeOriginal = 0x9003
        | DateTimeDigitized = 0x9004
        | ComponentsConfiguration = 0x9101
        | CompressedBitsPerPixel = 0x9102
        | ShutterSpeedValue = 0x9201
        | ApertureValue = 0x9202
        | BrightnessValue = 0x9203
        | ExposureBiasValue = 0x9204
        | MaxApertureValue = 0x9205
        | SubjectDistance = 0x9206
        | MeteringMode = 0x9207
        | LightSource = 0x9208
        | Flash = 0x9209
        | FocalLength = 0x920A
        | SubjectArea = 0x9214
        | MakerNote = 0x927C
        | UserComment = 0x9286
        | SubsecTime = 0x9290
        | SubsecTimeOriginal = 0x9291
        | SubsecTimeDigitized = 0x9292
        | FlashpixVersion = 0xA000
        | ColorSpace = 0xA001
        | PixelXDimension = 0xA002
        | PixelYDimension = 0xA003
        | RelatedSoundFile = 0xA004
        | FlashEnergy = 0xA20B
        | SpatialFrequencyResponse = 0xA20C
        | FocalPlaneXResolution = 0xA20E
        | FocalPlaneYResolution = 0xA20F
        | FocalPlaneResolutionUnit = 0xA210
        | SubjectLocation = 0xA214
        | ExposureIndex = 0xA215
        | SensingMethod = 0xA217
        | FileSource = 0xA300
        | SceneType = 0xA301
        | CFAPattern = 0xA302
        | CustomRendered = 0xA401
        | ExposureMode = 0xA402
        | WhiteBalance = 0xA403
        | DigitalZoomRatio = 0xA404
        | FocalLengthIn35mmFilm = 0xA405
        | SceneCaptureType = 0xA406
        | GainControl = 0xA407
        | Contrast = 0xA408
        | Saturation = 0xA409
        | Sharpness = 0xA40A
        | DeviceSettingDescription = 0xA40B
        | SubjectDistanceRange = 0xA40C
        | ImageUniqueID = 0xA420
     
        // GPS subifd items
        | GPSVersionID = 0x0
        | GPSLatitudeRef = 0x1
        | GPSLatitude = 0x2
        | GPSLongitudeRef = 0x3
        | GPSLongitude = 0x4
        | GPSAltitudeRef = 0x5
        | GPSAltitude = 0x6
        | GPSTimeStamp = 0x7
        | GPSSatellites = 0x8
        | GPSStatus = 0x9
        | GPSMeasureMode = 0xA
        | GPSDOP = 0xB
        | GPSSpeedRef = 0xC
        | GPSSpeed = 0xD
        | GPSTrackRef = 0xE
        | GPSTrack = 0xF
        | GPSImgDirectionRef = 0x10
        | GPSImgDirection = 0x11
        | GPSMapDatum = 0x12
        | GPSDestLatitudeRef = 0x13
        | GPSDestLatitude = 0x14
        | GPSDestLongitudeRef = 0x15
        | GPSDestLongitude = 0x16
        | GPSDestBearingRef = 0x17
        | GPSDestBearing = 0x18
        | GPSDestDistanceRef = 0x19
        | GPSDestDistance = 0x1A
        | GPSProcessingMethod = 0x1B
        | GPSAreaInformation = 0x1C
        | GPSDateStamp = 0x1D
        | GPSDifferential = 0x1E

    open System
    open System.Collections.Generic
    open System.IO
    open System.Text

    let toDate (datestr: string) = 
        match datestr with
        | DateTime.Value "yyyy:MM:dd HH:mm:ss" value -> Some value
        | _ -> None

    type Reader (getTagValue: (uint16 -> Object), reader: BinaryReader) =
        interface IDisposable with
            member x.Dispose() = reader.Dispose ()
        member this.GetTagValue with get() = fun (n: ExifTag) -> getTagValue (uint16 n)
        member this.getTagValue<'a>(n: ExifTag) = 
            getTagValue (uint16 n) 
            |> Option.checkNull
            |> Option.map (fun n -> n :?> 'a)

    let getDateValue (exifTag: ExifTag) (reader: Reader) = 
        toDate (string (reader.GetTagValue exifTag))

    let getExif fileName: Reader Option =  

        let mutable reader: BinaryReader = null

        let mutable isLittleEndian = false

        /// position where TIFF header starts
        let mutable tiffHeaderStart = 0L

        /// Tag ids and their absolute offsets within the file
        let mutable catalogue = null 

        let GetTIFFFieldLength tiffDataType = 
            match tiffDataType with
            | 1us | 2us | 6us -> 1uy
            | 3us | 8us -> 2uy
            | 4us | 7us| 9us| 11us -> 4uy
            | 5us | 10us -> 8uy
            | _ -> failwithf "Unknown TIFF datatype: %A" tiffDataType

        let ReadBytes byteCount = reader.ReadBytes(byteCount)

        let ReadBytesWithOffset (tiffOffset: uint16) byteCount =
            let originalOffset = reader.BaseStream.Position
            reader.BaseStream.Seek((int64)tiffOffset + tiffHeaderStart, SeekOrigin.Begin) |> ignore
            let data = reader.ReadBytes(byteCount)
            reader.BaseStream.Position <- originalOffset
            data

        let ToSByte (data: Byte[]) = sbyte (data.[0] - Byte.MaxValue)

        let ToShort (data: Byte []) = 
            match data.Length with
            | 0 -> 0s
            | _ -> 
                if isLittleEndian <> BitConverter.IsLittleEndian then System.Array.Reverse(data)
                BitConverter.ToInt16(data, 0)

        let ToUShort (data: Byte []) = 
            match data.Length with
            | 0 -> 0us
            | _ -> 
                if isLittleEndian <> BitConverter.IsLittleEndian then System.Array.Reverse(data)
                BitConverter.ToUInt16(data, 0)

        let ToInt (data: Byte[]) =
            if isLittleEndian <> BitConverter.IsLittleEndian then System.Array.Reverse(data)
            BitConverter.ToInt32(data, 0)

        let ToUInt (data: Byte[]) =
            if isLittleEndian <> BitConverter.IsLittleEndian then System.Array.Reverse(data)
            BitConverter.ToUInt32(data, 0)

        let ToRational (data: byte[]) =
            let numeratorData = Array.zeroCreate 4
            let denominatorData = Array.zeroCreate 4

            Array.Copy(data, numeratorData, 4)
            Array.Copy(data, 4, denominatorData, 0, 4)

            let numerator = ToInt(numeratorData)
            let denominator = ToInt(denominatorData)

            (double)numerator / double denominator

        let ToURational (data: byte[]) =
            let numeratorData = Array.zeroCreate 4
            let denominatorData = Array.zeroCreate 4

            Array.Copy(data, numeratorData, 4)
            Array.Copy(data, 4, denominatorData, 0, 4)

            let numerator = ToUInt(numeratorData)
            let denominator = ToUInt(denominatorData)

            (double)numerator / double denominator

        let ToSingle (data: byte[]) =
            if isLittleEndian <> BitConverter.IsLittleEndian then System.Array.Reverse(data)
            BitConverter.ToSingle(data, 0)

        let ToDouble (data: byte[]) =
            if isLittleEndian <> BitConverter.IsLittleEndian then System.Array.Reverse(data)
            BitConverter.ToDouble(data, 0)

        let ReadString (chars: int) =
            ReadBytes chars |> Encoding.ASCII.GetString

        let ReadUShort _ = ReadBytes 2 |> ToUShort

        let ReadUInt _ = ReadBytes 4 |> ToUInt

        let GetArray (data: byte[]) (elementLengthBytes: byte) (converter: Byte[] -> Object) = 
            let convertedData = Array.CreateInstance(typedefof<Object>, data.Length / int elementLengthBytes) 
            let buffer = Array.zeroCreate (int elementLengthBytes)

            [0..data.Length/int elementLengthBytes - 1] |> List.iter (fun elementCount ->
                Array.Copy(data, elementCount * (int elementLengthBytes), buffer, 0, int elementLengthBytes)
                convertedData.SetValue(converter(buffer), elementCount))

            convertedData

        let getDoubleFromArray (data: byte array) (elementLengthBytes: int) (converter: Byte[] -> Object) = 
            let convertedData = Array.CreateInstance(typeof<double>, data.Length / elementLengthBytes)
            let buffer = Array.zeroCreate elementLengthBytes
            [0..data.Length/int elementLengthBytes - 1] |> List.iter (fun elementCount ->
                Array.Copy(data, elementCount * elementLengthBytes, buffer, 0, elementLengthBytes)
                convertedData.SetValue(converter(buffer), elementCount))
            
            match convertedData with
            | :? (double array) as da -> da[0] + da[1]/60.0 + da[2]/3600.0
            | _ -> 0                

        let ReadToExifStart () = 
            // The file has a number of blocks (Exif/JFIF), each of which
            // has a tag number followed by a length. We scan the document until the required tag (0xFFE1)
            // is found. All tags start with FF, so a non FF tag indicates an error.

            let mutable markerStart = 0uy
            let mutable markerNumber = 0uy

            let readStartAndMarker () =
                markerStart <- reader.ReadByte()
                markerNumber <- reader.ReadByte()
                markerStart = 0xFFuy && markerNumber <> 0xE1uy

            while readStartAndMarker () do
                let dataLength = ReadUShort ()
                reader.BaseStream.Seek(int64 dataLength - 2L, SeekOrigin.Current) |> ignore

            // success if 0xFFE1 marker is found
            match (markerStart, markerNumber) with
            | (0xFFuy, 0xE1uy) -> true
            | _ -> false 

        let CatalogueIFD _ = 
            if catalogue = null then catalogue <- new Dictionary<uint16, int64>() 
                    
            let entryCount = ReadUShort ()
                
            [1us..entryCount] |> List.iter (fun _ ->
                let currentTagNumber = ReadUShort ()

                catalogue.[currentTagNumber] <- reader.BaseStream.Position - 2L;

                // Go to the end of this item (10 bytes, as each entry is 12 bytes long)
                reader.BaseStream.Seek(10L, SeekOrigin.Current) |> ignore) 

        let GetTagBytes tagID = // , out ushort tiffDataType, out uint numberOfComponents)
            if catalogue = null || catalogue.ContainsKey(tagID) = false then (null, 0us, 0u)
            else
                let tagOffset = catalogue.[tagID]
                reader.BaseStream.Position <- tagOffset
                let currentTagID = ReadUShort ()
                if currentTagID <> tagID then failwith "Tag number not at expected offset"

                let tiffDataType = ReadUShort ()
                let numberOfComponents = ReadUInt ()
                let mutable tagData = ReadBytes 4

                let dataSize = (int)numberOfComponents * (int)(GetTIFFFieldLength tiffDataType)

                if dataSize > 4 then
                    let offsetAddress = ToUShort tagData
                    (ReadBytesWithOffset offsetAddress dataSize, tiffDataType, numberOfComponents)
                else
                    System.Array.Resize(&tagData, dataSize)
                    (tagData, tiffDataType, numberOfComponents)

        let GetTagValue tagID: Object =
            let (tagData, tiffDataType, numberOfComponents) = GetTagBytes tagID

            match tagData with 
            | null -> null
            | _ ->
                let fieldLength = GetTIFFFieldLength tiffDataType

                match tiffDataType with
                | 1us ->
                    if numberOfComponents = 1u then tagData.[0] :> Object
                    else tagData :> Object
                | 2us ->
                    let rawstr = Encoding.ASCII.GetString tagData

                    let nullCharIndex = rawstr.IndexOf "\0"
                    let str = 
                        match nullCharIndex with
                        | -1 -> rawstr
                        | _str -> rawstr.Substring(0, nullCharIndex)
                    str :> Object
                | 3us ->
                    match numberOfComponents with 
                    | 1u -> ToUShort tagData :> Object
                    | _ -> GetArray tagData fieldLength (fun bs -> ToUShort(bs) :> Object) :> Object
                | 4us ->
                    match numberOfComponents with 
                    | 1u -> ToUInt tagData :> Object
                    | _ -> GetArray tagData fieldLength (fun bs -> ToUInt(bs) :> Object) :> Object
                | 5us ->
                    match numberOfComponents with 
                    | 1u -> ToURational tagData :> Object
                    | _ -> getDoubleFromArray tagData (int fieldLength) (fun bs -> ToURational(bs) :> Object) :> Object
                | 6us ->
                    match numberOfComponents with 
                    | 1u -> ToSByte tagData :> Object
                    | _ -> GetArray tagData fieldLength (fun bs -> ToSByte(bs) :> Object) :> Object
                | 7us ->
                    match numberOfComponents with 
                    | 1u -> ToUInt tagData :> Object
                    | _ -> GetArray tagData fieldLength (fun bs -> ToUInt(bs) :> Object) :> Object
                | 8us ->
                    match numberOfComponents with 
                    | 1u -> ToShort tagData :> Object
                    | _ -> GetArray tagData fieldLength (fun bs -> ToShort(bs) :> Object) :> Object
                | 9us ->
                    match numberOfComponents with 
                    | 1u -> ToInt tagData :> Object
                    | _ -> GetArray tagData fieldLength (fun bs -> ToInt(bs) :> Object) :> Object
                | 10us ->
                    match numberOfComponents with 
                    | 1u -> ToRational tagData :> Object
                    | _ -> GetArray tagData fieldLength (fun bs -> ToRational(bs) :> Object) :> Object
                | 11us ->
                    match numberOfComponents with 
                    | 1u -> ToSingle tagData :> Object
                    | _ -> GetArray tagData fieldLength (fun bs -> ToSingle(bs) :> Object) :> Object
                | 12us ->
                    match numberOfComponents with 
                    | 1u -> ToDouble tagData :> Object
                    | _ -> GetArray tagData fieldLength (fun bs -> ToDouble(bs) :> Object) :> Object
                | _ -> null

        let CreateTagIndex () = 
            ReadUShort () |> ignore
            if ReadString 4 <> "Exif" then failwith "Exif data not found"
            if ReadUShort () <> 0us then failwith "Malformed Exif data"
                    
            tiffHeaderStart <- reader.BaseStream.Position
            isLittleEndian <- ReadString 2 = "II"

            if ReadUShort () <> 0x002Aus then failwith "Error in TIFF data"
                    
            let ifdOffset = ReadUInt ()

            reader.BaseStream.Position <- int64 ifdOffset + tiffHeaderStart

            CatalogueIFD ()

            let mutable offset = GetTagValue 0x8769us 
            if offset = null then failwith "Unable to locate Exif data"

            let mutable offsetInt:uint32 = unbox offset
            reader.BaseStream.Position <- int64 offsetInt + tiffHeaderStart

            CatalogueIFD ()

            offset <- GetTagValue 0x8825us 
            if offset <> null then
                let mutable offsetInt:uint32 = unbox offset
                reader.BaseStream.Position <- int64 offsetInt + tiffHeaderStart 
                CatalogueIFD ()

        let HasExif _ =
            let result = 
                match ReadUShort () with
                | 0xFFD8us -> ReadToExifStart ()
                | _ -> false
            result
                
        let GetExifData _ =
            CreateTagIndex ()
            new Reader(GetTagValue, reader)

        try 
            let fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            reader <- new BinaryReader(fileStream)
            match HasExif () with 
            | false -> 
                reader.Dispose();
                None
            | true -> Some (GetExifData ())
        with  
            _ -> 
                reader.Dispose();
                None
            
        


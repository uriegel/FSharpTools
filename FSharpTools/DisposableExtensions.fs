module DisposableExtensions

open System

type IFinalizable = abstract member DoFinalize: unit->unit

type IUnmanagedDisposable = 
    inherit IFinalizable
    inherit IDisposable


/// <summary>Returns a new type that implements managed disposal
/// You have to implement <c>IDisposable</c> and call in implemented <c>Dispose</c> the new type's <c>d.Dispose()</c> method
/// That's all!</summary>
/// <param name="doDispose">The function that disposes.</param>
///<returns>Implementation logic of <c>IDisposable</c>.</returns>
let getDisposable (doDispose: unit->unit) = 

    let mutable disposedValue = false // To detect redundant calls
        
    let dispose disposing = 
        if not disposedValue then
            if disposing then
                doDispose ()
        disposedValue <- true

    let createDisposable () = {
        new IDisposable with 
            member x.Dispose() = dispose true
    }
    createDisposable ()

/// <summary>Returns a new type that implements managed and unmanaged disposal
/// You have to implement <c>IDisposable</c> and call in implemented <c>Dispose</c> the new type's <c>d.Dispose()</c> method
/// You have to also create a Finalizer which calls the new type's <c>DoFinalize()</c> method
/// That's all!</summary>
/// <param name="doDispose">The function that disposes managed resources.</param>
/// <param name="doUnmanagedDispose">The function that disposes unmanaged resources.</param>
/// <returns>Implementation logic of <c>IDisposable</c>.</returns>
let getUnmanagedDisposable (doDispose: (unit->unit) option) (doUnmanagedDispose: unit->unit) = 

    let mutable disposedValue = false // To detect redundant calls
        
    let dispose disposing = 
        if not disposedValue then
            match disposing, doDispose with
            | true, Some value -> value ()
            | _ -> ()
            doUnmanagedDispose ()
        disposedValue <- true
    let createDisposable () = {
        new IUnmanagedDisposable with 
            member this.Dispose() = 
                dispose true
                GC.SuppressFinalize this
            member this.DoFinalize() = dispose false
    }
    createDisposable ()
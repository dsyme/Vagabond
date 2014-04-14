## Vagrant

A library that facilitates the distribution of code in the .NET framework. 

### Introduction

On-demand distribution of code (e.g. lambdas) for remote execution is a problem
often exacerbated by the presence of dynamic assemblies. Dynamic assemblies 
appear in situations where code is emitted at runtime, such as interpreters,
optimization libraries or F# type providers. By default, dynamic assemblies
are not persistable. What's more, the inherently incremental nature of dynamic
assemblies makes their exportation even more challenging.

Vagrant attempts to solve this problem by providing an automated dependency resolution
and dynamic assembly compilation framework. It also addresses issues inherent
in REPL loops. It fully supports code generated by F# Interactive and type providers.

It is based on the `Mono.Cecil` and `FsPickler` libraries and uses code from JB Evain's
[AssemblySaver](https://github.com/jbevain/mono.reflection/blob/assembly-saver/Mono.Reflection/AssemblySaver.cs) project.

### Demo

To see a demo of Vagrant, build the project and execute the
[`thunkServer.fsx`](https://github.com/nessos/Vagrant/blob/master/tests/Vagrant.Tests/thunkServer.fsx) 
script found inside the samples folder. As the name suggests, it allows for execution of 
arbitrary thunks in a remote server.

### Overview

The included implementation of ThunkServer is a straightforward example of the Vagrant API.
What follows is a brief overview of the basic API.

A code export environment can be initialized as follows:
```fsharp
open Nessos.Vagrant

let server = new VagrantServer()
```
Given an arbitrary object, dependencies are resolved like so:
```fsharp
let obj = Some(fun x -> printfn "%d" x; x + 1) :> obj

let assemblies : Assembly list = server.ComputeObjectDependencies(obj, permitCompilation = true)
```
An assembly can be exported by writing
```fsharp
let portableAssembly : PortableAssembly = vagrant.MakePortableAssembly(assembly, includeAssemblyImage = true)
```
A portable assembly contains necessary data to load the specified assembly in a remote process.

On the client side, assemblies can be loaded like so:
```fsharp
let client = new VagrantClient()

let response : AssemblyLoadResponse = client.LoadPortableAssembly(portableAssembly)
```
To transparently submit all required dependencies of an object:
```fsharp
do server.SubmitObjectDependencies(submitF, obj, permitCompilation = true)
```
where ``submitF : PortableAssembly list -> Async<AssemblyLoadResponse list>`` is
a user-provided dependency transport implementation.

### Mono Support

Vagrant makes heavy use of the reflection API. Mono support is patchy for the moment, 
mostly due to a number of bugs in the mono runtime 
[[1](https://bugzilla.xamarin.com/show_bug.cgi?id=19045),[2](https://bugzilla.xamarin.com/show_bug.cgi?id=19039)].
If you would like to see stable mono support for Vagrant soon, 
make sure to contribute code/nag until these bugs are resolved.

﻿namespace System
open System.Reflection

[<assembly: AssemblyVersionAttribute("0.12.4")>]
[<assembly: AssemblyFileVersionAttribute("0.12.4")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.12.4"

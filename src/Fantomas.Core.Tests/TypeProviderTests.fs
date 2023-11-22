module Fantomas.Core.Tests.TypeProviderTests

open Fantomas.Core
open NUnit.Framework
open FsUnit
open Fantomas.Core.Tests.TestHelpers

[<Test>]
let ``type providers`` () =
    formatSourceString
        """
type Northwind = ODataService<"http://services.odata.org/Northwind/Northwind.svc/">"""
        config
    |> prepend newline
    |> should
        equal
        """
type Northwind = ODataService<"http://services.odata.org/Northwind/Northwind.svc/">
"""

[<Test>]
let ``should add space before type provider params`` () =
    formatSourceString
        """
type IntegerRegex = FSharpx.Regex< @"(?<value>\d+)" >"""
        config
    |> prepend newline
    |> should
        equal
        """
type IntegerRegex = FSharpx.Regex< @"(?<value>\d+)" >
"""

[<Test>]
let ``should add space before type provider named argument, 1209`` () =
    formatSourceString
        """
type Graphml = XmlProvider<Schema= @"http://graphml.graphdrawing.org/xmlns/1.0/graphml-structure.xsd">
"""
        config
    |> prepend newline
    |> should
        equal
        """
type Graphml = XmlProvider<Schema= @"http://graphml.graphdrawing.org/xmlns/1.0/graphml-structure.xsd">
"""

[<Test>]
let ``should throw FormatException on unparsed input`` () =
    Assert.Throws<ParseException>(fun () ->
        formatSourceString
            """
    type GeoResults = JsonProvider<Sample= "A" + "GitHub.json" >"""
            config
        |> ignore)
    |> ignore

[<Test>]
let ``should handle lines with more than 512 characters`` () =
    formatSourceString
        """
(new CsvFile<string * decimal * decimal>(new Func<obj, string[], string * decimal * decimal>(fun (parent : obj) (row : string[]) -> CommonRuntime.GetNonOptionalValue("Name", CommonRuntime.ConvertString(TextConversions.AsOption(row.[0])), TextConversions.AsOption(row.[0])), CommonRuntime.GetNonOptionalValue("Distance", CommonRuntime.ConvertDecimal("", TextConversions.AsOption(row.[1])), TextConversions.AsOption(row.[1])), CommonRuntime.GetNonOptionalValue("Time", CommonRuntime.ConvertDecimal("", TextConversions.AsOption(row.[2])), TextConversions.AsOption(row.[2]))), new Func<string * decimal * decimal, string[]>(fun (row : string * decimal * decimal) -> [| CommonRuntime.ConvertStringBack(CommonRuntime.GetOptionalValue((let x, _, _ = row in x))); CommonRuntime.ConvertDecimalBack("", CommonRuntime.GetOptionalValue((let _, x, _ = row in x))); CommonRuntime.ConvertDecimalBack("", CommonRuntime.GetOptionalValue((let _, _, x = row in x))) |]), (ProviderFileSystem.readTextAtRunTimeWithDesignTimeOptions @"C:\Dev\FSharp.Data-master\src\..\tests\FSharp.Data.Tests\Data" "" "SmallTest.csv"), "", '"', true, false)).Cache()
"""
        config
    |> prepend newline
    |> should
        equal
        """
(new CsvFile<string * decimal * decimal>(
    new Func<obj, string[], string * decimal * decimal>(fun (parent: obj) (row: string[]) ->
        CommonRuntime.GetNonOptionalValue(
            "Name",
            CommonRuntime.ConvertString(TextConversions.AsOption(row.[0])),
            TextConversions.AsOption(row.[0])
        ),
        CommonRuntime.GetNonOptionalValue(
            "Distance",
            CommonRuntime.ConvertDecimal("", TextConversions.AsOption(row.[1])),
            TextConversions.AsOption(row.[1])
        ),
        CommonRuntime.GetNonOptionalValue(
            "Time",
            CommonRuntime.ConvertDecimal("", TextConversions.AsOption(row.[2])),
            TextConversions.AsOption(row.[2])
        )),
    new Func<string * decimal * decimal, string[]>(fun (row: string * decimal * decimal) ->
        [| CommonRuntime.ConvertStringBack(CommonRuntime.GetOptionalValue((let x, _, _ = row in x)))
           CommonRuntime.ConvertDecimalBack("", CommonRuntime.GetOptionalValue((let _, x, _ = row in x)))
           CommonRuntime.ConvertDecimalBack("", CommonRuntime.GetOptionalValue((let _, _, x = row in x))) |]),
    (ProviderFileSystem.readTextAtRunTimeWithDesignTimeOptions
        @"C:\Dev\FSharp.Data-master\src\..\tests\FSharp.Data.Tests\Data"
        ""
        "SmallTest.csv"),
    "",
    '"',
    true,
    false
))
    .Cache()
"""

[<Test>]
let ``multiple arguments in type provider`` () =
    formatSourceString
        """
type Northwind = ODataService<"http://services.odata.org/Northwind/Northwind.svc/", "password">"""
        config
    |> prepend newline
    |> should
        equal
        """
type Northwind = ODataService<"http://services.odata.org/Northwind/Northwind.svc/", "password">
"""

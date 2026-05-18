// include Fake lib
#r @"packages/build/FAKE/tools/FakeLib.dll"
open Fake

System.Environment.SetEnvironmentVariable("MSBuild",@"C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe")

// Properties
let mode = getBuildParamOrDefault "mode" "Release"
let buildDir = "./bin/" + mode + "/"

// Targets
Target "Clean" (fun _ -> 
        CleanDir buildDir
        "Cleaning " + mode + " configuration" |> trace
        !! "src/**/*.csproj"
            |>
            match mode.ToLower() with
                | "release" -> MSBuildRelease null "Clean"
                | _ -> MSBuildDebug null "Clean"
            |> Log "AppBuild-Output: "
)

Target "Build" (fun _ ->
        "Building " + mode + " configuration" |> trace
        !! "src/**/*.csproj"
            |>
            match mode.ToLower() with
                | "release" -> MSBuildRelease null "Build"
                | _ -> MSBuildDebug null "Build"
            |> Log "AppBuild-Output: "
)

Target "Default" (fun _ ->
    ()
)

// Dependencies
"Clean"
    ==> "Build"
    ==> "Default"

// start build
RunTargetOrDefault "Default"
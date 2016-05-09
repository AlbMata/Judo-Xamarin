#r @"packages/FAKE.3.5.4/tools/FakeLib.dll"
#load "build-helpers.fsx"
open Fake
open System
open System.IO
open System.Linq
open BuildHelpers
open Fake.XamarinHelper

Target "core-build" (fun () ->
    RestorePackages "JudoXamarin.JudoDotNetXamarin.sln"

    MSBuild "src/JudoDotNetXamarin/bin/Debug" "Build" [ ("Configuration", "Release"); ("Platform", "Any CPU") ] [ "JudoXamarin.JudoDotNetXamarin.sln" ] |> ignore
    TeamCityHelper.PublishArtifact (project + "/src/JudoDotNetXamarin/bin/Release/*.dll")
)

// Target "core-tests" (fun () -> 
//     RunNUnitTests "src/TipCalc/bin/Debug/TipCalc.Tests.dll" "src/TipCalc/bin/Debug/testresults.xml"
// )

Target "ios-build" (fun () ->
    RestorePackages "JudoXamarin.JudoDotNetXamariniOSSDK.sln"

    iOSBuild (fun defaults ->
        {defaults with
            ProjectPath = "JudoXamarin.JudoDotNetXamariniOSSDK.sln"
            Configuration = "Debug|iPhoneSimulator"
            Target = "Build"
        })
)

Target "ios-Release" (fun () ->
    RestorePackages "JudoXamarin.JudoDotNetXamariniOSSDK.sln"

    iOSBuild (fun defaults ->
        {defaults with
            ProjectPath = "JudoXamarin.JudoDotNetXamariniOSSDK.sln"
            Configuration = "Release|iPhone"
            Target = "Build"
        })
  TeamCityHelper.PublishArtifact (project + "/src/JudoDotNetXamariniOSSDK/bin/Release/*.dll")
)
    // let appPath = Directory.EnumerateFiles(Path.Combine("src", "JudoXamarin.JudoDotNetXamariniOSSDK", "bin", "iPhone", "Ad-Hoc"), "*.ipa").First()

    // TeamCityHelper.PublishArtifact appPath


// Target "ios-appstore" (fun () ->
//     RestorePackages "TipCalc.iOS.sln"

//     iOSBuild (fun defaults ->
//         {defaults with
//             ProjectPath = "TipCalc.iOS.sln"
//             Configuration = "AppStore|iPhone"
//             Target = "Build"
//         })

//     let outputFolder = Path.Combine("src", "TipCalc.iOS", "bin", "iPhone", "AppStore")
//     let appPath = Directory.EnumerateDirectories(outputFolder, "*.app").First()
//     let zipFilePath = Path.Combine(outputFolder, "TipCalc.iOS.zip")
//     let zipArgs = String.Format("-r -y '{0}' '{1}'", zipFilePath, appPath)

//     Exec "zip" zipArgs

//     TeamCityHelper.PublishArtifact zipFilePath
// )

// Target "ios-uitests" (fun () ->
//     let appPath = Directory.EnumerateDirectories(Path.Combine("src", "TipCalc.iOS", "bin", "iPhoneSimulator", "Debug"), "*.app").First()

//     RunUITests appPath
// )

// Target "ios-testcloud" (fun () ->
//     RestorePackages "TipCalc.iOS.sln"

//     iOSBuild (fun defaults ->
//         {defaults with
//             ProjectPath = "TipCalc.iOS.sln"
//             Configuration = "Debug|iPhone"
//             Target = "Build"
//         })

//     let appPath = Directory.EnumerateFiles(Path.Combine("src", "TipCalc.iOS", "bin", "iPhone", "Debug"), "*.ipa").First()

//     getBuildParam "devices" |> RunTestCloudTests appPath
// )

Target "android-build" (fun () ->
    RestorePackages "JudoXamarin.JudoDotNetXamarinAndroidSDK.sln"

    MSBuild "src/JudoDotNetXamarinAndroidSDK/bin/Release" "Build" [ ("Configuration", "Release") ] [ "JudoXamarin.JudoDotNetXamarinAndroidSDK.sln" ] |> ignore
    
     TeamCityHelper.PublishArtifact (project + "/src/JudoDotNetXamarinAndroidSDK/bin/Release/*.dll")
)

// Target "android-package" (fun () ->
//     AndroidPackage (fun defaults ->
//         {defaults with
//             ProjectPath = "src/TipCalc.Android/TipCalc.Android.csproj"
//             Configuration = "Release"
//             OutputPath = "src/TipCalc.Android/bin/Release"
//         }) 
//     |> AndroidSignAndAlign (fun defaults ->
//         {defaults with
//             KeystorePath = "tipcalc.keystore"
//             KeystorePassword = "tipcalc" // TODO: don't store this in the build script for a real app!
//             KeystoreAlias = "tipcalc"
//         })
//     |> fun file -> TeamCityHelper.PublishArtifact file.FullName
// )

// Target "android-uitests" (fun () ->
//     AndroidPackage (fun defaults ->
//         {defaults with
//             ProjectPath = "src/TipCalc.Android/TipCalc.Android.csproj"
//             Configuration = "Release"
//             OutputPath = "src/TipCalc.Android/bin/Release"
//         }) |> ignore

//     let appPath = Directory.EnumerateFiles(Path.Combine("src", "TipCalc.Android", "bin", "Release"), "*.apk", SearchOption.AllDirectories).First()

//     RunUITests appPath
// )

// Target "android-testcloud" (fun () ->
//     AndroidPackage (fun defaults ->
//         {defaults with
//             ProjectPath = "src/TipCalc.Android/TipCalc.Android.csproj"
//             Configuration = "Release"
//             OutputPath = "src/TipCalc.Android/bin/Release"
//         }) |> ignore

//     let appPath = Directory.EnumerateFiles(Path.Combine("src", "TipCalc.Android", "bin", "Release"), "*.apk", SearchOption.AllDirectories).First()

//     getBuildParam "devices" |> RunTestCloudTests appPath
// )

// "core-build"
//   ==> "core-tests"


let PackageComponent solutionFile =
    Exec "xamarin-component.exe" ("package " + solutionFile)
    TeamCityHelper.PublishArtifact (project + "/component/*"+releaseNumber+".xam")
    // solutionFile |> RestoreComponents (fun defaults -> {defaults with ToolPath = "tools/xpkg/xamarin-component.exe" })
    
    

"ios-build"
  ==> "core-build"

"android-build"
  ==> "core-build"

// "android-build"
//   ==> "android-testcloud"

// "android-build"
//   ==> "android-package"

RunTarget() 
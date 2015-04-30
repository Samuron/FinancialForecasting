// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open FinancialForecasting.Equation.Service
open FinancialForecasting.Migration
open LMDotNet
open System.ServiceModel
open System

[<EntryPoint>]
let main argv = 
    let context = new FinancialForecastingContext()
    let nodes = [ YNode; EnabledNode; EnabledNode; EnabledNode; EnabledNode; EnabledNode; EnabledNode ]
    
    let indices = 
        context.EnterpriseIndices
        |> Seq.map (fun index -> [| index.Y; index.X1; index.X2; index.X3; index.X4; index.X5; index.X6 |])
        |> Seq.toList
    
    let initial = [| 0.0; 0.0; 0.0; 0.0; 0.0; 0.0; 0.0 |]
    let someshit = Equation.solve nodes indices initial
    printfn "%s" someshit.Message
    printfn "%g %g %g %g %g %g %g" someshit.OptimizedParameters.[0] someshit.OptimizedParameters.[1] 
        someshit.OptimizedParameters.[2] someshit.OptimizedParameters.[3] someshit.OptimizedParameters.[4] 
        someshit.OptimizedParameters.[5] someshit.OptimizedParameters.[6]
    0 // return an integer exit code

namespace FinancialForecasting.Equation.Service

open LMDotNet

type EquationNode = 
    | EnabledNode
    | DisabledNode
    | YNode

module Equation = 
    let nodeCalculator node c p = 
        match node with
        | EnabledNode -> p * c
        | DisabledNode -> 0.0
        | YNode -> -c
    
    let equationBuilder weighter nodes indices parameters = List.map3 weighter nodes indices parameters |> List.sum
    
    let solve (nodes : EquationNode list) (indices : float [] list) initial = 
        let findMin = 
            LMA.defaultSettings
            |> LMA.init
            |> LMA.minimize
        
        let calculate row parameters = 
            equationBuilder nodeCalculator nodes row (List.ofArray parameters)
        
        let calcucalation (p : float []) (r : float []) = 
            for i in 0..indices.Length - 1 do
                r.[i] <- calculate (List.ofArray indices.[i]) p
        
        calcucalation |> findMin initial indices.Length

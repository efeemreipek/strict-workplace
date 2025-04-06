using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MustNotBeNearTypeRule", menuName = "Scriptable Objects/Crate Rule/Must Not Be Near Type")]
public class MustNotBeNearTypeRule : CrateRule
{
    public Crate.EType TargetType;
    public Crate.EType NeighbourType;

    public override bool IsRelevant(Crate crate)
    {
        return crate.Type == TargetType;
    }
    public override bool Validate(Crate crate, List<Crate> allCrates)
    {
        var neighbours = crate.GetNeighbourCrateTypes();

        foreach(var neighbour in neighbours)
        {
            if(neighbour == NeighbourType) return false;
        }

        return true;
    }
}

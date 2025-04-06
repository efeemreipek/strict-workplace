using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MustBeNearSameTypeRule", menuName = "Scriptable Objects/Crate Rule/Must Be Near Same Type")]
public class MustBeNearSameTypeRule : CrateRule
{
    public Crate.EType TargetType;

    public override bool IsRelevant(Crate crate)
    {
        return crate.Type == TargetType;
    }

    public override bool Validate(Crate crate, List<Crate> allCrates)
    {
        int count = 0;
        foreach(Crate c in allCrates)
        {
            if (c.Type == TargetType)
            {
                count++;
            }
        }

        if(count <= 1) return true;

        var neighbours = crate.GetNeighbourCrateTypes();

        foreach (var neighbourType in neighbours)
        {
            if(neighbourType == TargetType) return true;
        }

        return false;
    }
}

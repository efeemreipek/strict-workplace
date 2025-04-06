using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MustBeAloneRule", menuName = "Scriptable Objects/Crate Rule/Must Be Alone")]
public class MustBeAloneRule : CrateRule
{
    public Crate.EType TargetType;

    public override bool IsRelevant(Crate crate)
    {
        return crate.Type == TargetType;
    }

    public override bool Validate(Crate crate, List<Crate> allCrates)
    {
        var neighbours = crate.GetNeighbourCrateTypes();

        foreach(var neighbour in neighbours)
        {
            if(neighbour != Crate.EType.None) return false;
        }

        return true;
    }
}

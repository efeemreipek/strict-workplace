using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MustBeNearTypeRule", menuName = "Scriptable Objects/Crate Rule/Must Be Near Type")]
public class MustBeNearTypeRule : CrateRule
{
    public Crate.EType TargetType;
    public Crate.EType NeighbourType;

    public override bool IsRelevant(Crate crate)
    {
        return crate.Type == TargetType;
    }
    public override bool Validate(Crate crate, List<Crate> allCrates)
    {
        bool isNeighbourExist = false;
        foreach(Crate c in allCrates)
        {
            if (c.Type == NeighbourType)
            {
                isNeighbourExist = true;
                break;
            }
        }

        if(!isNeighbourExist) return true;

        var neighbours = crate.GetNeighbourCrateTypes();

        foreach(var neighbour in neighbours)
        {
            if(neighbour == NeighbourType) return true;
        }

        return false;
    }
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MustBeInStorageAreaRule", menuName = "Scriptable Objects/Crate Rule/Must Be In Storage Area")]
public class MustBeInStorageAreaRule : CrateRule
{
    public Vector2 XBounds, ZBounds;


    public override bool Validate(Crate crate, List<Crate> allCrates)
    {
        if(crate.Transform.position.x >= XBounds.x && crate.Transform.position.x <= XBounds.y
            && crate.Transform.position.z >= ZBounds.x && crate.Transform.position.z <= ZBounds.y)
        {
            return true;
        }

        return false;
    }
}

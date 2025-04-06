using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MustBePlacedOnPalletRule", menuName = "Scriptable Objects/Crate Rule/Must Be Placed On Pallet")]
public class MustBePlacedOnPalletRule : CrateRule
{
    public Crate.EType TargetType;

    public override bool IsRelevant(Crate crate)
    {
        return crate.Type == TargetType;
    }

    public override bool Validate(Crate crate, List<Crate> allCrates)
    {
        bool isAboveGround = crate.Transform.position.y > 0.1f;
        return isAboveGround;
    }
}

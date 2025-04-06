using System.Collections.Generic;
using UnityEngine;

public abstract class CrateRule : ScriptableObject
{
    public string Name;
    [TextArea] public string Description;

    public virtual bool IsRelevant(Crate crate) => true;
    public abstract bool Validate(Crate crate, List<Crate> allCrates);
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrateTypeData", menuName = "Scriptable Objects/CrateTypeData")]
public class CrateTypeData : ScriptableObject
{
    public List<Material> MaterialList = new List<Material>();
    public List<Material> SymbolMaterialList = new List<Material>();
}

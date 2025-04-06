using TMPro;
using UnityEngine;

public class CrateGeneratorUI : MonoBehaviour
{
    [SerializeField] private TMP_Text remainingCrateCountText;

    public void UpdateText(int amount)
    {
        string t = "Rema�n�ng Crates : ";
        remainingCrateCountText.text = t + amount;
    }
}

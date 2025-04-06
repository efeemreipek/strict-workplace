using TMPro;
using UnityEngine;

public class RuleUI : MonoBehaviour
{
    [SerializeField] private TMP_Text ruleNameText;
    [SerializeField] private TMP_Text ruleDescriptionText;

    public void InitializeRule(string name, string description)
    {
        ruleNameText.text = name;
        ruleDescriptionText.text = description;
    }
}

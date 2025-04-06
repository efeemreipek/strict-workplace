using DG.Tweening;
using UnityEngine;

public class CrateRuleHandlerUI : MonoBehaviour
{
    [SerializeField] private RectTransform ruleListPanel;
    [SerializeField] private float moveDuration;
    [SerializeField] private Ease moveEase;
    [SerializeField] private GameObject rulePrefab;
    [SerializeField] private RectTransform ruleListContainer;

    private Vector2 ruleListPanelOnScreenPos = new Vector2(0, 0);
    private Vector2 ruleListPanelOffScreenPos = new Vector2(600, 0);

    private bool isPanelOnScreen;
    private bool isAnimating;
    private bool canInteract = true;

    private void Update()
    {
        if(InputHandler.Current.TabPressed && !isAnimating && canInteract)
        {
            isAnimating = true;

            ruleListPanel.DOAnchorPos(isPanelOnScreen ? ruleListPanelOffScreenPos : ruleListPanelOnScreenPos, moveDuration)
                .SetEase(moveEase)
                .OnComplete(() =>
                {
                    isPanelOnScreen = !isPanelOnScreen;
                    isAnimating = false;
                });
        }
    }

    public void AddRuleToList(CrateRule rule)
    {
        GameObject ruleGO = Instantiate(rulePrefab, ruleListContainer);
        RuleUI ruleUI = ruleGO.GetComponent<RuleUI>();
        ruleUI.InitializeRule(rule.Name, rule.Description);
    }
    public void MovePanelOffScreen()
    {
        ruleListPanel.DOAnchorPos(ruleListPanelOffScreenPos, moveDuration)
            .SetEase(moveEase)
            .OnComplete(() =>
            {
                canInteract = false;
                isAnimating = false;
            });
    }
}

using UnityEngine;
using DG.Tweening;
using TMPro;

public class NewRuleUI : MonoBehaviour
{
    [SerializeField] private RectTransform newRulePanel;
    [SerializeField] private float moveDuration;
    [SerializeField] private Ease moveEase;
    [SerializeField] private float readTime;
    [SerializeField] private TMP_Text newRuleNameText;
    [SerializeField] private TMP_Text newRuleDescriptionText;

    private bool isComplete;
    private float remainingReadTime;

    private Vector2 newRulePanelOnScreenPos = new Vector2(0, 0);
    private Vector2 newRulePanelOffScreenPos = new Vector2(0, 800);

    private void OnEnable()
    {
        ScreenFadeHandler.Current.OnFadeInComplete += Fade_OnFadeInComplete;
        CrateRuleHandler.OnNewRuleAdded += CrateRuleHandler_OnNewRuleAdded;
    }
    private void OnDisable()
    {
        ScreenFadeHandler.Current.OnFadeInComplete -= Fade_OnFadeInComplete;
        CrateRuleHandler.OnNewRuleAdded -= CrateRuleHandler_OnNewRuleAdded;
    }
    private void Start()
    {
        remainingReadTime = readTime;
    }
    private void Update()
    {
        if(!isComplete) return;

        remainingReadTime -= Time.deltaTime;
        if(remainingReadTime < 0)
        {
            newRulePanel.DOAnchorPos(newRulePanelOffScreenPos, moveDuration)
            .SetEase(moveEase)
            .OnComplete(() => ScreenFadeHandler.Current.FadeOut());

            isComplete = false;
            remainingReadTime = readTime;
        }
    }

    private void Fade_OnFadeInComplete()
    {
        if(!CrateRuleHandler.Current.IsGameOver)
        {
            newRulePanel.DOAnchorPos(newRulePanelOnScreenPos, moveDuration)
                .SetEase(moveEase)
                .OnComplete(() => isComplete = true);
        }
    }
    private void CrateRuleHandler_OnNewRuleAdded(CrateRule rule)
    {
        newRuleNameText.text = rule.Name;
        newRuleDescriptionText.text = rule.Description;
    }
}

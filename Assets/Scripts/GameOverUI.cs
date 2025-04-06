using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private RectTransform firedPanel;
    [SerializeField] private RectTransform resignedPanel;
    [SerializeField] private TMP_Text firedInfoText;
    [SerializeField] private TMP_Text resignedInfoText;
    [SerializeField] private float moveDuration;
    [SerializeField] private Ease moveEase;

    private Vector2 panelOnScreenPos = new Vector2(0, 0);
    private Vector2 panelOffScreenPos = new Vector2(0, 800);

    private void OnEnable()
    {
        CrateRuleHandler.OnGameOver += CrateRuleHandler_OnGameOver;
        CrateRuleHandler.OnResign += CrateRuleHandler_OnResign;
    }
    private void OnDisable()
    {
        CrateRuleHandler.OnGameOver -= CrateRuleHandler_OnGameOver;
        CrateRuleHandler.OnResign -= CrateRuleHandler_OnResign;
    }

    private void CrateRuleHandler_OnGameOver(int days)
    {
        firedPanel.DOAnchorPos(panelOnScreenPos, moveDuration).SetEase(moveEase);
        firedInfoText.text = $"You lasted {days} days.";
    }
    private void CrateRuleHandler_OnResign(int days)
    {
        resignedPanel.DOAnchorPos(panelOnScreenPos, moveDuration).SetEase(moveEase);
        resignedInfoText.text = $"You lasted {days} days.";
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

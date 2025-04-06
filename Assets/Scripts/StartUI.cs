using System;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    [SerializeField] private RectTransform startPanel;

    public static event Action OnGameStart;

    public void PlayButton()
    {
        OnGameStart?.Invoke();
        startPanel.gameObject.SetActive(false);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}

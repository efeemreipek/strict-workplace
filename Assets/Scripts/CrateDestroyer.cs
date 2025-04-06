using UnityEngine;

public class CrateDestroyer : MonoBehaviour
{

    private void OnEnable()
    {
        ScreenFadeHandler.Current.OnFadeOutBegin += Fade_OnFadeOutBegin;
    }
    private void OnDisable()
    {
        ScreenFadeHandler.Current.OnFadeOutBegin -= Fade_OnFadeOutBegin;
    }

    private void Fade_OnFadeOutBegin()
    {
        var crates = FindObjectsByType<Crate>(FindObjectsSortMode.None);
        foreach(var crate in crates)
        {
            Destroy(crate.gameObject);
        }
    }
}

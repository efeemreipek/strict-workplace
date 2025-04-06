using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public static event Action OnDoorInteracted;
    public Transform Transform => transform;

    public bool CanInteract(PlayerInteractor interactor) => interactor.CurrentCarriedObject == null;
    public void OnInteract(PlayerInteractor interactor)
    {
        if(!CanInteract(interactor)) return;
        if(CrateGenerator.Current.RemainingCrateCount > 0) return;

        ScreenFadeHandler.Current.FadeIn();
        OnDoorInteracted?.Invoke();
    }
}

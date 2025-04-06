using UnityEngine;

public interface IInteractable
{
    public void OnInteract(PlayerInteractor interactor);
    public bool CanInteract(PlayerInteractor interactor);
    public Transform Transform { get; }
}

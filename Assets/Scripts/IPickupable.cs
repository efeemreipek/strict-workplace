using UnityEngine;

public interface IPickupable : IInteractable
{
    public void OnPickup(PlayerInteractor interactor);
    public void OnDrop(Vector3 position, Quaternion rotation);
}

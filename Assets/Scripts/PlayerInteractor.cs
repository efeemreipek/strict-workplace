using DG.Tweening;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Transform interactableHolderTransform;
    [SerializeField] private float pickupAnimDuration = 0.3f;
    [SerializeField] private float dropAnimDuration = 0.2f;
    [SerializeField] private Ease pickupEase = Ease.OutBack;
    [SerializeField] private Ease dropEase = Ease.OutBounce;

    private PlayerScanner scanner;
    private PlayerController controller;
    private IPickupable currentCarriedObject;
    private bool isAnimating = false;

    public IPickupable CurrentCarriedObject => currentCarriedObject;
    public bool IsAnimating => isAnimating;

    private void Awake()
    {
        scanner = GetComponent<PlayerScanner>();
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(controller.IsMoving) return;
        if(controller.IsRotating) return;
        if(isAnimating) return;

        if(InputHandler.Current.InteractPressed)
        {
            HandleInteraction();
        }
    }

    private void HandleInteraction()
    {
        if(currentCarriedObject != null)
        {
            if(scanner.TryGetPlacePosition(out Vector3 placePosition))
            {
                TryDropObject(placePosition, Quaternion.identity);
            }
            return;
        }

        if(scanner.TryScanForInteractable(out IInteractable interactable) && interactable.CanInteract(this))
        {
            interactable.OnInteract(this);
        }
    }
    public void UpdateCarriedObjectPosition()
    {
        if(currentCarriedObject != null && !isAnimating)
        {
            currentCarriedObject.Transform.position = interactableHolderTransform.position;
            currentCarriedObject.Transform.rotation = Quaternion.identity;
        }
    }

    public void PickupObject(IPickupable pickupable)
    {
        if(currentCarriedObject != null || isAnimating) return;

        currentCarriedObject = pickupable;
        isAnimating = true;

        Vector3 startPosition = pickupable.Transform.position;

        pickupable.Transform.SetParent(interactableHolderTransform);
        pickupable.Transform.rotation = Quaternion.identity;

        Sequence pickupSequence = DOTween.Sequence();

        pickupSequence.Append(pickupable.Transform.DOMove(
            new Vector3(startPosition.x, startPosition.y + 0.5f, startPosition.z),
            pickupAnimDuration * 0.4f)
            .SetEase(Ease.OutQuad));

        pickupSequence.Append(pickupable.Transform.DOMove(
            interactableHolderTransform.position,
            pickupAnimDuration * 0.6f)
            .SetEase(pickupEase));

        pickupSequence.Join(pickupable.Transform.DOScale(
            new Vector3(1.1f, 1.1f, 1.1f),
            pickupAnimDuration * 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => {
                pickupable.Transform.DOScale(Vector3.one, pickupAnimDuration * 0.3f)
                    .SetEase(Ease.InQuad);
            }));

        pickupSequence.OnComplete(() => {
            pickupable.OnPickup(this);
            isAnimating = false;
        });
    }

    public bool TryDropObject()
    {
        if(scanner.TryGetPlacePosition(out Vector3 placePosition))
        {
            return TryDropObject(placePosition, Quaternion.identity);
        }
        return false;
    }

    public bool TryDropObject(Vector3 position, Quaternion rotation)
    {
        if(currentCarriedObject == null || isAnimating) return false;

        isAnimating = true;
        IPickupable droppedObject = currentCarriedObject;

        droppedObject.Transform.rotation = rotation;

        Sequence dropSequence = DOTween.Sequence();

        Vector3 intermediatePosition = new Vector3(position.x, position.y + 0.5f, position.z);
        dropSequence.Append(droppedObject.Transform.DOMove(
            intermediatePosition,
            dropAnimDuration * 0.4f)
            .SetEase(Ease.OutQuad));

        dropSequence.Append(droppedObject.Transform.DOMove(
            position,
            dropAnimDuration * 0.6f)
            .SetEase(dropEase));

        dropSequence.OnComplete(() => {
            currentCarriedObject = null;

            droppedObject.Transform.SetParent(null);
            droppedObject.Transform.position = position;

            droppedObject.OnDrop(position, rotation);

            isAnimating = false;
        });

        return true;
    }
}

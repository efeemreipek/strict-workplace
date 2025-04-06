using UnityEngine;

public class PlayerScanner : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float rayCheckHeight = 0.4f;
    [SerializeField] private float rayCheckLength = 1f;

    private PlayerController controller;

    public IInteractable DetectedInteractable { get; private set; }

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    public bool IsBlocked(Vector3 direction)
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(origin, direction, rayCheckLength);
    }
    public bool TryGetPlacePosition(out Vector3 placePosition)
    {
        Vector3 direction = controller.LastInputDirection;
        placePosition = Vector3.zero;

        if(direction == Vector3.zero)
        {
            direction = transform.forward;
        }

        Vector3 targetPos = transform.position + direction;

        if(Physics.Raycast(GetRayOrigin(), direction, rayCheckLength)) return false;

        Vector3 groundCheckOrigin = GetRayOrigin() + direction;
        if(Physics.Raycast(groundCheckOrigin, Vector3.down, out RaycastHit hit, rayCheckLength))
        {
            placePosition = hit.point;
            return true;
        }

        return false;
    } 
    public bool TryScanForInteractable(out IInteractable interactable)
    {
        if(Physics.Raycast(GetRayOrigin(), transform.forward, out RaycastHit hit, rayCheckLength, interactableLayer))
        {
            interactable = hit.collider.GetComponent<IInteractable>();
            DetectedInteractable = interactable;
            return interactable != null;
        }

        interactable = null;
        DetectedInteractable = null;
        return false;
    }

    private Vector3 GetRayOrigin() => transform.position + Vector3.up * rayCheckHeight;
}

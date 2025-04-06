using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 10f, -10f);
    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField] private Vector2 xBounds;
    [SerializeField] private Vector2 zBounds;

    private Vector3 currentVelocity = Vector3.zero;
    private float targetY;

    private void Start()
    {
        targetY = target.position.y;
    }

    private void LateUpdate()
    {
        if(target == null)
            return;

        Vector3 desiredPosition = new Vector3(target.position.x, targetY, target.position.z) + offset;

        desiredPosition.x = Mathf.Clamp(desiredPosition.x, xBounds.x, xBounds.y);
        desiredPosition.z = Mathf.Clamp(desiredPosition.z, zBounds.x, zBounds.y);

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);
    }
}
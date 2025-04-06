using UnityEngine;
using DG.Tweening;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveDuration = 0.2f;
    [SerializeField] private float jumpHeight = 0.5f;
    [SerializeField] private Ease jumpEaseIn;
    [SerializeField] private Ease jumpEaseOut;

    private bool isMoving;
    private bool isRotating;
    private Vector3 lastInputDirection;
    private PlayerScanner scanner;
    private PlayerInteractor interactor;
    private bool canMove = true;
    private bool isGameStarted;

    public bool IsMoving => isMoving;
    public bool IsRotating => isRotating;
    public Vector3 LastInputDirection => lastInputDirection;

    private void Awake()
    {
        scanner = GetComponent<PlayerScanner>();
        interactor = GetComponent<PlayerInteractor>();
    }
    private void OnEnable()
    {
        Door.OnDoorInteracted += Door_OnDoorInteracted;
        ScreenFadeHandler.Current.OnFadeOutComplete += Fade_OnFadeOutComplete;
        ScreenFadeHandler.Current.OnFadeInComplete += Fade_OnFadeInComplete;
        StartUI.OnGameStart += StartUI_OnGameStart;
    }
    private void OnDisable()
    {
        Door.OnDoorInteracted -= Door_OnDoorInteracted;
        ScreenFadeHandler.Current.OnFadeOutComplete -= Fade_OnFadeOutComplete;
        ScreenFadeHandler.Current.OnFadeInComplete -= Fade_OnFadeInComplete;
        StartUI.OnGameStart -= StartUI_OnGameStart;
    }
    private void Door_OnDoorInteracted() => canMove = false;
    private void Fade_OnFadeOutComplete() => canMove = true;
    private void Fade_OnFadeInComplete() => transform.position = Vector3.zero;
    private void StartUI_OnGameStart() => isGameStarted = true;
    private void Update()
    {
        if(!canMove) return;
        if(isMoving) return;
        if(isRotating) return;
        if(!isGameStarted) return;

        if(interactor != null && interactor.IsAnimating) return;

        Vector2 input = InputHandler.Current.Move;

        if(Mathf.Abs(input.x) + Mathf.Abs(input.y) > 1.1f) input = Vector2.zero;

        if(input != Vector2.zero)
        {
            Vector3 inputDirection = new Vector3(input.x, 0f, input.y).normalized;
            Vector3 currentForward = transform.forward;

            if(Vector3.Angle(currentForward, inputDirection) > 1f)
            {
                RotateTowards(inputDirection);
            }
            else
            {
                if(!scanner.IsBlocked(inputDirection))
                {
                    Vector3 targetPosition = transform.position + inputDirection;
                    MoveTo(targetPosition);
                }
            }

            lastInputDirection = inputDirection;
        }
    }

    private void MoveTo(Vector3 targetPos)
    {
        isMoving = true;
        AudioManager.Current.PlayMoveSFX(0.2f);

        float startY = transform.position.y;

        Sequence moveSequence = DOTween.Sequence();

        Tween moveTween = transform.DOMove(new Vector3(targetPos.x, startY, targetPos.z), moveDuration)
            .SetEase(Ease.Linear);

        Tween jumpUpTween = transform.DOMoveY(startY + jumpHeight, moveDuration * 0.5f)
            .SetEase(jumpEaseIn);

        moveSequence.Join(moveTween);
        moveSequence.Join(jumpUpTween);

        moveSequence.AppendCallback(() =>
        {
            transform.DOMoveY(
                startY,
                moveDuration * 0.5f)
                .SetEase(jumpEaseOut);
        });

        moveSequence.OnComplete(() =>
        {
            transform.position = new Vector3(targetPos.x, startY, targetPos.z);
            isMoving = false;

            if(interactor != null && interactor.CurrentCarriedObject != null)
            {
                interactor.UpdateCarriedObjectPosition();
            }
        });
    }
    private void RotateTowards(Vector3 direction)
    {
        if(direction == Vector3.zero) return;

        isRotating = true;

        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.DORotateQuaternion(lookRotation, moveDuration * 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                isRotating = false;
                if(interactor != null && interactor.CurrentCarriedObject != null)
                {
                    interactor.UpdateCarriedObjectPosition();
                }
            });
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : Singleton<InputHandler>
{
    private InputAction actionMove;
    private InputAction actionInteract;
    private InputAction actionTab;

    private Vector2 move;
    private bool interact, interactDown;
    private bool tab, tabDown;

    public Vector2 Move => move;
    public bool InteractPressed => interactDown;
    public bool InteractHeld => interact;
    public bool TabPressed => tabDown;

    private void OnEnable()
    {
        actionMove = InputSystem.actions.FindAction("Move");
        actionInteract = InputSystem.actions.FindAction("Interact");
        actionTab = InputSystem.actions.FindAction("Tab");

        actionMove.performed += Move_Performed;
        actionMove.canceled += Move_Canceled;

        actionInteract.performed += Interact_Performed;
        actionInteract.canceled += Interact_Canceled;

        actionTab.performed += Tab_Performed;
        actionTab.canceled += Tab_Canceled;

        InputSystem.actions.Enable();
    }
    private void OnDisable()
    {
        actionMove.performed -= Move_Performed;
        actionMove.canceled -= Move_Canceled;

        actionInteract.performed -= Interact_Performed;
        actionInteract.canceled -= Interact_Canceled;

        actionTab.performed -= Tab_Performed;
        actionTab.canceled -= Tab_Canceled;

        InputSystem.actions.Disable();
    }
    private void LateUpdate()
    {
        interactDown = false;
        tabDown = false;
    }

    private void Move_Performed(InputAction.CallbackContext obj) => move = obj.ReadValue<Vector2>();
    private void Move_Canceled(InputAction.CallbackContext obj) => move = Vector2.zero;
    private void Interact_Performed(InputAction.CallbackContext obj)
    {
        interact = true;
        interactDown = true;
    }
    private void Interact_Canceled(InputAction.CallbackContext obj)
    {
        interact = false;
    }
    private void Tab_Performed(InputAction.CallbackContext obj)
    {
        tab = true;
        tabDown = true;
    }
    private void Tab_Canceled(InputAction.CallbackContext obj)
    {
        tab = false;
    }
}

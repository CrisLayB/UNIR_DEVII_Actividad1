using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Gameplay";

    [Header("Action Name References")]
    [SerializeField] private string moveActionName = "Move";
    [SerializeField] private string lookActionName = "Look";
    [SerializeField] private string interactActionName = "Interact";

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction interactAction;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool InteractPressed { get; private set; }



    private void Awake()
    {
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);

        moveAction = mapReference.FindAction(moveActionName);
        lookAction = mapReference.FindAction(lookActionName);
        interactAction = mapReference.FindAction(interactActionName);

        SubscribeActionValuesToInputEvents();
    }

    private void SubscribeActionValuesToInputEvents()
    {
        moveAction.performed += inputInfo => MoveInput = inputInfo.ReadValue<Vector2>();
        moveAction.canceled += inputInfo => MoveInput = Vector2.zero;

        lookAction.performed += inputInfo => LookInput = inputInfo.ReadValue<Vector2>();
        lookAction.canceled += inputInfo => LookInput = Vector2.zero;

        if (interactAction != null)
        {
            interactAction.performed += _ => InteractPressed = true;
            interactAction.canceled += _ => InteractPressed = false;
        }
        else
        {
            Debug.LogWarning(gameObject.name + ": Interact action not found in action map '" + actionMapName + "'.");
        }
    }



    private void OnEnable()
    {
        playerControls.FindActionMap(actionMapName).Enable();
    }

    private void OnDisable()
    {
        playerControls.FindActionMap(actionMapName).Disable();
    }
}

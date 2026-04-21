using UnityEngine;
using TMPro;

public class InteractSystem : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask interactableLayer;

    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private TMP_Text actionText;

    private Interactable _currentInteractable;

    private void Awake()
    {
        if (inputHandler == null)
            Debug.LogWarning(gameObject.name + ": No PlayerInputHandler component found.");

        if (playerCamera == null)
            playerCamera = Camera.main;

        if (actionText == null)
            Debug.LogWarning(gameObject.name + ": No TextMeshPro component assigned for action prompts.");
        else
            actionText.text = "";
    }

    private void Update()
    {
        DetectInteractable();

        if (_currentInteractable != null && inputHandler != null && inputHandler.InteractPressed)
            _currentInteractable.Interact();
    }

    private void DetectInteractable()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out RaycastHit hit, interactionDistance, interactableLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                _currentInteractable = interactable;

                if (actionText != null)
                    actionText.text = $"[E] - {_currentInteractable.InteractableName}";

                Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            }
            else
            {
                ClearInteractable();
                Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward) * interactionDistance, Color.green);
            }
        }
        else
        {
            ClearInteractable();
            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward) * interactionDistance, Color.green);
        }
    }

    private void ClearInteractable()
    {
        _currentInteractable = null;

        if (actionText != null)
            actionText.text = "";
    }
}

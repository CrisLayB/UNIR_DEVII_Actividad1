using System.Collections.Generic;
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
    private HashSet<string> _collectedPickableNames = new HashSet<string>();

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

        if (_currentInteractable != null && inputHandler != null && inputHandler.InteractPressed && CanInteract(_currentInteractable))
        {
            _currentInteractable.Interact();

            if (_currentInteractable is Pickable pickable)
            {
                _collectedPickableNames.Add(pickable.InteractableName);
                if (pickable.IsDestroyabelAfterUse)
                    Destroy(pickable.gameObject);
            }
        }
    }

    private bool CanInteract(Interactable interactable)
    {
        string[] required = interactable.RequiredPickableNames;
        if (required != null)
        {
            foreach (string name in required)
            {
                if (!_collectedPickableNames.Contains(name))
                    return false;
            }
        }

        ItemsOn[] requiredOn = interactable.RequiredObjectsOn;
        if (requiredOn != null)
        {
            foreach (ItemsOn item in requiredOn)
            {
                ObjectOn oo = item.objectOn as ObjectOn;
                if (oo == null || oo.IsOn != item.requiredState)
                    return false;
            }
        }

        return true;
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
                {
                    actionText.text = $"[E] - {_currentInteractable.InteractableName}";

                    string[] required = _currentInteractable.RequiredPickableNames;
                    ItemsOn[] requiredOn = _currentInteractable.RequiredObjectsOn;
                    bool hasRequirements = (required != null && required.Length > 0) || (requiredOn != null && requiredOn.Length > 0);

                    if (hasRequirements)
                    {
                        actionText.text += "\nNecesitas:";

                        if (required != null)
                        {
                            foreach (string name in required)
                            {
                                bool hasItem = _collectedPickableNames.Contains(name);
                                actionText.text += $" [{name}] {(hasItem ? "\u2713" : "\u2717")}";
                            }
                        }

                        if (requiredOn != null)
                        {
                            foreach (ItemsOn item in requiredOn)
                            {
                                if (item.objectOn == null) continue;
                                ObjectOn oo = item.objectOn as ObjectOn;
                                bool met = oo != null && oo.IsOn == item.requiredState;
                                string label = item.objectOn.gameObject.name;
                                actionText.text += $" [{label}: {(item.requiredState ? "ON" : "OFF")}] {(met ? "\u2713" : "\u2717")}";
                            }
                        }
                    }
                }

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

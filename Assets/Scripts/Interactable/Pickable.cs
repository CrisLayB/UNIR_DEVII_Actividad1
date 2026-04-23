using UnityEngine;

public class Pickable : Interactable
{
    [Header("Pickable Settings")]
    [SerializeField] private bool isDestroyabelAfter = false;

    public bool IsDestroyabelAfterUse => isDestroyabelAfter;
    
    public override void Interact()
    {
        base.Interact();

    }
}

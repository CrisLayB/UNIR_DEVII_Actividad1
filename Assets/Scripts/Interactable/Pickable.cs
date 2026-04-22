using UnityEngine;

public class Pickable : Interactable
{
    [SerializeField] private bool isDestroyabelAfter = false;

    public bool IsDestroyabelAfterUse => isDestroyabelAfter;
    
    public override void Interact()
    {
        base.Interact();

    }
}

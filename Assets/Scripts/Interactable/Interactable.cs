using UnityEngine;

[System.Serializable]
public class ItemsOn
{
    public MonoBehaviour objectOn;
    public bool requiredState;
}

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    [Header("Interactable Settings")]
    [SerializeField] private string interactableName;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private GameObject dropableObject;
    [SerializeField] protected string[] requiredPickableNames;
    [SerializeField] protected ItemsOn[] requiredObjectsOn;

    private Animator _animator;
    private bool _interacted = false;

    public string InteractableName => interactableName;
    public string[] RequiredPickableNames => requiredPickableNames;
    public ItemsOn[] RequiredObjectsOn => requiredObjectsOn;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogWarning(gameObject.name + ".cs: No Animator component found");
        }
    }

    public virtual void Interact()
    {
        if (_interacted) return;

        _interacted = true;

        if (_animator != null)
        {
            _animator.SetTrigger("Interact");
        }

        if (dropableObject != null)
        {
            Instantiate(dropableObject, transform.position + Vector3.up, Quaternion.identity);
        }
    }
}

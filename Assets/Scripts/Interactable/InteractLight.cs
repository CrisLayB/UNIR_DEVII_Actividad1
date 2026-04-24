using UnityEngine;

public class InteractLight : Interactable, ObjectOn
{
    [Header("Light Settings")]
    [SerializeField] private Light _light;

    public bool IsLightOn => _light != null && _light.enabled;
    public bool IsOn { get; set; }

    private void Awake()
    {
        if (_light == null)
        {
            Debug.LogWarning(gameObject.name + ".cs: No Light component found");
        }
        else
        {
            _light.enabled = false; // Ensure the light starts off
        }
    }

    public override void Interact()
    {
        base.Interact();

        if (_light != null)
        {
            _light.enabled = !_light.enabled;
            IsOn = _light.enabled;
        }
    }
}
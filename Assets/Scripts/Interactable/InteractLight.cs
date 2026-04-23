using UnityEngine;

public class InteractLight : Interactable, ObjectOn
{
    [Header("Light Settings")]
    [SerializeField] private Light[] _lights;

    public bool IsLightOn => _lights != null && _lights.Length > 0 && _lights[0].enabled;
    public bool IsOn { get; set; }

    private void Awake()
    {
        if (_lights == null || _lights.Length == 0)
        {
            Debug.LogWarning(gameObject.name + ".cs: No Light component found");
        }
        else
        {
            foreach (var light in _lights)
            {
                light.enabled = false; // Ensure the light starts off
            }
        }
    }

    public override void Interact()
    {
        base.Interact();

        if (_lights != null && _lights.Length > 0)
        {
            foreach (var light in _lights)
            {
                light.enabled = !light.enabled;
            }
            IsOn = _lights[0].enabled;
        }
    }
}

using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Drawer : Interactable
{
    [SerializeField] private int drawerID;
    [SerializeField] private GameObject order;
    [SerializeField] private GameObject door;

    private bool isOpen = false;

    public event Action<int, bool> OnDrawerStateChanged;

    private float originalXPosition;
    private bool isMoving = false;

    private void Start()
    {
        originalXPosition = transform.localPosition.x;
        order = GetComponentInChildren<TextMeshPro>().gameObject;
        order.SetActive(false);
    }

    public void Open()
    {
        if (isOpen || isMoving)
            return;

        isOpen = true;
        StartCoroutine(RotateDoor(true));
    }

    public void Close()
    {
        if (!isOpen || isMoving)
            return;

        isOpen = false;
        StartCoroutine(RotateDoor(false));
    }

    public void Toggle()
    {
        if (isMoving)
            return;

        if (isOpen)
            Close();
        else
            Open();
    }

    private IEnumerator RotateDoor(bool finalOpenState)
    {
        isMoving = true;

        var targetRot = finalOpenState ? Quaternion.Euler(0, -90, 0) : Quaternion.identity;

        while (Quaternion.Angle(door.transform.localRotation, targetRot) > 0.1f)
        {
            door.transform.localRotation = Quaternion.Slerp(
                door.transform.localRotation,
                targetRot,
                Time.deltaTime * 8f
            );
            yield return null;
        }

        isMoving = false;

        OnDrawerStateChanged?.Invoke(drawerID, isOpen);
    }

    public void ShowOrder()
    {
        order.SetActive(true);
    }

    public void HideOrder()
    {
        order.SetActive(false);
    }

    override public void Interact()
    {
        Toggle();
    }
}

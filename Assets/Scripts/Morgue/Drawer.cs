using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    [SerializeField] private int drawerID;
    [SerializeField] private GameObject order;

    private bool isOpen = false;

    public event Action<int, bool> OnDrawerStateChanged;

    private float originalXPosition;
    private float destinationX = -2f;
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
        StartCoroutine(MoveToPosition(destinationX, true));
    }

    public void Close()
    {
        if (!isOpen || isMoving)
            return;

        isOpen = false;
        StartCoroutine(MoveToPosition(originalXPosition, false));
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

    private IEnumerator MoveToPosition(float targetX, bool finalOpenState)
    {
        isMoving = true;

        Vector3 startPos = transform.localPosition;
        Vector3 targetPos = new Vector3(targetX, startPos.y, startPos.z);

        while (Vector3.Distance(transform.localPosition, targetPos) > 0.001f)
        {
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                targetPos,
                Time.deltaTime * 8f
            );

            yield return null;
        }

        transform.localPosition = targetPos;

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
}

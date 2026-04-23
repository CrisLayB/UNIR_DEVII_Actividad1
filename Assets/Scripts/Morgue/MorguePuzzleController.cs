using System;
using UnityEngine;

public class MorguePuzzleController : MonoBehaviour
{
    [SerializeField] private Drawer[] drawers;

    [SerializeField] private int[] drawersOrder = new int[5];

    [SerializeField] private GameObject[] lights;

    public Drawer[] Drawers => drawers;

    int currentDrawerIndex = 0;

    public event Action OnPuzzleCompleted;

    void Start()
    {
        foreach (var drawer in drawers)
        {
            drawer.OnDrawerStateChanged += RegisterDrawerState;
        }

        currentDrawerIndex = 0;
    }

    private void RegisterDrawerState(int drawerID, bool isOpen)
    {
        if (isOpen)
        {
            HandleOpen(drawerID);
        }
        else
        {
            HandleClose(drawerID);
        }
    }

    private void HandleOpen(int drawerID)
    {
        // correcto
        if (drawerID == drawersOrder[currentDrawerIndex])
        {
            currentDrawerIndex++;
            CheckPuzzleCompletion();
            return;
        }

        // incorrecto
        ResetPuzzle();
    }

    private void HandleClose(int drawerID)
    {
        if (currentDrawerIndex <= 0)
            return;

        int previousCorrectDrawer = drawersOrder[currentDrawerIndex - 1];

        if (drawerID == previousCorrectDrawer)
        {
            currentDrawerIndex--;
        }
    }

    private void CheckPuzzleCompletion()
    {
        if (currentDrawerIndex >= drawersOrder.Length)
        {
            Debug.Log("Puzzle completed!");
            OnPuzzleCompleted?.Invoke();
        }
    }

    private void ResetPuzzle()
    {
        Debug.Log("Wrong drawer! Resetting puzzle.");

        foreach (var drawer in drawers)
        {
            drawer.Close();
        }

        currentDrawerIndex = 0;
    }

    private void OnDisable()
    {
        foreach (var drawer in drawers)
        {
            drawer.OnDrawerStateChanged -= RegisterDrawerState;
        }
    }

    public void LightsOn()
    {
        foreach (var light in lights)
        {
            light.SetActive(true);
        }

        foreach (var drawer in drawers)
        {
            drawer.ShowOrder();
        }
    }

    public void LightsOff()
    {
        foreach (var light in lights)
        {
            light.SetActive(false);
        }

        foreach (var drawer in drawers)
        {
            drawer.HideOrder();
        }
    }

    public void SwitchPressed()
    {
        if (lights[0].activeSelf)
        {
            LightsOff();
        }
        else
        {
            LightsOn();
        }
    }
}

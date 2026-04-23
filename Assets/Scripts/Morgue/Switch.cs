using UnityEngine;

public class Switch : Interactable
{
    [SerializeField] private MorguePuzzleController _puzzleController;

    override public void Interact()
    {
        _puzzleController.SwitchPressed();
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class MorgueDebugger : MonoBehaviour
{
    [SerializeField] private MorguePuzzleController puzzleController;

    private Drawer[] drawers;

    void Start()
    {
        drawers = puzzleController.Drawers;
    }

    private void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard.digit0Key.wasPressedThisFrame)
            drawers[0].Toggle();

        if (keyboard.digit1Key.wasPressedThisFrame)
            drawers[1].Toggle();

        if (keyboard.digit2Key.wasPressedThisFrame)
            drawers[2].Toggle();

        if (keyboard.digit3Key.wasPressedThisFrame)
            drawers[3].Toggle();

        if (keyboard.digit4Key.wasPressedThisFrame)
            drawers[4].Toggle();

        if (keyboard.oKey.wasPressedThisFrame)
            puzzleController.LightsOn();

        if (keyboard.fKey.wasPressedThisFrame)
            puzzleController.LightsOff();
    }
}

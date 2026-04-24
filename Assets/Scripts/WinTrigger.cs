using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    private void OnTriggerEnter(Collider other)
    {
        gameController.WinGame();
    }
}

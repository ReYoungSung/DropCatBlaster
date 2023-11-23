using UnityEngine;

public class GameOverMenuManager : PauseManager
{
    [SerializeField] private GameObject[] mainMenu = new GameObject[2];
    private int menuPointer = 0;

    private _PlayerInputActions PlayerInputActions;
}

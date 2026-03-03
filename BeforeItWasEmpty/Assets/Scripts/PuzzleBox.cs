using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleBox : Interactable
{
    public override void Interact(CCPlayer ccplayer)
    {
        if(ccplayer.interactPressed == false)
        {
            SceneManager.LoadScene("Puzzle");
            Debug.Log("Scene Loaded");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}

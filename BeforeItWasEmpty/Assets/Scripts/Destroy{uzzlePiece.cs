using UnityEngine;

public class DestroyPuzzlePiece : Interactable
{
    public override void Interact(CCPlayer ccplayer)
    {
        Destroy(gameObject);
        Debug.Log("Destroy");
        
    }

}

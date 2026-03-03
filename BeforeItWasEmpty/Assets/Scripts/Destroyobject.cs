using UnityEngine;

public class Destroyobject : Interactable
{
    public override void Interact(CCPlayer ccplayer)
    {
        Destroy(gameObject);
        Debug.Log("Destroy");
    }
}

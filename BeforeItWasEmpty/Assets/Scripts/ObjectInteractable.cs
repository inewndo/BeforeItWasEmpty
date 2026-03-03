using UnityEngine;
using UnityEngine.Rendering;

public class ObjectInteractable : Interactable
{
    public ObjectData objectData; 
    public override void Interact(CCPlayer ccplayer)
    {
        if(objectData == null)
        {
            Debug.Log("object has no data" + gameObject.name);
        }
        
       
    }
}

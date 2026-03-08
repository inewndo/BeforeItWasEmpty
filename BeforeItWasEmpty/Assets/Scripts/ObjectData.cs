using UnityEngine;
[CreateAssetMenu(menuName = "Object Data")]

public class ObjectData : ScriptableObject
{
    public string displayName;
    [TextArea(3, 10)]
    public string[] lines;
    public ObjectData nextNode;
    //public GameObject puzzlePiece;

    ////public string displayName;
    ////public string Description;

    //public void DestroyPiece()
    //{
    //    Destroy(puzzlePiece); 
    //}
}



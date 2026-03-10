using UnityEngine;

public class BillBoard : MonoBehaviour
{
    //private BillBoard lookAtCamera;

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
    }
}

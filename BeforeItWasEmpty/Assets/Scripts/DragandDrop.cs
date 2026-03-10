using Unity.VisualScripting;
using UnityEngine;

public class DragandDrop : MonoBehaviour
{
    private GameObject selectedObject;
   
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0)) //0 is identifier for left mouse button
        {
            //assign selected object and pick something up, otherwise set the object down
            //maybe not idk at this point
            //somthing something checks that object isnt picked up maybe???
            if(selectedObject == null)
            {
                RaycastHit hit = CastRay();

                //check if collider was hit
                if(hit.collider != null)
                {
                    if (!hit.collider.CompareTag("Drag"))
                    {
                        return;
                    }
                    selectedObject = hit.collider.gameObject;
                    Cursor.visible = false;
                }
            }
            else
            {
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                //drop the piece
                selectedObject.transform.position = new Vector3(worldPosition.x, 0f, worldPosition.z);

                selectedObject = null;
                Cursor.visible = true;

            }
        }

        if (selectedObject != null)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            //pick up piece and slightly lift it
            selectedObject.transform.position = new Vector3(worldPosition.x, .25f, worldPosition.z);

            //rotation of pieces when picked up on right click
            if (Input.GetMouseButtonDown(1)) //1 is identifier for right mouse button
            {
                selectedObject.transform.rotation = Quaternion.Euler(new Vector3(

                    selectedObject.transform.rotation.eulerAngles.x,
                    selectedObject.transform.rotation.eulerAngles.y + 90f,
                    selectedObject.transform.rotation.eulerAngles.z));
            }
        }
    }

    private RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);

        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);

        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;

    }
}

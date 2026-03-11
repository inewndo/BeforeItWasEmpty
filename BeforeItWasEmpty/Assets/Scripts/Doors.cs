using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Doors : MonoBehaviour
{
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public bool isOpen = false;

    //starting rotation
    private Quaternion closedRotation;
    //calculate what rotation should look like when opening
    private Quaternion openRoatation;
    //for smooth rotation
    private Coroutine currentCoroutine;

    //public CCPlayer currentInteractable;
    public Transform cameraTransform;

    void Start()
    {
        //save current rotation
        closedRotation = transform.rotation;
        //where the door should rotate when open
        openRoatation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }
    void Update()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f))
        {
            if (hit.collider.gameObject.tag == "Door")
            {
                if (Input.GetKeyDown(KeyCode.Q))
                { 
                //make sure there is no ongoing animation; stop any ongoing Coroutine
                if (currentCoroutine != null) StopCoroutine(currentCoroutine);
                //start new coroutine to start door opening
                currentCoroutine = StartCoroutine(ToggleDoor());
                }
            }
        }
    }
   

    //handles opening and closing
    public IEnumerator ToggleDoor()
    {
        //decides to open or close based on current state
        Quaternion targetRotation = isOpen ? closedRotation : openRoatation;
        isOpen = !isOpen;
        //smooth movement
        while(Quaternion.Angle(transform.rotation, targetRotation) > 0.01f) //quarternion rotate towards
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }
        //lock in place to prevent jittering
        transform.rotation = targetRotation;
    }
}

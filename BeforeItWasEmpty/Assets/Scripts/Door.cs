using UnityEngine;

public class Door : MonoBehaviour
{
    public float interactionDistance;
    //public GameObject intText;

    public string doorOpenAnimName, doorCloseAnimName;

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, interactionDistance))
        {
            if(hit.collider.gameObject.tag == "Door")
            {
                GameObject doorParent = hit.collider.transform.root.gameObject;
                Animator doorAnim = doorParent.GetComponent<Animator>();
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName(doorOpenAnimName))
                    {
                        doorAnim.ResetTrigger("open");
                        doorAnim.ResetTrigger("close");
                    }
                    if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName(doorCloseAnimName))
                    {
                        doorAnim.ResetTrigger("close");
                        doorAnim.ResetTrigger("open");
                    }
                }
            }
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float FOV = 90;
    public float viewDistance;
    public Transform target;
    public Transform rayOrigin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        Vector3 angleFrom = target.position - transform.position;
        float angle = Vector3.Angle(angleFrom, transform.forward);
        RaycastHit hitInfor;
        if (distance < viewDistance && angle < FOV/2)
        {
            if(Physics.Raycast(rayOrigin.position, angleFrom, out hitInfor, viewDistance))
            {
                if(hitInfor.collider.gameObject.layer == 6)
                {
                    Debug.Log("fou");
                }
            }
        }
    }
}

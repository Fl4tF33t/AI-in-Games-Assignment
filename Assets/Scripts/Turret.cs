using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Turret : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    public Transform turretBarrel;


    public float fieldOfViewRange = 90;
    public float viewDistance;

    GameObject[] target;
    public Transform rayOrigin;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ShootBullet", 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindGameObjectsWithTag("Player");
        if(target != null )
        {
            for (int i = 0; i < target.Length; i++)
            {
                CanSeePlayer(target[i]);
            }
        }
    }

    bool CanSeePlayer(GameObject target)
    {
        RaycastHit hit;
        Vector3 rayDirection = target.transform.position - transform.position;

        if (Vector3.Angle(rayDirection, transform.forward) < fieldOfViewRange/2)
        { // Detect if player is within the field of view
            if (Physics.Raycast(transform.position, rayDirection, out hit))
            {
                if (hit.transform.tag == "Player")
                {
                    Debug.Log("Can see player");
                    return true;
                }
                else
                {
                    Debug.Log("Can not see player");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    private void ShootBullet()
    {
        
        GameObject bullet = Instantiate(bulletPrefab, turretBarrel);
        bullet.GetComponent<Rigidbody>().AddForce(Vector3.forward * 5, ForceMode.Impulse);
    }
}

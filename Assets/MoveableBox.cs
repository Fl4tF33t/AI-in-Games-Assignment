using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableBox : MonoBehaviour
{
    private Rigidbody boxRB;

    private void Awake()
    {
        boxRB = GetComponent<Rigidbody>();
    }

    public void MoveBox(Vector3 direction)
    {
        boxRB.AddForce(direction * 10, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}

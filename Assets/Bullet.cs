using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int bulletDamage = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BaseAgent>())
        {
            other.GetComponent<BaseAgent>().DamageTaken(bulletDamage);
        }
    }
}

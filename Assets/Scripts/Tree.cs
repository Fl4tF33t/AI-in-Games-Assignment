using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, ITakeDamage
{
    public int health = 10;

    public void TakeDamage(int amount)
    {
        health -= amount;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }


   
}

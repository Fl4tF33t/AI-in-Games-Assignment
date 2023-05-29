using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeData : MonoBehaviour, IDamage
{
    int treeHealth = 100;
    public void DamageTaken(int damage)
    {
        treeHealth -= damage;
    }

    private void Update()
    {
        if (treeHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}

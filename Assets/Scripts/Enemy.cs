using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _health = 3;
    
    public void TakeDamage(float damage)
    {
        _health--;
        
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}

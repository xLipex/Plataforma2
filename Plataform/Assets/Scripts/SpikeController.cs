using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    public int damage = 10;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 direction = collision.transform.position - transform.position;
            direction = direction.normalized;
            
            collision.gameObject.GetComponent<PlayerControle>().ChangeHearth(-damage, true, direction);
        }
    }
}

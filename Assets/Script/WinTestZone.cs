using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTestZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Animator playerAnim = other.GetComponent<Animator>();
            if (playerAnim != null)
            {
                playerAnim.SetTrigger("Win");
                Debug.Log("Player has reached the goal and triggered Win!");
            }
        }
    }
}

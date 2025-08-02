using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTestAnimation : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(string attackType)
    {
        if (attackType == "normal")
        {
            Debug.Log("Dummy bị trúng đòn thường → Hurt");
            anim.SetTrigger("Hurt");
        }
        else if (attackType == "heavy")
        {
            Debug.Log("Dummy bị trúng Heavy Attack → Dizzy");
            anim.SetTrigger("Dizzy"); // Optional animation
        }
    }
}
using System;
using UnityEngine;

public class DoorRemote : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.SetTrigger("Door_Open");
        }
    }
}

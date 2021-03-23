using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundChaker : MonoBehaviour
{

    PlayerController playerController;
    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("cEnter");
        if (collision.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedSate(true);
    }
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("cExit");
        if (collision.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedSate(false);
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("cstay");
        if (collision.gameObject == playerController.gameObject)
            return;
        
        playerController.SetGroundedSate(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("tEnter");
        if (other.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedSate(true);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("tExit");
        if (other.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedSate(false);
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("tstay");
        if (other.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedSate(true);
    }
}

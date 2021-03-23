using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundChaker : MonoBehaviour
{

    PlayerController playerController;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedSate(true);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedSate(false);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedSate(true);
    }
}

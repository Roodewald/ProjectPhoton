using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject self;

    private void Awake()
    {
        self.SetActive(false);
    }
}

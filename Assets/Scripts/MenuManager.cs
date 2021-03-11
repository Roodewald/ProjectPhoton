using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] public GameObject[] menus;


    private void Start()
    {
        
    }

    public void QuitGame()
    {
        Debug.Log("Application Quit");
        Application.Quit();
    }

    public void Open(GameObject menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].activeSelf)
            {
                menus[i].SetActive(false);
            }
        }
        menu.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnd : MonoBehaviour
{
    [SerializeField] GameObject menuManagerReference;
    MenuManager menuManager;
    void Awake()
    {
        menuManager = menuManagerReference.GetComponent<MenuManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            menuManager.FinishGame();
        }
    }
}

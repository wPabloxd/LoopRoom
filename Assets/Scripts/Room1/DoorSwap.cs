using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwap : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject door1;
    [SerializeField] GameObject door2;
    [SerializeField] GameObject entrance;
    [SerializeField] GameObject frontroom;
    [SerializeField] GameObject exit;
    [SerializeField] GameObject backroom;
    [SerializeField] GameObject tp1;
    [SerializeField] GameObject tp2;
    [SerializeField] GameObject chair1;
    [SerializeField] GameObject chair2;
    [SerializeField] GameObject exitRoom2;
    Animator door1an;
    Animator door2an;
    float oldPos;
    bool swap;
    public bool door1Open;
    public bool door2Open;
    public bool chairStuck;
    bool portal;
    public bool doorStucked;
    bool cD;
    bool swapCD;
    bool doors = true;

    void Start()
    {
        door1an = door1.GetComponent<Animator>();
        door2an = door2.GetComponent<Animator>();
    }

    void Update()
    {
        if(((player.transform.position.x > 0 && oldPos < 0) || (player.transform.position.x < 0 && oldPos > 0)) && cD && GameManager.teleported)
        {
            DoorTravel();
        }
        oldPos = player.transform.position.x;
        if (GameManager.swap)
        {
            Vector3 aux = door1.transform.position;
            door1.transform.position = door2.transform.position;
            door2.transform.position = aux;
            GameManager.swap = false;
            if (swap)
            {
                swap = false;
            }
            else
            {
                swap = true;
            }
        }

        if (door1an.GetCurrentAnimatorStateInfo(0).IsName("doorOpen"))
        {
            if (swap)
            {
                door2Open = true;
            }
            else
            {
                door1Open = true;
            }
        }
        else if (door1an.GetCurrentAnimatorStateInfo(0).IsName("doorClosed"))
        {
            doorStucked = false;
            if (swap)
            {
                door2Open = false;
            }
            else
            {
                door1Open = false;
            }
        }
        else if (door1an.GetCurrentAnimatorStateInfo(0).IsName("doorOpened"))
        {
            if (swap)
            {
                door2Open = true;
            }
            else
            {
                door1Open = true;
            }
        }
        else if (door1an.GetCurrentAnimatorStateInfo(0).IsName("doorClose"))
        {
            if (swap)
            {
                door2Open = false;
            }
            else
            {
                door1Open = false;
            }
        }

        if (door2an.GetCurrentAnimatorStateInfo(0).IsName("doorOpen"))
        {
            if (swap)
            {
                door1Open = true;
            }
            else
            {
                door2Open = true;
            }
        }
        else if (door2an.GetCurrentAnimatorStateInfo(0).IsName("doorClosed"))
        {
            doorStucked = false;
            if (swap)
            {
                door1Open = false;
            }
            else
            {
                door2Open = false;
            }
        }
        else if (door2an.GetCurrentAnimatorStateInfo(0).IsName("doorOpened"))
        {
            if (swap)
            {
                door1Open = true;
            }
            else
            {
                door2Open = true;
            }
        }
        else if (door2an.GetCurrentAnimatorStateInfo(0).IsName("doorClose"))
        {
            if (swap)
            {
                door1Open = false;
            }
            else
            {
                door2Open = false;
            }
        }

        if (chairStuck && !door2Open)
        {
            entrance.SetActive(true);
            frontroom.SetActive(false);
            exit.SetActive(true);
            backroom.SetActive(false);
            doorStucked = true;
        }
        if (door1Open && !door2Open)
        {
            if (doors)
            {
                doors = false;
                StartCoroutine(Portal());
            }
            if (!portal)
            {
                portal = true;
            }
            if (swapCD)
            {
                if (chair1.GetComponent<ObjectDetection>().place == 2)
                {
                    chair1.SetActive(false);
                }
                if (chair2.GetComponent<ObjectDetection>().place == 2)
                {
                    chair2.SetActive(false);
                }
                entrance.SetActive(false);
                frontroom.SetActive(true);
                exit.SetActive(false);
                backroom.SetActive(true);
            }
            else
            {
                if (chair1.GetComponent<ObjectDetection>().place == 1)
                {
                    chair1.SetActive(true);
                }
                if (chair2.GetComponent<ObjectDetection>().place == 1)
                {
                    chair2.SetActive(true);
                }
                entrance.SetActive(true);
                frontroom.SetActive(false);
            }
            exitRoom2.SetActive(false);
        }
        else if (door1Open && door2Open && !chairStuck && !doorStucked)
        {
            doors = true;
            if (chair1.GetComponent<ObjectDetection>().place == 1 || chair1.GetComponent<ObjectDetection>().place == 2)
            {
                chair1.SetActive(false);
            }
            if (chair2.GetComponent<ObjectDetection>().place == 1 || chair2.GetComponent<ObjectDetection>().place == 2)
            {
                chair2.SetActive(false);
            }
            portal = false;
            tp1.SetActive(true);
            tp2.SetActive(true);
            entrance.SetActive(false);
            frontroom.SetActive(true);
            exit.SetActive(false);
            backroom.SetActive(true);
        }
        else if (!door1Open && door2Open)
        {
            if (doors)
            {
                doors = false;
                StartCoroutine(Portal());
            }
            if (!portal)
            {
                portal = true;
            }
            if (swapCD)
            {
                if (chair1.GetComponent<ObjectDetection>().place == 1)
                {
                    chair1.SetActive(false);
                }
                if (chair2.GetComponent<ObjectDetection>().place == 1)
                {
                    chair2.SetActive(false);
                }
                entrance.SetActive(false);
                frontroom.SetActive(true);
                exit.SetActive(false);
                backroom.SetActive(true);
            }
            else
            {
                if (chair1.GetComponent<ObjectDetection>().place == 2)
                {
                    chair1.SetActive(true);
                }
                if (chair2.GetComponent<ObjectDetection>().place == 2)
                {
                    chair2.SetActive(true);
                }
                exit.SetActive(true);
                backroom.SetActive(false);
            }
            exitRoom2.SetActive(false);
        }
        else if (!door1Open && !door2Open)
        {
            doors = true;
            portal = false;
            tp1.SetActive(false);
            tp2.SetActive(false);
            entrance.SetActive(true);
            frontroom.SetActive(false);
            exit.SetActive(true);
            backroom.SetActive(false);
        }
    }
    void DoorTravel()
    {
        if (swapCD == true)
        {
            swapCD = false;
        }
        else
        {
            swapCD = true;
        }
    }
    IEnumerator Portal()
    {
        cD = true;
        yield return new WaitForSeconds(1);
        GameManager.teleported = false;
        cD = false;
        tp1.SetActive(false);
        tp2.SetActive(false);
        if (door1Open && !door2Open)
        {
            if (swapCD)
            {
                if (chair1.GetComponent<ObjectDetection>().place == 1)
                {
                    chair1.SetActive(true);
                }
                if (chair2.GetComponent<ObjectDetection>().place == 1)
                {
                    chair2.SetActive(true);
                }
                entrance.SetActive(true);
                frontroom.SetActive(false);
            }
            else
            {
                if (chair1.GetComponent<ObjectDetection>().place == 2)
                {
                    chair1.SetActive(true);
                }
                if (chair2.GetComponent<ObjectDetection>().place == 2)
                {
                    chair2.SetActive(true);
                }
                exit.SetActive(true);
                backroom.SetActive(false);
            }
        }
        else if (!door1Open && door2Open)
        {  
            if (swapCD)
            {
                if (chair1.GetComponent<ObjectDetection>().place == 2)
                {
                    chair1.SetActive(true);
                }
                if (chair2.GetComponent<ObjectDetection>().place == 2)
                {
                    chair2.SetActive(true);
                }
                exit.SetActive(true);
                backroom.SetActive(false);
            }
            else
            {
                if (chair1.GetComponent<ObjectDetection>().place == 1)
                {
                    chair1.SetActive(true);
                }
                if (chair2.GetComponent<ObjectDetection>().place == 1)
                {
                    chair2.SetActive(true);
                }
                entrance.SetActive(true);
                frontroom.SetActive(false);
            }
        }
        swapCD = false;
    }
}
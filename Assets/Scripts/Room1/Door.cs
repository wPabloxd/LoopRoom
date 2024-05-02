using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator an;
    public bool swap;
    bool cooldown;
    ToggleAn toggle;
    bool open;
    void Start()
    {
        an = GetComponent<Animator>();
        toggle = GetComponent<ToggleAn>();
    }
    void Update()
    {
        if (toggle.interacted && !cooldown)
        {
            cooldown = true;
            if (!swap)
            {
                swap = true;
                an.SetTrigger("doorOpen");
                open = true;
            }
            else
            {
                swap = false;
                an.SetTrigger("doorClose");
                open = false;
            }
            StartCoroutine("Cd");
            toggle.interacted = false;
        }
    }
    IEnumerator Cd()
    {
        yield return new WaitForSeconds(1);
        cooldown = false;
        toggle.interacted = false;
    }
    private void OnEnable()
    {
        if (open)
        {
            an.SetTrigger("doorOpened");
        }
    }
}

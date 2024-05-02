using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject cam;
    [SerializeField] GameObject EscMenu;
    [SerializeField] GameObject EndMenu;
    [SerializeField] Slider slide;
    bool ended;
    bool endedOpen;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (!GameManager.pause && !endedOpen)
            {
                Cursor.lockState = CursorLockMode.None;
                GameManager.pause = true;
                EscMenu.gameObject.SetActive(true);
            }
            else
            {
                Resume();
            }
        }
    }
    public void Resume()
    {
        endedOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.pause = false;
        EscMenu.gameObject.SetActive(false);
        EndMenu.gameObject.SetActive(false);
    }
    public void Sensitivity()
    {
        cam.GetComponent<MouseController>().mouseSens = slide.value;
        GameManager.Instance.sens = slide.value;
    }
    void LoadSensitivity()
    {
        cam.GetComponent<MouseController>().mouseSens = GameManager.Instance.sens;
        slide.value = GameManager.Instance.sens;
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        GameManager.pause = false;
        Application.Quit();
    }
    public void FinishGame()
    {
        if (!ended)
        {
            Cursor.lockState = CursorLockMode.Confined;
            GameManager.pause = true;
            ended = true;
            endedOpen = true;
            EndMenu.SetActive(true);
        }
    }
}
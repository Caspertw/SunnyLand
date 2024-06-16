using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackScene : MonoBehaviour
{
    public GameObject enterDialog;
    private bool playerIsHere = false;

    void Update()
    {
        if((Input.GetKeyDown(KeyCode.E)) && playerIsHere)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            enterDialog.SetActive(true);
            playerIsHere = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            enterDialog.SetActive(false);
            playerIsHere = false;
        }
    }
}

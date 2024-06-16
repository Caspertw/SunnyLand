using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogOpen : MonoBehaviour
{
    public GameObject enterDialog;

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            enterDialog.SetActive(true);
            //playerIsHere = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            enterDialog.SetActive(false);
            //playerIsHere = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPathDialogueTrigger : MonoBehaviour
{
    public GameObject rightPathDialogueBox;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            rightPathDialogueBox.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            rightPathDialogueBox.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenRiftDialogueTrigger : MonoBehaviour
{
    public GameObject openRiftDialogueBox;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            openRiftDialogueBox.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            openRiftDialogueBox.SetActive(false);
        }
    }
}

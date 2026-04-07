using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTeleporter : MonoBehaviour
{
    public EndOfTutorialScreen endOfTutorial;

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            endOfTutorial.EndOfTutorial();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SketchbookPickup : MonoBehaviour
{
    public bool sketchbook = false;

    public GameObject rightPathDialogueTrigger;
    public GameObject rightPathBarrier;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            sketchbook = true;
            Destroy(gameObject);
            Destroy(rightPathDialogueTrigger);
            Destroy(rightPathBarrier);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPuzzleComplete : MonoBehaviour
{
    public GameObject pillarFlame1;
    public GameObject pillarFlame2;
    public GameObject pillarFlame3;
    public GameObject pillarFlame4;

    public GameObject towerDoor;

    public GameObject finalDialogue;

    void Start()
    {
        towerDoor.SetActive(true);
        finalDialogue.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (pillarFlame1.activeInHierarchy && pillarFlame2.activeInHierarchy && pillarFlame3.activeInHierarchy && pillarFlame4.activeInHierarchy)
        {
            towerDoor.SetActive(false);
            finalDialogue.SetActive(true);
        }
    }
}

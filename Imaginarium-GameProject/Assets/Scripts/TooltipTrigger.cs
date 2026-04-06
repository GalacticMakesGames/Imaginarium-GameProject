using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipTrigger : MonoBehaviour
{
    public GameObject tooltipUI;

    private TextMeshProUGUI tooltipTextComponent;

    // Start is called before the first frame update
    void Start()
    {
        if (tooltipUI != null)
        {
            tooltipTextComponent = tooltipUI.GetComponent<TextMeshProUGUI>();
            tooltipUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (tooltipUI != null)
            {
                tooltipUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (tooltipUI != null)
            {
                // Hide and destroy the UI
                tooltipUI.SetActive(false);
                Destroy(tooltipUI);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryToggle : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject invCanvas;

    public Button inventoryButton;
    public Button statusButton;
    public Image selectorIndicator;
    public RectTransform scrollable;
    public int transitionAmount = 200;
    public float transitionSpeed = 25f;
    Vector3 originalPos;
    private void Start()
    {
        originalPos = scrollable.position;
    }
    void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button6))
        {
            isPaused = !isPaused;
            invCanvas.SetActive(isPaused);
        }

	}
    public void SetSelection(int mode)
    {
        StopAllCoroutines();
        switch (mode)
        {
            case 0:
                StartCoroutine(ScrollToDirection(originalPos));
                selectorIndicator.rectTransform.position = inventoryButton.GetComponent<RectTransform>().position;
                break;
            case 1:
                StartCoroutine(ScrollToDirection(originalPos + new Vector3(-transitionAmount, 0, 0)));
                selectorIndicator.rectTransform.position = statusButton.GetComponent<RectTransform>().position;
                break;
            default:
                Debug.LogError("Invalid menu selection");
                break;
        }
    }
    IEnumerator ScrollToDirection(Vector3 desiredPos)
    {
        while(scrollable.position != desiredPos)
        {
            scrollable.position = Vector3.Lerp(scrollable.position, desiredPos, transitionSpeed * Time.unscaledDeltaTime);
            yield return null;
        }
    }
}

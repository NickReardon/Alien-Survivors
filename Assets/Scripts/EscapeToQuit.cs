using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeToQuit : MonoBehaviour
{
    // Public fields to assign GameObjects in the editor
    public GameObject[] gameObjectsToCheck;
    public GameObject exitPanel;

    public bool allInactive = true;
    public GameObject highestSortOrderObject = null;
    public int highestSortOrder = int.MinValue;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Iterate through each GameObject in the array
        allInactive = true;
        highestSortOrderObject = null;
        highestSortOrder = int.MinValue;

        foreach (GameObject obj in gameObjectsToCheck)
        {
            if (obj != null && obj.activeInHierarchy)
            {
                allInactive = false;
                Canvas canvas = obj.GetComponent<Canvas>();
                if (canvas != null && canvas.sortingOrder > highestSortOrder)
                {
                    highestSortOrder = canvas.sortingOrder;
                    highestSortOrderObject = obj;
                }
            }
        }
        // Check for Canvas components if no Renderer is found
        if (highestSortOrderObject == null)
        {
            foreach (GameObject obj in gameObjectsToCheck)
            {
                if (obj != null && obj.activeInHierarchy)
                {

                }
            }
        }
        if (allInactive && Input.GetKeyDown(KeyCode.Escape))
        {
            exitPanel.SetActive(true);
        }
        else if (highestSortOrderObject != null && Input.GetKeyDown(KeyCode.Escape))
        {
            highestSortOrderObject.SetActive(false);
        }
    }

}
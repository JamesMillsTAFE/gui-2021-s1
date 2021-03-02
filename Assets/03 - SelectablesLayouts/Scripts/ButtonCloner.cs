using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCloner : MonoBehaviour
{
    [SerializeField]
    private GameObject toClone;
    [SerializeField]
    private Transform content;
    [SerializeField, Min(5)]
    private int count = 5;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject cloned = Instantiate(toClone, content);
            cloned.SetActive(true);
        }
    }

    public void LogMessage()
    {
        Debug.Log("Clicked!");
    }
}

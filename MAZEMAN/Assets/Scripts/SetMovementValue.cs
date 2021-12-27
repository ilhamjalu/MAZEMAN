using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SetMovementValue : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string valueName;
    public GameManagerScript gmScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(valueName == "horipos")
        {
            gmScript.movHor = 1;
        }   
        else if (valueName == "horineg")
        {
            gmScript.movHor = -1;
        }
        else if (valueName == "vertpos")
        {
            gmScript.movVert = 1;
        }
        else if (valueName == "vertneg")
        {
            gmScript.movVert = -1;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        gmScript.movHor = 0;
        gmScript.movVert = 0;
    }
}

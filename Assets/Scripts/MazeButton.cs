using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class MazeButton : MonoBehaviour
{
    public bool isWall = false;
    public bool isInitial = false;
    public bool isGoal = false;

    public Color wallColor;
    public Color initialColor;
    public Color goalColor;
    public Color mazeColor;

    public void Start()
    {
        EventTrigger.Entry entry1 = new EventTrigger.Entry();
        entry1.eventID = EventTriggerType.PointerClick;
        entry1.callback.AddListener((eventData) => { onClick(); });

        
        GetComponent<EventTrigger>().triggers.Add(entry1);

    }

    public void setInitialButton() 
    {
        GetComponent<Image>().color = initialColor;
    }

    public void setGoalButton()
    {
        GetComponent<Image>().color = goalColor;
    }

    public void onClick()
    {
        if (!isGoal && !isInitial)
        {
            isWall = !isWall;

            if (isWall)
                GetComponent<Image>().color = wallColor;
            else
                GetComponent<Image>().color = mazeColor;
        }
    }
}

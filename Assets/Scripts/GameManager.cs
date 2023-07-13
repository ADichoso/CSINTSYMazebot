using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
        if (this != Instance) Debug.LogWarning("WARNING! MORE THAN 1 INSTANCE OF GameManager DETECTED!");
    }
}

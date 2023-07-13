using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    public TileInfo tileInfo = new TileInfo();

    public GridTile parent = null;
    public List<GridTile> adjacentTiles = new List<GridTile>();

    public bool isWall = false;
    public bool isSensed = false;

    public GameObject wall;
    public GameObject move_location;

    // Start is called before the first frame update
    void Start()
    {
        if(isWall) activateWall();
        else deactivateWall();
    }

    public void activateWall() 
    {
        wall.SetActive(true);
    }

    public void deactivateWall() 
    {
        wall.SetActive(false);
    }

    public void displayAdjacentUnexploredTiles() 
    {
        foreach (GridTile child in expandTiles())
            if (child != GridManager.Instance.initialTile && child != GridManager.Instance.goalTile && !child.isSensed) 
                child.GetComponent<MeshRenderer>().material = GridManager.Instance.sensedMat;
    }
    public void exploreTile(GridTile tileParent) 
    {
        isSensed = true;
        parent = tileParent;
        
        if(this != GridManager.Instance.initialTile && this != GridManager.Instance.goalTile) GetComponent<MeshRenderer>().material = GridManager.Instance.exploredMat;
    }
    public List<GridTile> expandTiles() 
    {
        List<GridTile> unexploredChildren = new List<GridTile>();

        foreach (GridTile child in adjacentTiles)
            if (!child.isWall)
            {
                child.tileInfo.pathCost = tileInfo.pathCost + 1;
                unexploredChildren.Add(child);
            }

        return unexploredChildren;
    }

    public void setAsWall()
    {
        isWall = true;
        wall.SetActive(true);
    }
}

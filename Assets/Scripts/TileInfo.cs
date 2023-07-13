using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo
{
    public int x_coord = -1;
    public int y_coord = -1;
    public int pathCost = 0;
    public int heuristicCost = 0;

    public TileInfo(TileInfo tileInfo)
    {
        x_coord = tileInfo.x_coord;
        y_coord = tileInfo.y_coord;
        pathCost = tileInfo.pathCost;
        heuristicCost = tileInfo.heuristicCost;
    }

    public TileInfo() 
    {
        x_coord = -1; 
        y_coord = -1;
        pathCost = 0;
        heuristicCost = 0;
    }
}

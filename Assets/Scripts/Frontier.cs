using System.Collections.Generic;

public class Frontier
{
    public List<GridTile> queue;

    public Frontier()
    {
        queue = new List<GridTile>();
    }

    public GridTile pop() 
    {
        if (queue.Count > 0)
        {
            GridTile tile = queue[0];
            queue.RemoveAt(0);

            return tile;
        }

        return null;
    }

    public void sort() 
    {
        int lowestCost = queue[0].tileInfo.pathCost + queue[0].tileInfo.heuristicCost;
        int lowestIndex = 0;
        for (int j = 0; j < queue.Count; j++)
        {
            int currCost = queue[j].tileInfo.pathCost + queue[j].tileInfo.heuristicCost;

            if (lowestCost == currCost) 
            {
                if (queue[lowestIndex].tileInfo.heuristicCost > queue[j].tileInfo.heuristicCost) 
                {
                    lowestCost = currCost;
                    lowestIndex = j;
                }
            }
            else if (lowestCost > currCost)
            {
                lowestCost = currCost;
                lowestIndex = j;
            }
        }

        GridTile temp = queue[0];
        queue[0] = queue[lowestIndex];
        queue[lowestIndex] = temp;
    }
}

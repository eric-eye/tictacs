using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Helpers : MonoBehaviour
{
    public static List<List<Cursor>> CursorMatrix()
    {
        return (CursorController.cursorMatrix);
    }

    public static Cursor GetTile(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < CursorMatrix().Count && z < CursorMatrix()[x].Count)
        {
            return CursorMatrix()[x][z];
        }
        else
        {
            return null;
        }
    }

    public static List<Cursor> Neighbors(int xPos, int zPos)
    {
        List<Cursor> list = new List<Cursor>();

        list.Add(GetTile(xPos + 1, zPos));
        list.Add(GetTile(xPos - 1, zPos));
        list.Add(GetTile(xPos, zPos + 1));
        list.Add(GetTile(xPos, zPos - 1));
        list.RemoveAll(r => (r == null));

        return list;
    }

    public static bool CanHitTarget(Cursor targetTile)
    {
        RaycastHit hit;
        GameObject target = targetTile.gameObject;

        if (targetTile.standingUnit) target = targetTile.standingUnit.transform.Find("Hittable").gameObject;
        if (Physics.Linecast(Unit.current.transform.Find("Hittable").transform.position, target.transform.position, out hit, 1 << 8))
        {
            return (hit.collider.gameObject == target.gameObject);
        }

        return (false);
    }
    public static List<Cursor> GetRadialTiles(int originX, int originZ, int maxHops, bool allowOthers)
    {
        List<int[]> queue = new List<int[]>();
        queue.Add(new int[] { originX, originZ, 0 });
        for (int i = 0; i < queue.Count; i++)
        {
            int[] entry = queue[i];
            int counter = entry[2] + 1;
            if (counter > maxHops) continue;

            List<Cursor> neighbors = Neighbors(entry[0], entry[1]);

            List<int[]> newCells = new List<int[]>();

            foreach (Cursor cursor in neighbors)
            {
                if (allowOthers || !cursor.standingUnit || cursor.standingUnit == Unit.current)
                {
                    int elevation = VoxelController.GetElevation(cursor.xPos, cursor.zPos);
                    if (Mathf.Abs(elevation - VoxelController.GetElevation(entry[0], entry[1])) < 2)
                    {
                        newCells.Add(new int[] { cursor.xPos, cursor.zPos, counter, elevation });
                    }
                }
            }

            for (int a = newCells.Count - 1; a >= 0; a--)
            {
                for (int g = 0; g < queue.Count; g++)
                {
                    if (newCells[a][0] == queue[g][0] &&
                      newCells[a][1] == queue[g][1] &&
                      newCells[a][2] <= queue[g][2])
                    {
                        newCells.RemoveAt(a);
                        break;
                    }
                }
            }

            for (int a = 0; a < newCells.Count; a++)
            {
                queue.Add(newCells[a]);
            }
        }

        HashSet<Cursor> cursors = new HashSet<Cursor>();

        foreach (int[] entry in queue)
        {
            cursors.Add(GetTile(entry[0], entry[1]));
        }

        return (cursors.ToList());
    }

    public static List<Cursor> GetLineTiles(int originX, int originZ, int destinationX, int destinationZ, int maxTiles)
    {
        int incrementX = 0;
        int incrementZ = 0;

        int diffX = destinationX - originX;
        int diffZ = destinationZ - originZ;

        if(Mathf.Abs(diffX) > Mathf.Abs(diffZ)){
          incrementX = diffX/Mathf.Abs(diffX);
        }
        else
        {
          incrementZ = diffZ/Mathf.Abs(diffZ);
        }

        List<Cursor> tiles = new List<Cursor>();

        int[] coordinates = { incrementX + originX, incrementZ + originZ };
        bool tileExists = true;

        while (tileExists && tiles.Count < maxTiles)
        {
            Cursor tile = Helpers.GetTile(coordinates[0], coordinates[1]);
            if (tile)
            {
                tileExists = true;
                tiles.Add(tile);
                coordinates[0] += incrementX;
                coordinates[1] += incrementZ;
            }
            else
            {
                tileExists = false;
            }
        }

        return (tiles);
    }

    public static List<int[]> DeriveShortestPath(int xPos, int zPos, int originX, int originZ)
    {
        List<int[]> queue = new List<int[]>();
        List<int[]> shortestPath = new List<int[]>();
        queue.Add(new int[] { xPos, zPos, 0, VoxelController.GetElevation(xPos, zPos) });
        for (int i = 0; i < queue.Count; i++)
        {
            int[] entry = queue[i];
            int counter = entry[2] + 1;

            List<Cursor> neighbors = Helpers.Neighbors(entry[0], entry[1]);

            List<int[]> newCells = new List<int[]>();

            foreach (Cursor cursor in neighbors)
            {
                if (!cursor.standingUnit || cursor.standingUnit == Unit.current)
                {
                    int elevation = VoxelController.GetElevation(cursor.xPos, cursor.zPos);
                    if (Mathf.Abs(elevation - VoxelController.GetElevation(entry[0], entry[1])) < 2)
                    {
                        newCells.Add(new int[] { cursor.xPos, cursor.zPos, counter, elevation });
                    }
                }
            }

            bool reachedDestination = false;

            for (int a = 0; a < newCells.Count; a++)
            {
                if (newCells[a][0] == originX && newCells[a][1] == originZ)
                {
                    reachedDestination = true;
                    break;
                }
            }

            for (int a = newCells.Count - 1; a >= 0; a--)
            {
                for (int g = 0; g < queue.Count; g++)
                {
                    if (newCells[a][0] == queue[g][0] &&
                        newCells[a][1] == queue[g][1] &&
                        newCells[a][2] >= queue[g][2])
                    {
                        newCells.RemoveAt(a);
                        break;
                    }
                }
            }

            for (int a = 0; a < newCells.Count; a++)
            {
                queue.Add(newCells[a]);
            }

            if (reachedDestination)
            {
                queue.Reverse();
                int firstIndex = queue.FindIndex(r => (r[0] == originX && r[1] == originZ));
                shortestPath.Add(queue[firstIndex]);

                int[] previousElement = queue[firstIndex];

                for (int b = firstIndex; b < queue.Count; b++)
                {
                    int[] currentElement = queue[b];

                    if (
                        (
                         (
                          currentElement[0] == previousElement[0] - 1 &&
                          currentElement[1] == previousElement[1]
                         ) ||
                         (
                          currentElement[0] == previousElement[0] + 1 &&
                          currentElement[1] == previousElement[1]
                         ) ||
                         (
                          currentElement[0] == previousElement[0] &&
                          currentElement[1] == previousElement[1] + 1
                         ) ||
                         (
                          currentElement[0] == previousElement[0] &&
                          currentElement[1] == previousElement[1] - 1
                         )
                        ) && currentElement[2] == previousElement[2] - 1
                      )
                    {
                        shortestPath.Add(currentElement);
                        previousElement = currentElement;
                    }
                }

                break;
            }
        }

        return (shortestPath);
    }
}

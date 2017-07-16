using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Helpers : MonoBehaviour
{
    public enum Affinity
    {
        None,
        Fire,
        Water,
        Earth
    }

    public static int Distance(Cursor origin, Cursor destination)
    {
        return Mathf.Abs(origin.xPos - destination.xPos) +
               Mathf.Abs(origin.zPos - destination.zPos);
    }

    public static void SetTransformPosition(Transform objectTransform, int x, int y, int z)
    {
        Vector3 position = objectTransform.position;

        position.x = x + .5f;
        position.z = z + .5f;
        position.y = y + 1.5f;

        objectTransform.position = position;
    }

    public static List<List<Cursor>> CursorMatrix()
    {
        return (CursorController.cursorMatrix);
    }

    public static Vector3 WorldPointFromCursor(Cursor cursor)
    {
        return (new Vector3(cursor.xPos + 0.5f, cursor.yPos + 1f, cursor.zPos + 0.5f));
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

    public static bool CanHitTarget(Vector3 origin, Cursor targetTile)
    {
        if (targetTile.standingUnit)
        {
            foreach (Buff buff in targetTile.standingUnit.Buffs())
            {
                if (buff.CanBeSeenThroughObjects())
                {
                    return (true);
                }
            }
        }

        RaycastHit hit;
        GameObject target = targetTile.gameObject;

        // if (targetTile.standingUnit) target = targetTile.standingUnit.transform.Find("Hittable").gameObject;
        if (Physics.Linecast(origin, target.transform.position,
            out hit, 1 << 8))
        {
            return (hit.collider.gameObject == target.gameObject);
        }

        return (false);
    }

    public static List<Cursor> GetRadialTiles(int originX, int originZ, int maxHops, int maxHeightChange, bool allowOthers,
        int minDistance = 0, bool heightAugmented = false, bool forMovement = false)
    {
        print("max height diff: " + maxHeightChange);
        Cursor origin = GetTile(originX, originZ);
        List<int[]> queue = new List<int[]>();
        queue.Add(new int[] {originX, originZ, 0});
        for (int i = 0; i < queue.Count; i++)
        {
            int[] entry = queue[i];
            int counter = entry[2] + 1;
            int hopAugment = heightAugmented ? origin.yPos : 0;
            if (counter > maxHops + hopAugment) continue;

            List<Cursor> neighbors = Neighbors(entry[0], entry[1]);

            List<int[]> newCells = new List<int[]>();

            foreach (Cursor cursor in neighbors)
            {
                if (allowOthers || !cursor.standingUnit || cursor.standingUnit == Unit.Subject())
                {
                    if (forMovement)
                    {
                        Unit unit = Helpers.GetTile(originX, originZ).standingUnit;
                        if (!CanMoveThere(unit, cursor))
                        {
                            continue;
                        }
                    }

                    int elevation = VoxelController.GetElevation(cursor.xPos, cursor.zPos);
                    bool movable = (!forMovement && Mathf.Abs(elevation - origin.yPos) <= maxHeightChange) || (elevation >= 0 && Helpers.CanMoveToNextTile(cursor, Helpers.GetTile(entry[0], entry[1]), maxHeightChange));
                    if (movable)
                    {
                        newCells.Add(new int[] {cursor.xPos, cursor.zPos, counter, elevation});
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
            int distance = Mathf.Abs(entry[0] - originX) + Mathf.Abs(entry[1] - originZ);
            bool satisfiesMinDistance = distance >= minDistance;
            bool satisfiesHeightOffset = !heightAugmented;
            if (heightAugmented)
            {
                Cursor destination = GetTile(entry[0], entry[1]);
                int distanceDiff = distance - maxHops;
                int heightDiff = origin.yPos - destination.yPos;
                satisfiesHeightOffset = distanceDiff <= heightDiff;
            }

            if (satisfiesMinDistance && satisfiesHeightOffset) cursors.Add(GetTile(entry[0], entry[1]));
        }

        return (cursors.ToList());
    }

    public static List<Cursor> GetLineTiles(int originX, int originZ, int destinationX, int destinationZ, int maxTiles)
    {
        int incrementX = 0;
        int incrementZ = 0;

        int diffX = destinationX - originX;
        int diffZ = destinationZ - originZ;

        if (Mathf.Abs(diffX) > Mathf.Abs(diffZ))
        {
            incrementX = diffX / Mathf.Abs(diffX);
        }
        else
        {
            incrementZ = diffZ / Mathf.Abs(diffZ);
        }

        List<Cursor> tiles = new List<Cursor>();

        int[] coordinates = {incrementX + originX, incrementZ + originZ};
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

    private static List<Cursor> GetSequentialTiles(int originX, int originZ, int directionX, int directionZ)
    {
        List<Cursor> directionalTiles = new List<Cursor>();

        bool tileExists = true;
        int magnitude = 0;

        while (tileExists)
        {
            Cursor tile = Helpers.GetTile(
                originX + (directionX * magnitude),
                originZ + (directionZ * magnitude)
            );
            if (tile)
            {
                tileExists = true;
                directionalTiles.Add(tile);
                magnitude++;
            }
            else
            {
                tileExists = false;
            }
        }

        return (directionalTiles);
    }

    public static List<Cursor> GetAlarmTiles(int x, int z, Vector3 direction){
        Vector3 newDirection;

        List<Cursor> cursors = new List<Cursor>();

        // 12 oclock
        Cursor tile = Helpers.GetTile(
            x + Mathf.RoundToInt(direction.x),
            z + Mathf.RoundToInt(direction.z));

        if(tile){
            cursors.Add(tile);
        }

        // 1:30 oclocko
        newDirection = new Vector3(
            direction.x + direction.z,
            0,
            direction.z + direction.x
            );

        tile = Helpers.GetTile(
            x + Mathf.RoundToInt(newDirection.x),
            z + Mathf.RoundToInt(newDirection.z));

        if(tile){
            cursors.Add(tile);
        }

        // // 3 oclock
        newDirection = new Vector3(
            direction.x - direction.x + direction.z,
            0,
            direction.z - direction.z + direction.x
            );

        tile = Helpers.GetTile(
            x + Mathf.RoundToInt(newDirection.x),
            z + Mathf.RoundToInt(newDirection.z));

        if(tile){
            cursors.Add(tile);
        }

        // 9 oclock
        newDirection = new Vector3(
            direction.x - direction.x - direction.z,
            0,
            direction.z - direction.z - direction.x 
            );


        tile = Helpers.GetTile(
            x + Mathf.RoundToInt(newDirection.x),
            z + Mathf.RoundToInt(newDirection.z));

        if(tile){
            cursors.Add(tile);
        }

        // 10:30 oclock
        newDirection = new Vector3(
            (direction.x + direction.z) * -(direction.z - direction.x) * (direction.x + direction.z),
            0,
            (direction.z + direction.x) * -(direction.x - direction.z) * (direction.z + direction.x)
            );

        tile = Helpers.GetTile(
            x + Mathf.RoundToInt(newDirection.x),
            z + Mathf.RoundToInt(newDirection.z));

        if(tile){
            cursors.Add(tile);
        }

        return(cursors);
    }

    public static Cursor GetDirectionalCursor(int x, int z, Vector3 direction) {
        return(Helpers.GetTile(
            x + Mathf.RoundToInt(direction.x),
            z + Mathf.RoundToInt(direction.z)));
    }

    public static List<Cursor> GetDiagonalTiles(int originX, int originZ)
    {
        List<Cursor> tiles = new List<Cursor>();

        tiles.AddRange(GetSequentialTiles(originX, originZ, 1, 1));
        tiles.AddRange(GetSequentialTiles(originX, originZ, -1, -1));
        tiles.AddRange(GetSequentialTiles(originX, originZ, 1, -1));
        tiles.AddRange(GetSequentialTiles(originX, originZ, -1, 1));

        return (tiles);
    }

    public static List<Cursor> GetHorizontalTiles(int originX, int originZ)
    {
        List<Cursor> tiles = new List<Cursor>();

        tiles.AddRange(GetSequentialTiles(originX, originZ, 1, 0));
        tiles.AddRange(GetSequentialTiles(originX, originZ, -1, 0));
        tiles.AddRange(GetSequentialTiles(originX, originZ, 0, 1));
        tiles.AddRange(GetSequentialTiles(originX, originZ, 0, -1));

        return (tiles);
    }

    public static List<int[]> DeriveShortestPath(int xPos, int zPos, int originX, int originZ, Unit requestor)
    {
        List<int[]> queue = new List<int[]>();
        List<int[]> shortestPath = new List<int[]>();
        queue.Add(new int[] {xPos, zPos, 0, VoxelController.GetElevation(xPos, zPos)});
        for (int i = 0; i < queue.Count; i++)
        {
            int[] entry = queue[i];
            int counter = entry[2] + 1;

            List<Cursor> neighbors = Helpers.Neighbors(entry[0], entry[1]);

            List<int[]> newCells = new List<int[]>();

            foreach (Cursor cursor in neighbors)
            {
                if (!cursor.standingUnit || cursor.standingUnit == requestor)
                {
                    int elevation = VoxelController.GetElevation(cursor.xPos, cursor.zPos);
                    bool movable = elevation >= 0 && Helpers.CanMoveToNextTile(cursor, Helpers.GetTile(entry[0], entry[1]), requestor.JumpHeight());
                    
                    if (movable)
                    {
                        newCells.Add(new int[] {cursor.xPos, cursor.zPos, counter, elevation});
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

    private static bool CanMoveThere(Unit unit, Cursor tile)
    {
        bool canMoveThere = true;

        foreach (Buff buff in unit.Buffs())
        {
            canMoveThere = buff.CanMoveTo(tile);
            if (!canMoveThere)
            {
                break;
            }
        }

        return canMoveThere;
    }

    private static bool CanMoveToNextTile(Cursor target, Cursor origin, int allowance)
    {
        int targetElevation = VoxelController.GetElevation(target.xPos, target.zPos);
        int originElevation = VoxelController.GetElevation(origin.xPos, origin.zPos);
        int diff = Mathf.Abs(targetElevation - originElevation);
        return(diff <= allowance);
    }
}
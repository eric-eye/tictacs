using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers : MonoBehaviour {
	public static List<List<Cursor>> CursorMatrix(){
		return(CursorController.cursorMatrix);
	}

	public static Cursor GetTile(int x, int z){
		if(x >= 0 && z>= 0 && x < CursorMatrix().Count && z < CursorMatrix()[x].Count){
		return CursorMatrix()[x][z];
		} else {
		return null;
		}
	}

	public static List<Cursor> Neighbors(int xPos, int zPos){
		List<Cursor> list = new List<Cursor>();

		list.Add(GetTile(xPos + 1, zPos));
		list.Add(GetTile(xPos - 1, zPos));
		list.Add(GetTile(xPos, zPos + 1));
		list.Add(GetTile(xPos, zPos - 1));
		list.RemoveAll(r => (r == null));

		return list;
	}

	public static List<int[]> GetAllPaths(int originX, int originZ, int maxHops, bool allowOthers) {
		List<int[]> queue = new List<int[]>();
		queue.Add(new int[] { originX, originZ, 0 });
		for(int i = 0; i < queue.Count; i++){
		int[] entry = queue[i];
		int counter = entry[2] + 1;
		if(counter > maxHops) continue;

		List<Cursor> neighbors = Neighbors(entry[0], entry[1]);

		List<int[]> newCells = new List<int[]>();

		foreach(Cursor cursor in neighbors){
			if(allowOthers || !cursor.standingUnit || cursor.standingUnit == Unit.current){
			int elevation = VoxelController.GetElevation(cursor.xPos, cursor.zPos);
			if(Mathf.Abs(elevation - VoxelController.GetElevation(entry[0], entry[1])) < 2){
				newCells.Add(new int[] { cursor.xPos, cursor.zPos, counter, elevation });
			}
			}
		}

		for(int a = newCells.Count - 1; a >= 0; a--){
			for(int g = 0; g < queue.Count; g++) {
			if(newCells[a][0] == queue[g][0] &&
				newCells[a][1] == queue[g][1] &&
				newCells[a][2] <= queue[g][2]) {
				newCells.RemoveAt(a);
				break;
			}
			}
		}

		for(int a = 0; a < newCells.Count; a++){
			queue.Add(newCells[a]);
		}
		}

		return(queue);
	}
}

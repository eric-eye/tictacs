using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class UnitMovement : NetworkBehaviour
{
    public class CoordinateList : List<CursorController.Coordinate> {};
  
    private Unit _unit;
    private int _jumpHeight = 0;
    private float jumpDownAcceleration = 0;
    private bool _isMoving;
    private bool _isMovingUp;
    private bool _isMovingDown;
    private float _moveSpeed = 5f;
    private Vector3 _goal;
    private bool _resetPath = false;
    private CoordinateList _path = new CoordinateList();
    private int _pathIndex = 0;
    private bool _canWalkPath = false;
    private int _currentxPos;
    private int _currentzPos;
    private bool _turnBased;

    public bool TraversedAlertTile(){
      List<CursorController.Coordinate> listToCheck = new List<CursorController.Coordinate>(_path);
      CursorController.Coordinate newTile = new CursorController.Coordinate();
      newTile.x = _unit.xPos;
      newTile.z = _unit.zPos;
      listToCheck.Add(newTile);
      return(listToCheck.Where((p) => Helpers.GetTile(p.x, p.z).alarm).Count() > 0);
    }

    void Awake()
    {
        this._unit = this.GetComponent<Unit>();
        ResetCurrentPosition();
    }

    void FixedUpdate() {
        if (IsMovingAnywhere()) {
            Move();
        } else {
            PickNext();
        }
    }

    public void MoveAlongPath(CursorController.Coordinate[] path, bool turnBased)
    {
        print(path.Length);
        
        _turnBased = turnBased;
        _canWalkPath = true;
        _path.Clear();
    
        foreach(CursorController.Coordinate coordinate in path){
            _path.Add(coordinate);
        }

        ResetCurrentPosition();
        
        print(path.Length);
        
        this._unit.SetPosition(path[path.Length - 1].x, _path[path.Length - 1].z);
    }

    public void ForceWalk(int tilesToMove, int directionX, int directionZ)
    {
        int actualMoveTiles = tilesToMove;
        Cursor destination;
        
        //first, can they actually move there?

        while (actualMoveTiles > 0)
        {
            List<Cursor> tiles = Helpers.GetRadialTiles(_unit.xPos, _unit.zPos, actualMoveTiles, _unit.JumpHeight(), false, 1, false, true);
            print("tiles " + tiles.Count);
            Cursor projectedCursor = Helpers.GetTile(
                _unit.xPos + (actualMoveTiles * directionX),
                _unit.zPos + (actualMoveTiles * directionZ));

            if (projectedCursor && tiles.Contains(projectedCursor))
            {
                destination = projectedCursor;
                break;
            }
            
            actualMoveTiles--;
        }
        
        print("actualMoveTiles " + actualMoveTiles);

        if (actualMoveTiles == 0)
        {
            //no possible movement, so exit
            return;
        }
        
        int newX = _unit.xPos + (actualMoveTiles * directionX);
        int newZ = _unit.zPos + (actualMoveTiles * directionZ);
        
        //second, let's get the path for that tile

        List<int[]> path = Helpers.DeriveShortestPath(
            newX, newZ, _unit.xPos, _unit.zPos, _unit
        );
        
        //now let's go there!
        
        print("walking " + path.Count);
        
        CursorController.Coordinate[] coordinates = new CursorController.Coordinate[path.Count];
        int c = 0;
        foreach (int[] array in path)
        {
        CursorController.Coordinate coordinate = new CursorController.Coordinate();
            coordinate.x = array[0];
            coordinate.z = array[1];
            coordinate.counter = array[2];
            coordinate.elevation = array[3];
            coordinates[c] = coordinate;
            c++;
        }
        
        MoveAlongPath(coordinates, false);
    }

    private void ResetCurrentPosition()
    {
        _currentxPos = _unit.xPos;
        _currentzPos = _unit.zPos;
    }

    private void JumpUp(Vector3 position, float t){
        _unit.SetJumpingAnimation(true);

        position.x = Mathf.MoveTowards(transform.position.x, _goal.x, t * 0.5f);
        position.z = Mathf.MoveTowards(transform.position.z, _goal.z, t * 0.5f);

        float hAxis = (Mathf.Abs(transform.position.x - _goal.x) > Mathf.Abs(transform.position.z - _goal.z) ?
                          Mathf.Abs(position.x - _goal.x) : Mathf.Abs(position.z - _goal.z)) - 0.5f;

        float amplitude = 5;
        float offset = 2.5f + _jumpHeight;
        position.y = -(Mathf.Pow(hAxis, 2) * amplitude) + offset;

        transform.position = position;

        float deltaY = Mathf.Abs(transform.position.y - _goal.y);

        if( deltaY < 0.2f && hAxis < 0) {
            transform.position = new Vector3(_goal.x, _goal.y, _goal.z);
            _unit.SetJumpingAnimation(false);
            _isMovingUp = false;
        }
    }

    private bool IsMovingAnywhere(){
        return(_isMoving || _isMovingDown || _isMovingUp);
    }

    private void Walk(Vector3 position, float t){
        _unit.SetWalkingAnimation(true);

        position.x = Mathf.MoveTowards(transform.position.x, _goal.x, t);
        position.z = Mathf.MoveTowards(transform.position.z, _goal.z, t);

        transform.position = position;

        var deltaX = Mathf.Abs(transform.position.x - _goal.x);
        var deltaZ = Mathf.Abs(transform.position.z - _goal.z);
        
        if( deltaX < 0.01f && deltaZ < 0.01f) {
            _isMoving = false;
        }
    }

    private void JumpDown(Vector3 position, float t){
        _unit.SetJumpingAnimation(true);

        position.y = Mathf.MoveTowards(transform.position.y, _goal.y, t * (1.5f + jumpDownAcceleration));

        jumpDownAcceleration += Time.deltaTime * 10;

        transform.position = position;

        var deltaY = Mathf.Abs(transform.position.y - _goal.y);
        
        if (deltaY < 0.01f)
        {
            _unit.SetJumpingAnimation(false);
            _isMovingDown = false;
            jumpDownAcceleration = 0;
        }
    }

    private void Move() {
        var t = _moveSpeed * Time.deltaTime;
        var position = transform.position;

        if(_isMovingUp){
            JumpUp(position, t);
        }else if(_isMoving){
            Walk(position, t);
        }else if(_isMovingDown){
            JumpDown(position, t);
        }

        MainCamera.CenterOnWorldPoint(transform.position);

        if(!_isMoving && !_isMovingUp && !_isMovingDown && _resetPath){
            _unit.SetWalkingAnimation(false);
            _resetPath = false;
            this._unit.FinishMoving(_turnBased);
        }
    }

    private void PickNext()
    {
        if(_canWalkPath){
            Vector3 direction;

            if(_pathIndex >= _path.Count){
                return;
            }

            CursorController.Coordinate nextStep = _path[_pathIndex];
      
            _pathIndex++;
            if(_pathIndex >= _path.Count) {
                _resetPath = true;
                _canWalkPath = false;
                _pathIndex = 0;
            }

            int newY = nextStep.elevation - _unit.yPos;
            _jumpHeight = nextStep.elevation - _unit.yPos;
            _unit.yPos = _unit.yPos + newY;
      
            if (nextStep.x > _currentxPos) {
                print("going x");
                direction = new Vector3(1, newY, 0);
                _currentxPos++;
            } else if (nextStep.x < _currentxPos) {
                direction = new Vector3(-1, newY, 0);
                _currentxPos--;
            } else if (nextStep.z > _currentzPos) {
                direction = new Vector3(0, newY, 1);
                _currentzPos++;
            } else if (nextStep.z < _currentzPos) {
                direction = new Vector3(0, newY, -1);
                _currentzPos--;
            } else {
                print("return case");
                return;
            }

            _unit.lookDirection = new Vector3(direction.x, 0, direction.z);

            _goal = VoxelController.Grid().WorldToGrid(transform.position) + direction;

            _goal = VoxelController.Grid().GridToWorld(_goal);
            _isMoving = true;

            if(newY > 0) _isMovingUp = true;
            if(newY < 0) _isMovingDown = true;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using GridFramework.Grids;
using GridFramework.Renderers.Rectangular;
using GridFramework.Extensions.Align;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class Unit: NetworkBehaviour {

  public string unitName;

  private bool _isMoving;
  private bool _isMovingUp;
  private bool _isMovingDown;
  private float _moveSpeed = 5f;
  private Vector3 _goal;
  private RectGrid _grid;
  private Parallelepiped _renderer;

  private static List<Unit> all = new List<Unit>();

  [SyncVar]
  public int playerIndex;

  [SyncVar]
  public int xPos;

  [SyncVar]
  public int zPos;

  [SyncVar]
  public bool stanceRevealed = false;

  public int currentxPos;
  public int currentzPos;

  public class CoordinateList : SyncListStruct<CursorController.Coordinate> {};

  public int yPos;
  private bool resetPath = false;

  [SyncVar]
  private CoordinateList _path = new CoordinateList();

  [SyncVar]
  private Color _color;

  public static Unit current;
  public GameObject hitsPrefab;
  public GameObject actionDialoguePrefab;
  public GameObject stanceDialoguePrefab;
  public static Unit hovered;

  public int maxHp = 30;

  [SyncVar(hook = "OnChangeHp")]
  public int currentHp = 30;

  public int maxTp = 100;
  private int _pathIndex = 0;
  private bool _canWalkPath = false;

  [SyncVar(hook = "OnChangeIsCurrent")]
  private bool isCurrent = false;

  [SyncVar]
  public int currentTp = 0;

  public int maxMp;
  [SyncVar]
  public int currentMp;

  [SyncVar(hook = "OnStanceIndexChanged")]
  private int stanceIndex = 0;

  public string defense = "Free";

  [SyncVar(hook = "OnChangeHasActed")]
  public bool hasActed = false;

  [SyncVar(hook = "OnChangeHasMoved")]
  public bool hasMoved = false;

  public float attackModifier = 1;
  public float physicalResistModifier = 1;

  //public List<GameObject> buffs = new List<GameObject>();

	// Use this for initialization
	void Start () {
    _grid = GameObject.Find("Grid").GetComponent<RectGrid>();
    _renderer = _grid.gameObject.GetComponent<Parallelepiped>();

    Vector3 position = transform.position;

    yPos = VoxelController.GetElevation(xPos, zPos);

    currentxPos = xPos;
    currentzPos = zPos;

    position.x = xPos + .5f;
    position.z = zPos + .5f;
    position.y = yPos + 1.5f;

    transform.position = position;

    transform.Find("Marker").GetComponent<Renderer>().material.color = Color.white;

    transform.Find("Body").Find("CharacterModel").Find("ArmLeft1").GetComponent<Renderer>().material.color = _color;
    transform.Find("Body").Find("CharacterModel").Find("ArmRight1").GetComponent<Renderer>().material.color = _color;
    transform.Find("Body").Find("CharacterModel").Find("Body1").GetComponent<Renderer>().material.color = _color;
    transform.parent = GameObject.Find("Units").transform;

    CursorController.cursorMatrix[xPos][zPos].standingUnit = this;

    all.Add(this);

    if(NetworkServer.active){
      GameObject instance;

      List<string> actionList = new List<string> {
        "DelayAttack", "Fire", "Razz", "ThrowStone" 
      };

      List<string> stanceList = new List<string> {
        "Neutral", "Defend"
      };

      foreach(string actionName in actionList){
        instance = Instantiate(Resources.Load("Actions/Action" + actionName, typeof(GameObject))) as GameObject;
        NetworkServer.Spawn(instance);
        instance.GetComponent<Action>().parentNetId = netId;
      }

      foreach(string stanceName in stanceList){
        instance = Instantiate(Resources.Load("Stances/Stance" + stanceName, typeof(GameObject))) as GameObject;
        NetworkServer.Spawn(instance);
        instance.GetComponent<Stance>().parentNetId = netId;
      }
    }

    ReflectCurrent();
	}

  public List<GameObject> Actions(){
    List<GameObject> actions = new List<GameObject>();
    foreach(Transform child in transform.Find("Actions")){
      actions.Add(child.gameObject);
    }
    return(actions);
  }

  public List<GameObject> Stances(){
    List<GameObject> stances = new List<GameObject>();
    foreach(Transform child in transform.Find("Stances")){
      stances.Add(child.gameObject);
    }
    return(stances);
  }

  public static List<Unit> All(){
    return(all);
  }

  bool IsMovingAnywhere(){
    return(_isMoving || _isMovingDown || _isMovingUp);
  }

  public int CurrentTp(){
    return(currentTp);
  }

  [Command]
  public void CmdSetTp(int newTp){
    currentTp = newTp;
  }

  public void ReceiveBuff(GameObject buff){
    if(NetworkServer.active){
      NetworkServer.Spawn(buff);
    }
    buff.transform.parent = transform.Find("Buffs");
    buff.GetComponent<IBuff>().Up(this);
  }

  public List<GameObject> Buffs(){
    List<GameObject> buffs = new List<GameObject>();
    foreach(Transform child in transform.Find("Buffs")){
      buffs.Add(child.gameObject);
    }
    return(buffs);
  }

  public void AdvanceBuffs(){
    foreach(Transform buffTransform in transform.Find("Buffs")){
      IBuff buff = buffTransform.GetComponent<IBuff>();
      if(buff.TurnsLeft() < 1){
        buff.Down();
        Destroy(buffTransform.gameObject);
      }else{
        buff.DeductTurn();
      }
    }
  }

  void FixedUpdate() {
    if (IsMovingAnywhere()) {
      Move();
    } else {
      PickNext();
    }
  }

  [Command]
  public void CmdAddTp(int tpToAdd){
    currentTp += tpToAdd;
  }

  [Command]
  public void CmdSetColor(Color color){
    _color = color;
  }

  public int TpDiff(){
    return(maxTp - currentTp);
  }

  private void Die(){
    GameController.RemoveUnit(this);
    Destroy(gameObject);
  }

  public int MoveLength(){
    return(Stance().NegotiateMoveLength(10));
  }

  [Command]
  public void CmdSetStance(int newStanceIndex, GameObject player){
    stanceIndex = newStanceIndex;
  }

  public void OnStanceIndexChanged(int newStanceIndex){
    stanceIndex = newStanceIndex;
    GameController.FinishStanceChange();
  }

  public IStance Stance(){
    return(Stances()[stanceIndex].GetComponent<IStance>());
  }

  public void ReceiveDamage(int damage){
    if(NetworkServer.active){
      foreach(GameObject stance in Stances()){
          if(stance.GetComponent<IStance>() == Stance()){
            stance.GetComponent<Stance>().used = true;
          }
      }
      damage = Stance().NegotiateDamage(damage);
      stanceRevealed = true;
      currentHp -= damage;
      if(currentHp < 1){
        Die();
      }
    }

    System.Action showHits = () => {
      GameObject hitsObject = Instantiate(hitsPrefab, transform.position, Quaternion.identity);
      hitsObject.GetComponent<Hits>().damage = damage.ToString() + " HP";
    };

    GameObject stanceDialogueObject = Instantiate(stanceDialoguePrefab, transform.position, Quaternion.identity);
    stanceDialogueObject.GetComponent<StanceDialogue>().stance = Stance();
    stanceDialogueObject.GetComponent<StanceDialogue>().whenDone = showHits;
  }

  public void ReceiveTpDamage(int damage){
    if(NetworkServer.active){
      CmdSetTp(currentTp - damage);
    }

    System.Action showHits = () => {
      GameObject hitsObject = Instantiate(hitsPrefab, transform.position, Quaternion.identity);
      hitsObject.GetComponent<Hits>().damage = damage.ToString() + " TP";
    };

    GameObject stanceDialogueObject = Instantiate(stanceDialoguePrefab, transform.position, Quaternion.identity);
    stanceDialogueObject.GetComponent<StanceDialogue>().stance = Stance();
    stanceDialogueObject.GetComponent<StanceDialogue>().whenDone = showHits;
  }

  public void OnChangeHp(int newHp){
    int difference = currentHp - newHp;
    currentHp = newHp;

    // System.Action showHits = () => {
    //   GameObject hitsObject = Instantiate(hitsPrefab, transform.position, Quaternion.identity);
    //   hitsObject.GetComponent<Hits>().damage = difference;
    // };

    // GameObject stanceDialogueObject = Instantiate(stanceDialoguePrefab, transform.position, Quaternion.identity);
    // stanceDialogueObject.GetComponent<StanceDialogue>().stance = Stance();
    // stanceDialogueObject.GetComponent<StanceDialogue>().whenDone = showHits;
  }

  public void OnChangeIsCurrent(bool newIsCurrent){
    isCurrent = newIsCurrent;
    ReflectCurrent();
    GameController.RefreshPlayerView();
  }

  public void ReflectCurrent(){
    if(isCurrent) {
      Unit.current = this;
      GetComponent<UnitUIBehavior>().SetMarker();
    }else{
      GetComponent<UnitUIBehavior>().UnsetMarker();
    }
  }

  [Command]
  public void CmdSetCurrent(){
    if(Unit.current){
      Unit.current.isCurrent = false;
      //SYNCVAR bug
      if(NetworkServer.active) Unit.current.OnChangeIsCurrent(false);
    }
    hasMoved = false;
    hasActed = false;
    isCurrent = true;
    stanceRevealed = false;
    AdvanceBuffs();
  }

  [Command]
  public void CmdSetPath(CursorController.Coordinate[] path){
    currentTp -= 25;
    _path.Clear();
    foreach(CursorController.Coordinate coordinate in path){
      _path.Add(coordinate);
    }
    RpcUpdateUnitsOnGrid(xPos, zPos, _path.Last().x, _path.Last().z, gameObject);
    hasMoved = true;
    xPos = _path.Last().x;
    zPos = _path.Last().z;
  }

  [ClientRpc]
  public void RpcUpdateUnitsOnGrid(int fromX, int fromZ, int toX, int toZ, GameObject unitObject){
    Unit unit = unitObject.GetComponent<Unit>();
    CursorController.cursorMatrix[fromX][fromZ].standingUnit = null;
    CursorController.cursorMatrix[toX][toZ].standingUnit = unit;
  }

  public void StartMoving(){
    _canWalkPath = true;
    ActionInformation.Hide();
    GameController.StartMoving(this);
  }

  [ClientRpc]
  public void RpcDoAction(int x, int z, int actionIndex){
    ActionInformation.Hide();
    GameObject actionObject = Actions()[actionIndex];
    IAction action = actionObject.GetComponent<IAction>();

    if(NetworkServer.active){
      currentTp -= action.TpCost();
      currentMp -= action.MpCost();
    }

    Cursor cursor = CursorController.cursorMatrix[x][z];

    GameObject actionDialogueObject = Instantiate(actionDialoguePrefab, transform.position, Quaternion.identity);
    actionDialogueObject.GetComponent<ActionDialogue>().action = action;
    System.Action beginAction = () => {
      actionObject.GetComponent<Action>().BeginAction(cursor.gameObject);
    };
    actionDialogueObject.GetComponent<ActionDialogue>().whenDone = beginAction;
  }

  public void FinishAction(){
    GameController.UnfreezeInputs();
    if(NetworkServer.active) hasActed = true;
  }

  public void OnChangeHasActed(bool newHasActed){
    hasActed = newHasActed;
    if(hasActed) {
      GameController.FinishAction();
    }else{
      GameController.RefreshPlayerView();
    }
  }

  public void OnChangeHasMoved(bool newHasMoved){
    hasMoved = newHasMoved;
    if(hasMoved) {
      StartMoving();
    }else{
      GameController.RefreshPlayerView();
    }
  }

  public bool DoneWithTurn(){
    return(hasActed && hasMoved);
  }

  public void ReadyNextTurn(){
    hasActed = false;
    hasMoved = false;
  }

  private void Move() {
    var t = _moveSpeed * Time.deltaTime;
    var position = transform.position;

    if(_isMovingUp){
      position.y = Mathf.MoveTowards(transform.position.y, _goal.y, t);

      transform.position = position;

      var deltaY = Mathf.Abs(transform.position.y - _goal.y);
      if( deltaY < 0.01f) {
        _isMovingUp = false;
      }
    }else if(_isMoving){
      position.x = Mathf.MoveTowards(transform.position.x, _goal.x, t);
      position.z = Mathf.MoveTowards(transform.position.z, _goal.z, t);

      transform.position = position;

      // Check if we reached the destination (use a certain tolerance so
      // we don't miss the point becase of rounding errors)
      var deltaX = Mathf.Abs(transform.position.x - _goal.x);
      var deltaZ = Mathf.Abs(transform.position.z - _goal.z);
      if( deltaX < 0.01f && deltaZ < 0.01f) {
        _isMoving = false;
      }
    }else if(_isMovingDown){
      position.y = Mathf.MoveTowards(transform.position.y, _goal.y, t);

      transform.position = position;

      var deltaY = Mathf.Abs(transform.position.y - _goal.y);
      if( deltaY < 0.01f) {
        _isMovingDown = false;
      }
    }

    if(!_isMoving && !_isMovingUp && !_isMovingDown && resetPath){
      FinishMoving();
    }
  }

  public void FinishMoving(){
    resetPath = false;
    GameController.FinishMoving();
  }

  private void PickNext() {
    if(_canWalkPath){
      Vector3 direction;  // Direction to move in (grid-coordinates)

      if(_pathIndex >= _path.Count){
        return;
      }

      CursorController.Coordinate nextStep = _path[_pathIndex];
      _pathIndex++;
      if(_pathIndex >= _path.Count) {
        resetPath = true;
        _canWalkPath = false;
        _pathIndex = 0;
      }

      int newY = nextStep.elevation - yPos;
      yPos = yPos + newY;

      if (nextStep.x > currentxPos) {
        direction = new Vector3(1, newY, 0);
        currentxPos++;
      } else if (nextStep.x < currentxPos) {
        direction = new Vector3(-1, newY, 0);
        currentxPos--;
      } else if (nextStep.z > currentzPos) {
        direction = new Vector3(0, newY, 1);
        currentzPos++;
      } else if (nextStep.z < currentzPos) {
        direction = new Vector3(0, newY, -1);
        currentzPos--;
      } else {
        return;
      }

      Vector3 lookDirection = new Vector3(direction.x, 0, direction.z);

      Quaternion rotation = Quaternion.LookRotation(lookDirection);
      transform.rotation = rotation;

      CursorController.cursorMatrix[xPos][zPos].standingUnit = this;

      _goal = _grid.WorldToGrid(transform.position) + direction;

      _goal = _grid.GridToWorld(_goal);
      _isMoving = true;

      if(newY > 0) _isMovingUp = true;
      if(newY < 0) _isMovingDown = true;
    }
  }
}

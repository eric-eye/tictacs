using System.Collections;
using System.Collections.Generic;
using GridFramework.Grids;
using GridFramework.Renderers.Rectangular;
using GridFramework.Extensions.Align;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class Unit : NetworkBehaviour {

  [SyncVar]
  public string unitName;

  public int turnsDead = 0;

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

  public int xPos;
  public int zPos;

  [SyncVar]
  public bool stanceRevealed = false;

  public int currentxPos;
  public int currentzPos;

  public class CoordinateList : List<CursorController.Coordinate> {};

  public int yPos;
  private bool resetPath = false;

  private CoordinateList _path = new CoordinateList();
  private CoordinateList _pathToSync = new CoordinateList();

  [SyncVar]
  private Color _color;

  public static Unit current;
  public GameObject hitsPrefab;
  public GameObject actionDialoguePrefab;
  public GameObject stanceDialoguePrefab;
  public static Unit hovered;

  public int maxHp = 30;

  public int currentHp = 30;

  public int maxTp = 100;
  private int _pathIndex = 0;
  private bool _canWalkPath = false;

  private bool isCurrent = false;

  public int currentTp = 0;

  public int maxMp;
  public int currentMp;

  public int stanceIndex = 0;

  public string defense = "Free";

  public bool hasActed = false;
  public bool hasMoved = false;

  [SyncVar(hook = "OnChangePoints")]
  public int points = 100;

  [SyncVar]
  public bool dead = false;

  public float attackModifier = 1;
  public float physicalResistModifier = 1;

  public GameObject pointsPrefab;

  //public List<GameObject> buffs = new List<GameObject>();

  public static List<string> allActions = new List<string>{
    "Attack",
    "ChainLightning",
    "DelayAttack",
    "Fire",
    "LightningStab",
    "Meteor",
    "Punish",
    "Razz",
    "SpinAttack",
    "ThrowStone",
  };

  // public static List<string> allActions = new List<string>{
  //   "Kill",
  //   "Kill",
  //   "Kill",
  //   "Kill",
  // };

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

    GameObject pointsObject = Instantiate(pointsPrefab, Vector3.zero, Quaternion.identity);

    pointsObject.GetComponent<Points>().unit = this;

    PlayerPointsBar.ResizeByIndex(playerIndex);

    if(NetworkServer.active){
      GameObject instance;

      List<string> actionList = allActions.OrderBy(item => Random.value).ToList().GetRange(0, 4);

      List<string> stanceList = new List<string> {
        "Neutral", "Defend"
      };

      foreach(string actionName in actionList){
        instance = Instantiate(Resources.Load("Actions/Action" + actionName, typeof(GameObject))) as GameObject;
        instance.GetComponent<Action>().parentNetId = netId;
        NetworkServer.Spawn(instance);
      }

      foreach(string stanceName in stanceList){
        instance = Instantiate(Resources.Load("Stances/Stance" + stanceName, typeof(GameObject))) as GameObject;
        instance.GetComponent<Stance>().parentNetId = netId;
        NetworkServer.Spawn(instance);
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

  public void AddTp(int tpToAdd){
    currentTp += tpToAdd;
  }

  public void SetColor(Color color){
    _color = color;
  }

  public int TpDiff(){
    return(maxTp - currentTp);
  }

  private void Die(Unit aggressor){
    print("time to die");
    Cursor tile = Helpers.GetTile(xPos, zPos);
    tile.standingUnit = null;
    xPos = -1;
    zPos = -1;
    yPos = -1;
    dead = true;
    aggressor.points += points;
    points = 100;
    StartCoroutine(MoveAway());
  }

  private IEnumerator MoveAway(){
    yield return new WaitForSeconds(1f);
    transform.position = new Vector3(9999, 9999, 9999);
  }

  private IEnumerator MoveBack(){
    yield return new WaitForSeconds(1f);
    transform.position = new Vector3(
      xPos + .5f,
      yPos + 1.5f,
      zPos + .5f
    );
  }

  public void Revive(){
    dead = false;
    Cursor tile = GameController.GetRespawnTile();
    xPos = tile.xPos;
    yPos = tile.yPos;
    zPos = tile.zPos;
    points = 100;
    currentHp = 30;
    dead = false;
    tile.standingUnit = this;
    currentxPos = xPos;
    currentzPos = zPos;
    GameController.refreshView = true;
    StartCoroutine(MoveBack());
  }

  public int MoveLength(){
    return(Stance().NegotiateMoveLength(10));
  }

  public void SetStance(int newStanceIndex){
    stanceIndex = newStanceIndex;
  }

  public Stance Stance(){
    return(Stances()[stanceIndex].GetComponent<Stance>());
  }

  public void ReceiveDamage(int damage, Unit aggressor){
    int startingHp = currentHp;
    damage = Stance().NegotiateDamage(damage);

    foreach(GameObject stance in Stances()){
        if(stance.GetComponent<Stance>() == Stance()){
          stance.GetComponent<Stance>().used = true;
        }
    }
    stanceRevealed = true;
    currentHp -= damage;

    System.Action showHits = () => {
      GameObject hitsObject = Instantiate(hitsPrefab, transform.position, Quaternion.identity);
      hitsObject.GetComponent<Hits>().damage = damage.ToString() + " HP";
    };

    GameObject stanceDialogueObject = Instantiate(stanceDialoguePrefab, transform.position, Quaternion.identity);
    stanceDialogueObject.GetComponent<StanceDialogue>().stance = Stance();
    stanceDialogueObject.GetComponent<StanceDialogue>().whenDone = showHits;

    if (damage >= startingHp)
    {
        Die(aggressor);
    }
  }

  public void ReceiveTpDamage(int damage){
    currentTp -= damage;

    System.Action showHits = () => {
      GameObject hitsObject = Instantiate(hitsPrefab, transform.position, Quaternion.identity);
      hitsObject.GetComponent<Hits>().damage = damage.ToString() + " TP";
    };

    GameObject stanceDialogueObject = Instantiate(stanceDialoguePrefab, transform.position, Quaternion.identity);
    stanceDialogueObject.GetComponent<StanceDialogue>().stance = Stance();
    stanceDialogueObject.GetComponent<StanceDialogue>().whenDone = showHits;
  }

  public void OnChangePoints(int newPoints){
    points = newPoints;
    if(Player.ByIndex(playerIndex).CurrentPoints() >= 1000){
      GameController.EndGame();
    }
    PlayerPointsBar.ResizeByIndex(this.playerIndex);
  }

  public void SetIsCurrent(bool newIsCurrent){
    isCurrent = newIsCurrent;
    ReflectCurrent();
    GameController.RefreshPlayerView();

    if(isCurrent){
      GameController.instance.ResolveDeathPhase();
    }
  }

  public void ReflectCurrent(){
    if(isCurrent) {
      Unit.current = this;
      GetComponent<UnitUIBehavior>().SetMarker();
    }else{
      GetComponent<UnitUIBehavior>().UnsetMarker();
    }
  }

  public void SetCurrent(){
    if(Unit.current){
      Unit.current.SetIsCurrent(false);
    }
    SetHasMoved(false);
    SetHasActed(false);
    SetIsCurrent(true);
    stanceRevealed = false;
    AdvanceBuffs();
  }

  public void SetPath(CursorController.Coordinate[] path){
    currentTp -= 25;
    _path.Clear();
    foreach(CursorController.Coordinate coordinate in path){
      _path.Add(coordinate);
    }
    UpdateUnitsOnGrid(xPos, zPos, _path.Last().x, _path.Last().z, gameObject);
    SetHasMoved(true);
    xPos = _path.Last().x;
    zPos = _path.Last().z;
  }

  private void UpdateUnitsOnGrid(int fromX, int fromZ, int toX, int toZ, GameObject unitObject){
    Unit unit = unitObject.GetComponent<Unit>();
    CursorController.cursorMatrix[fromX][fromZ].standingUnit = null;
    CursorController.cursorMatrix[toX][toZ].standingUnit = unit;
  }

  public void StartMoving(){
    _canWalkPath = true;
    ActionInformation.Hide();
    GameController.StartMoving(this);
    MainCamera.Lock();
  }

  public void DoAction(int x, int z, int actionIndex){
    MainCamera.Lock();
    MainCamera.CenterOnWorldPoint(Unit.current.transform.position);
    ActionInformation.Hide();
    GameObject actionObject = Actions()[actionIndex];
    Action action = actionObject.GetComponent<Action>();

    currentTp -= action.TpCost();
    currentMp -= action.MpCost();

    Cursor cursor = CursorController.cursorMatrix[x][z];

    GameObject actionDialogueObject = Instantiate(actionDialoguePrefab, transform.position, Quaternion.identity);
    actionDialogueObject.GetComponent<ActionDialogue>().action = action;
    System.Action beginAction = () => {
      actionObject.GetComponent<Action>().BeginAction(cursor.gameObject);
    };
    actionDialogueObject.GetComponent<ActionDialogue>().whenDone = beginAction;
  }

  public void FinishAction(){
    MainCamera.Unlock();
    GameController.UnfreezeInputs();
    SetHasActed(true);
  }

  private void SetHasActed(bool newHasActed){
    hasActed = newHasActed;
    if(hasActed) {
      GameController.FinishAction();
    }else{
      GameController.RefreshPlayerView();
    }
  }

  private void SetHasMoved(bool newHasMoved){
    hasMoved = newHasMoved;
    if(hasMoved) {
      StartMoving();
    }else{
      GameController.RefreshPlayerView();
    }
  }

  public bool DoneWithTurn(){
    return(dead || (hasActed && hasMoved));
  }

  public void ReadyNextTurn(){
    SetHasActed(false);
    SetHasMoved(false);
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

    MainCamera.CenterOnWorldPoint(transform.position);

    if(!_isMoving && !_isMovingUp && !_isMovingDown && resetPath){
      FinishMoving();
    }
  }

  public void FinishMoving(){
    resetPath = false;
    GameController.FinishMoving();
    MainCamera.Unlock();
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

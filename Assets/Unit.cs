using System;
using System.Collections;
using System.Collections.Generic;
using GridFramework.Grids;
using GridFramework.Renderers.Rectangular;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class Unit : NetworkBehaviour
{
    public string unitClass;
    public Vector3 lookDirection = new Vector3(0, 0, 0);
    public int unitIndex;
    public int jumpBase;
    public int moveBase;

    private UnitMovement _unitMovement;

    public struct Coordinate
    {
        public int xPos;
        public int zPos;
    }

    [SyncVar] public string unitName;

    public Helpers.Affinity affinity = Helpers.Affinity.None;

    private bool controllersInitialized = false;

    public Material transparentMaterial;

    public int turnsDead = 0;

    private static List<Unit> all = new List<Unit>();

    [SyncVar] public int playerIndex;

    public int xPos;
    public int zPos;
    public bool stanceRevealed = false;

    public class CoordinateList : List<CursorController.Coordinate>
    {
    };

    public int yPos;

    [SyncVar] public Color _color;

    public static Unit current;
    public GameObject hitsPrefab;
    public GameObject actionDialoguePrefab;
    public static Unit hovered;

    public int maxHp;

    public int currentHp;

    public static bool loaded = false;

    private bool isCurrent = false;

    public int turnIndex;

    public int maxMp;
    public int currentMp;

    public int stanceIndex = 0;

    public string defense = "Free";

    public bool hasActed = false;
    public bool hasMoved = false;

    private int _points = 100;

    public bool dead = false;

    public float attackModifier = 1;
    public float physicalResistModifier = 1;
    public float moveModifier = 1;

    public GameObject pointsPrefab;
    private Animator animator;

    public List<string> actionNames;
    public List<string> stanceNames;

    public Coordinate ProjectedCoordinate()
    {
        Coordinate coordinate = new Coordinate();
        coordinate.xPos = !ModelController.inModelMode ? xPos : GetComponent<ModelBehavior>().modelCursor.xPos;
        coordinate.zPos = !ModelController.inModelMode ? zPos : GetComponent<ModelBehavior>().modelCursor.zPos;
        return(coordinate);
    }

    public Transform ProjectedHittable(){
        Transform hittable = Unit.Subject().transform.Find("Body").Find("Hittable").transform;
        if(ModelController.inModelMode){
            hittable = Unit.Subject().transform.Find("Model").Find("Body(Clone)").Find("Hittable").transform;
        }
        return(hittable);
    }

    private void SetClass(string className)
    {
        unitClass = className;
        transform.Find("Body").Find("Hats").Find(unitClass).gameObject.active = true;
        switch(unitClass){
            case "Warrior":
              maxHp = 50;
              moveBase = 5;
              jumpBase = 1;
              break;
            case "Medic":
              maxHp = 35;
              moveBase = 5;
              jumpBase = 1;
              break;
            case "Archer":
              maxHp = 30;
              moveBase = 6;
              jumpBase = 2;
              break;
            case "Mage":
              maxHp = 30;
              moveBase = 4;
              jumpBase = 1;
              break;
        }
        currentHp = maxHp;
    }

    public void PopulateActionsAndStances(string actionString, string stanceString, string classString)
    {
        // List<string> actionNames = new List<string>{
        //     "ActionKill",
        //     "ActionSickness",
        //     "ActionPanacea",
        //     "ActionAffinityWipe"
        // };
        List<string> actionNames = actionString.Split(',').ToList().GetRange(unitIndex * 4, 4);
        List<string> stanceNames = stanceString.Split(',').ToList().GetRange(unitIndex * 3, 3);
        SetClass(classString.Split(',')[unitIndex]);
        
        GameObject instance;

        foreach (string actionName in actionNames)
        {
            instance = Instantiate(Resources.Load("Actions/" + actionName, typeof(GameObject))) as GameObject;
            instance.transform.parent = transform.Find("Actions");
        }

        foreach (string stanceName in new List<string> {"StanceNeutral"}.Concat(stanceNames))
        {
            instance = Instantiate(Resources.Load("Stances/" + stanceName, typeof(GameObject))) as GameObject;
            instance.transform.parent = transform.Find("Stances");
        }
    }

    // public static List<string> allActions = new List<string>{
    //   "Attack",
    //   "ChainLightning",
    //   "DelayAttack",
    //   "Fire",
    //   "LightningStab",
    //   "Meteor",
    //   "Punish",
    //   "Razz",
    //   "SpinAttack",
    //   "ThrowStone",
    // };

    public void SetJumpingAnimation(bool state)
    {
        animator.SetBool("Jumping", state);
    }

    public void SetWalkingAnimation(bool state)
    {
        animator.SetBool("Walking", state);
    }

    // Use this for initialization
    public static Unit Subject()
    {
        if (ModelController.inModelMode)
        {
            return(ModelController.unit);
        }
        else
        {
            return(Unit.current);
        }
    }

    public void RecoverTurnMp()
    {
        float mpRate = 1;

        foreach (Buff buff in Buffs())
        {
            mpRate *= buff.ReturnMpMod();
        }

        currentMp += Mathf.RoundToInt(mpRate);
    }

    public void RemoveBuffs()
    {
        foreach (Buff buff in Buffs())
        {
            buff.Remove();
        }
    }

    public int MaxTurnsDead()
    {
        float maxTurnsDead = 2;
        foreach (Buff buff in Buffs())
        {
            maxTurnsDead *= buff.TurnsDeadMod();
        }

        return(Mathf.RoundToInt(maxTurnsDead));
    }

    void Awake()
    {
        this._unitMovement = this.GetComponent<UnitMovement>();
    }

    void Start(){

    }

    void Load()
    {
        if (!controllersInitialized && CursorController.loaded && VoxelController.instance)
        {
            Cursor cursor = CursorController.cursorMatrix[xPos][zPos];
            print("cursor for " + unitName + ": " + cursor);
            loaded = true;
            GetComponent<ModelBehavior>().Load();
            yPos = 0;
            SetTransformPosition();
            controllersInitialized = true;

            animator = transform.Find("Body").Find("CharacterModel").GetComponent<Animator>();

            SetTransformPosition();

            transform.Find("Marker").GetComponent<Renderer>().material.color = Color.white;

            transform.Find("Body").Find("CharacterModel").Find("ArmLeft1").GetComponent<Renderer>().material.color =
                _color;
            transform.Find("Body").Find("CharacterModel").Find("ArmRight1").GetComponent<Renderer>().material.color =
                _color;
            transform.Find("Body").Find("CharacterModel").Find("Body1").GetComponent<Renderer>().material.color =
                _color;
            transform.parent = GameObject.Find("Units").transform;

            cursor.standingUnit = this;

            all.Add(this);

            GameObject pointsObject = Instantiate(pointsPrefab, Vector3.zero, Quaternion.identity);

            pointsObject.GetComponent<Points>().unit = this;

            PlayerPointsBar.ResizeByIndex(playerIndex);

            GameObject instance;

            ReflectCurrent();
        }
    }

    public void SetTransformPosition()
    {
        Helpers.SetTransformPosition(transform, xPos, yPos, zPos);
    }

    public List<GameObject> Actions()
    {
        List<GameObject> actions = new List<GameObject>();
        foreach (Transform child in transform.Find("Actions"))
        {
            actions.Add(child.gameObject);
        }

        return(actions);
    }

    public List<GameObject> Stances()
    {
        List<GameObject> stances = new List<GameObject>();
        foreach (Transform child in transform.Find("Stances"))
        {
            stances.Add(child.gameObject);
        }

        return(stances);
    }

    public static List<Unit> All()
    {
        return(all);
    }

    public void ReceiveBuff(GameObject buff)
    {
        Buff existingBuff = Buffs().Find(x => x.Name() == buff.GetComponent<Buff>().Name());

        if (existingBuff)
        {
            existingBuff.Remove();
        }

        GameObject hitsObject = Instantiate(hitsPrefab, transform.position, Quaternion.identity);
        hitsObject.transform.parent = GameObject.Find("Popups").transform;
        hitsObject.transform.position = transform.Find("UIAnchors").Find("Damage").position;
        hitsObject.GetComponent<Hits>().damage = "+" + buff.GetComponent<Buff>().Name().ToString();
        hitsObject.GetComponent<Hits>().color = Color.magenta;

        buff.GetComponent<Buff>().unit = this;
        buff.transform.parent = transform.Find("Buffs");

        if (Buffs().Count > 3)
        {
            Buffs()[0].Remove();
        }
    }

    public List<Buff> Buffs()
    {
        List<GameObject> buffs = new List<GameObject>();
        foreach (Transform child in transform.Find("Buffs"))
        {
            buffs.Add(child.gameObject);
        }

        return(buffs.Select(x => x.GetComponent<Buff>()).ToList());
    }

    public void AdvanceBuffs()
    {
        foreach (Buff buff in Buffs())
        {
            if (buff.TurnsLeft() < 1)
            {
                Destroy(buff.gameObject);
            }
            else
            {
                buff.DeductTurn();
            }
        }
    }

    void FixedUpdate()
    {
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = rotation;

        Load();
    }

    public void SetPosition(int x, int z)
    {
        UnsetPosition();
        CursorController.cursorMatrix[x][z].standingUnit = this;

        xPos = x;
        zPos = z;
    }

    public void UnsetPosition()
    {
        CursorController.cursorMatrix[xPos][zPos].standingUnit = null;
    }

    public void SetColor(Color color)
    {
        _color = color;
    }

    private void Die(Unit aggressor)
    {
        Cursor tile = Helpers.GetTile(xPos, zPos);
        tile.standingUnit = null;
        UnsetPosition();
        yPos = -1;
        dead = true;
        int pointsToAdd = _points;
        int defaultPoints = 100;
        if (aggressor.playerIndex == playerIndex) pointsToAdd -= defaultPoints;
        aggressor.AddPoints(pointsToAdd);
        SetPoints(defaultPoints);
        foreach (Buff buff in Buffs())
        {
            if (buff.ExpiresOnDeath()) buff.Remove();
        }

        StartCoroutine(MoveAway());
    }

    private IEnumerator MoveAway()
    {
        yield return new WaitForSeconds(3f);
        transform.position = new Vector3(9999, 9999, 9999);
    }

    private IEnumerator MoveBack()
    {
        yield return new WaitForSeconds(1f);
        RenderPosition();
    }

    public void RenderPosition()
    {
        transform.position = new Vector3(
            xPos + .5f,
            yPos + 1.5f,
            zPos + .5f
        );
    }

    public void Revive()
    {
        dead = false;
        Cursor tile = GameController.GetRespawnTile();
        SetPosition(tile.xPos, tile.zPos);
        yPos = tile.yPos;
        currentHp = 30;
        dead = false;
        tile.standingUnit = this;
        GameController.refreshView = true;
        foreach (Buff buff in Buffs())
        {
            if (buff.ExpiresOnRevive()) buff.Remove();
        }

        StartCoroutine(MoveBack());
    }

    public float MoveModifier()
    {
        float modifier = 1;
        foreach (Buff buff in Buffs())
        {
            modifier *= buff.MoveMod();
        }

        return(modifier);
    }

    public int MoveLength()
    {
        Stance stance = ModelController.inModelMode ? GetComponent<ModelBehavior>().Stance() : Stance();
        return(Mathf.RoundToInt(stance.NegotiateMoveLength(moveBase * MoveModifier())));
    }

    public float JumpModifier()
    {
        float modifier = 1;
        foreach (Buff buff in Buffs())
        {
            modifier *= buff.JumpMod();
        }

        return(modifier);
    }

    public int JumpHeight()
    {
        return(Mathf.RoundToInt(JumpModifier() * jumpBase));
    }

    public void SetStance(int newStanceIndex)
    {
        stanceIndex = newStanceIndex;
    }

    public Stance Stance()
    {
        return(Stances()[stanceIndex].GetComponent<Stance>());
    }

    public void SetAffinity(Helpers.Affinity newAffinity)
    {
        affinity = newAffinity;

        GameObject hitsObject = Instantiate(hitsPrefab, transform.position, Quaternion.identity);
        hitsObject.transform.parent = GameObject.Find("Popups").transform;
        hitsObject.transform.position = transform.Find("UIAnchors").Find("Damage").position;
        hitsObject.GetComponent<Hits>().damage = "Affinity: " + newAffinity.ToString();
        hitsObject.GetComponent<Hits>().color = Color.cyan;
    }

    public void HealDamage(int damage)
    {
        currentHp += damage;
        if (currentHp > maxHp) currentHp = maxHp;

        GameObject hitsObject = Instantiate(hitsPrefab, transform.position, Quaternion.identity);
        hitsObject.transform.parent = GameObject.Find("Popups").transform;
        hitsObject.transform.position = transform.Find("UIAnchors").Find("Damage").position;
        hitsObject.GetComponent<Hits>().damage = "+" + damage.ToString() + " HP";
        hitsObject.GetComponent<Hits>().color = Color.green;
    }

    public void ReceiveDamage(int damage, Unit aggressor, UnitAction action)
    {
        bool backAttacked = 
            aggressor.playerIndex != this.playerIndex &&
            action.actionType() == UnitAction.ActionType.Melee &&
            !aggressor.GetComponent<UnitMovement>().TraversedAlertTile();

        float calculatedDamage = damage;
        int startingHp = currentHp;

        foreach (Buff buff in Buffs())
        {
            if (action.actionType() == UnitAction.ActionType.Melee)
            {
                calculatedDamage /= buff.MeleeResistMod();
            }

            if (buff.CommunicableType() == action.actionType())
            {
                action.Unit().ReceiveBuff(Instantiate(buff.gameObject, Vector3.zero, Quaternion.identity));
            }
        }

        foreach (Buff buff in action.Unit().Buffs())
        {
            if (buff.CommunicableType() == action.actionType())
            {
                ReceiveBuff(Instantiate(buff.gameObject, Vector3.zero, Quaternion.identity));
            }
        }

        if (action.Affinity() != Helpers.Affinity.None)
        {
            if (action.Affinity() == affinity)
            {
                calculatedDamage *= 0.5f;
            }

            if (affinity == Helpers.Affinity.None)
            {
                SetAffinity(action.Affinity());
            }

            if (
                (action.Affinity() == Helpers.Affinity.Water && affinity == Helpers.Affinity.Fire) ||
                (action.Affinity() == Helpers.Affinity.Fire && affinity == Helpers.Affinity.Earth) ||
                (action.Affinity() == Helpers.Affinity.Earth && affinity == Helpers.Affinity.Water)
            )
            {
                calculatedDamage *= 2;
                SetAffinity(action.Affinity());
            }
        }

        calculatedDamage = backAttacked ? calculatedDamage + 5 : Stance().NegotiateDamage(calculatedDamage, action);

        RevealCurrentStance();

        int actualReceivedDamage = Mathf.RoundToInt(calculatedDamage);

        currentHp -= actualReceivedDamage;

        System.Action showHits = () =>
        {
            GameObject hitsObject = Instantiate(hitsPrefab, transform.position, Quaternion.identity);
            hitsObject.transform.parent = GameObject.Find("Popups").transform;
            hitsObject.transform.position = transform.Find("UIAnchors").Find("Damage").position;
            hitsObject.GetComponent<Hits>().damage = "-" + actualReceivedDamage.ToString() + " HP";
        };

        GameObject stanceDialogueObject = Instantiate(actionDialoguePrefab, transform.position, Quaternion.identity);
        ActionDialogue dialogue = stanceDialogueObject.GetComponent<ActionDialogue>();
        string messageToDisplay = Stance().Name();
        if(backAttacked) messageToDisplay += " (Back)";
        dialogue.message = messageToDisplay;
        dialogue.unit = this;
        dialogue.whenDone = showHits;
        dialogue.color = Color.green;

        stanceDialogueObject.transform.parent = GameObject.Find("Popups").transform;
        stanceDialogueObject.transform.position = transform.Find("UIAnchors").Find("Points").position;

        if (actualReceivedDamage >= startingHp)
        {
            Die(aggressor);
        }
    }

    public void HealMp(int mp)
    {
        currentMp += mp;
        currentMp = Mathf.Clamp(currentMp, 0, maxMp);
        GameObject hitsObject = Instantiate(hitsPrefab, transform.position, Quaternion.identity);
        hitsObject.transform.parent = GameObject.Find("Popups").transform;
        hitsObject.transform.position = transform.Find("UIAnchors").Find("Damage").position;
        hitsObject.GetComponent<Hits>().damage = "+" + mp.ToString() + " MP";
        hitsObject.GetComponent<Hits>().color = Color.green;
    }

    public int DamageMp(int mp)
    {
        int startingMp = currentMp;
        currentMp -= mp;
        currentMp = Mathf.Clamp(currentMp, 0, maxMp);
        GameObject hitsObject = Instantiate(hitsPrefab, transform.position, Quaternion.identity);
        hitsObject.transform.parent = GameObject.Find("Popups").transform;
        hitsObject.transform.position = transform.Find("UIAnchors").Find("Damage").position;
        hitsObject.GetComponent<Hits>().damage = "-" + (startingMp - currentMp).ToString() + " MP";

        return(startingMp - currentMp);
    }

    public void RevealCurrentStance()
    {
        Stance().used = true;
        stanceRevealed = true;
    }

    private void AnnounceTpChange(int points)
    {
        string symbol = "-";


        if (points < 0) symbol = "+";

        GameObject hitsObject = Instantiate(hitsPrefab, transform.position, Quaternion.identity);
        hitsObject.transform.parent = GameObject.Find("Popups").transform;
        hitsObject.transform.position = transform.Find("UIAnchors").Find("Damage").position;
        hitsObject.GetComponent<Hits>().damage = symbol + Mathf.Abs(points).ToString() + " TP";
    }

    public void IncreaseTurnPoint(int points)
    {
        if (turnIndex == 0) return;

        List<Unit> units = UnitsSortedByTurnIndex();
        List<Unit> unitsSlice = units.GetRange(Mathf.Clamp(turnIndex - points, 0, turnIndex - 1),
            Mathf.Clamp(points, 1, turnIndex));

        foreach (Unit unit in unitsSlice)
        {
            unit.turnIndex += 1;
        }

        turnIndex -= unitsSlice.Count;

        AnnounceTpChange(-unitsSlice.Count);
    }

    public void DecreaseTurnPoint(int points)
    {
        if (turnIndex == Unit.All().Count - 1) return;

        List<Unit> units = UnitsSortedByTurnIndex();
        List<Unit> unitsSlice = units.GetRange(turnIndex + 1, Mathf.Clamp(points, 1, units.Count - 1 - turnIndex));

        foreach (Unit unit in unitsSlice)
        {
            unit.turnIndex -= 1;
        }

        turnIndex += unitsSlice.Count;

        AnnounceTpChange(unitsSlice.Count);
    }

    private List<Unit> UnitsSortedByTurnIndex()
    {
        List<Unit> units = Unit.All();
        units.Sort((a, b) => a.GetComponent<Unit>().turnIndex.CompareTo(b.GetComponent<Unit>().turnIndex));

        return(units);
    }

    public int Points()
    {
        return(_points);
    }

    public void AddPoints(int pointsToAdd)
    {
        _points += pointsToAdd;
        if (Player.ByIndex(playerIndex).CurrentPoints() >= 1000)
        {
            GameController.EndGame();
        }

        PlayerPointsBar.ResizeByIndex(this.playerIndex);
    }

    public void SetPoints(int newPoints)
    {
        _points = newPoints;
        if (Player.ByIndex(playerIndex).CurrentPoints() >= 1000)
        {
            GameController.EndGame();
        }

        PlayerPointsBar.ResizeByIndex(this.playerIndex);
    }

    public void SetIsCurrent(bool newIsCurrent)
    {
        isCurrent = newIsCurrent;
        ReflectCurrent();
        if (isCurrent)
        {
            GameController.instance.ResolveDeathPhase();
        }

        GameController.RefreshPlayerView();
    }

    public void ReflectCurrent()
    {
        if (isCurrent)
        {
            Unit.current = this;
            GetComponent<UnitUIBehavior>().SetMarker();
        }
        else
        {
            GetComponent<UnitUIBehavior>().UnsetMarker();
        }
    }

    public void SetCurrent()
    {
        if (Unit.current)
        {
            Unit.current.SetIsCurrent(false);
        }

        SetHasMoved(false);
        SetHasActed(false);
        SetIsCurrent(true);
        stanceRevealed = false;
        AdvanceBuffs();
    }

    public void SetPath(CursorController.Coordinate[] path)
    {
        this._unitMovement.MoveAlongPath(path, true);
        SetHasMoved(true);
    }

    public void ForceWalkAwayFrom(int tilesToMove, int directionX, int directionZ)
    {
        Coordinate direction = Direction(directionX, directionZ);

        _unitMovement.ForceWalk(tilesToMove, direction.xPos * -1, direction.zPos * -1);
    }

    public void StartMoving()
    {
        ActionInformation.Hide();
        GameController.StartMoving(this);
        MainCamera.Lock();
    }

    public void DoAction(int x, int z, int actionIndex)
    {
        MainCamera.Lock();
        MainCamera.CenterOnWorldPoint(Unit.current.transform.position);
        ActionInformation.Hide();
        GameObject actionObject = Actions()[actionIndex];
        UnitAction action = actionObject.GetComponent<UnitAction>();

        Cursor cursor = CursorController.cursorMatrix[x][z];

        GameObject actionDialogueObject = Instantiate(actionDialoguePrefab, transform.position, Quaternion.identity);
        actionDialogueObject.GetComponent<ActionDialogue>().message = action.Name();
        actionDialogueObject.GetComponent<ActionDialogue>().unit = this;
        actionObject.GetComponent<UnitAction>().currentMp = currentMp;
        System.Action beginAction = () => { actionObject.GetComponent<UnitAction>().BeginAction(cursor.gameObject); };
        currentMp -= action.MpCost();
        actionDialogueObject.GetComponent<ActionDialogue>().whenDone = beginAction;

        LookToward(x, z);
    }

    public void LookToward(int x, int z, int magnitude)
    {
        int xDiff = x - xPos;
        int zDiff = z - zPos;

        if(xDiff == 0 && zDiff == 0){
            return;
        }

        if (Mathf.Abs(xDiff) > Mathf.Abs(zDiff))
        {
            xDiff /= Mathf.Abs(xDiff);
            zDiff = 0;
        }
        else
        {
            zDiff /= Mathf.Abs(zDiff);
            xDiff = 0;
        }

        lookDirection = new Vector3(xDiff, 0, zDiff) * magnitude;
    }

    public void LookToward(int x, int z)
    {
        LookToward(x, z, 1);
    }

    public void LookAwayFrom(int x, int z)
    {
        LookToward(x, z, -1);
    }

    private Coordinate Direction(int x, int z)
    {
        int xDiff = x - xPos;
        int zDiff = z - zPos;

        if (Mathf.Abs(xDiff) > Mathf.Abs(zDiff))
        {
            xDiff /= Mathf.Abs(xDiff);
            zDiff = 0;
        }
        else
        {
            zDiff /= Mathf.Abs(zDiff);
            xDiff = 0;
        }

        Coordinate coordinate = new Coordinate();
        coordinate.xPos = xDiff;
        coordinate.zPos = zDiff;

        return coordinate;
    }

    public void FinishAction()
    {
        MainCamera.Unlock();
        GameController.UnfreezeInputs();
        SetHasActed(true);
    }

    public void EndTurn()
    {
        hasActed = true;
        hasMoved = true;
        // SpendTp(EndTurnCost());
        TurnController.Next();
        GameController.RefreshPlayerView();
    }

    private void SetHasActed(bool newHasActed)
    {
        hasActed = newHasActed;
        if (hasActed)
        {
            GameController.FinishAction();
        }
        else
        {
            GameController.RefreshPlayerView();
        }
    }

    private void SetHasMoved(bool newHasMoved)
    {
        hasMoved = newHasMoved;
        if (hasMoved)
        {
            StartMoving();
        }
        else
        {
            GameController.RefreshPlayerView();
        }
    }

    public bool DoneWithTurn()
    {
        return(dead || (hasActed && hasMoved));
    }

    public void ReadyNextTurn()
    {
        SetHasActed(false);
        SetHasMoved(false);
    }

    private bool IsIdle()
    {
        return(animator.GetCurrentAnimatorStateInfo(0).IsName("UnitIdleAnimation"));
    }

    private bool IsWalking()
    {
        return(animator.GetCurrentAnimatorStateInfo(0).IsName("UnitWalkingAnimation"));
    }

    private bool IsJumping()
    {
        return(animator.GetCurrentAnimatorStateInfo(0).IsName("UnitJumpingAnimation"));
    }

    public void FinishMoving(bool turnBased)
    {
        Treasure treasure = Helpers.GetTile(xPos, zPos).standingTreasure;
        if (treasure)
        {
            AddPoints(treasure.points);
            treasure.Remove();
        }

        if (turnBased)
        {
            GameController.FinishMoving();
            MainCamera.Unlock();
        }
    }
}
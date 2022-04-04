using UnityEngine;
using Pathfinding; // Aaron Granberg A*
using System.Collections;

public class BasicEnemyContext : MonoBehaviour
{
    // SECTION - Field ===================================================================
    #region REGION - HIDDEN - State
    private IEnemyState currState;
    private IEnemyState oldState;
    #endregion


    #region REGION - HIDDEN - Entity Specific
    private LivingEntityContext myLivingEntity;
    private Transform mySpriteTransform;
    #endregion


    #region REGION - HIDDEN - AStar Specific
    private bool canUseBehaviour = true;
    private Transform myTemporaryTargetTransform;
    private AIPath myAIPath;                           // Movement, rotation, End Reached Distance, etc.
    private AIDestinationSetter myAIDestinationSetter; // Pathfinding Target
    private float maxSpeed;
    private const float defaultEndReachedDistance = 0.96f;
    #endregion


    #region REGION - HIDDEN - Animator Strings

    // Animator
    private Animator anim;

    // Parameters
    private readonly string animParam_ExitDeathAnim = "ExitDeathAnim";
    private readonly string animParam_maxSpeed = "maxSpeed";
    private readonly string animParam_animAngle = "animAngle";

    // Animation States & Parameters
    private readonly string animState_Iddle = "Iddle";
    private readonly string animState_OnAwake = "OnAwake";
    private readonly string animState_OnMoveBlendTree = "BlendTree _ Movement";
    private readonly string animState_OnDeath = "OnDeath";

    private readonly string animState_01_Transition = "State_01_Transition";
    private readonly string animState_01_NoToken = "State_01_NoToken";
    private readonly string animState_01_Token = "State_01_Token";

    private readonly string animState_02_Transition = "State_02_Transition";
    private readonly string animState_02_NoToken = "State_02_NoToken";
    private readonly string animState_02_Token = "State_02_Token";
    #endregion


    [Header("    ======= On Start Specifications =======\n")]
    [SerializeField] private GameObject myTemporaryTargetPrefab;
    [Space(10)]
    [SerializeField] private BasicEnemy_States startingState = BasicEnemy_States.ONE;
    [SerializeField] private bool startAtMaxSpeed = true;

    [Header("    ======= Animator Specifications =======\n")]
    [Tooltip("Allows to keep last sprite for lingering dead enemies")]
    [SerializeField] private bool exitDeathAnim = true;

    [Header("    ======= Other Specifications =======\n")]
    [SerializeField] private bool isHoveringEnemy = false;

    [Space(10)]
    [Header("    ========== State One ==========\n")]
    [Header("Weapon Manager")]
    [Tooltip("[Range] is used for [endReachedDistance]" +
             "[MainWeaponIsReloading] is used for time between two attacks")]
    [SerializeField] private WeaponManager weaponManager_1;

    [Header("Animator")]
    [Tooltip("If false, OnDeath animation will stay at last frame until object is destroyed")]

    [SerializeField] private bool to_2_OnAtkExit = false;
    [Tooltip("Animation event must be set manually")]
    [SerializeField] private bool animExecuteAtk_1 = false;
    private AbstractBehaviour behaviour_NoToken_1;
    private AbstractBehaviour behaviour_Token_1;

    [Space(10)]
    [Header("    ========= State Two =========\n")]
    [Header("Weapon Manager")]
    [SerializeField] private WeaponManager weaponManager_2;

    [Header("Animator")]
    [SerializeField] private bool to_1_OnAtkExit = false;
    [Tooltip("Animation event must be set manually")]
    [SerializeField] private bool animExecuteAtk_2 = false;
    private AbstractBehaviour behaviour_NoToken_2;
    private AbstractBehaviour behaviour_Token_2;

    [SerializeField] private bool hasToken = false;
    // SECTION - Property ===================================================================
    #region REGION - PROPERTY
    // State
    public IEnemyState CurrState { get => currState; set => currState = value; }

    // General
    public LivingEntityContext MyLivingEntity { get => myLivingEntity; set => myLivingEntity = value; }
    public Animator Anim { get => anim; set => anim = value; }

    // AI
    public AIPath MyAIPath { get => myAIPath; set => myAIPath = value; }
    public Transform MyTemporaryTargetTransform { get => myTemporaryTargetTransform; }

    // State One
    public WeaponManager WeaponManager_1 { get => weaponManager_1; set => weaponManager_1 = value; }
    public bool To_2_OnAtkExit { get => to_2_OnAtkExit; }
    public bool AnimExecuteAtk_1 { get => animExecuteAtk_1; }
    public AbstractBehaviour Behaviour_NoToken_1 { get => behaviour_NoToken_1; }
    public AbstractBehaviour Behaviour_Token_1 { get => behaviour_Token_1; }

    // State Two
    public WeaponManager WeaponManager_2 { get => weaponManager_2; set => weaponManager_2 = value; }
    public bool To_1_OnAtkExit { get => to_1_OnAtkExit; }
    public bool AnimExecuteAtk_2 { get => animExecuteAtk_2; }
    public AbstractBehaviour Behaviour_NoToken_2 { get => behaviour_NoToken_2; }
    public AbstractBehaviour Behaviour_Token_2 { get => behaviour_Token_2; }

    public bool HasToken { get => hasToken; set => hasToken = value; }
    public bool CanUseBehaviour { get => canUseBehaviour; set => canUseBehaviour = value; }
    #endregion


    // SECTION - Method - Unity Specific ===================================================================
    private void OnDestroy()
    {
        if (MyTemporaryTargetTransform)
            Destroy(myTemporaryTargetTransform.gameObject);
    }

    private void Start()
    {
        // Get Set Components & Variables
        GetSetHiddensHandler();

        // Set State Machine
        FirstStateHandler();
    }

    private void FixedUpdate()
    {
        if (oldState != currState)
        {
            oldState = currState;
            OnStateEnter();
        }

        OnStateUpdate();
        OnStateExit();
    }


    // SECTION - Method - State Specific ===================================================================
    public void OnStateEnter()
    {
        currState.OnStateEnter(this);
    }

    public void OnStateUpdate()
    {
        currState.OnStateUpdate(this);
    }

    public void OnStateExit()
    {
        currState = currState.OnStateExit(this);
    }


    // SECTION - Method - Utility ===================================================================
    #region REGION - On Start Handlers
    private void FirstStateHandler()
    {
        switch(startingState)
        {
            case BasicEnemy_States.ONE:
                SetFiniteStateMachine(BasicEnemy_States.ONE);
                break;
            case BasicEnemy_States.TWO:
                SetFiniteStateMachine(BasicEnemy_States.TWO);
                break;
            default: Debug.Log($"An error as occured at [FirstStateHandler()] of [EnemyContext.cs] from enemy: {gameObject.name}"); break;
        }

        oldState = currState;

        // Instantiate WeaponSOs && Set endReachedDistance
        FirstSetMainWeaponAndAIDistance(currState);     
    }

    private void FirstSetMainWeaponAndAIDistance(IEnemyState myState)
    {
        // Weapons
        WeaponSO myWeaponSO = null;

        if (myState is BasicEnemyState_One)
        {
            if (weaponManager_1 != null)
            {
                // Clone WeaponSO and set it up as main weapon
                myWeaponSO = Instantiate(weaponManager_1.Weapon);
                weaponManager_1.Weapon = myWeaponSO;
                return;
            }
        }
        else if (myState is BasicEnemyState_Two)
        {
            if (weaponManager_2 != null)
            {
                // Clone WeaponSO and set it up as main weapon
                myWeaponSO = Instantiate(weaponManager_2.Weapon);
                weaponManager_2.Weapon = myWeaponSO;
                return;
            }
        }

        if (myWeaponSO != null)
            SetEndReachedDistance(myWeaponSO.Range);
        else
            SetEndReachedDistance(defaultEndReachedDistance);
    }

    private void GetSetHiddensHandler()
    {
        // AI ========================================
        // Get Components
        MyAIPath = GetComponentInChildren<AIPath>();
        myAIDestinationSetter = GetComponentInChildren<AIDestinationSetter>();

        // Set Variables
        myTemporaryTargetTransform = Instantiate(myTemporaryTargetPrefab, GameObject.Find("--------------------- DYNAMIC").transform).transform;
        maxSpeed = myAIPath.maxSpeed;


        if (myAIDestinationSetter.target == null)
            SetTargetAsPlayer();
            //myAIDestinationSetter.target = GameManager.instance.PlayerTransformRef;

        if (!startAtMaxSpeed)
            SetSpeed(0.0f);


        // Miscellaneous ========================================
        myLivingEntity = GetComponentInChildren<LivingEntityContext>();
        mySpriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
        anim = GetComponentInChildren<Animator>();
        anim.SetBool(animParam_ExitDeathAnim, exitDeathAnim);

        behaviour_NoToken_1 = transform.GetChild(1).transform.GetChild(0).GetComponentInChildren<AbstractBehaviour>();
        behaviour_Token_1 = transform.GetChild(1).transform.GetChild(1).GetComponentInChildren<AbstractBehaviour>();

        behaviour_NoToken_2 = transform.GetChild(2).transform.GetChild(0).GetComponentInChildren<AbstractBehaviour>();
        behaviour_Token_2 = transform.GetChild(2).transform.GetChild(1).GetComponentInChildren<AbstractBehaviour>();
    }
    #endregion

    #region REGION - Default Behaviours
    public void OnDefaultAttackBehaviour()
    {
        SetSpeed(0.0f);
    }

    public void OnDefaultMoveBehaviour()
    {
        anim.SetFloat(animParam_maxSpeed, myAIPath.maxSpeed);
    }

    private float lastIndex = 0;
    public void OnDefaultSetMoveAnim()
    {
        float angle = StaticEnemyAnimHandler.GetAngle(GameManager.instance.PlayerTransformRef, transform);
        anim.SetFloat(animParam_animAngle, StaticEnemyAnimHandler.GetIndex(angle, lastIndex));
        StaticEnemyAnimHandler.SetSpriteFlip(mySpriteTransform, angle);
    }

    public void SetTargetAsPlayer()
    {
        if (!myAIDestinationSetter.target == GameManager.instance.PlayerTransformRef)
            myAIDestinationSetter.target = GameManager.instance.PlayerTransformRef;
    }

    public void SetTarget(Transform newTarget)
    {
        myAIDestinationSetter.target = newTarget;
    }

    public Transform GetTargetTransform()
    {
        return myAIDestinationSetter.target;
    }

    public void OnDefaultManageToken()
    {
        // TODO:
        //      - REFACTORISE WHAT DICTATES RETURN TOKEN

        if (IsIddleOrMoving() && IsTargetNear())
        {
            HasToken = AIManager.instance.MyTokenHandlerSO.TryGetToken();
        }
        else if (HasToken && (!HasPath() || HasReachedEndOfPath())) // Replace token when out of reach
        {
            HasToken = false;
            AIManager.instance.MyTokenHandlerSO.ReturnToken(hasToken);
        }
        else if (!HasToken && (HasPath() && GetTargetTransform().CompareTag("Player"))) // || context.HasReachedEndOfPath()))       // Try get Token
        {
            HasToken = AIManager.instance.MyTokenHandlerSO.TryGetToken();
        }
    }
    #endregion

    #region REGION - Utility
    // Miscellaneous
    public void SetRigidBodyConstraint_Y()
    {
        if (isHoveringEnemy)
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
    }

    public bool IsTargetNear()
    {

        if (GetCurrentWeaponManager() == null)
        {
            Collider[] hit;
            hit = StaticRayCaster.IsOverlapSphereTouching(transform, MyAIPath.endReachedDistance, myAIDestinationSetter.target.gameObject.layer, true);

            return hit == null;
            //return !(hit[0].transform == null);
            // return true;
        }

        if (GetCurrentWeaponManager().TracksPlayer)
            return GetCurrentWeaponManager().IsTargetInFront();

        return GetCurrentWeaponManager().IsTargetAround();
    }

    // Target
    public void SetMyTemporaryTargetAs(Transform setAs)
    {
        myTemporaryTargetTransform.position = setAs.position;
    }

    public void SetMyTemporaryTargetAs(Vector3 setAs)
    {
        myTemporaryTargetTransform.position = setAs;
    }

    public bool HasReachedEndOfPath()
    {
        return myAIPath.reachedEndOfPath;
    }

    // Weapon
    public bool TryFireMainWeapon()
    {
        if (currState is BasicEnemyState_One)
            if (weaponManager_1 != null)
                return weaponManager_1.TriggerWeapon();
        else if (currState is BasicEnemyState_Two)
            if (weaponManager_2 != null)
                return weaponManager_2.TriggerWeapon();

        return false; // true == prevent using main weapon when checking !IsMainWeaponReloading()
    }

    public bool TryFireMainWeapon(BasicEnemy_States stateSpecificCheck)
    {
        if (stateSpecificCheck == BasicEnemy_States.ONE)
            if (weaponManager_1 != null)
                return weaponManager_1.TriggerWeapon();
        else if (stateSpecificCheck == BasicEnemy_States.TWO)
            if (weaponManager_2 != null)
                return weaponManager_2.TriggerWeapon();

        return false; // true == prevent using main weapon when checking !IsMainWeaponReloading()
    }

    public bool IsCurrentWeaponManagerNull()
    {
        if (currState is BasicEnemyState_One && weaponManager_1 == null)
            return true;
        else if (currState is BasicEnemyState_Two && WeaponManager_2 == null)
            return true;

        return false;
    }

    public WeaponManager GetCurrentWeaponManager()
    {
        if (currState is BasicEnemyState_One && weaponManager_1 != null)
            return weaponManager_1;
        else if (currState is BasicEnemyState_Two && WeaponManager_2 != null)
            return WeaponManager_2;

        return null;
    }

    public WeaponManager GetSpecificWeaponManager(BasicEnemy_States specificState)
    {
        if (specificState == BasicEnemy_States.ONE)
            return weaponManager_2;
        else if (specificState == BasicEnemy_States.TWO)
            return WeaponManager_1;

        return null;
    }

    public bool IsWeaponReloading()
    {
        WeaponManager myWeaponManager = GetCurrentWeaponManager();

        if (myWeaponManager == null)
            return false;

        // Manage token in case of reload
        if (myWeaponManager.WeaponIsReloading && hasToken)
        {
            AIManager.instance.MyTokenHandlerSO.ReturnToken(hasToken);
            hasToken = false;
        }

        return myWeaponManager.WeaponIsReloading;
    }

    // PathFinding
    public void SetSpeedAsDefault() // Note : Also used as animator event
    {
        MyAIPath.maxSpeed = maxSpeed;
    }

    public void SetSpeed(float newSpeed)
    {
        MyAIPath.maxSpeed = newSpeed;
    }

    public bool HasPath()
    {
        return MyAIPath.hasPath;
    }

    public void SetEndReachedDistance(float newEndReachedDistance = defaultEndReachedDistance)
    {
        myAIPath.endReachedDistance = newEndReachedDistance;
    }

    public void SetEndReachedDistance_ToCurrState()
    {
        if (currState is BasicEnemyState_One)
        {
            if (weaponManager_1 != null)
            {
                SetEndReachedDistance(weaponManager_1.Weapon.Range);
                return;
            }
        }
        else if (currState is BasicEnemyState_Two)
        {
            if (weaponManager_2 != null)
            {
                SetEndReachedDistance(weaponManager_2.Weapon.Range);
                return;
            }
        }

        SetEndReachedDistance();
    }

    // State 
    public void SetFiniteStateMachine(BasicEnemy_States transitionTo)
    {
        switch (transitionTo)
        {
            case BasicEnemy_States.ONE:
                if (!(currState is BasicEnemyState_One))
                    currState = new BasicEnemyState_One();
                break;
            case BasicEnemy_States.TWO:
                if (!(currState is BasicEnemyState_Two))
                    currState = new BasicEnemyState_Two();
                break;
        }
    }
    public void ToggleState()
    {
        if (currState is BasicEnemyState_One)
            SetFiniteStateMachine(BasicEnemy_States.TWO);
        else
            SetFiniteStateMachine(BasicEnemy_States.ONE);
    }


    // Animator
    public void SetAwakingTransitionOnHit(bool shouldToggleState)
    {
        if (IsInAnimationState(BasicEnemy_AnimationStates.IDDLE))
        {
            SetTransitionAnim();

            if (shouldToggleState)
            {
                if (currState is BasicEnemyState_One)
                    currState = new BasicEnemyState_Two();
                else if (currState is BasicEnemyState_Two)
                    currState = new BasicEnemyState_One();
            }
        }

    }

    public void SetTransitionAnim()
    {
        if (currState is BasicEnemyState_One)
            anim.SetTrigger(animState_01_Transition);
        else if (currState is BasicEnemyState_Two)
            anim.SetTrigger(animState_02_Transition);
    }

    public void SetAnimTrigger(BasicEnemy_AnimTriggers trigger)
    {
        switch (trigger)
        {
            case BasicEnemy_AnimTriggers.DEATH:
                anim.SetTrigger(animState_OnDeath);
                break;

            case BasicEnemy_AnimTriggers.EXITDEATH:
                anim.SetBool(animParam_ExitDeathAnim, true);
                break;

            case BasicEnemy_AnimTriggers.STATE_01_TRANSITION:
                anim.SetTrigger(animState_01_Transition);
                break;

            case BasicEnemy_AnimTriggers.STATE_01_NOTOKEN:
                anim.SetTrigger(animState_01_NoToken);
                break;

            case BasicEnemy_AnimTriggers.STATE_01_TOKEN:
                anim.SetTrigger(animState_01_Token);
                break;


            case BasicEnemy_AnimTriggers.STATE_02_TRANSITION:
                anim.SetTrigger(animState_02_Transition);
                break;

            case BasicEnemy_AnimTriggers.STATE_02_NOTOKEN:
                anim.SetTrigger(animState_02_NoToken);
                break;

            case BasicEnemy_AnimTriggers.STATE_02_TOKEN:
                anim.SetTrigger(animState_02_Token);
                break;

            default: Debug.Log($"An error as occured at [SetAnimTrigger()] of [EnemyContext.cs] from enemy: {gameObject.name}"); break;
        }
    }

    public bool IsAnimExecuteAttack()
    {
        if (currState is BasicEnemyState_One)
            return animExecuteAtk_1;
        else if (currState is BasicEnemyState_Two)
            return animExecuteAtk_2;

        return false;
    }

    public bool IsInAnimationState(BasicEnemy_AnimationStates checkAnimation)
    {
        switch (checkAnimation)
        {
            case BasicEnemy_AnimationStates.IDDLE:
                return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_Iddle);

            case BasicEnemy_AnimationStates.ONAWAKE:
                return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_OnAwake);

            case BasicEnemy_AnimationStates.MOVEMENT:
                return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_OnMoveBlendTree);

            case BasicEnemy_AnimationStates.DEAD:
                return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_OnDeath);



            case BasicEnemy_AnimationStates.STATE_01_TRANSITION:
                return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_01_Transition);

            case BasicEnemy_AnimationStates.STATE_01_NOTOKEN:
                return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_01_NoToken);

            case BasicEnemy_AnimationStates.STATE_01_TOKEN:
                return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_01_Token);


            case BasicEnemy_AnimationStates.STATE_02_TRANSITION:
                return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_02_Transition);

            case BasicEnemy_AnimationStates.STATE_02_NOTOKEN:
                return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_02_NoToken);

            case BasicEnemy_AnimationStates.STATE_02_TOKEN:
                return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_02_Token);


            default: Debug.Log($"An error as occured at [IsInAnimationState()] of [EnemyContext.cs] from enemy: {gameObject.name}"); break;
        }

        return false;
    }

    public bool IsIddleOrMoving()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_Iddle) ||
                anim.GetCurrentAnimatorStateInfo(0).IsName(animState_OnMoveBlendTree);
    }

    public float GetCurrentAnimStateLength()
    {
        return anim.GetCurrentAnimatorStateInfo(0).length;
    }

    private void AE_ExecuteState_01_Token() // Animator Event
    {
        // TryFireMainWeapon() will execute damage regardless if there is additional behaviours
        TryFireMainWeapon();

        if ( behaviour_Token_1 != null)
            behaviour_Token_1.Execute();
    }

    private void AE_ExecuteState_02_Token() // Animator Event
    {
        // TryFireMainWeapon() will execute damage regardless if there is additional behaviours
        TryFireMainWeapon();

        if (behaviour_Token_2 != null)
            behaviour_Token_2.Execute();
    }

    private void AE_FreezeRigidBodyDisableCollider()
    {
        Rigidbody myRigidBody = GetComponent<Rigidbody>();
        myRigidBody.constraints = RigidbodyConstraints.FreezePosition; 
        GetComponent<Collider>().enabled = false;
    }
    #endregion
}

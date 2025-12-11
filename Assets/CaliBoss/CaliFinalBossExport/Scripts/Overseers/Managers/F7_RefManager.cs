using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Cali7
{ 
    public class F7_RefManager : MonoBehaviour
    {
        public static F7_RefManager Instance;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

        private void Start()
        {
            BCNT = bossCentral;
            BSTM = bossStateMachine;
            BACM = bossActionManager;
            BUIM = bossUIManager;
            BEVM = bossEventManager;
            BANM = bossAnimManager;
            BSDR = bossStateDeterminer;

            BSTA = bossAttack;
            BSTC = bossChase;
            BSTD = bossDefending;
            BSTI = bossIdling;
            BSTR = bossRecovering;

            BGOJ = bossMainObj;
            BDCC = bossClawDamagerCollider;
            BDCS = bossSlamDamagerCollider;
            BRBY = bossRigidbody;
            BNMA = bossNavMeshAgent;
            BAIM = bossAnimator;
            BPSP = bossProjStart;

            GOBB = barrierPrefab;
            GOPP = pillarPrefab;
            GORP = rightPunch;
            GOLP = leftPunch;
            GORR = ringObject;
            GORS = ringStartPoint;
            GORE = ringEndPoint;
            GOSS = shardPrefab;
            GOSL = slamObject;
            GOSW = swipeObject;
            GOBW = wallPrefab;
            GOMP = meleePivot;

            BACT = bossActor;
            BDGL = bossDamageable;
            //BDGR = bossDamager;
            BNGT = bossNavigator;
            BMSR = bossMeleeSensor;

            PLGS = playerScript;

            BPSO = bossPhysSO;

            UIPC = playerHealthCanvas;
            UIBC = enemyHealthCanvas;
            UIBT = bossNameText;
            UIBH = bossHealthBarScript;
        }

        //------Static Refs-------------------------------------
        //      |> Cali Components
        public static F7_Central BCNT;
        public static F7_StateMachine BSTM;
        public static F7_ActionManager BACM;
        public static F7_UIManager BUIM;
        public static F7_EventManager BEVM;
        public static F7_AnimManager BANM;
        public static F7_Phys_SO BPSO;
        public static F7_StateDeterminer BSDR;

//      |> Boss States
        public static F7_Attack BSTA;
        public static F7_Chase BSTC;
        public static F7_Defending BSTD;
        public static F7_Idling BSTI;
        public static F7_Recovering BSTR;
        
//      |> Boss Game Components
        public static GameObject BGOJ;
        public static Collider BDCC;
        public static Collider BDCS;
        public static Rigidbody BRBY;
        public static NavMeshAgent BNMA;
        public static Animator BAIM;
        public static Transform BPSP;

//      |> Prefab Objects
        public static GameObject GOBB;
        public static GameObject GOPP;
        public static GameObject GORP;
        public static GameObject GOLP;
        public static GameObject GORR;
        public static GameObject GORS;
        public static GameObject GORE;
        public static GameObject GOSS;
        public static GameObject GOSL;
        public static GameObject GOSW;
        public static GameObject GOBW;
        public static GameObject GOMP;

//      |> Brolive Components
        public static Actor BACT;
        public static Damageable BDGL;
        //public static Damager BDGR;
        public static Navigator BNGT;
        public static Sensor BMSR;
        public static PlayerLogic PLGS;

//      |> UI Components
        public static Canvas UIPC;
        public static Canvas UIBC;
        public static TMP_Text UIBT;
        public static Bar UIBH;


//------Inspector Refs-------------------------------------
        [Header("Serialized Refs")]
        [Header("Cali Components")]
        [SerializeField] private F7_Central bossCentral;
        [SerializeField] private F7_StateMachine bossStateMachine;
        [SerializeField] private F7_ActionManager bossActionManager;
        [SerializeField] private F7_UIManager bossUIManager;
        [SerializeField] private F7_EventManager bossEventManager;
        [SerializeField] private F7_AnimManager bossAnimManager;
        [SerializeField] private F7_Phys_SO bossPhysSO;
        [SerializeField] private F7_StateDeterminer bossStateDeterminer;

        [Header("Cali States")]
        [SerializeField] private F7_Attack bossAttack;
        [SerializeField] private F7_Idling bossIdling;
        [SerializeField] private F7_Chase bossChase;
        [SerializeField] private F7_Defending bossDefending;
        [SerializeField] private F7_Recovering bossRecovering;

        [Header("Boss Game Components")]
        [SerializeField] private GameObject bossMainObj;
        [SerializeField] private Collider bossClawDamagerCollider;
        [SerializeField] private Collider bossSlamDamagerCollider;
        [SerializeField] private Rigidbody bossRigidbody;
        [SerializeField] private NavMeshAgent bossNavMeshAgent;
        [SerializeField] private Animator bossAnimator;
        [SerializeField] private Transform bossProjStart;

        [Header("Action Prefabs")]
        [SerializeField] private GameObject barrierPrefab;
        [SerializeField] private GameObject pillarPrefab;
        [SerializeField] private GameObject rightPunch;
        [SerializeField] private GameObject leftPunch;
        [SerializeField] private GameObject ringObject;
        [SerializeField] private GameObject ringStartPoint;
        [SerializeField] private GameObject ringEndPoint;
        [SerializeField] private GameObject shardPrefab;
        [SerializeField] private GameObject slamObject;
        [SerializeField] private GameObject swipeObject;
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject meleePivot;

        [Header("Brolive Components")]
        [SerializeField] private Actor bossActor;
        [SerializeField] private Damageable bossDamageable;
        //[SerializeField] private Damager bossDamager;
        [SerializeField] private Navigator bossNavigator;
        [SerializeField] private Sensor bossMeleeSensor;
        [SerializeField] private PlayerLogic playerScript;

        [Header("UI Components")]
        [SerializeField] private Canvas playerHealthCanvas;
        [SerializeField] private Canvas enemyHealthCanvas;
        [SerializeField] private TMP_Text bossNameText;
        [SerializeField] private Bar bossHealthBarScript;

    }
}

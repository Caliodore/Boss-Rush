using UnityEngine;

namespace Cali6
{ 
    public class A6_AnimManager : MonoBehaviour
    {
        public static A6_AnimManager Instance;
        public A6_AnimManager() { }
        public Animator bossAnim;
        public Animator mixamoAnimator;

        public AnimTrigger attTrig;
        public AnimTrigger defTrig;
        public AnimTrigger moveTrig;
        public AnimTrigger recovTrig;
        public AnimTrigger barrBrokeTrig;

        public AnimInt attInt;
        public AnimInt defInt;
        public AnimInt moveInt;
        public AnimInt recovInt;

        public AnimBool meleeBool;


        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

        private void Start()
        {
            bossAnim = A6_Brain.Instance.BossAnimator;
            attTrig = new AnimTrigger("AttackStart", bossAnim);
            defTrig = new AnimTrigger("DefenseStart", bossAnim);
            moveTrig = new AnimTrigger("MovementStart", bossAnim);
            recovTrig = new AnimTrigger("RecoveryStart", bossAnim);
            barrBrokeTrig = new AnimTrigger("BarrierBroken", bossAnim);

            attInt = new AnimInt("AttackInt", bossAnim);
            defInt = new AnimInt("DefenseInt", bossAnim);
            moveInt = new AnimInt("MovementInt", bossAnim);
            recovInt = new AnimInt("RecoveryInt", bossAnim);

            meleeBool = new AnimBool("AttMeleeOrRanged", bossAnim);
        }

        private void Update()
        {
            
        }
        
        public void PlayerEnterArena() { mixamoAnimator.SetTrigger("StartEncounter"); }

        public void SetAnimTrigger(string triggerName) { bossAnim.SetTrigger(triggerName); }
        public void SetAnimInt(string intName, int intValue) { bossAnim.SetInteger(intName, intValue); }
        public void SetAnimBool(string boolName, bool boolValue) { bossAnim.SetBool(boolName, boolValue); }
    }

    public class AnimTrigger { 
        Animator targAnim;
        public AnimTrigger(string nameIn, Animator animIn) { triggerName = nameIn; targAnim = animIn; }
        public string triggerName;
        public void SetTrig() { targAnim.SetTrigger(triggerName); }
    }

    public class AnimInt { 
        Animator targAnim;
        public AnimInt(string intName, Animator animIn) { integerName = intName; targAnim = animIn; }
        public string integerName;
        public int intValue;

        public void SetIntVal(int intAmount) { intValue = intAmount; targAnim.SetInteger(integerName, intValue); }
    }

    public class AnimBool { 
        Animator targAnim;
        public AnimBool(string nameIn, Animator animIn) { boolName = nameIn; targAnim = animIn; }
        public string boolName;
        public bool boolState;

        public void SetBool(bool boolValue) { boolState = boolValue; targAnim.SetBool(boolName, boolState); }
    }
}

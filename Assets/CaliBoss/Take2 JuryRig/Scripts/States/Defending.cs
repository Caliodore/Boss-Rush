using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Caliodore;

namespace CaliJR
{ 
    public class Defending : JRState
    {
        public Defending() : base("Defending") { }

        private void Awake()
        {
            
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
        }

        public override void ModifyIncomingDamageByState(Damage dmgIn)
        {
            float dmgOut = dmgIn.amount;
            if(C2JR_Brain.BossSM.currentState.ToString() != "Defending")
            { 
                print("AttackedWhileDefending called while the current state is not set to Defending.");
                return;
            }
            else
            { 
                if(C2JR_Brain.BossBrain.hitsTakenRecently >= C2JR_Brain.BossBrain.hitsUntilBarrierBreak)
                { 
                    C2JR_Brain.BossBrain.ShatterBloodBarrier();
                    return;
                }
                else 
                {
                    float dmgMod = 1 - (C2JR_Brain.BossBrain.defenseIncreaseRate * C2JR_Brain.BossBrain.hitsTakenRecently);
                    dmgOut *= dmgMod;
                    C2JR_Brain.BossBrain.DamageBloodBarrier(dmgOut);
                    return;
                }
            }
        }

        
    }
}
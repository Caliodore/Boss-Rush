using UnityEngine;
using UnityEngine.Events;

namespace Caliodore
{
    namespace States_Phase1
    {
        public class Aggro : Phase1
        {
            public Aggro() : base("Aggro") { }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                if(!clergyBrain.BossAlerted)
                { 
                    clergySM.ChangeState(clergySM.AttP1States["Idle"]);
                }
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
                //Need an interrupt for when a clergy becomes chosen.
                AggroPlayer();
            }

            public void AggroPlayer()
            { 
                if(clergyBrain.isChosen)
                { 
                    ChosenBuffing();
                }
                else
                { 
                    if(!clergyBrain.playerNear)
                    { 
                        clergyBrain.PursuePlayer();
                    }
                    else if(clergyBrain.canAttack)
                    {
                        clergyBrain.AttackPlayer(clergyBrain.clergySO.RecoveryTime);
                    }
                }
            }

            public void ChosenBuffing()
            { 
                
            }
        } 
    }
}

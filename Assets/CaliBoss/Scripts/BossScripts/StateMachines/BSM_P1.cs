using UnityEngine;
using Caliodore;
using Caliodore.States_Phase1;

/// <summary>
/// BossStateMachine child that is specific to Phase 1 of boss fight. <br/>
/// Handles swapping between states and signifying to the BossMain of Phase 1 when to transition.
/// </summary>
public class BSM_P1 : BossStateMachine
{
    //BossMain attachedBM;
    public BSM_P1() : base() { }

    private int currentPhase;
    public override int CurrentPhase { get { return currentPhase;} protected set { currentPhase = value;} }

    //private BossMain attBM;
    //public override BossMain AttachedBM { get { return attBM;} protected set { attBM = value;} }

    private void Start()
    {
        CurrentPhase = 1;
    }
}

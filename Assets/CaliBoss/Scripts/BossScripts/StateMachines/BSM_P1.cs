using UnityEngine;
using Caliodore;
using Caliodore.States_Phase1;

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

using UnityEngine;

namespace Caliodore
{
    public class CaliBossManager : MonoBehaviour
    {
    /* 
     * Hostellus, the Eternal Martyr
     * and The Sacerdotes Cleridatus
--------------------------------------------------------------------------------------------------------------------------------------------------------
     * Necessities for Assignment:
     * } Makes use of a state machine or behaviour tree
     *  - DONT USE ENUM-BASED STATE MACHINE
     *  - Can use: inheritance, component, or animator for example
     * 
     * } 3 Phases (Initial, Mid-Battle, Enraged)
     *  - Simple first phase
     *  - Adds more attacks, 2nd stage
     *  - Ultimate attack/faster moveset
     * 
     * } Attack variety
     *  - At least 1 melee, 1 ranged
     *  - AoE/Multi-stage to make the player move
     *  - Special mechanic/environmental interaction/ultimate attack
     *  - Defensive manuever/punishment
     * 
     * } Telegraphed movements
     *  - Make sure there is a visual component to communicate to the player when and where an attack is happening
     * 
--------------------------------------------------------------------------------------------------------------------------------------------------------
     * Script Purposes and Intended Functionalities:
     *  } Keep track of boss health in relation to phases.
     *  } Managing swapping between phases.
     *  } Communicating between the scripts as a whole*
     *      - Some lower scripts will directly connect and interact, this is moreso a central repository/exchange to help monitor and relay info.
     *  } Centralizing references for other scripts.
     *  } Communication with other managers.
     *      - Objects will still most likely interact directly, but for most larger events and changes, this script will be the means of propagation.
--------------------------------------------------------------------------------------------------------------------------------------------------------
     *
     */
    }
}

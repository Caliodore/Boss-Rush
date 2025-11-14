using UnityEngine;
using Caliodore;
using Caliodore.States_Phase1;
using Caliodore.States_Phase2;
using Caliodore.States_Phase3;

[CreateAssetMenu(fileName = "SO_EnemyValFramework", menuName = "Scriptable Objects/SO_EnemyValFramework")]
public abstract class SO_EnemyValFramework : ScriptableObject
{
    [SerializeField] public int MaxHealth = 100;
    [SerializeField] public int CurrentHealth = 100;
    [SerializeField] public int CurrentPhase = 1;
    [SerializeField] public State CurrentState = new Caliodore.States_Phase1.Entry();

    public virtual int maxHealth { get; protected set; }
    public virtual int currentHealth { get; protected set; }
    public virtual int currentPhase { get; protected set; }
    public virtual State currentState { get; protected set; }

    public SO_EnemyValFramework() 
    {
        maxHealth = 100;
        currentHealth = maxHealth;
        currentPhase = 1;
        currentState = new Caliodore.States_Phase1.Entry();

        MaxHealth = maxHealth;
        CurrentHealth = currentHealth;
        CurrentPhase = currentPhase;
        CurrentState = currentState;
    }

    /*public SO_EnemyValFramework(int phaseInt)
    {
        currentPhase = phaseInt;

        switch(phaseInt)
        { 
            case(1):
                currentState = new Caliodore.States_Phase1.Entry();
                maxHealth = 1000;
                break;

            case(2):
                currentState = new Caliodore.States_Phase2.Entry();
                maxHealth = 2;
                break;

            case(3):
                currentState = new Caliodore.States_Phase3.Entry();
                maxHealth = 30;
                break;
        }
        if(currentState == null)
            currentState = new Caliodore.States_Phase1.Entry();
        currentHealth = maxHealth;
        currentHealth -= phaseInt;
    }   */

    /*//Return methods
    public float HealthPercentage()
    { 
        return (currentHealth/maxHealth);
    }*/
}

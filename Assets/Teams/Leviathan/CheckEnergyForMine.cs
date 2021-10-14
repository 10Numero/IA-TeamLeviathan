using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leviathan;

public class CheckEnergyForMine : Conditional
{

    public SharedFloat minEnergyValue;
    public float spaceshipEnergy;
    public LeviathanController leviathan;
    public BehaviorTree tree;
    public SharedBool checkEnergyShoot;

    public override void OnStart()
    {
        leviathan = tree.GetComponentInParent<LeviathanController>();
        spaceshipEnergy = leviathan.getSpaceship().Energy;
        checkEnergy(minEnergyValue.Value, spaceshipEnergy);
    }

    private void checkEnergy(float requiredEnergy, float actualSpaceshipEnergy)
    {

        if (actualSpaceshipEnergy >= requiredEnergy)
        {
            tree.SetVariableValue("EnergyToDropMine", true);
        }
        else
        {
            tree.SetVariableValue("EnergyToDropMine", false);
        }

    }
}

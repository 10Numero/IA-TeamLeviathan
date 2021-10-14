using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leviathan;

public class CheckEnergyForShockwave : Conditional
{
    public SharedFloat threshold;
    public SharedFloat minEnergyValue;
    private float spaceshipEnergy;
    private LeviathanController leviathan;
    private BehaviorTree tree;

    public override void OnStart()
    {
        tree = gameObject.GetComponentInParent<BehaviorTree>();
        leviathan = tree.GetComponentInParent<LeviathanController>();
        spaceshipEnergy = leviathan.getSpaceship().Energy;
        checkEnergy( spaceshipEnergy);
    }

    private void checkEnergy( float actualSpaceshipEnergy)
    {
        if (actualSpaceshipEnergy >= minEnergyValue.Value + threshold.Value)
        {
            tree.SetVariableValue("EnergyToShockwave", true);
        }
        else
        {
            tree.SetVariableValue("EnergyToShockwave", false);
        }
    }
}

using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leviathan;

public class CheckEnergyForShockwave : Conditional
{

    public SharedFloat minEnergyValue;
    public float spaceshipEnergy;
    public LeviathanController leviathanController;

    public SharedBool checkShockwave;

    public override void OnStart()
    {
        spaceshipEnergy = leviathanController.getSpaceship().Energy;
        checkShockwave.SetValue(checkEnergy(minEnergyValue.Value, spaceshipEnergy));
    }

    private bool checkEnergy(float requiredEnergy, float actualSpaceshipEnergy)
    {
        if (actualSpaceshipEnergy >= requiredEnergy) return true;
        else return false;
    }
}

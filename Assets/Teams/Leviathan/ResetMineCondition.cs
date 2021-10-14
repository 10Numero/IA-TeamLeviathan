using BehaviorDesigner.Runtime.Tasks;

namespace Leviathan
{
    public class ResetMineCondition : Action
    {
        public override void OnStart() => LeviathanController.instance.tree.SetVariableValue("EnergyToShoot", false);
    }
}



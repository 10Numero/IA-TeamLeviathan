using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

namespace Leviathan
{
    public class CheckEnergyForMine : Conditional
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
            checkEnergy(spaceshipEnergy);
        }

        private void checkEnergy(float actualSpaceshipEnergy)
        {

            if (actualSpaceshipEnergy >= minEnergyValue.Value + threshold.Value)
                tree.SetVariableValue("EnergyToDropMine", true);
            else
                tree.SetVariableValue("EnergyToDropMine", false);
        }
    }
}



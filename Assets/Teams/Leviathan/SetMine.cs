using BehaviorDesigner.Runtime.Tasks;

namespace Leviathan
{
    public class SetMine : Action
    {
        public override void OnStart() => LeviathanController.instance.setMineCondition(true);
    }
}
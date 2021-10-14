using BehaviorDesigner.Runtime.Tasks;
using Leviathan;

namespace Leviathan
{
    public class SetShockwave : Action
    {
        public override void OnStart() => LeviathanController.instance.setShockwaveCondition(true);
    }

}

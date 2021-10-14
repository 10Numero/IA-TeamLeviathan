using BehaviorDesigner.Runtime.Tasks;
using Leviathan;

public class SetShockwave : Action
{
    public override void OnStart()=> LeviathanController.instance.setShockwaveCondition(true);
}

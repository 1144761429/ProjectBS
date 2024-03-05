using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

// TODO: not implemented
public class Shoot : Action
{

    public override TaskStatus OnUpdate()
    {
        Debug.Log("You're being shot at!");
        return TaskStatus.Success;
    }
}

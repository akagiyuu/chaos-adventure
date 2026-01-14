using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Move to Target", story: "Move [Agent] to [Target]", category: "Action", id: "1dfeadaec42d5856081f333babfaf85d")]
public partial class MoveToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<MovementManager> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnUpdate()
    {
        var entityTransform = Agent.Value.GetComponentInChildren<Transform>();

        Vector3 entity, target;
        entity = entityTransform.position;
        target = Target.Value.transform.position;
        if (Mathf.Abs(entity.x - target.x) <= 0.5f) {
            Agent.Value.Stop();
            return Status.Success;
        }

        var direction = target.x > entity.x ? Vector3.right : Vector3.left;
        Agent.Value.Move(direction);

        return Status.Running;
    }

    protected override void OnEnd()
    {
        Agent.Value.Stop();
    }
}


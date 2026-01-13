using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack enemy", story: "[Agent] attack enemy", category: "Action", id: "383dfbdcf87908ddb95471b496b083d5")]
public partial class AttackEnemyAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    private AttackController combat;

    protected override Status OnStart()
    {
        combat = Agent.Value.GetComponent<AttackController>();

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        combat.Attack();

        return Status.Success;
    }
}


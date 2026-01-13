using System;
using System.Linq;
using Unity.Behavior;
using Unity.Collections;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Move to enemy", story: "Move to nearest enemy within [Radius] from [Agent]", category: "Action", id: "e2a1346c0b886a7d6fdee127e3c13e6e")]
public partial class MoveToNearestEnemyAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Radius;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    private Movement movement;
    private AttackController combat;

    protected override Status OnStart()
    {
        movement = Agent.Value.GetComponent<Movement>();
        combat = Agent.Value.GetComponent<AttackController>();

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        var enemies = Physics2D.OverlapCircleAll(
            Agent.Value.transform.position,
            Radius,
            combat.EnemyLayer
        );
        if (enemies.Length == 0) return Status.Failure;

        var attackPoint = combat.AttackPoint.position;

        var nearest = enemies[0];
        var distance = (attackPoint - enemies[0].transform.position).magnitude;
        for (int i = 1; i < enemies.Length; i++)
        {
            var current = (attackPoint - enemies[i].transform.position).magnitude;
            if (current < distance)
            {
                distance = current;
                nearest = enemies[i];
            }
        }
        var enemy = nearest.transform.position;

        if ((enemy - attackPoint).magnitude <= combat.AttackRadius)
        {
            movement.Stop();
            return Status.Success;
        }

        var direction = enemy.x > attackPoint.x ? Vector3.right : Vector3.left;
        movement.Move(direction);

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}


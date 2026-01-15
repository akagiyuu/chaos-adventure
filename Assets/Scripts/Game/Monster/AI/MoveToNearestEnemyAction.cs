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

    private MovementManager movement;
    private AttackManager combat;

    protected override Status OnStart()
    {
        movement = Agent.Value.GetComponent<MovementManager>();
        combat = Agent.Value.GetComponent<AttackManager>();

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

        Collider2D nearest = null;
        var distance = Mathf.Infinity;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].GetComponent<Stats>().IsDeath) continue;
            var current = (attackPoint - enemies[i].transform.position).magnitude;
            if (current < distance)
            {
                distance = current;
                nearest = enemies[i];
            }
        }
        if (distance == Mathf.Infinity) return Status.Failure;
        var enemy = nearest.transform.position;

        var front = Agent.Value.transform.position + 0.2f * Vector3.right;
        if (!Physics2D.Raycast(front, Vector2.down, 1000f, movement.GroundLayer))
        {
            movement.Stop();
            return Status.Failure;
        }

        if ((enemy - attackPoint).magnitude <= combat.AttackRadius)
        {
            movement.Stop();
            return Status.Success;
        }

        var direction = enemy.x > attackPoint.x ? Vector3.right : Vector3.left;
        movement.Move(direction);

        return Status.Running;
    }
}


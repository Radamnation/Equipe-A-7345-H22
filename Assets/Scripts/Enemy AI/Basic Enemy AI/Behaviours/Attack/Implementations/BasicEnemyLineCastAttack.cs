using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyLineCastAttack : AbstractAttackBehaviour
{
    

    public override void Execute()
    {
        //Physics hit = StaticRayCaster.IsLineCastTouching();
        //if ()

    }

    public override bool IsExecutionValid()
    {
        return base.animConditionBool;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackLogic
{
    void ExecuteAttack(GameObject executer);
    bool CanExecute(GameObject executer);
}

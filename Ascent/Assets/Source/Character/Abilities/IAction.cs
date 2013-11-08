using UnityEngine;
using System.Collections;

public interface IAction 
{
    void Initialise(Character owner);
    void StartAbility();
    void UpdateAbility();
    void EndAbility();
}

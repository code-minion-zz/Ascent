using UnityEngine;
using System.Collections;

public interface IAbility 
{
    void Initialise(Character owner);
    void StartAbility();
    void UpdateAbility();
    void EndAbility();
}

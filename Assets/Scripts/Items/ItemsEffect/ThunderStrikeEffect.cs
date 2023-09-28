using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThunderStrikeEffect", menuName = "Data/Item effect/Thunder strike")]
public class ThunderStrikeEffect : ItemEffect
{
    [SerializeField] GameObject thunderStrikePref;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePref, _enemyPosition.position, Quaternion.identity);
    }
}

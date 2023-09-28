using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathBringerAnimTriggers : EnemyAnimationTriggers
{
    DeathBringer deathBringer => GetComponentInParent<DeathBringer>();

    void Relocate() => deathBringer.FindPosition();

    void MakeInvisible() => deathBringer.fx.MakeTransparent(true);

    void MakeVisible() => deathBringer.fx.MakeTransparent(false);
}

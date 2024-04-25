using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaeBulga : Gun
{
    public GaeBulga()
    {
        bulletCount = 10;
        bulletSpeed = 20f;
        bulletDelay = 0.25f;
        spreadness = 0f;
        bulletsPerShot = 1;
        randomSpeedRange = 1f;
    }

    public override void PlayFireSound()
    {
        SoundManager.instance.PlaySound("gun");
    }
}

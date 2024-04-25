using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UZI : Gun
{
    public UZI()
    {
        bulletCount = 20;
        bulletSpeed = 20f;
        bulletDelay = 0.1f;
        spreadness = 5f;
        bulletsPerShot = 1;
        randomSpeedRange = 1f;
    }

    public override void PlayFireSound()
    {
        SoundManager.instance.PlaySound("AKFire");
    }
}

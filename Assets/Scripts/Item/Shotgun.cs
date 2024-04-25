using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    public Shotgun()
    {
        bulletCount = 10;
        bulletSpeed = 20f;
        bulletDelay = 1.5f;
        spreadness = 7f;
        bulletsPerShot = 10;
        randomSpeedRange = 0.5f;
    }
    public override void PlayFireSound()
    {
        SoundManager.instance.PlaySound("shotgun2");
    }
}

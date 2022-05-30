using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class RocketGun : MonoBehaviour, IWeapon
{
    public Projectile rocketMissile;

    public void Shoot(Transform transform)
    {
        Vector3 transformVector = transform.position;
        transformVector.x -= 0.5f;
        Instantiate(rocketMissile, transformVector, Quaternion.identity);
        transformVector.x += 1.0f;
        Instantiate(rocketMissile, transformVector, Quaternion.identity);
    }
}
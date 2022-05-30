using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class CircleGun : MonoBehaviour, IWeapon
{
    public Mysterytile missilePrefab;
    
    public void Shoot(Transform transform)
    {
        Instantiate(missilePrefab, transform.position, Quaternion.identity);
    }
}
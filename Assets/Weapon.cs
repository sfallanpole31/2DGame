using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon : MonoBehaviour
{
    public float damage = 5.0f;
    public bool isMagic;
    public float FallSpeed ;

    private void Update()
    {
        if (isMagic == true)
        {
            Vector3 movement = Vector3.right * FallSpeed * Time.deltaTime + Vector3.down * Math.Abs(FallSpeed) * Time.deltaTime;
            transform.Translate(movement);

        }

    }

}

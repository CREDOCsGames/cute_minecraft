using PlatformGame.Character.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asf : MonoBehaviour
{
    private void Awake()
    {


        if (Physics.CheckBox(transform.position, transform.lossyScale / 2f, Quaternion.identity, LayerMask.GetMask("Bridge")))
        {
            Debug.Log("asdg");
        }

    }
}

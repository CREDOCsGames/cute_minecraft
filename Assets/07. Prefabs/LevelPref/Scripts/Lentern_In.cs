using Flow;
using NW;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class Lentern_In : MonoBehaviour, IInstance
{
    public DataReader DataReader { get; private set; } = new SystemReader();

    public float MoveSpeed;
    public float delayTime;

    public Transform frontSide;
    public Transform leftSide;
    public Transform rightSide;
    public Transform backSide;

    private bool Bright = false;
    private bool Apear = false;
    private bool Disapear = false;
    private float time;

    public void InstreamData(byte[] data)
    {
        if(data == SystemReader.CLEAR_TOP_FACE) // Å¬¸®¾î.
        {
            Lentern_Active(rightSide);
        }

        if (data == SystemReader.CLEAR_RIGHT_FACE)
        {
            Lentern_Active(rightSide);
        }

        if (data == SystemReader.CLEAR_BOTTOM_FACE)
        {
            Lentern_Active(frontSide);
        }

        if (data == SystemReader.CLEAR_FRONT_FACE)
        {
            Lentern_Active(leftSide);
        }

        if (data == SystemReader.CLEAR_LEFT_FACE)
        {
            Lentern_Active(backSide);
        }

        if (data == SystemReader.CLEAR_BACK_FACE)
        {
            // Todo
        }

        if (data == null) // need => Rotation Data
        {
            Disapear = true;
        }

    }

    public void SetMediator(IMediatorInstance mediator)
    {
        
    }

    private void Update()
    {
        if (Apear)
        {
            if (this.transform.position.y < 2.5f)
            {
                this.transform.position += new Vector3(0, MoveSpeed, 0);
            }
            else if (this.transform.position.y < 2.8f)
            {
                this.transform.position += new Vector3(0, MoveSpeed * 0.5f, 0);
            }
            else
            {
                time += Time.deltaTime;
                if(time > delayTime)
                {
                    Apear = false;
                }
            }
        }

        if(!Apear && Bright)
        {
            GetComponent<Animator>().SetTrigger("Bright");
            Bright = false;
        }

        if (Disapear)
        {
            if (this.transform.position.y > -3.5f)
            {
                this.transform.position += new Vector3(0, -MoveSpeed, 0);
            }
            else
            {
                Disapear = false;
                this.gameObject.SetActive(false);
            }
        }
    }


    private void Lentern_Active(Transform spwanPos)
    {
        this.gameObject.transform.position = spwanPos.position;
        this.gameObject.SetActive(true);

        Apear = true;
        Bright = true;
        time = 0;
    }

}

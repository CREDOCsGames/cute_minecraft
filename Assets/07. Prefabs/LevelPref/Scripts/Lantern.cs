using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    public bool Apear;
    public bool Bright;
    public bool Disapear;
    public float MoveSpeed;

    private void Update()
    {
        if (Apear)
        {
            if (this.transform.position.y < 2.5f)
            {
                this.transform.position += new Vector3(0, MoveSpeed, 0);
            }
            else if(this.transform.position.y < 2.8f)
            {
                this.transform.position += new Vector3(0, MoveSpeed * 0.5f, 0);
            }
            else
            {
                Apear = false;
            }
        }
        if(Bright)
        {
            GetComponent<Animator>().SetTrigger("Bright");
            Bright = false;
        }
        if(Disapear)
        {
            if(this.transform.position.y > -3.5f )
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
    
}

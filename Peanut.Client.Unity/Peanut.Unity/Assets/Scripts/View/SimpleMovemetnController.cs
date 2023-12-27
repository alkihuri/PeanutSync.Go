using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovemetnController : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Input.GetAxis("Horizontal") / 15, Input.GetAxis("Vertical") / 15, 0);
    }
}

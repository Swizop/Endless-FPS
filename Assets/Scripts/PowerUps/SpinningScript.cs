using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningScript : MonoBehaviour
{
    private void FixedUpdate()
    {
        GetComponent<Transform>().Rotate(0, 2, 0);
    }
}

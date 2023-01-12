
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public interface ISaveable//2 metode pt save si load data
{  
    object CaptureState(); //return object cause we dont know what kind of data we save , so basically can store anything
    void RestoreState(object state);
}

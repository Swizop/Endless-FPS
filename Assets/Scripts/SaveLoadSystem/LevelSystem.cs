// using System;
// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// namespace SaveLoadSystem
// {
//     [System.Serializable]
//     public class LevelSystem : MonoBehaviour, ISaveable
//     {
//        // [SerializeField] private int level;
//        // [SerializeField] private int health;
//         //public float[] Position;
//         public int health;
        
        
//         public LevelSystem(PlayerHealth  playerhealth)
//         {
//             health = playerhealth.health;
//             // position = new float[3];
//             // position[0] = player.transform.position.x;
//             // position[1] = player.transform.position.y;
//             // position[2] = player.transform.position.z;
//         }

//         public object CaptureState()
//         {
//             return new SaveData
//             {
//                // level = level,
//                 health = health
//             };
//         }

//         public void RestoreState(object state)
//         {
//             var saveData = (SaveData)state;
//            // level = saveData.level;
//             health = saveData.health;
            
//         }
    
//         [Serializable]
//         private struct SaveData
//         {
//            // public int level;
//             public int health;
//         }
//     }

    
// }

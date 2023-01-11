using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanvas : MonoBehaviour
{
    [SerializeField]
    private Transform _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    /// <summary>
    /// Face ca bara de viata sa fie mereu in directia playerului pe axa X
    /// </summary>
    private void LateUpdate()
    {
        var direction = _player.position - transform.position;
        var angle = Mathf.Atan2(direction.x, direction.z);
        transform.rotation = Quaternion.Euler(0.0f, angle * Mathf.Rad2Deg, 0.0f);
    }
}

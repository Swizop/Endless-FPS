using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PopupScript : MonoBehaviour
{
    public float disappearTime;
    public Color textColor;
    public TextMeshPro textMesh;

    private void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        textMesh.text = 
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<WeaponSystem>()
            .damage.ToString();
        textColor.a = 1;
        textMesh.color = textColor;
        
    }

    void Update()
    {
        float moveSpeedY = 1f;
        transform.position += new Vector3(0, moveSpeedY, 0) * Time.deltaTime;

        disappearTime -= Time.deltaTime;
        if (disappearTime <= 0f)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

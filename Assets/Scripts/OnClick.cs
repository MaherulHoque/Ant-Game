using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClick : MonoBehaviour
{
    [SerializeField]
    private GameObject dieEffect;

    private Vector3 mousePos;

    private void OnMouseDown()
    {
        if (GameManager.instance.isGameOver) return;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Quaternion antRotateValue = gameObject.transform.rotation;

        if(dieEffect != null)
        {
            GameObject hitEffect = Instantiate(dieEffect, new Vector3(mousePos.x, mousePos.y, 0), antRotateValue) as GameObject;
            Destroy(hitEffect, 1f);
        }
        

        if (gameObject.tag == "Ant")
        {
            GameManager.instance.AddScore(10);
        }
        if(gameObject.tag == "Spider")
        {
            GameManager.instance.AddScore(-10);
        }
        
        // Sound Manager
        AudioManager.instance.AntDied();


        Destroy(gameObject);


    }
}

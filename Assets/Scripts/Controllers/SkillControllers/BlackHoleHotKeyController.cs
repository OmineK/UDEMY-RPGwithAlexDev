using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHoleHotKeyController : MonoBehaviour
{
    KeyCode myHotKey;

    BlackHoleController blackHole;
    SpriteRenderer sr;
    TextMeshProUGUI myText;
    Transform myEnemy;


    void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            blackHole.AddEnemyToList(myEnemy);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }

    public void SetupHotKey(KeyCode _myNewHotKey, Transform _myEnemy, BlackHoleController _myBlackHole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myHotKey = _myNewHotKey;
        myEnemy = _myEnemy;
        blackHole = _myBlackHole;

        myText.text = _myNewHotKey.ToString();
    }
}

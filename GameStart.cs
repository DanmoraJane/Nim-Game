using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class GameStart : MonoBehaviour
{

public Button pcFirstBtn;
public Button humanFirstBtn;
public static string firstTurn;

    void Start()
    {
        pcFirstBtn.onClick.AddListener(() => assignPC());
        humanFirstBtn.onClick.AddListener(() => assignHuman());
    }

    void assignPC(){
        firstTurn = "PC";
        gameSceneSwitch();

    }

    void assignHuman(){
        firstTurn = "Human";
        gameSceneSwitch();
    }

    void gameSceneSwitch(){
        SceneManager.LoadScene("GameScene");
    }

}

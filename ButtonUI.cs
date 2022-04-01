using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonUI : MonoBehaviour
{
    [SerializeField] private string nextTurn = "Turn1";
    void NewGameButton(){
        SceneManager.LoadScene(nextTurn);
    }
}

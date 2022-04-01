using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class MiniMaxAlgorithmNim : MonoBehaviour{

public string turnText;
public TMP_Text turnTextTMP;
public TMP_Text amountTextTMP;
public TMP_Text whoWonTextTMP;
public Button[] buttons;
public Button restartBtn;
public Button nextTurnBtn;
public Button mainMenuBtn;
public int[] stickPiles;
public int btnID;
bool maximizing;
public int[] pcMoveValues;
public int allSticks;
public bool onePileTaken;
bool isMinLayer;
public Image[] stickOne;
public Image[] stickTwo;
public Image[] stickThree;

    void Start(){
        for (int i = 0; i < buttons.Length; i++){
            int indexX = i;
            buttons[i].onClick.AddListener(() => playerMove(indexX));
        }
        nextTurnBtn.onClick.AddListener(() => nextTurn());
        restartBtn.onClick.AddListener(() => resetGame());
        mainMenuBtn.onClick.AddListener(() => mainMenu());
        setVariables();
    }

    void resetGame(){
        setVariables();
        updateInfo();
    }

    void setVariables(){
        isMinLayer = false;
        onePileTaken = false;
        stickPiles = new int[3] {3,2,1};
        allSticks = countSticks(stickPiles);
        whoWonTextTMP.enabled = false;
        maximizing = true;
        nextTurnBtn.interactable = false;

        for (int i = 0; i < stickThree.Length; i++){
            stickThree[i].enabled = true;
        }
        for (int i = 0; i < stickTwo.Length; i++){
            stickTwo[i].enabled = true;
        }
        for (int i = 0; i < stickOne.Length; i++){
            stickOne[i].enabled = true;
        }

        for (int i = 0; i < buttons.Length; i++){
            int indexX = i;
            buttons[indexX].GetComponent<Button>().interactable = true;
        }

        if(GameStart.firstTurn == "Human"){
            turnText = "Human";
        }else{
            turnText = "AI";
            computerMove();
        }
        
        turnTextTMP.text = "Turn: " + turnText;
        amountTextTMP.text = "Amount of sticks in a heap: " + allSticks;

    }

    public void updateInfo(){
        int stickThreeLength = 0;
        int stickTwoLength = 0;
        int stickOneLength = 0;
        allSticks = countSticks(stickPiles);
        if (maximizing == true){
            if(GameStart.firstTurn == "Human"){
                turnText = "Human";
            }else{
                turnText = "AI";
            }
        }
        else{
            if(GameStart.firstTurn == "AI"){
                turnText = "AI";
            }else{
                turnText = "Human"; 
            }
        }

        for (int i = 0; i < stickThree.Length; i++){
            if(stickThree[i].enabled == true){
                stickThreeLength++;
            }
        }
        for (int i = 0; i < stickTwo.Length; i++){
            if(stickTwo[i].enabled == true){
                stickTwoLength++;
            }
        }
        for (int i = 0; i < stickOne.Length; i++){
            if(stickOne[i].enabled == true){
                stickOneLength++;
            }
        }

        for (int i = 0; i < stickPiles.Length; i++){
            if(stickThreeLength != stickPiles[0] && stickThree[i].enabled == true){
                stickThree[i].enabled = false;
                stickThreeLength--;
            }
            if(stickTwoLength != stickPiles[1] && stickTwo[i].enabled == true){
                stickTwo[i].enabled = false;
                stickTwoLength--;
            }
            if(stickOneLength != stickPiles[2] && stickOne[i].enabled == true){
                stickOne[i].enabled = false;
                stickOneLength--;
            }
        }

        turnTextTMP.text = "Turn: " + turnText;
        amountTextTMP.text = "Amount of sticks in a heap: " + allSticks;
    }

    void Update(){
        if(maximizing == false){
            computerMove();
            if(!gameOver()){
                nextTurn();
            }else{
                nextTurnBtn.interactable = false;
                whoWonTextTMP.enabled = true;
                whoWonTextTMP.text = "PC has won!";
            }
        }else{
            if(gameOver()){
                nextTurnBtn.interactable = false;
                whoWonTextTMP.enabled = true;
                whoWonTextTMP.text = "You won!";
            }
        }
    }

    public void playerMove(int btnID){
        nextTurnBtn.interactable = true;
        onePileTaken = false;
        if(btnID == 0){

            if (stickPiles[0] != 0){
                stickPiles[0] = stickPiles[0] - 1;
                buttons[1].GetComponent<Button>().interactable = false;
                buttons[2].GetComponent<Button>().interactable = false;
                if (stickPiles[0] == 0){
                    buttons[0].GetComponent<Button>().interactable = false; 
                }
            }
        }else if(btnID == 1){
            if (stickPiles[1] != 0){
                stickPiles[1] = stickPiles[1] - 1;
                buttons[0].GetComponent<Button>().interactable = false;
                buttons[2].GetComponent<Button>().interactable = false;
                if (stickPiles[1] == 0){
                    buttons[1].GetComponent<Button>().interactable = false; 
                }
            }

        }else if(btnID == 2){
            if (stickPiles[2] != 0){
                stickPiles[2] = stickPiles[2] - 1;
                buttons[0].GetComponent<Button>().interactable = false;
                buttons[1].GetComponent<Button>().interactable = false;
                if (stickPiles[2] == 0){
                    buttons[2].GetComponent<Button>().interactable = false; 
                }
            }
        }
        updateInfo();
    }

    public void computerMove(){
        int[] chosenMove = evaluatePCMove(stickPiles);
        if (chosenMove[0] > 0 || chosenMove[1] > 0){
            stickPiles[chosenMove[0]] = stickPiles[chosenMove[0]] - chosenMove[1];
            for (int k = 0; k < stickPiles.Length; k++){
                if(stickPiles[k] == 0){
                    buttons[k].GetComponent<Button>().interactable = false;
                }
            }
            onePileTaken = true;
            nextTurnBtn.interactable = true;
            updateInfo();
        }
    }

    public void disableButtons(){
        for (int k = 0; k < buttons.Length; k++){
            buttons[k].GetComponent<Button>().interactable = false;
        }
    }

    public void mainMenu(){
        SceneManager.LoadScene("StartScene");
    }

    public void nextTurn(){
        maximizing = !maximizing;
        nextTurnBtn.interactable = !maximizing;
        for (int k = 0; k < buttons.Length; k++){
            if(stickPiles[k] != 0){
                buttons[k].GetComponent<Button>().interactable = true;
            } 
        }
    }
    
    public int toScore(int[] stickPiles){
             if(stickPiles[0] == 1 && stickPiles[1] == 0 && stickPiles[2] == 0){
                if(isMinLayer == true){
                    return 200;
                }else if (isMinLayer == false){
                    return -200;
                }
            }

            if(stickPiles[1] == 1 && stickPiles[0] == 0 && stickPiles[2] == 0){
                if(isMinLayer == true){
                    return 200;
                }else if (isMinLayer == false){
                    return -200;
                }
            }

            if(stickPiles[2] == 1 && stickPiles[0] == 0 && stickPiles[1] == 0){
                if(isMinLayer == true){
                    return 200;
                }else if (isMinLayer == false){
                    return -200;
                }
            }
 
        return 0;
    }
    
    //minimaksa algoritms
    public int minimax(int[] stickPiles, int depth, bool maximizing){
        isMinLayer = !isMinLayer;
        //novertejuma funkcija (kurs speletajs uzvares - HUMAN vai PC)
        int scoreNow = toScore(stickPiles);
        if (scoreNow == 200) return scoreNow;
        if (scoreNow == -200) return scoreNow;

        //MAX speletaja gajiens
        if (maximizing){
            int bestScore = -500;
            for (int k = stickPiles.Length - 1; k >= 0; k--){
                //var iznemt no 1 lidz 3 objektiem no rindas
                for(int z = 3; z > 0; z--){
                    if (stickPiles[k] >= z) {
                        stickPiles[k] = stickPiles[k] - z;
                        scoreNow = minimax(stickPiles, depth + 1, false);
                        bestScore = Math.Max(bestScore, scoreNow);
                        stickPiles[k] = stickPiles[k] + z;
                    }
                }
            }
            return bestScore;
        //MIN speletaja gajiens
        }else{
            int bestScore = 500;
            for (int k = stickPiles.Length - 1; k >= 0; k--){
                for(int z = 3; z > 0; z--){
                    if (stickPiles[k] >= z){
                        stickPiles[k] = stickPiles[k] - z;
                        scoreNow = minimax(stickPiles, depth + 1, true);
                        bestScore = Math.Min(bestScore, scoreNow);
                        stickPiles[k] = stickPiles[k] + z;
                    }
                }
            }
            return bestScore;
        }
    }

    //datora gajiena funkcija
    public int[] evaluatePCMove(int[] stickPiles){
        int bestMoveValue = -500;
        int[] pcMoveValues = new int[2] {-1, -1};
        int stickCountNow = countSticks(stickPiles);
        for (int i = stickPiles.Length - 1; i >= 0; i--){
            for(int j = 3; j > 0; j--){
                if (stickPiles[i] >= j && onePileTaken == false){
                    if(j != countSticks(stickPiles)){
                        stickPiles[i] = stickPiles[i] - j;
                        int currentMove = minimax(stickPiles, 0, false);
                        stickPiles[i] = stickPiles[i] + j;
                        if(currentMove > bestMoveValue){
                            pcMoveValues[0] = i;
                            pcMoveValues[1] = j;
                            bestMoveValue = currentMove;
                        }   
                        //Debug.Log(currentMove + "<- current move value. && best Value ->" + bestMoveValue);
                    }
                }
            }
        }
        
        return pcMoveValues;
    }

    public bool gameOver(){
        if (allSticks > 1){
            return false;
        }else{
            disableButtons();
            return true;
        }
    }

    public int countSticks(int[] stickPiles){
    int allSticks = 0;
        for(int j = 0; j < stickPiles.Length; j++){
            allSticks = allSticks+stickPiles[j];
        }
    return allSticks;
    }

}

    

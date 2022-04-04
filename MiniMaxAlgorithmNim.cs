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
public bool onePileTaken;
bool isMinLayer;
public Image[] stickOne;
public Image[] stickTwo;
public Image[] stickThree;
public int allSticks;
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
        isMinLayer = true;
        onePileTaken = false;
        stickPiles = new int[3] {3,2,1};
        int allSticks = countSticks(stickPiles);
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
            turnText = "You start first!";
        }else{
            turnText = "Computer has started first!";
            maximizing = !maximizing;
            computerMove();
        }
        
        turnTextTMP.text = turnText;
        updateInfo();

    }

    public void updateInfo(){
        int stickThreeLength = 0;
        int stickTwoLength = 0;
        int stickOneLength = 0;
        int allSticks = countSticks(stickPiles);


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

        amountTextTMP.text = "Amount of sticks in a heap: " + allSticks;
    }

    void Update(){
    if(gameOver()){
        if(maximizing == true && GameStart.firstTurn == "PC" || maximizing == false && GameStart.firstTurn == "Human"){
            nextTurnBtn.interactable = false;
            whoWonTextTMP.enabled = true;
            whoWonTextTMP.text = "PC has won!";
        }else if (maximizing == false && GameStart.firstTurn == "PC" || maximizing == true && GameStart.firstTurn == "Human") {
            nextTurnBtn.interactable = false;
            whoWonTextTMP.enabled = true;
            whoWonTextTMP.text = "You won!";
        }
    }
    
        if(maximizing == false){
            computerMove();
            if(!gameOver()){
                nextTurn();
            } 
        }

    }

    public void playerMove(int btnID){
        nextTurnBtn.interactable = true;
        //atjauno gājiena mainīgo
        onePileTaken = false;
        //pārbauda, kuras kaudzes poga tika nospiesta un attiecīgi veic izmaiņas spēles laikumā un tā grafiskos komponentos
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
        //atjauno spēles informāciju
        updateInfo();
    }

    public void computerMove(){
        //piešķir datora gājienu pie mainīga
        int[] chosenMove = chooseComputerMove(stickPiles);
        //pārbauda, lai gājieni ir derīgi (> 0)
        if (chosenMove[0] > 0 || chosenMove[1] > 0){
            //pielieto gājienu vērtības spēles laukumā
            stickPiles[chosenMove[0]] = stickPiles[chosenMove[0]] - chosenMove[1];
            //padara gājienu pogas neaktīvas, ja kaudze ir = 0
            for (int k = 0; k < stickPiles.Length; k++){
                if(stickPiles[k] == 0){
                    buttons[k].GetComponent<Button>().interactable = false;
                }
            }
            //mainīgais, kas rāda, ka dators jau ir veicis gajienu
            onePileTaken = true;
            nextTurnBtn.interactable = true;
            //atjauno saskarnes informāciju par spēles laukumu
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
    
    //heiristiska novertejuma funkcija
    public int toScore(int[] stickPiles){
        //hieristiskais novērtējums
        int totalScoreValue = 0;

        //cikls, lai atrastu num summu
        int nimSum = stickPiles[0];
        for (int i = 1; i < stickPiles.Length; i++)
            nimSum ^= stickPiles[i];

        //novērtējums virstonēm, izmantojot šīs virsotnes nim summu
            //papildnosācījums, lai nesajauktu nimSum = 0 ar virsotni, kas vienmēr noved pie zaudējuma (vai uzvaras), kad kaudzes konfigurācija ir (1 1 0, 1 0 1, 0 1 1)
            if(nimSum == 0 && countSticks(stickPiles) != 2){
                if(isMinLayer == false){
                    totalScoreValue = totalScoreValue - 50;
                }else if (isMinLayer == true){
                    totalScoreValue = totalScoreValue + 50;
                }
            //papildnosācījums, lai nesajauktu nimSum = 1 ar uzvaras virsotni
            }else if (nimSum == 1 && countSticks(stickPiles) > 1){
                if(isMinLayer == false){
                    totalScoreValue = totalScoreValue - 25;
                }else if (isMinLayer == true){
                    totalScoreValue = totalScoreValue + 25;
                }
            }else if (nimSum > 1){
                if(isMinLayer == false){
                    totalScoreValue = totalScoreValue - 5;
                }else if (isMinLayer == true){
                    totalScoreValue = totalScoreValue + 5;
                }
            }
        
            //novērtējums strupceļa virstonēm
             if(stickPiles[0] == 1 && stickPiles[1] == 0 && stickPiles[2] == 0){
                if(isMinLayer == true){
                   totalScoreValue = totalScoreValue + 200;
                }else if (isMinLayer == false){
                    totalScoreValue = totalScoreValue - 200;
                }
            }

            if(stickPiles[1] == 1 && stickPiles[0] == 0 && stickPiles[2] == 0){
                if(isMinLayer == true){
                    totalScoreValue = totalScoreValue + 200;
                }else if (isMinLayer == false){
                   totalScoreValue = totalScoreValue - 200;
                }
            }

            if(stickPiles[2] == 1 && stickPiles[0] == 0 && stickPiles[1] == 0){
                if(isMinLayer == true){
                    totalScoreValue = totalScoreValue + 200;
                }else if (isMinLayer == false){
                    totalScoreValue = totalScoreValue - 200;
                }
            }

        return totalScoreValue;
    }
    
    //minimaksa algoritms
    public int minimax(int[] stickPiles, int depth, bool maximizing){
        //heiristiska novertejuma funkcija
        int scoreNow = toScore(stickPiles);
        
        //ja ir strupceļa virstone, beidz algoritmu un atgriež vērtību
        if (scoreNow == 200) return scoreNow;
        if (scoreNow == -200) return scoreNow;
        //ja dziļums ir 0, tad beidz algoritmu - t.i. neturpina meklēt vērtības dziļākā koka līmēnī un izdod labāko vertību no šī līmeņa
        if (depth == 0) return scoreNow;
        
        //līmeņu pārslēgšana (ir min vai max spēlētāja līmenis)
        isMinLayer = !isMinLayer;

        //MAX speletaja  gajiens
        if (maximizing){
            int bestScore = -500;
            for (int k = stickPiles.Length - 1; k >= 0; k--){
                //var iznemt no 1 lidz 3 objektiem no rindas
                for(int z = 3; z > 0; z--){
                    //objektu skaits kaudzē nedrīkst būt zemāks par izņemto objektu skaitu
                    if (stickPiles[k] >= z) {
                        //tiek veikts gājiens
                        stickPiles[k] = stickPiles[k] - z;
                        //gājiens tiek novērtēts
                        scoreNow = minimax(stickPiles, depth + 1, false);
                        //tiek saglābāta vislābāka gājiena vērtība
                        bestScore = Math.Max(bestScore, scoreNow);
                        //gājiens tiek atgriezts (lai spēles laukums nemainītos)
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
    public int[] chooseComputerMove(int[] stickPiles){
        //tiek definēts vislabāka gājiena novērtējuma mainīgais
        int bestMoveValue = -500;
        //tiek definēts vislabāka gājiena mainīgais
        int[] pcMoveValues = new int[2] {-1, -1};
        //tiek saskaitīti visi objekti kaudzē
        int stickCountNow = countSticks(stickPiles);
        for (int i =  stickPiles.Length - 1; i >= 0; i--){
            for(int j = 3; j > 0; j--){
                //tiek pārbaudīts, vai dators jau nav veicis gājienu
                if (stickPiles[i] >= j && onePileTaken == false){
                    //tiek pārbaudīts, vai visu objektu skaits nav vienāds ar izņēmto objektu skaitu, lai beigās objektu skaits nebūtu 0
                    if(j != countSticks(stickPiles)){
                        //tiek veikts gājiens
                        stickPiles[i] = stickPiles[i] - j;
                        //tiek veikts gājiena novērtējums
                        int currentMove = minimax(stickPiles, 0, false);
                        //tiek atgriezts gājiens
                        stickPiles[i] = stickPiles[i] + j;
                        //Debug.Log("Current move: " + currentMove + " best move " + bestMoveValue);
                        //tiek salīdzināta tagadēja gājiena vērtība ar vislabāko gājiena vērtību
                        if(currentMove >= bestMoveValue){
                            //tiek piešķirtas gājiena vērtības, kur i ir kaudzes numurs un j ir objektu skaits, kas tiks noņemts no kaudzes
                            pcMoveValues[0] = i;
                            pcMoveValues[1] = j;
                            bestMoveValue = currentMove;
                        }   
                    }
                }
            }
        }
        //tiek atgrieztas gājiena vērtības, lai veiktu gājienu
        return pcMoveValues;
    }

    public bool gameOver(){
        if (countSticks(stickPiles) > 1){
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

    

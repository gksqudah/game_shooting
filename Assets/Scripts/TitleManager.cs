using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

    public UILabel NameLabel;               //유저의 이름이 입력될 라벨의 텍스트값을 uilabel 타입의 변수 namelabel로 선언.
    public GameObject BestDate;             //비활성화 된 bestdata 활성화 시켜주기 위해 gameobject 타입의 변수 선언.
    public UILabel BestUserDate_Label;      //활성화된 오브젝트의 uilabel 컴포넌트에 접근하기 위해 변수 선언.

    void GoPlay()       //Start로 하면 시점함수와 겹치므로 goplay라 정함.
    {
        if(NameLabel.text == "Type your name")
        {
            return;                                 //네임라벨 텍스트를 입력하지 않았으면 이 함수가 가진 다른 기능들은 동작하지 않게 됨.
        }

        //username 이라는 string 타입의 데이터베이스에 namelabel에 있는 text 값을 저장하기.
        PlayerPrefs.SetString("UserName", NameLabel.text);

        //씬 이동하기.
        Application.LoadLevel("MainPlay");
      
    }

    void BestScore()
    {
        //Label에 베스트 플레이어의 이름과 최고점수 표기하기.
        BestUserDate_Label.text = string.Format(
            "{0}:{1:N0}",
            PlayerPrefs.GetString("BestPlayer"),
            PlayerPrefs.GetFloat("BestScore"));
        //여러개의 숫자들을 하나의 텍스트에 넣고 싶을때는 {0},{1},{2}와 같이 대괄호 안에 숫자들을 넣어 string.Format("요부분", ```,```)에 넣어준다. 
        //코딩시 플레이어의 이름:점수 형식으로 label에 표기된다.

        //아직까지 아무도 기록을 세우지 않은 경우가 아니라면.
        if(BestUserDate_Label.text != ":0")
        {
            BestDate.SetActive(true);       
        }       //아무 데이터가 없는 경우 텍스트 내용에 ":0"이라는 값이 들어감.
    }

    void Quit()
    {
        Application.Quit();
    }
}

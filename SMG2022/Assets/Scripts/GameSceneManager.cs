using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    public List<EventData> eventDataList = new List<EventData>();
    public Button redButton;
    public Button greenButton;
    public Button blueButton;
    public Button pauseButton;
    public Button resumeButton;
    public Button returnButton;
    public Transform puzzleParent;
    public GameObject pausePanel;
    public Timer timer;
    public Image goalImage;

    private EventData targetEventData;

    private enum SelectedColor
    {
        None = 0,
        Red = 1 << 0,
        Green = 1 << 1,
        Blue = 1 << 2,
        All = int.MaxValue
    };
    private SelectedColor selectedColor;


    public void ReturnResult(bool isSuccess)
    {
        if (isSuccess)
        {
            GameMgr.In.clearedEvent[GameMgr.In.EventNumber] = isSuccess;
        }

        SceneManager.LoadScene("MoveScene");
    }

    private void Awake()
    {
        var eventData = eventDataList.Find(x => x.eventNumber == GameMgr.In.EventNumber);
        targetEventData = Instantiate(eventData, puzzleParent);
        foreach (var piece in targetEventData.pieceDataList)
        {
            piece.button.onClick.AddListener(delegate { SetPuzzleColor(piece); });
        }
        redButton.onClick.AddListener(OnClickRedButton);
        greenButton.onClick.AddListener(OnClickGreenButton);
        blueButton.onClick.AddListener(OnClickBlueButton);
        pauseButton.onClick.AddListener(OnClickPauseButton);
        resumeButton.onClick.AddListener(OnClickResumeButton);
        returnButton.onClick.AddListener(OnClickReturnButton);

        goalImage.sprite = targetEventData.goalSprite;
        timer.StartTimer(this, targetEventData.limitTime);
    }

    private void OnClickPauseButton()
    {
        pausePanel.SetActive(true);
        timer.PauseTimer();
    }

    private void OnClickResumeButton()
    {
        pausePanel.SetActive(false);
        timer.ResumeTimer();
    }

    private void OnClickReturnButton()
    {
        ReturnResult(false);
    }

    private void OnClickRedButton()
    {
        // TestCode
        ReturnResult(true);
        return;
        SetColorButtonColor(redButton, SelectedColor.Red);
    }

    private void OnClickGreenButton()
    {
        SetColorButtonColor(greenButton, SelectedColor.Green);
    }

    private void OnClickBlueButton()
    {
        SetColorButtonColor(blueButton, SelectedColor.Blue);
    }

    private void SetColorButtonColor(Button btn, SelectedColor color)
    {
        bool isSelected = (selectedColor & color) != 0;
        var currentColor = btn.image.color;

        if (isSelected)
        {
            selectedColor &= ~color;
            currentColor.a = 0.5f;
        }
        else
        {
            selectedColor |= color;
            currentColor.a = 1;
        }

        btn.image.color = currentColor;
    }

    private void SetPuzzleColor(PieceData selectedPiece)
    {
        selectedPiece.button.image.color = GetColor(selectedPiece.button);

        foreach (var piece in selectedPiece.connectedPieceList)
        {
            piece.button.image.color = GetColor(piece.button);
        }
    }

    private Color GetColor(Button targetButton)
    {
        if (selectedColor == SelectedColor.None)
        {
            return Color.black;
        }

        var btnColor = targetButton.image.color;
        bool isRed = ((selectedColor & SelectedColor.Red) != 0) || btnColor.r != 0;
        bool isGreen = ((selectedColor & SelectedColor.Green) != 0) || btnColor.g != 0;
        bool isBlue = ((selectedColor & SelectedColor.Blue) != 0) || btnColor.b != 0;
        Color result = new Color(
            isRed ? 1 : 0,
            isGreen ? 1 : 0,
            isBlue ? 1 : 0
        );

        return result;
    }

    [ContextMenu("Test")]
    public void Test()
    {
        Debug.Log("Clicked");
    }
}

using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LevelStartUpUtility : MonoBehaviour
{
    private AudioManager audioManager = null;
    private Image levelLogo = null;
    private GameObject checkMark_Tutorial = null;
    private Vector3 checkMarkSize = new Vector3(625f, 625f, 1f);
    private GameObject objectiveMessageObj = null;
    private float objectiveMessageScale = 0.6f;
    [SerializeField] private float tweeningTime = 0.5f;
    [SerializeField] private string objectiveDescription = null;
    [SerializeField] private Sprite[] objectiveImages = null;
    [SerializeField] private string BGMName = null;

    private Coroutine coroutine = null;

    private void Awake()
    {
        audioManager = GameObject.Find("[AudioManager]").GetComponent<AudioManager>();
        Transform eventDrivenCanvas = GameObject.Find("[EventDrivenCanvas]").transform;
        checkMark_Tutorial = eventDrivenCanvas.GetChild(0).gameObject;
        levelLogo = eventDrivenCanvas.GetChild(2).GetComponent<Image>();
        objectiveMessageObj = eventDrivenCanvas.GetChild(5).gameObject;
    }

    private void Start()
    {
        SetCurrentBGM();
    }

    public void SetCurrentBGM()
    {
        // 3 was subtracted from indexNum : logoMenu, TitleMenu, TutorialScene
        int levelIndex = SceneManager.GetActiveScene().buildIndex - 3;
        switch (levelIndex % 5)
        {
            // CatInvasion Levels
            case 0:
                this.BGMName = "[OTR]_CatInvasion_01";
                break;
            case 1:
                this.BGMName = "[OTR]_CatInvasion_03";
                break;
            case 2:
                this.BGMName = "[OTR]_CatInvasion_02";
                break;
            case 3:
                this.BGMName = "[OTR]_CatInvasion_04";
                break;
            // CatTower Levels
            case 4: 
                if(levelIndex == 7)
                {
                    this.BGMName = "[OTR]_CatTower_01";
                }
                else if(7 < levelIndex && levelIndex < 23)
                {
                    this.BGMName = "[OTR]_CatTower_02";
                }
            // Final Level
                else if (levelIndex == 27)
                {
                    this.BGMName = "[OTR]_CatInvasion_05";
                }
                break;
        }
        audioManager.PlayBGM(this.BGMName);
    }

    public void SetCurrentBGM(string name)
    {
        audioManager.PlayBGM(name);
    }

    public void DisplayCheckMark()
    {
        StartCoroutine(DisplayEventMarkObject(checkMark_Tutorial, checkMarkSize, 0.7f, "MenuClick", null));
    }

    public void PopUpObjectiveMark(ObjectSpawnManager.ModeName modeName)
    {
        if (modeName == ObjectSpawnManager.ModeName.CatHouseFalling)
        {
            objectiveMessageObj.transform.GetChild(4).GetComponent<Image>().sprite = objectiveImages[0];
        }
        else if(modeName == ObjectSpawnManager.ModeName.DjClimbing)
        {
            objectiveMessageObj.transform.GetChild(4).GetComponent<Image>().sprite = objectiveImages[1];
        }
        StartCoroutine(DisplayEventMarkObject(objectiveMessageObj, objectiveMessageScale, 2f, "ObjectiveMenuPopUp", "ObjectiveMenuDragDown"));
    }

    public void PopUpObjectiveMark()
    {
        objectiveMessageObj.transform.GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1172f, 490f);
        objectiveMessageObj.transform.GetChild(4).gameObject.SetActive(false);
        StartCoroutine(DisplayEventMarkObject(objectiveMessageObj, objectiveMessageScale, 2f, "ObjectiveMenuPopUp", "ObjectiveMenuDragDown"));
    }

    public IEnumerator DisplayLogo(float imageDisplayTime)
    {
        audioManager.PlaySFX("CoinUp");
        levelLogo.GetComponent<RectTransform>().LeanMoveLocalY(0f, 0.4f).setEaseOutCubic();
        LeanAlpha(levelLogo, 1, 0.5f);
        yield return new WaitForSeconds(imageDisplayTime);
        levelLogo.GetComponent<RectTransform>().LeanMoveLocalY(75f, 0.4f).setEaseInOutBack();
        LeanAlpha(levelLogo, 0, 0.3f);
        yield break;
    }

    private LTDescr LeanAlpha(Image image, float to, float time)
    {
        Color color = image.color;
        LTDescr tween = LeanTween
            .value(image.gameObject, color.a, to, time)
            .setOnUpdate((float value) => {
                color.a = value;
                image.color = color;
            });
        return tween;
    }

    private IEnumerator DisplayEventMarkObject(GameObject eventMarkObject, Vector3 popUpSize,float duration, string popSFX, string dragSFX)
    {
        PopUpEventMarkObject(eventMarkObject, popUpSize);
        audioManager.PlaySFX(popSFX);
        yield return new WaitForSeconds(duration);
        DragDownEventMarkObject(eventMarkObject);
        audioManager.PlaySFX(dragSFX);
        yield break;
    }

    private IEnumerator DisplayEventMarkObject(GameObject eventMarkObject, float scaleSize, float duration, string popSFX, string dragSFX)
    {
        PopUpEventMarkObject(eventMarkObject, scaleSize);
        audioManager.PlaySFX(popSFX);
        yield return new WaitForSeconds(duration);
        DragDownEventMarkObject(eventMarkObject, 0);
        audioManager.PlaySFX(dragSFX);
        yield break;
    }

    private void PopUpEventMarkObject(GameObject popUpObject, Vector3 popUpSize)
    {
        LeanTween.init();
        popUpObject.GetComponent<RectTransform>().LeanSize(popUpSize, tweeningTime).setEaseOutCirc().setIgnoreTimeScale(true);
    }

    private void PopUpEventMarkObject(GameObject popUpObject, float scaleSize)
    {
        LeanTween.init();
        popUpObject.GetComponent<RectTransform>().LeanScale(new Vector3(scaleSize, scaleSize, scaleSize), tweeningTime).setEaseOutCirc().setIgnoreTimeScale(true);
    }

    private void DragDownEventMarkObject(GameObject dragDownObject)
    {
        LeanTween.init();
        dragDownObject.GetComponent<RectTransform>().LeanSize(Vector2.zero, tweeningTime).setEaseOutBounce().setIgnoreTimeScale(true);
    }
    private void DragDownEventMarkObject(GameObject dragDownObject, float scaleSize)
    {
        LeanTween.init();
        dragDownObject.GetComponent<RectTransform>().LeanScale(new Vector3(scaleSize, scaleSize, scaleSize), tweeningTime).setEaseOutBounce().setIgnoreTimeScale(true);
    }

}

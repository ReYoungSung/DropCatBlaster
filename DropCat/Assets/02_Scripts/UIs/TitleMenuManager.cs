using Spine.Unity;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TitleMenuManager : MonoBehaviour
{
    private void Awake()
    {
        SetGameStartButton();
    }

    private void SetGameStartButton()
    {
        if(SaveData.Current.playerProfile.maxLevelCompleted == 0)
        {
            this.gameObject.GetComponent<SkeletonGraphic>().Skeleton.SetSkin("Button_NewGame");
        }
        else
        {
            this.gameObject.GetComponent<SkeletonGraphic>().Skeleton.SetSkin("Button_ContinuingGame");
        }
    }
}

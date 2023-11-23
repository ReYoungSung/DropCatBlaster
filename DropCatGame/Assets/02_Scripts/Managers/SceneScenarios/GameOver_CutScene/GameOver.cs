using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameOver : MonoBehaviour
{
    private Chapter chapterGameOver = null;
    public Chapter GetGameOverChapter { get { return chapterGameOver; } }

    private int explosionsCount = 5;
    private int exploded = 0;
    [SerializeField] private GameObject baseHouse = null;
    private PolygonCollider2D houseCollider = null;
    private VFXSpawningManager vFXSpawningManager = null;

    [SerializeField] private GameObject virtualCamera = null;

    private AudioManager audioManager;
    private PauseManager pauseManager = null;

    Timer timer = null;
    private Coroutine coroutine = null;
    private GameObject gamePlayCanvas = null;
    [SerializeField] private GameObject gameOverMenuUI = null;
    private GameObject motherSpaceshipLaser = null;

    private void Awake()
    {
        InitializeCachingReference();
        gameOverMenuUI = GameObject.Find("GameOverMenu");
        gameOverMenuUI.SetActive(false);
    }

    private void InitializeCachingReference()
    {
        chapterGameOver = new Chapter();
        houseCollider = baseHouse.GetComponent<PolygonCollider2D>();
        vFXSpawningManager = baseHouse.GetComponent<VFXSpawningManager>();
        audioManager = GameObject.Find("[AudioManager]").GetComponent<AudioManager>();
        pauseManager = this.GetComponent<PauseManager>();
        gamePlayCanvas = GameObject.Find("[UI] GamePlayCanvas");
        timer = this.GetComponent<Timer>();
        GameObject motherSpaceShip = GameObject.FindGameObjectWithTag("ENEMY_Spaceship");
        if(1 < motherSpaceShip.transform.childCount)
        {
            motherSpaceshipLaser = motherSpaceShip.transform.GetChild(1).gameObject;
            motherSpaceshipLaser.SetActive(false);
        }
    }

    private void Start()
    {
        LoadSequence_GameOver();
        GameOverEventManager.current.onGameOverTriggerEnter += chapterGameOver.StartChapter;
        if(motherSpaceshipLaser != null)
        {
            BoxCollider2D groundCollider = GameObject.Find("[GroundCollider2D]").GetComponent<BoxCollider2D>();
            Vector3 groundPos = groundCollider.bounds.center + new Vector3(0f, groundCollider.bounds.extents.y, 0f);
            float laserLength = groundPos.y - motherSpaceshipLaser.transform.position.y;
            motherSpaceshipLaser.transform.GetChild(0).position = groundPos;
            motherSpaceshipLaser.transform.GetChild(1).GetComponent<LineRenderer>().SetPosition(0, new Vector3(0f, laserLength * 1.22f, 0f));
            motherSpaceshipLaser.transform.GetChild(2).GetComponent<LineRenderer>().SetPosition(0, new Vector3(0f, laserLength * 1.22f, 0f));
        }
    }

    private void LoadSequence_GameOver()
    {
        Sequence gameOverSeq = new Sequence("GameOver");

        Beat beatCache = new Beat();
        beatCache.AddEventsToBeatEnter(
                delegate
                {
                    coroutine = StartCoroutine(GameOverSequence());
                }
            );
        beatCache.AddEventsToBeatRepeat(
                delegate
                {
                    timer.StartTicking();
                    coroutine = StartCoroutine(timer.CoroutineTimer(2f, gameOverSeq.ContinueSequence));
                }
            );
        beatCache.AddEventsToBeatExit(() => pauseManager.GameOverPause(gameOverMenuUI));
        beatCache.AddEventsToBeatExit(() => gamePlayCanvas.SetActive(false));

        gameOverSeq.AddBeatToCurrent(beatCache);
        //gameOverSeq.AddBeatToCurrent(beatCache_02);
        chapterGameOver.AddSequenceToCurrent(gameOverSeq);
    }

    public IEnumerator GameOverSequence()
    {
        if(motherSpaceshipLaser != null)
            motherSpaceshipLaser.SetActive(true);
        virtualCamera.GetComponent<CameraConstraint>().enabled = false;
        virtualCamera.GetComponent<CameraFocusOperation>().TargetObject = baseHouse;
        virtualCamera.GetComponent<CameraFocusOperation>().FocusCameraToBaseHouse();
        while (exploded < explosionsCount)
        {
            string explosionName = "Explosion_0" + exploded.ToString();
            float width = houseCollider.bounds.extents.x;
            float height = houseCollider.bounds.extents.y;
            if (exploded < explosionsCount - 1)
            {
                int spawnWidth = Random.Range(2, 137);
                int spawnHeight = Random.Range(-27, -120);
                vFXSpawningManager.ActivateHitEffect(new Vector2(spawnWidth, spawnHeight), exploded);
                yield return new WaitForSeconds(0.2f);
            }
            else if (exploded == explosionsCount - 1)
            {
                int effectIndex = vFXSpawningManager.GetArrayLength - 1;
                vFXSpawningManager.ActivateHitEffect(baseHouse.transform.position, effectIndex - 1);
                vFXSpawningManager.ActivateHitEffect(baseHouse.transform.position, effectIndex);
                baseHouse.SetActive(false);
                if(motherSpaceshipLaser != null)
                    motherSpaceshipLaser.SetActive(false);
                yield return null;
            }
            audioManager.PlaySFX(explosionName, 160f);
            ++exploded;
        }
        audioManager.StopBGM();
        yield break;
    }
}

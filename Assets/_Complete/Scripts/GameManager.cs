using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
namespace Complete
{
    public class GameManager : MonoBehaviour
    {
        //單例(Singleton)變數
        public static GameManager instance;
        //UI元件宣告
        public CanvasGroup menuPanel;
        public CanvasGroup gamingPanel;
        public CanvasGroup missionSuccesPanel;
        public CanvasGroup missionFailPanel;
        //遊戲中各模組宣告
        public CrystalController crystalController;
        public HeroController heroController;
        public BattleFieldController battleFieldController;
        public MoveTargetBehavior moveTargetBehavior;
        public EnemySpawner enemySpawner;
        //音效元件宣告
        public AudioMixerSnapshot pauseSnapshot;
        public AudioMixerSnapshot gamingSnapshot;
        //內部暫存變數
        private GameObject resultPanel;

        private void Awake()
        {
            instance = this;
            //初始關閉所有遊戲控件，等待玩家把AR場景設置好再激活
            menuPanel.interactable = false;
            crystalController.gameObject.SetActive(false);
            heroController.gameObject.SetActive(false);
            enemySpawner.gameObject.SetActive(false);
            Pause();
        }

        public void OnCrystalChargeComplete()
        {
            //殺光所有活著的敵人
            foreach (var rhino in GameObject.FindObjectsOfType<RhinoController>())
            {
                rhino.GetComponent<AttackableBehavior>().Hurt(int.MaxValue);
            }
            resultPanel = missionSuccesPanel.gameObject;
            StartCoroutine(DelayShowGameOver());
        }

        public void OnCrystalDown()
        {
            resultPanel = missionFailPanel.gameObject;
            StartCoroutine(DelayShowGameOver());
        }

        //等待2秒後再開啟結束面板
        private IEnumerator DelayShowGameOver()
        {
            enemySpawner.gameObject.SetActive(false);
            yield return new WaitForSeconds(2f);
            resultPanel.SetActive(true);
        }

        //當已偵測到主場景的AR圖像
        public void BattleFieldReady()
        {
            crystalController.gameObject.SetActive(true);
            heroController.gameObject.SetActive(true);
            moveTargetBehavior.gameObject.SetActive(true);
            enemySpawner.gameObject.SetActive(true);
            menuPanel.interactable = true;
            gamingPanel.gameObject.SetActive(true);
        }

        //當主場景的AR圖像遺失，暫停遊戲並開啟主選單
        public void BattleFieldLost()
        {
            Pause();
            menuPanel.interactable = false;
        }

        //當移動的AR圖像位置發生改變
        public void BroadcastMovePos(Vector3 movePos)
        {
            //如果英雄還未激活，不要對其呼叫
            if (heroController.gameObject.activeInHierarchy == false)
                return;
            heroController.Move(movePos);
        }

        //當玩家確定主場景已經設置完畢，即可開始遊戲
        public void OnStageSetup()
        {
            Resume();
        }

        public void Pause()
        {
            Time.timeScale = 0;
            menuPanel.gameObject.SetActive(true);
            pauseSnapshot.TransitionTo(0);
        }

        public void Resume()
        {
            Time.timeScale = 1;
            menuPanel.gameObject.SetActive(false);
            gamingSnapshot.TransitionTo(0);
        }

        //從新載入場景
        public void Reload()
        {
            SceneManager.LoadScene(0);
        }
    }
}


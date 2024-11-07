using CodeGraph;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : BaseSceneManager
{

    [SerializeField]
    Animator animator;

    [Space]
    [Header("Grafos del Modo de combate")]
    [SerializeField]
    private CodeGraphAsset MoneyGraph;
    
    [Space]
    [SerializeField]
    private CodeGraphAsset ViolenceGraph;
    
    [Space]
    [SerializeField]
    private CodeGraphAsset InfluenceGraph;

    private void Start()
    {
        Init();
        GameManager.Instance.ProvideSoundManager().PlayMenuMusic();
    }
    public void GoToMainGame() {
        GameManager.Instance.ProvideSoundManager().StopMenuMusic();
        GameManager.Instance.ChangeSceneWithAnimation(animator, ScenesNames.MainGameScene);

        //SceneManager.LoadScene(ScenesNames.MainGameScene);
    }
    public void GoToCreditsMenu() {
        GameManager.Instance.ProvideSoundManager().StopMenuMusic();
        GameManager.Instance.ChangeSceneWithAnimation(animator, ScenesNames.CreditsMenuScene);
        
        //SceneManager.LoadScene(ScenesNames.CreditsMenuScene);
    }
    public void GoToBattleRush()
    {
        GameManager.Instance.ProvideSoundManager().StopMenuMusic();
        GameManager.Instance.ChangeSceneWithAnimation(animator, ScenesNames.BattleRushScene);

        //SceneManager.LoadScene(ScenesNames.CreditsMenuScene);
    }

    public void GoToBattleRushSelector(int branch)
    {
        GameManager.Instance.ProvideBrainManager().bCustomGraphForCombatMode = true;
        //Money
        if (branch == 0)
        {
            GameManager.Instance.ProvideBrainManager().nextGraph = MoneyGraph;
        }
        //Violence
        else if(branch == 1)
        {
            GameManager.Instance.ProvideBrainManager().nextGraph = ViolenceGraph;
        }
        //Influence
        else if (branch == 2)
        {
            GameManager.Instance.ProvideBrainManager().nextGraph = InfluenceGraph;
        }

        GameManager.Instance.ProvideSoundManager().StopMenuMusic();
        GameManager.Instance.ChangeSceneWithAnimation(animator, ScenesNames.BattleRushScene);

        //SceneManager.LoadScene(ScenesNames.CreditsMenuScene);
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class CombatCard : MonoBehaviour
{
    [Header("Visual configurations")]
    [SerializeField] private TextMeshPro nameOfCard;
    [SerializeField] protected SpriteRenderer backgroundCombatSprite;
    [SerializeField] protected SpriteRenderer characterSprite;
    [SerializeField] private GameObject inactiveOverlay;

    [Header("Overlay text configurations")]
    [SerializeField] GameObject overlayTextContainer;
    [SerializeField] TextMeshProUGUI overlayTextMesh;

    [Header("Combat stats configurations")]
    [Header("Attack stat")]
    [Header("Unit numbers")]
    [SerializeField] private GameObject attackStatUnitNumberContainer;
    [SerializeField] private Image attackStatUnitNumberImage;
    [Header("Tens numbers")]
    [SerializeField] private GameObject attackStatTensNumberContainer;
    [SerializeField] private Image attackStatTensUnitNumberImage;
    [SerializeField] private Image attackStatTensTensNumberImage;

    [Header("Defense stat")]
    [Header("Unit numbers")]
    [SerializeField] private GameObject defenseStatUnitNumberContainer;
    [SerializeField] private Image defenseStatUnitNumberImage;
    [Header("Tens numbers")]
    [SerializeField] private GameObject defenseStatTensNumberContainer;
    [SerializeField] private Image defenseStatTensUnitNumberImage;
    [SerializeField] private Image defenseStatTensTensNumberImage;

    [Header("HP stat")]
    [Header("Health bar")]
    [SerializeField] private Slider healthBar;
    [SerializeField] protected Image healthBarFill;
    [Header("Current HP")]
    [Header("Unit numbers")]
    [SerializeField] private GameObject currentHpStatUnitNumberContainer;
    [SerializeField] private Image currentHpStatUnitNumberImage;
    [Header("Tens numbers")]
    [SerializeField] private GameObject currentHpStatTensNumberContainer;
    [SerializeField] private Image currentHpStatTensUnitNumberImage;
    [SerializeField] private Image currentHpStatTensTensNumberImage;
    [Header("Total HP")]
    [Header("Unit numbers")]
    [SerializeField] private GameObject totalHpStatUnitNumberContainer;
    [SerializeField] private Image totalHpStatUnitNumberImage;
    [Header("Tens numbers")]
    [SerializeField] private GameObject totalHpStatTensNumberContainer;
    [SerializeField] private Image totalHpStatTensUnitNumberImage;
    [SerializeField] private Image totalHpStatTensTensNumberImage;
    [Header("HP current and total HP separator")]
    [SerializeField] private Image hpPointsSeparator;

    private int healthPoints;
    private int damage = 0;
    private int armor = 0;
    private int initialEnergy = 0;
    private int currentEnergy = 0;
    private int turns = 0;
    protected CombatTypes combatType;

    private string initialText;
    private string superEffectiveText;
    private string notVeryEffectiveText;

    private string leftSwipeWarningText;
    private string rightSwipeWarningText;
    private string topSwipeWarningText;

    protected abstract Color healthBarColor { get; }

    protected CombatCardVisualComposerComponent visualComposerComponent;

    protected abstract void SetCardBackgroundSprite(CombatCardTemplate combatCardTemplate);

    protected abstract void SetCardCharacterSprite(CombatCardTemplate combatCardTemplate);

    protected abstract (Sprite, Sprite) GetCardStatsSprites(int stat);

    protected abstract (Sprite, Sprite) GetCardHpStatsSprites(int stat);

    protected abstract Sprite GetCardHpSeparatorSprite();

    protected abstract void SetUpEnergyPoints(int energyPoints);

    void Awake()
    {
        visualComposerComponent = GetComponent<CombatCardVisualComposerComponent>();
    }

    public void SetDataCard(CombatCardTemplate dataCard)
    {
        nameOfCard.text = dataCard.NameOfCard;
        damage = dataCard.Damage;
        armor = dataCard.Armor;
        turns = dataCard.Turns;
        initialEnergy = CombatUtils.CalculateEnergy(turns);
        currentEnergy = initialEnergy;
        combatType = dataCard.CombatType;

        initialText = dataCard.InitialText;
        superEffectiveText = dataCard.EffectiveText;
        notVeryEffectiveText = dataCard.NonEffectiveText;

        SetCardBackgroundSprite(dataCard);
        SetCardCharacterSprite(dataCard);
        
        SetCardAttackStatsSprites(dataCard.Damage);
        SetCardDefenseStatsSprites(dataCard.Armor);

        SetUpHealthPoints(dataCard);

        SetUpEnergyPoints(currentEnergy);
    }

    void SetUpHealthPoints(CombatCardTemplate combatCardTemplate)
    {
        healthBarFill.color = healthBarColor;
        healthBar.maxValue = combatCardTemplate.HealthPoints;
        healthPoints = combatCardTemplate.HealthPoints;

        SetCardTotalHpStatsSprites(healthPoints);
        SetCardCurrentHpStats(healthPoints);
    }

    void SetCardCurrentHpStats(int hpStat)
    {
        void SetCardCurrentHpStatsSprites(int hpStat)
        {
            SpritesUtils.SetNumberAsSprites(
                unitNumberContainer: currentHpStatUnitNumberContainer,
                tensNumberContainer: currentHpStatTensNumberContainer,
                unitNumberImage: currentHpStatUnitNumberImage,
                tensUnitNumberImage: currentHpStatTensUnitNumberImage,
                tensTensNumberImage: currentHpStatTensTensNumberImage,
                number: hpStat,
                getNumberAsSprite: GetCardHpStatsSprites
            );
        }

        healthBar.value = hpStat;
        SetCardCurrentHpStatsSprites(hpStat);
    }

    void SetCardTotalHpStatsSprites(int hpStat)
    {
        SpritesUtils.SetNumberAsSprites(
            unitNumberContainer: totalHpStatUnitNumberContainer,
            tensNumberContainer: totalHpStatTensNumberContainer,
            unitNumberImage: totalHpStatUnitNumberImage,
            tensUnitNumberImage: totalHpStatTensUnitNumberImage,
            tensTensNumberImage: totalHpStatTensTensNumberImage,
            number: hpStat,
            getNumberAsSprite: GetCardHpStatsSprites
        );
    }

    void SetCardDefenseStatsSprites(int defenseStat)
    {
        SpritesUtils.SetNumberAsSprites(
            unitNumberContainer: defenseStatUnitNumberContainer,
            tensNumberContainer: defenseStatTensNumberContainer,
            unitNumberImage: defenseStatUnitNumberImage,
            tensUnitNumberImage: defenseStatTensUnitNumberImage,
            tensTensNumberImage: defenseStatTensTensNumberImage,
            number: defenseStat,
            getNumberAsSprite: GetCardStatsSprites
        );
    }

    void SetCardAttackStatsSprites(int attackStat)
    {
        SpritesUtils.SetNumberAsSprites(
            unitNumberContainer: attackStatUnitNumberContainer,
            tensNumberContainer: attackStatTensNumberContainer,
            unitNumberImage: attackStatUnitNumberImage,
            tensUnitNumberImage: attackStatTensUnitNumberImage,
            tensTensNumberImage: attackStatTensTensNumberImage,
            number: attackStat,
            getNumberAsSprite: GetCardStatsSprites
        );
    }

    public void ReduceEnergy(int energyToReduce)
    {
        currentEnergy = Mathf.Max(currentEnergy - energyToReduce, 0);
        SetUpEnergyPoints(currentEnergy);
    }

    public void RecoverEnergy(int energyToRecover)
    {
        currentEnergy = Mathf.Min(currentEnergy + energyToRecover, initialEnergy);
        SetUpEnergyPoints(currentEnergy);
    }

    public float GetCardWidth()
    {
        return backgroundCombatSprite.bounds.size.x;
    }

    public CombatTypes GetCombatType()
    {
        return combatType;
    }

    public int GetDamage()
    {
        return damage;
    }

    public int GetArmor()
    {
        return armor;
    }

    public void ReduceHealthPoints(int PointsToReduce)
    {
        healthPoints = Mathf.Max(healthPoints - PointsToReduce, 0);
        SetCardCurrentHpStats(healthPoints);
    }

    public int GetHealthPoints()
    {
        return healthPoints;
    }

    public int GetCardCurrentEnergy()
    {
        return currentEnergy;
    }

    public void EnableTopSwipeWarningText()
    {
        overlayTextMesh.text = topSwipeWarningText;
        overlayTextContainer.SetActive(true);
    }

    public void EnableLeftSwipeWarningText()
    {
        overlayTextMesh.text = leftSwipeWarningText;
        overlayTextContainer.SetActive(true);
    }

    public void EnableRightSwipeWarningText()
    {
        overlayTextMesh.text = rightSwipeWarningText;
        overlayTextContainer.SetActive(true);
    }

    public void DisableWarningText()
    {
        overlayTextContainer.SetActive(false);
    }

    public void SetInactiveOverlayActivation(bool isActive)
    {
        inactiveOverlay.SetActive(isActive);
    }

    public int GetCombatTurnsForCard()
    {
        return turns;
    }

    public string GetInitialText()
    {
        return initialText;
    }

    public string GetSuperEffectiveText()
    {
        return superEffectiveText;
    }

    public string GetNotVeryEffectiveText()
    {
        return notVeryEffectiveText;
    }

    public void SetTopSwipeWarningText(string topSwipeWarningText)
    {
        this.topSwipeWarningText = topSwipeWarningText;
    }

    public void SetLeftSwipeWarningText(string leftSwipeWarningText)
    {
        this.leftSwipeWarningText = leftSwipeWarningText;
    }

    public void SetRightSwipeWarningText(string rightSwipeWarningText)
    {
        this.rightSwipeWarningText = rightSwipeWarningText;
    }
}

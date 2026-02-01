using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.GraphicsBuffer;

public enum Team { Adventurer, Enemy }

public abstract class Unit : MonoBehaviour
{
    [SerializeField] protected Team unitTeam; public Team UnitTeam => unitTeam;

    [Header("Base Stats")]
    [SerializeField] protected string unitID; public string UnitID => unitID;
    [SerializeField] protected string unitName; public string UnitName => unitName;

    [SerializeField] protected int STR; // Strength     - Physical Damage
    [SerializeField] protected int INT; // Intelligence - Magic Damage
    [SerializeField] protected int DEX; // Dexterity    - Chance of physical damage used in attack
    [SerializeField] protected int WIS; // Wisdom       - Chance of magic damage used in attack
    [SerializeField] protected int VIT; // Vitality     - Base HP
    [SerializeField] protected int END; public int P_END => END; // Endurance    - Physical damage reduction
    [SerializeField] protected int SPI; public int P_SPI => SPI; // Spirit       - Magic damage reduction
    [SerializeField] protected int AGI; // Agility      - turn order

    [SerializeField] protected int GRO; // Growth       - Rate of improvement or reduction of stats      # Maybe make it change every year and reduce as age goes by
    [SerializeField] protected int age;

    [SerializeField] protected AllSpritesLibrary library; public AllSpritesLibrary Library => library;
    [SerializeField] protected int spriteID; public int SpriteID => spriteID;
    [SerializeField] protected int partyNum;
    [SerializeField] protected int baseStatTotal; public int BaseStatTotal => baseStatTotal;

    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float physicalDamage;
    [SerializeField] protected float magicDamage;

    [SerializeField] protected float outgoingDamage; public float OutgoingDamage => outgoingDamage;
    [SerializeField] protected float aggroWeight; public float AggroWeight => aggroWeight;
    [SerializeField] protected bool isHired = false; public bool IsHired => isHired;
    [SerializeField] protected int hiringPrice; public int HiringPrice => hiringPrice;
    [SerializeField] protected bool isDead = false; public bool IsDead => isDead;
    [SerializeField] protected int speed; 
    [SerializeField] protected int actionCost; public int ActionCost => actionCost;
    [SerializeField] public int nextActionTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void RandomizeStats() { }
    public virtual void CalculateStats()
    {
        maxHealth = VIT;
        currentHealth = maxHealth;
        physicalDamage = STR / 2;
        magicDamage = INT / 2;

        aggroWeight = 100;

        speed = AGI + 300;
        actionCost = 1000 - speed;
        if (actionCost < 100) actionCost = 100;
    }

    // Convert this unit's current stats into a data object
    public UnitSaveData SaveToData()
    {
        return new UnitSaveData
        {
            unitTeam = this.unitTeam,
            unitID = this.unitID,
            unitName = this.unitName,
            STR = this.STR,
            INT = this.INT,
            DEX = this.DEX,
            WIS = this.WIS,
            VIT = this.VIT,
            END = this.END,
            SPI = this.SPI,
            AGI = this.AGI,
            GRO = this.GRO,
            age = this.age,
            spriteID = this.spriteID,
            partyNum = this.partyNum
        };
    }

    // Apply data from a saved object back onto this unit
    public void LoadFromData(UnitSaveData data)
    {
        this.unitTeam = data.unitTeam;
        this.unitID = data.unitID;
        this.unitName = data.unitName;
        this.STR = data.STR;
        this.INT = data.INT;
        this.DEX = data.DEX;
        this.WIS = data.WIS;
        this.VIT = data.VIT;
        this.END = data.END;
        this.SPI = data.SPI;
        this.AGI = data.AGI;
        this.GRO = data.GRO;
        this.age = data.age;
        this.spriteID = data.spriteID;
        this.partyNum = data.partyNum;

        CalculateStats();
    }

    public void IncParty()
    {
        partyNum += 1;
    }
    public void DecParty()
    {
        partyNum -= 1;
    }

    // Combat
    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;

        RefreshMyUI();

        if (currentHealth <= 0) Die();
    }

    public virtual void Die()
    {
        isDead = true;
    }

    public virtual string GetDamageType()
    {
        float totalChance = DEX + WIS;
        float roll = Random.Range(0f, totalChance);
        if (roll <= DEX) return "physical";
        else return "magic";
    }

    public virtual void CalculateOutgoingDamage(float damage, float reduction, float reduction_cap)
    {
        float intendedDamage = damage * (1 - (reduction / 100));
        float cappedDamage = damage * (1 - (reduction_cap / 100));
        if (intendedDamage > cappedDamage) outgoingDamage = intendedDamage;
        else outgoingDamage = cappedDamage;
    }

    public virtual IEnumerator Attack(Unit target)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = target.transform.position;

        // 1. Lunge Forward (Move 90% of the way to the target)
        Vector3 lungeDestination = Vector3.Lerp(startPos, targetPos, 0.9f);
        float elapsedTime = 0f;
        float duration = 0.15f; // Fast lunge

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, lungeDestination, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // --- HIT POINT ---
        // This is where the actual damage happens (mid-animation)
        ExecuteDamageLogic(target);

        // 2. Return to Original Position
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(lungeDestination, startPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos; // Ensure we are exactly back at home
    }

    private void ExecuteDamageLogic(Unit target)
    {
        string damageType = GetDamageType();

        if (damageType == "physical")
            CalculateOutgoingDamage(physicalDamage, target.P_END, 80);
        else if (damageType == "magic")
            CalculateOutgoingDamage(magicDamage, target.P_SPI, 80);

        target.TakeDamage(outgoingDamage);
    }

    private void RefreshMyUI()
    {
        if (unitTeam == Team.Adventurer)
            GetComponent<AdventurerCombatUI>()?.DisplayStats();
        else
            GetComponent<EnemyCombatUI>()?.DisplayStats();
    }
}

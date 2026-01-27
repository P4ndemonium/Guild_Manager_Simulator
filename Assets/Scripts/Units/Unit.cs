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
    [SerializeField] protected string unitID;
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

    [SerializeField] protected int spriteID;
    [SerializeField] protected int squadNum;

    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float physicalDamage;
    [SerializeField] protected float magicDamage;

    [SerializeField] protected float outgoingDamage; public float OutgoingDamage => outgoingDamage;
    [SerializeField] protected float aggroWeight; public float AggroWeight => aggroWeight;
    [SerializeField] protected bool isHired = false; public bool IsHired => isHired;     // A public way for the SaveManager to check the status
    [SerializeField] protected bool isDead = false; public bool IsDead => isDead;
    [SerializeField] protected float speed; public float Speed => speed;

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
        speed = AGI;
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
            squadNum = this.squadNum
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
        this.squadNum = data.squadNum;

        CalculateStats();
    }

    public void IncSquad()
    {
        squadNum += 1;
    }
    public void DecSquad()
    {
        squadNum -= 1;
    }

    // Combat
    public virtual void Attack(Unit target)
    {
        // Damage Reduction Calculation
        string damageType = GetDamageType();
        if (damageType == "physical")
            CalculateOutgoingDamage(STR, target.P_END, 80f);
        else
            CalculateOutgoingDamage(INT, target.P_SPI, 80f);

        target.TakeDamage(outgoingDamage);
    }

    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;

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
        float intendedDamage = STR * (1 - (reduction / 100));
        float cappedDamage = STR * (1 - (reduction_cap / 100));
        if (intendedDamage > cappedDamage) outgoingDamage = intendedDamage;
        else outgoingDamage = cappedDamage;
    }
}

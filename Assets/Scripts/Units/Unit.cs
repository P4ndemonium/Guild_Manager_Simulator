using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public enum Team { Player, Enemy }

public abstract class Unit : MonoBehaviour
{
    [SerializeField] protected Team unitTeam;

    [Header("Base Stats")]
    [SerializeField] protected string unitID;
    [SerializeField] protected string unitName;

    [SerializeField] protected int STR; // Strength     - Physical Damage
    [SerializeField] protected int INT; // Intelligence - Magic Damage
    [SerializeField] protected int DEX; // Dexterity    - Chance of physical damage used in attack
    [SerializeField] protected int WIS; // Wisdom       - Chance of magic damage used in attack
    [SerializeField] protected int VIT; // Vitality     - Base HP
    [SerializeField] protected int END; // Endurance    - Physical damage reduction
    [SerializeField] protected int SPI; // Spirit       - Magic damage reduction
    [SerializeField] protected int AGI; // Agility      - Chance of getting an attack on this units turn

    [SerializeField] protected int GRO; // Growth       - Rate of improvement or reduction of stats      # Maybe make it change every year and reduce as age goes by
    [SerializeField] protected int age;

    [SerializeField] protected int spriteID;
    [SerializeField] protected int squadNum;

    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float physicalDamage;
    [SerializeField] protected float magicDamage;
    [SerializeField] protected float aggroWeight;

    [SerializeField] protected bool isHired = false;
    // A public way for the SaveManager to check the status
    public bool IsHired => isHired;

    protected bool isDead = false;

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
        maxHealth = VIT * 3;
        currentHealth = maxHealth;
        physicalDamage = STR / 2;
        magicDamage = INT / 2;
        aggroWeight = 100;
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
}

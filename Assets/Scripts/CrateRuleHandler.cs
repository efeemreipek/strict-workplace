using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrateRuleHandler : Singleton<CrateRuleHandler>
{
    [SerializeField] private List<CrateRule> currentRulesList = new List<CrateRule>();
    [SerializeField] private CrateRule[] allRules;

    private CrateRuleHandlerUI ui;

    public bool IsGameOver { get; set; }
    public static event Action<CrateRule> OnNewRuleAdded;
    public static event Action<int> OnGameOver;
    public static event Action<int> OnResign;

    private void OnEnable()
    {
        Door.OnDoorInteracted += Door_OnDoorInteracted;
    }
    private void OnDisable()
    {
        Door.OnDoorInteracted -= Door_OnDoorInteracted;
    }
    protected override void Awake()
    {
        base.Awake();

        ui = GetComponent<CrateRuleHandlerUI>();
    }
    private void Start()
    {
        OnNewRuleAdded(currentRulesList[0]);
        ui.AddRuleToList(currentRulesList[0]);
    }

    public bool ValidateAllRules()
    {
        return ValidateAllRules(FindObjectsByType<Crate>(FindObjectsSortMode.None).ToList());
    }
    public bool ValidateAllRules(List<Crate> allCrates)
    {
        foreach(Crate crate in allCrates)
        {
            foreach(CrateRule rule in currentRulesList)
            {
                if(!rule.IsRelevant(crate)) continue;

                if(!rule.Validate(crate, allCrates))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void Door_OnDoorInteracted()
    {
        if(ValidateAllRules())
        {
            AddNewRule();
        }
        else
        {
            IsGameOver = true;
            OnGameOver?.Invoke(currentRulesList.Count);
        }
    }
    private void AddNewRule()
    {
        CrateRule rule = allRules[UnityEngine.Random.Range(0, allRules.Length)];
        if(!currentRulesList.Contains(rule))
        {
            currentRulesList.Add(rule);
            OnNewRuleAdded?.Invoke(rule);
            ui.AddRuleToList(rule);
        }
        else
        {
            AddNewRule();
        }
    }

    public void Resign()
    {
        IsGameOver = true;
        ScreenFadeHandler.Current.FadeIn(() => OnResign?.Invoke(currentRulesList.Count));
        ui.MovePanelOffScreen();
    }
}

using UnityEngine;

public class CrateGenerator : Singleton<CrateGenerator>, IInteractable
{
    [SerializeField] private GameObject cratePrefab;
    [SerializeField] private int baseCrateCount = 5;

    private CrateGeneratorUI ui;
    private int remainingCrateCount;

    public Transform Transform => transform;
    public int RemainingCrateCount => remainingCrateCount;

    protected override void Awake()
    {
        base.Awake();

        remainingCrateCount = baseCrateCount;

        ui = GetComponent<CrateGeneratorUI>();
        ui.UpdateText(remainingCrateCount);
    }
    private void OnEnable()
    {
        ScreenFadeHandler.Current.OnFadeOutComplete += Fade_OnFadeOutComplete;
    }
    private void OnDisable()
    {
        ScreenFadeHandler.Current.OnFadeOutComplete -= Fade_OnFadeOutComplete;
    }

    private void Fade_OnFadeOutComplete()
    {
        remainingCrateCount = baseCrateCount;
        remainingCrateCount += Random.Range(2, 4);
        baseCrateCount = remainingCrateCount;
        ui.UpdateText(remainingCrateCount);
    }

    public bool CanInteract(PlayerInteractor interactor) => interactor.CurrentCarriedObject == null;
    public void OnInteract(PlayerInteractor interactor)
    {
        if(!CanInteract(interactor)) return;
        if(remainingCrateCount <= 0) return;

        GameObject crateGO = Instantiate(cratePrefab, transform.position, Quaternion.identity);
        Crate crate = crateGO.GetComponent<Crate>();

        crate.SetRandomType();

        interactor.PickupObject(crate);

        remainingCrateCount--;
        ui.UpdateText(remainingCrateCount);
    }
}

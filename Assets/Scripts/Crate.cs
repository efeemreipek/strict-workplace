using UnityEngine;

public class Crate : MonoBehaviour, IPickupable
{
    public enum EType
    {
        None,
        Basic,
        Red,
        Green,
        Blue,
        Explosive,
        Heavy,
        Fragile
    }

    [SerializeField] private CrateTypeData data;
    [SerializeField] private EType type;
    [SerializeField] private bool showDebug;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshRenderer quadMeshRenderer;

    private Vector3[] neighbourDirections = new Vector3[8];
    private RaycastHit[] neighbourHits = new RaycastHit[8];
    private EType[] neighbourCrateTypes = new EType[8];

    public EType Type => type;
    public Transform Transform => transform;

    private void Awake()
    {
        InitializeNeighbourDirections();

        quadMeshRenderer.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(showDebug)
        {
            DrawNeighbourRays();
            CheckNeighbourCrates();
            showDebug = false;
        }
    }
    private void InitializeNeighbourDirections()
    {
        neighbourDirections[0] = transform.forward;                                 // FORWARD
        neighbourDirections[1] = (transform.forward + transform.right).normalized;  // FORWARD-RIGHT
        neighbourDirections[2] = transform.right;                                   // RIGHT
        neighbourDirections[3] = (-transform.forward + transform.right).normalized; // BACK-RIGHT
        neighbourDirections[4] = -transform.forward;                                // BACK
        neighbourDirections[5] = (-transform.forward - transform.right).normalized; // BACK-LEFT
        neighbourDirections[6] = -transform.right;                                  // LEFT
        neighbourDirections[7] = (transform.forward - transform.right).normalized;  // FORWARD-LEFT
    }
    private void DrawNeighbourRays()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        for(int i = 0; i < neighbourDirections.Length; i++)
        {
            Debug.DrawRay(rayOrigin, neighbourDirections[i], Color.black);
        }
    }
    private void CheckNeighbourCrates()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        for(int i = 0; i < neighbourDirections.Length; i++)
        {
            if(Physics.Raycast(rayOrigin, neighbourDirections[i], out RaycastHit hit, 1f))
            {
                Crate otherCrate = hit.collider.GetComponentInParent<Crate>();
                if(otherCrate != null)
                {
                    neighbourCrateTypes[i] = otherCrate.Type;
                    neighbourHits[i] = hit;
                }
                else
                {
                    neighbourCrateTypes[i] = EType.None;
                }
            }
            else
            {
                neighbourCrateTypes[i] = EType.None;
            }
        }
    }
    public void SetRandomType()
    {
        type = (EType)Random.Range(1, System.Enum.GetValues(typeof(EType)).Length);
        meshRenderer.material = data.MaterialList[(int)type - 1];

        if(type == EType.Explosive || type == EType.Heavy || type == EType.Fragile)
        {
            quadMeshRenderer.gameObject.SetActive(true);
            quadMeshRenderer.material = data.SymbolMaterialList[(int)type - 5];
        }
    }
    public EType[] GetNeighbourCrateTypes()
    {
        CheckNeighbourCrates();
        return neighbourCrateTypes;
    }

    public bool CanInteract(PlayerInteractor interactor) => interactor.CurrentCarriedObject == null || interactor.CurrentCarriedObject == this;
    public void OnInteract(PlayerInteractor interactor)
    {
        if(!CanInteract(interactor)) return;

        if(interactor.CurrentCarriedObject == this)
        {
            interactor.TryDropObject();
        }
        else
        {
            interactor.PickupObject(this);
        }
    }
    public void OnPickup(PlayerInteractor interactor)
    {
        AudioManager.Current.PlayCrateSFX(0.5f);
    }
    public void OnDrop(Vector3 position, Quaternion rotation)
    {
        AudioManager.Current.PlayCrateSFX(0.5f);
    }
}

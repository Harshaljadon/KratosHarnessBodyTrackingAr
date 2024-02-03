using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FaceProtectionControler : MonoBehaviour
{
    [SerializeField]
    HarnessCaryForwardData _harnessCaryForwardData;
    [SerializeField]
    [Tooltip("The ARHumanBodyManager which will produce body tracking events.")]
    ARFaceManager m_HumanFaceManager;

    /// <summary>
    /// Get/Set the <c>ARHumanBodyManager</c>.
    /// </summary>
    public ARFaceManager humanBodyManager
    {
        get { return m_HumanFaceManager; }
        set { m_HumanFaceManager = value; }
    }

    public GameObject FaceSaftyPrefeb { get => faceSaftyPrefeb; set => faceSaftyPrefeb = value; }

    [SerializeField]
    [Tooltip("Helmat, Eyesafty glass, etc prfeb with ARKitBlendShapeVisualizer & AR face component")]
    GameObject faceSaftyPrefeb;

    [SerializeField]
    FaceSaftyEquipment faceWearEquipment;

    private void Awake()
    {
        m_HumanFaceManager.enabled = false;
        FilterFaceEquipProduct();
        //FilterFaceEquipProduct(SceneManag.Instance.produ);
        m_HumanFaceManager.facePrefab = faceSaftyPrefeb;
        m_HumanFaceManager.enabled = true;

    }

    void FilterFaceEquipProduct()
    {
      
        switch (SceneManag.Instance._currentProductSubCatagory)
        {
            case ProductSubCatagory.EYEGLASS:
                faceSaftyPrefeb = faceWearEquipment.eyeGlass[_harnessCaryForwardData.productItemScriptableIndex];
                break;
            case ProductSubCatagory.HELMET:
                faceSaftyPrefeb = faceWearEquipment.helemit[_harnessCaryForwardData.productItemScriptableIndex];
                break;
            case ProductSubCatagory.MASK:
                faceSaftyPrefeb = faceWearEquipment.mask[_harnessCaryForwardData.productItemScriptableIndex];
                break;
            default:
                break;
        }
    }

    private void OnEnable()
    {
        //if (m_HumanFaceManager != null)
        //{
        //    m_HumanFaceManager.facesChanged += M_HumanFaceManager_facesChanged;
        //}
    }

    //private void M_HumanFaceManager_facesChanged(ARFacesChangedEventArgs obj)
    //{
    //    foreach (var item in obj.added)
    //    {

    //    }

    //    foreach (var item in obj.updated)
    //    {

    //    }

    //    foreach (var item in obj.removed)
    //    {

    //    }
    //}

    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    //private void OnDisable()
    //{
    //    if (m_HumanFaceManager != null)
    //    {

    //    m_HumanFaceManager.facesChanged -= M_HumanFaceManager_facesChanged;
    //    }

    //}
}

[System.Serializable]
public class FaceSaftyEquipment
{
    public List<GameObject> eyeGlass;
    public List<GameObject> helemit;
    public List<GameObject> mask;
}

[System.Serializable]
public enum FaceProductCatagory
{
    eye, helmet, mask

}


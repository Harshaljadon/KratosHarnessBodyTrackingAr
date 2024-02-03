using UnityEngine;

[RequireComponent(typeof(UiClickIdentify))]
public class IdHolder : MonoBehaviour
{
    [SerializeField]
    public NameIdGameObj[] data;
    // Start is called before the first frame update

    private void OnEnable()
    {
        byte length = (byte)data.Length;
        for (int i = 0; i < length; i++)
        {
            data[i].id = data[i].uiObj.GetInstanceID();
        }

    }
 
    
}

using UnityEngine;

public class ItemMenuManager : MonoBehaviour
{
    public static ItemMenuManager instance;

[SerializeField] GameObject equipmentDetails, consumableDetails, miscellaneousDetails, craftingMaterialDetails,
choosingAmount;

[SerializeField] UnityEngine.UI.Button reduceAmountButton, increaseAmountButton, confirmAmountButton, cancelAmountButton;
    // FIXME: Start
    void Start()
    {
        CreateInstance();
    }

    // FIXME: Update
    void Update()
    {

    }

    private void CreateInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
}

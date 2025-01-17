using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new ItemDropper", menuName = "Item/ItemDropper")]
public class ItemDrop : ScriptableObject
{
    [SerializeField] private List<GameObject> dropItems;

    [Header("TimeSet")]
    public float popingTime = .5f;
    public float livingTime = 2f;

    [Header("PopSpeed")]
    public float maxHorizontalPopSpeed = 5f;
    public float minVerticalPopSpeed = 2f;
    public float maxVerticalPopSpeed = 10f;

    public ItemDrop()
    {
        dropItems = new List<GameObject>();
    }

    public void SetItem(List<GameObject> dropItems)
    {
        this.dropItems = dropItems;
    }

    public void Random(Vector3 position, int count = 5)
    {
        for(int i = 0; i< count; i++)
        {
            var rItem = dropItems[CatDown.Random.Get().Next(dropItems.Count)].GetComponent<Item>();

            if(CatDown.Random.Get().Next(100) < rItem.i_Info.chacePercent)
            {
                //rItem.InstantiateItem(position);
                InstantiateItem(rItem.gameObject, position);
            }
        }
    }

    private void InstantiateItem(GameObject itemObject, Vector3 position)
    {
        var dropItem = Instantiate(itemObject, position, Quaternion.identity);
        var dropSetting = dropItem.GetComponent<Item>().i_Info.dropSetting;

        var rand = CatDown.Random.Get();
        var popSpeed = new Vector2(rand.Next(-(int)dropSetting.maxHorizontalPopSpeed, (int)dropSetting.maxHorizontalPopSpeed),
                                   rand.Next((int)dropSetting.minVerticalPopSpeed, (int)dropSetting.maxVerticalPopSpeed));

        dropItem.GetComponent<Rigidbody2D>().AddForce(popSpeed, ForceMode2D.Impulse);
        dropItem.GetComponent<Item>().Invoke("EndPoping", dropSetting.popingTime);
        dropItem.GetComponent<Item>().DestroyItem(dropSetting.livingTime);
    }
}

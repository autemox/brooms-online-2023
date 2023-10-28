using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class AppearanceCategory
{
    public GameObject gameObject;  // The GameObject associated with this model
    public string category;  // The category (e.g., "Eyes")
    public string[] labels;  // The labels within the category (e.g., "eyes blue", "green")

    public AppearanceCategory(GameObject gameObject, string category, string[] labels)
    {
        this.gameObject = gameObject;
        this.category = category;
        this.labels = labels;
    }

    public void ChangeLabel(string newLabel)
    {
        SpriteResolver spriteResolver = gameObject.GetComponent<SpriteResolver>();
        if (spriteResolver != null)
        {
            spriteResolver.SetCategoryAndLabel(category, newLabel);
            spriteResolver.ResolveSpriteToSpriteRenderer();
        }
        else
        {
            Debug.LogWarning("SpriteResolver component not found on associated GameObject.");
        }
    }
}

public class CharacterAppearance : MonoBehaviour
{
    public GameObject headAccessories;
    public GameObject headFeatures;
    public GameObject hairFront;
    public GameObject eyebrows;
    public GameObject eyes;
    public GameObject eyesBack;
    public GameObject mouth;
    public GameObject head;
    public GameObject handLeft;
    public GameObject item;
    public GameObject shoeLeft;
    public GameObject torsoAccessories;
    public GameObject torso;
    public GameObject hairBack;
    public GameObject handRight;
    public GameObject shoeRight;
    public AppearanceCategory[] appearance;

    void Start()
    {
        Dictionary<string, GameObject> gameObjectMap = new Dictionary<string, GameObject>
        {
            {"Head Accessories", headAccessories},
            {"Head Features", headFeatures},
            {"Hair Front", hairFront},
            {"Eyebrows", eyebrows},
            {"Eyes", eyes},
            {"Eyes Back", eyesBack},
            {"Mouth", mouth},
            {"Head", head},
            {"Hand Left", handLeft},
            {"Item", item},
            {"Shoe Left", shoeLeft},
            {"Torso Accessories", torsoAccessories},
            {"Torso", torso},
            {"Hair Back", hairBack},
            {"Hand Right", handRight},
            {"Shoe Right", shoeRight}
        };

        SpriteLibrary spriteLibrary = GetComponent<SpriteLibrary>();
        SpriteLibraryAsset spriteLibraryAsset = spriteLibrary.spriteLibraryAsset;
        List<AppearanceCategory> categoriesList = new List<AppearanceCategory>();
        foreach (var category in spriteLibraryAsset.GetCategoryNames())
        {
            string[] labels = spriteLibraryAsset.GetCategoryLabelNames(category).ToArray(); 
            Debug.Log("[CharacterAppearance Start()] Found category '" + category + "' with labels '" + string.Join(", ", labels) + "'");
            if (gameObjectMap.ContainsKey(category)) categoriesList.Add(new AppearanceCategory(gameObjectMap[category], category, labels));
            else Debug.LogWarning("[CharacterAppearance Start()] Category not found in gameObjectMap: " + category);
        }
        appearance = categoriesList.ToArray();

        // Tests:
        // InvokeRepeating("RandomlyChangeAppearance", 1.0f, 1.0f);

        Invoke("DebugOutSaveArr", 2.0f);  // Call DebugOutSaveArr after a delay to ensure all components are initialized
    }

    void LoadFromSaveArr(string[] saveArr)
    {
        for (int i = 0; i < saveArr.Length; i += 2)
        {
            string category = saveArr[i];
            string label = saveArr[i + 1];
            ChangeAppearance(category, label);
        }
    }

    void DebugOutSaveArr()
    {
        List<string> saveArrList = new List<string>();
        foreach (AppearanceCategory appearanceCategory in appearance)
        {
            saveArrList.Add(appearanceCategory.category);
            SpriteResolver spriteResolver = appearanceCategory.gameObject.GetComponent<SpriteResolver>();
            if (spriteResolver != null)
            {
                saveArrList.Add(spriteResolver.GetLabel());
            }
            else
            {
                saveArrList.Add("UNKNOWN_LABEL");
            }
        }

        string[] saveArr = saveArrList.ToArray();
        Debug.Log("[CharacterAppearance DebugOutSaveArr()] Save String: " + string.Join(", ", saveArr));
    }

    void RandomlyChangeAppearance()
    {
        AppearanceCategory randomCategory = appearance[Random.Range(0, appearance.Length)];
        ChangeAppearance(randomCategory.category, randomCategory.labels[Random.Range(0, randomCategory.labels.Length)]);
    }

    public void ChangeAppearance(string category, string newLabel)
    {
        // Find the AppearanceCategory object that matches the given 'category'
        foreach (AppearanceCategory appearanceCategory in appearance)
        {
            if (appearanceCategory.category == category)
            {
                // Call its ChangeLabel() method to change the sprite label
                Debug.Log("[CharacterAppearance ChangeAppearance()] Changing " + category + " to " + newLabel);
                appearanceCategory.ChangeLabel(newLabel);
                return;
            }
        }
        Debug.LogWarning("Category not found: " + category);
    }
}
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
    [SerializeField] private string sortingLayerName="Default";

    public GameObject hairFront;
    public GameObject headAccessories;
    public GameObject headFeatures;
    public GameObject eyes;
    public GameObject eyesBack;
    public GameObject eyebrows;
    public GameObject mouth;
    public GameObject head;
    public GameObject scarf;
    public GameObject handRight;
    public GameObject footRight;
    public GameObject torsoAccessories;
    public GameObject torso;
    public GameObject handLeft;
    public GameObject item;
    public GameObject footLeft;
    public GameObject hairBack;
    public AppearanceCategory[] appearance;

    public enum TemplateAppearance
    {
        Null,
        Anna,
        Loxie,
        Sevrus,
        Jake,
        Stew
    }

    [SerializeField] public TemplateAppearance templateAppearance;

    Dictionary<string, string[]> templates = new Dictionary<string, string[]>(); // temp way to store character appearances

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
            {"Scarf", scarf},
            {"Hand Left", handLeft},
            {"Item", item},
            {"Foot Left", footLeft},
            {"Torso Accessories", torsoAccessories},
            {"Torso", torso},
            {"Hair Back", hairBack},
            {"Hand Right", handRight},
            {"Foot Right", footRight}
        };

        // templates for now
        templates["Anna"] = new string[] { "Scarf", "empty sprite", "Foot Right", "foot right boot grey", "Foot Left", "foot left boot grey","Hair Front", "hair front bangs blonde", "Head Accessories", "empty sprite", "Head Features", "head feature freckles", "Eyes", "eyes blue", "Eyes Back", "eyes back eyeliner", "Eyebrows", "eyebrows light", "Mouth", "mouth lips", "Head", "head normal", "Hand Right", "hand right normal", "Torso Accessories", "torso accessories necklace gold", "Torso", "torso thin robe purple", "Hand Left", "hand left normal", "Item", "item wand normal", "Hair Back", "hair back parted blonde" };
        templates["Loxie"] = new string[] { "Scarf", "empty sprite", "Hair Front", "hair front parted pink", "Head Accessories", "empty sprite", "Head Features", "empty sprite", "Eyes", "eyes green", "Eyes Back", "eyes back eyeliner", "Eyebrows", "eyebrows dark", "Mouth", "mouth lips", "Head", "head normal", "Hand Right", "hand right normal", "Torso Accessories", "empty sprite", "Torso", "torso thin robe purple", "Hand Left", "hand left normal", "Item", "item wand normal", "Hair Back", "empty sprite" };
        templates["Sevrus"] = new string[] { "Scarf", "empty sprite", "Hair Front", "hair front parted black", "Head Accessories", "empty sprite", "Head Features", "empty sprite", "Eyes", "eyes brown", "Eyes Back", "eyes back coy", "Eyebrows", "eyebrows dark", "Mouth", "mouth normal", "Head", "head pointed", "Hand Right", "hand right normal", "Torso Accessories", "empty sprite", "Torso", "torso thin robe black", "Hand Left", "hand left normal", "Item", "empty sprite", "Hair Back", "hair back short black" };
        templates["Jake"] = new string[] { "Scarf", "empty sprite", "Hair Front", "hair front bangs short", "Head Accessories", "empty sprite", "Head Features", "empty sprite", "Eyes", "eyes brown", "Eyes Back", "eyes back normal", "Eyebrows", "eyebrows dark", "Mouth", "mouth silly", "Head", "head round", "Hand Right", "hand right normal", "Torso Accessories", "empty sprite", "Torso", "torso shirt red", "Hand Left", "hand left normal", "Item", "item wand normal", "Hair Back", "empty sprite" };
        templates["Stew"] = new string[] { "Scarf", "empty sprite", "Hair Front", "hair front bangs blonde", "Head Accessories", "empty sprite", "Head Features", "empty sprite", "Eyes", "eyes beedy teal", "Eyes Back", "empty sprite", "Eyebrows", "eyebrows light", "Mouth", "mouth normal", "Head", "head normal", "Hand Right", "hand right normal", "Torso Accessories", "empty sprite", "Torso", "torso robe green_0", "Hand Left", "hand left normal", "Item", "item wand normal", "Hair Back", "empty sprite" };

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
        StartCoroutine(LoadTemplateAfterDelay(0.5f, templateAppearance.ToString()=="Null"?null:templateAppearance.ToString())); 
        Invoke("DebugOutSaveArr", 2.0f);  // Call DebugOutSaveArr after a delay to ensure all components are initialized
    }

    IEnumerator LoadTemplateAfterDelay(float delay, string key)
    {
        yield return new WaitForSeconds(delay);
        LoadTemplate(key);
        UpdateSpriteRenderersSortingLayer();
    }

    public void UpdateSpriteRenderersSortingLayer()
    {
        // Iterate through each AppearanceCategory in the appearance array
        foreach (var appearanceCategory in appearance)
        {
            // Get the SpriteRenderer component from the GameObject
            var spriteRenderer = appearanceCategory.gameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                // Update the sorting layer of the SpriteRenderer
                spriteRenderer.sortingLayerName = sortingLayerName;
                //Debug.Log($"[CharacterAppearance UPdateSpriteRenderersSortingLayer()]  sorting layer of {appearanceCategory.gameObject.name} to {sortingLayerName}");
            }
            else
            {
                // If there is no SpriteRenderer, log a warning
                Debug.LogWarning($"[CharacterAppearance UPdateSpriteRenderersSortingLayer()]  component not found on {appearanceCategory.gameObject.name}");
            }
        }
    }

    void LoadTemplate(string key)
    {
        string randomKey = new List<string>(templates.Keys)[UnityEngine.Random.Range(0, templates.Count)];
        Debug.Log("[CharacterAppearance LoadTemplate()] Loading Template: " + (key == null ? randomKey : key));
        LoadFromSaveArr(templates[key == null ? randomKey : key]);

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

    public void ChangeAppearance(string category, string newLabel)
    {
        // Find the AppearanceCategory object that matches the given 'category'
        foreach (AppearanceCategory appearanceCategory in appearance)
        {
            if (appearanceCategory.category == category)
            {
                // Call its ChangeLabel() method to change the sprite label
                //Debug.Log("[CharacterAppearance ChangeAppearance()] Changing " + category + " to " + newLabel);
                appearanceCategory.ChangeLabel(newLabel);
                return;
            }
        }
        Debug.LogWarning("[CharacterAppearance ChangeAppearance()] Category not found: " + category);
    }
}
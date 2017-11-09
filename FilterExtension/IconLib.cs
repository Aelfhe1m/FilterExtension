using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FilterExtensions.Utility;
using KSP.UI.Screens;
using UnityEngine;
using KSP.Localization;

namespace FilterExtensions
{
    public static class IconLib
    {
        // Dictionary of icons created on entering the main menu
        public static Dictionary<string, RUI.Icons.Selectable.Icon> IconDict = new Dictionary<string, RUI.Icons.Selectable.Icon>();
        // if the name sent to get_icon is a key, switch with the linked name
        public static Dictionary<string, string> Icon_Alias = new Dictionary<string, string>();
        // if the icon isn't present, use this one
        private const string fallbackIcon = "stockIcon_fallback";

        /// <summary>
        /// loads all textures that are 32x32px into a dictionary using the filename as a key
        /// </summary>
        public static void Load()
        {
            GameDatabase.TextureInfo texInfo = null;
            Texture2D selectedTex = null;
            var texDict = new Dictionary<string, GameDatabase.TextureInfo>();
            for (int i = GameDatabase.Instance.databaseTexture.Count - 1; i >= 0; --i)
            {
                texInfo = GameDatabase.Instance.databaseTexture[i];
                if (texInfo.texture != null && texInfo.texture.width == 32 && texInfo.texture.height == 32)
                {
                    texDict.TryAdd(texInfo.name, texInfo);
                }
            }

            foreach (KeyValuePair<string, GameDatabase.TextureInfo> kvp in texDict)
            {
                if (kvp.Value.name.Contains("_selected"))
                {
                    continue;
                }

                if (texDict.TryGetValue(kvp.Value.name + "_selected", out texInfo))
                {
                    selectedTex = texInfo.texture;
                }
                else
                {
                    selectedTex = kvp.Value.texture;
                }

                string name = kvp.Value.name.Split(new char[] { '/', '\\' }).Last();
                var icon = new RUI.Icons.Selectable.Icon(name, kvp.Value.texture, selectedTex, false);
                IconDict.TryAdd(icon.name, icon);
            }
        }


        /// <summary>
        /// get the icon that matches a name
        /// </summary>
        /// <param name="name">the icon name</param>
        /// <returns>the icon if it is found, or the fallback icon if it is not</returns>
        public static RUI.Icons.Selectable.Icon GetIcon(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                name = name.Trim();
                if (IconDict.TryGetValue(name, out RUI.Icons.Selectable.Icon icon)
                    || PartCategorizer.Instance.iconLoader.iconDictionary.TryGetValue(name, out icon))
                {
                    return icon;
                }
            }
            return PartCategorizer.Instance.iconLoader.iconDictionary[fallbackIcon];
        }

        public static RUI.Icons.Selectable.Icon GetIcon(SubCategoryInstance subcat)
        {
            Debug.Log("GetIcon, subcat: " + subcat.Name + ", subcatDisplayName: " + subcat.displayName);
            if (Icon_Alias.ContainsKey(subcat.Name))
            {
                Debug.Log("GetIcon, subcat.Name: " + subcat.Name + ", name found");
                return GetIcon(Icon_Alias[subcat.Name]);
            }
            foreach (ConfigNode node in GameDatabase.Instance.GetConfigNodes("FilterRename"))
            {
                foreach (KeyValuePair<string, ConfigNodes.SubcategoryNodeModifier.FilterIconRename> kvp in ConfigNodes.SubcategoryNodeModifier.MakeRenamers(node))
                {
                    Debug.Log("kvp.Value.iconName: " + kvp.Value.iconName + " : subcat.Name: " + subcat.Name);
                    if (kvp.Value.iconName == subcat.Name)
                    if (Icon_Alias.ContainsKey(kvp.Value.iconName))
                    {
                        Debug.Log("GetIcon, subcat.Name: " + subcat.Name + ",  iconName (" + kvp.Value.iconName + ") found in FilterIconRename");
                        return GetIcon(Icon_Alias[kvp.Value.iconName]);
                    }
                }
            }
            Debug.Log("GetIcon, subcat.Name: " + subcat.Name + ", name NOT found");

            return GetIcon(subcat.Icon);
        }

        public static RUI.Icons.Selectable.Icon GetIcon(CategoryInstance cat)
        {
            if (Icon_Alias.ContainsKey(cat.Name))
            {
                return GetIcon(Icon_Alias[cat.Name]);
            }
            return GetIcon(cat.Icon);
        }
    }

    
}

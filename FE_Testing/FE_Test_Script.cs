﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using KSP.UI.Screens;
using KSP.Localization;

namespace FE_Testing
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterTestCategory : MonoBehaviour
    {
        static readonly List<string> allowedParts = new List<string>() { "asasmodule1-2", "fuelTank3-2", "basicFin" };

        public void Start()
        {
            GameEvents.onGUIEditorToolbarReady.Add(AddSubCat);
        }

        public void AddSubCat()
        {
            Debug.Log("[FE_Testing Testing] Test subcat created");
            PartCategorizer.AddCustomSubcategoryFilter(
                PartCategorizer.Instance.filters.Find(c => string.Equals(c.button.categoryName, Localizer.Format("fe_filterByFunction") /* "Filter by Function" */, StringComparison.OrdinalIgnoreCase))
                , "testing"
                , Localizer.Format("testing")
                , new RUI.Icons.Selectable.Icon("abc", null, null, true)
                , p => allowedParts.Contains(p.name));
        }
    }

    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class FE_Test_Script : MonoBehaviour
    {
        const string testCatName = "Testing";
        Dictionary<string, string> partSubCatMap = new Dictionary<string, string>()
        {
            { "By Category - Control", "asasmodule1-2" },
            { "By Check", "fuelTank3-2" },
            { "By Cost < 100", "basicFin" },
            { "By Crash Tolerance - 6.0m/s", "fuelTank3-2" },
            { "By Crew - 1", "Mark1Cockpit" },
            { "Unpurchased", "" },
            { "By Folder - Squad", "Mark1Cockpit" },
            { "Not Squad", "" },
            { "By Manufacturer - Ionic", "ionEngine" },
            { "By Mass - 500kg", "fuelTank4-2" },
            { "By Max Temp > 2k", "nuclearEngine" },
            { "By Module Name - ModuleEngines", "nuclearEngine" },
            { "By Module Name - !contains(ModuleEnginesFX & ModuleRCS)" , "basicFin" },
            { "By Module Title - Module Engines & Module Parachute", "nuclearEngine" },
            { "By Module Title 2 - !contains(Module Engines & Module Parachute)", "asasmodule1-2" },
            { "By Name - nosecone & rtg", "pointyNoseConeA" },
            { "By Path - Squad/Parts/FuelTank/", "fuelTank4-2" },
            { "By Profile - mk2", "mk2FuselageShortMono" },
            { "By Profile 2 - !contains(mk2 & mk1)", "basicFin" },
            { "By Propellant - LF", "nuclearEngine" },
            { "By Resource - LF", "fuelTankSmallFlat" },
            { "By Resource 2 - !contains(LF - Ox)", "mk2FuselageShortMono" },
            { "By Size - 1", "nuclearEngine" },
            { "By Size 2 - !contains(0-1-3)", "fuelTank4-2" },
            { "By Subcategory - By Resource", "fuelTank4-2" },
            { "By Tag - jet", "turboJet" },
            { "By Tag 2 - !contains(jet | only)", "nuclearEngine" },
            { "By Tech - experimentalElectronics", "rtg" },
            { "By Title - PB-NUK & Aerodynamic Nose Cone", "rtg" }
        };

        public void Start()
        {
            StartCoroutine(CheckTestSubcategories());
        }

        IEnumerator CheckTestSubcategories()
        {
            yield return new WaitForSeconds(5); // wait for the editor to complete startup

//            PartCategorizer.Category testCat = PartCategorizer.Instance.filters.FirstOrDefault(C => string.Equals(Localizer.Format(C.button.categoryName), testCatName));
            PartCategorizer.Category testCat = PartCategorizer.Instance.filters.FirstOrDefault(C => string.Equals(C.button.categoryName, testCatName));
            if (testCat == null)
            {
                LogTestResult($"Category named \"{testCatName}\" found", false);
                yield break;
            }

            foreach (PartCategorizer.Category testingSubCat in testCat.subcategories)
            {
                if (partSubCatMap.ContainsKey(testingSubCat.button.categoryName))
                {
                    LogTestResult(testingSubCat.button.categoryName, PartExistsInSubCategory(partSubCatMap[testingSubCat.button.categoryName], testingSubCat));
                    partSubCatMap.Remove(testingSubCat.button.categoryName);
                }
            }

            // anything that didn't run gets an automatic fail
            foreach (KeyValuePair<string, string> kvp in partSubCatMap)
            {
                LogTestResult(kvp.Key, false);
            }
        }

        void LogTestResult(string test, bool result)
        {
            if (result)
            {
                Debug.Log($"[FE Testing] {test} with part \"{partSubCatMap[test]}\": {result}");
            }
            else
            {
                Debug.LogError($"[FE Testing] {test} with part \"{partSubCatMap[test]}\": {result}");
            }
        }

        bool PartExistsInSubCategory(string partID, PartCategorizer.Category toCheck)
        {
            AvailablePart part = PartLoader.LoadedPartsList.FirstOrDefault(ap => string.Equals(ap.name, partID, StringComparison.OrdinalIgnoreCase));
            if (part == null)
            {
                Debug.Log($"[FE Testing] could not find any part with the ID {partID}");
                return false;
            }
            return toCheck.exclusionFilter.FilterCriteria.Invoke(part);
        }
    }
}

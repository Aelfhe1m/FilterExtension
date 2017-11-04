using System;
using KSP.Localization;
using UnityEngine;

namespace FilterExtensions.ConfigNodes
{
    public class SubCategoryItem : IEquatable<SubCategoryItem>
    {
        public string SubcategoryName { get; }
        public bool ApplyTemplate { get; }

        public SubCategoryItem(string name, bool useTemplate = true)
        {
            Debug.Log("SubCategoryItem instantate, name: " + name + " -> " + Localizer.Format(name).Trim());
            SubcategoryName = Localizer.Format(name).Trim();
            ApplyTemplate = useTemplate;
        }

        public bool Equals(SubCategoryItem sub)
        {
            if (ReferenceEquals(null, sub))
            {
                return false;
            }
            if (ReferenceEquals(this, sub))
            {
                return true;
            }

            return SubcategoryName.Equals(sub.SubcategoryName);
        }

        public override int GetHashCode()
        {
            return SubcategoryName.GetHashCode();
        }

        public override string ToString()
        {
            return SubcategoryName;
        }
    }
}
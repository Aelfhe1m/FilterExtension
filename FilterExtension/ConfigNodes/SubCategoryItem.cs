using System;

namespace FilterExtensions.ConfigNodes
{
    public class SubCategoryItem : IEquatable<SubCategoryItem>
    {
        public string SubcategoryName { get; private set; }
        public string SubcatIconName { get; private set; }
        public bool ApplyTemplate { get; private set; }

        void SetSubCategoryItem(string name, string iconName, bool useTemplate)
        {
            SubcategoryName = name.Trim();
            SubcatIconName = iconName;
            ApplyTemplate = useTemplate;
        }
        public SubCategoryItem(string name, bool useTemplate = true)
        {
            SetSubCategoryItem(name, name, useTemplate);
        }
        public SubCategoryItem(string name, string iconName, bool useTemplate = true)
        {
            SetSubCategoryItem(name, iconName, useTemplate);
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
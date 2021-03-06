namespace XmlUpgrader.Core
{
    using System;
    using System.Collections.Generic;

    public class UpgradePlan
    {
        public UpgradePlan AddElements(dynamic elementsToAdd)
        {
            AddedElements = elementsToAdd;
            return this;
        }

        public UpgradePlan RenameElements(dynamic elementsToRename)
        {
            RenamedElements = elementsToRename;
            return this;
        }

        public UpgradePlan SetVersion(Version version)
        {
            UpgradeToVersion = version;
            return this;
        }

        public UpgradePlan SetVersion(string version)
        {
            UpgradeToVersion = new Version(version);
            return this;
        }

        public UpgradePlan RemoveElements(IEnumerable<string> removeElements)
        {
            RemovedElements.AddRange(removeElements);
            return this;
        }

        internal dynamic RenamedElements { get; set; }

        internal dynamic AddedElements { get; set; }
        
        public List<string> RemovedElements { get; set; } = new List<string>();

        internal Version UpgradeToVersion { get; set; }
    }
}
namespace XmlUpgrader.Test.Examples
{
    using System;
    using Core;

    internal class ExampleRenamingConfigV2 : IUpgradePlanProvider
    {
        public string TestString { get; set; }

        public int RenamedInteger { get; set; }


        public UpgradePlan GetUpgradePlan()
        {
            return new UpgradePlan();

            // todo use in test
        }

        public Version Version => new Version(2,0);
    }
}
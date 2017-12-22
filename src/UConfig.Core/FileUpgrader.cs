﻿namespace UConfig.Core
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Xml.Linq;

    internal class FileUpgrader
    {
        private readonly ConfigurationFile configFile;
        private readonly UpgradePlan upgradePlan;


        public FileUpgrader(UpgradePlan upgradePlan, ConfigurationFile configFile)
        {
            this.upgradePlan = upgradePlan;
            this.configFile = configFile;
        }


        public ConfigurationFile Upgrade()
        {
            var treeToUpgrade = new XElement(configFile.Document);

            extendXml(upgradePlan.AddedSettings, "", treeToUpgrade);

            return new ConfigurationFile
            {
                Document = treeToUpgrade,
                Version = upgradePlan.UpgradeToVersion
            };
        }

        private XElement extendXml(dynamic node, string nodeName, XElement treeToUpgrade)
        {
            XElement xmlNode;
            if (string.IsNullOrEmpty(nodeName))
            {
                xmlNode = treeToUpgrade;
            }
            else
            {
                xmlNode = treeToUpgrade.Element(nodeName); // try to grab existing node
            }

            if (xmlNode == null)
            {
                xmlNode = new XElement(nodeName); // node not yet existing, create new
                treeToUpgrade.Add(xmlNode);
            }

            foreach (KeyValuePair<string, object> property in (IDictionary<string, object>) node)
            {
                if (IsExpandoObject(property))
                {
                    extendXml(property.Value, property.Key, xmlNode);
                }

                else if (IsDynamicList(property))
                {
                    foreach (dynamic element in (List<dynamic>) property.Value)
                    {
                        xmlNode.Add(extendXml(element, property.Key, xmlNode));
                    }
                }
                else
                {
                    xmlNode.Add(new XElement(property.Key, property.Value));
                }
            }

            return xmlNode;
        }

        private static bool IsDynamicList(KeyValuePair<string, object> property)
        {
            return property.Value.GetType() == typeof(List<dynamic>);
        }

        private static bool IsExpandoObject(KeyValuePair<string, object> property)
        {
            return property.Value.GetType() == typeof(ExpandoObject);
        }
    }
}
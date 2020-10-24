using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Engine.Shared
{
    public static class ExtensionMethods
    {
        //all must be static and have 'this' for 1st param
        public static int AttributeAsInt(this XmlNode node, string attributeName) =>
            Convert.ToInt32(node.AttributeAsString(attributeName));

        public static double AttributeAsDouble(this XmlNode node, string attributeName) =>
            Convert.ToDouble(node.AttributeAsString(attributeName));

        public static string AttributeAsString(this XmlNode node, string attributeName)
            {
            XmlAttribute attribute = node.Attributes?[attributeName];

            if (attribute is null)
                throw new ArgumentException($"The attribute '{attributeName}' is not valid.");

            return attribute.Value;
            }
        public enum ColorCategory
        {
            None,
            Quest,
            Battle,
            Player
        }
    }
}

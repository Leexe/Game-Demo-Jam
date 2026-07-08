using UnityEngine;
using System;

namespace Micwu.Util.Attributes
{
    /// <summary>
    /// Place on a serialized Component field to auto-populate it with a found
    /// component on the same GameObject. The value can be overriden if needed,
    /// but cannot be set to null.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FindOnSelfAttribute : PropertyAttribute { }

    [AttributeUsage(AttributeTargets.Field)]
    public class DescAttribute : PropertyAttribute
    {
        public string Text;
        public DescAttribute(string text) => Text = text;
    }
}

namespace LessonMonitor.API
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DiscriptionAttribute : Attribute
    {
        public string Value { get; set; }
        public DiscriptionAttribute(string value = "")
        {
            Value = value;
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ValidationTools.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public class RequiredIntAttribute : ValidationAttribute
    {
        public int DefaultValue { get; set; }

        public override bool IsValid(object? value)
        {
            var intVal = value as int?;
            if (intVal is null)
                return false;

            return intVal != DefaultValue;
        }
    }
}

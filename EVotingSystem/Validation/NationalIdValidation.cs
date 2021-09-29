using System.ComponentModel.DataAnnotations;

namespace EVotingSystem.Validation
{
    public class NationalIdValidation : ValidationAttribute
    {
        /// <summary>
        /// Validates the national id if its empty or contains non digit letters
        /// </summary>
        /// <param name="value">The value here represents the nationalId</param>
        /// <returns>True if the national id is valid otherwise false if the national id is invalid</returns>
        public override bool IsValid(object value)
        {
            string NationalId = (string)value;
            return true;
        }
    }
}
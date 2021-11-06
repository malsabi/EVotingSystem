using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using EVotingSystem.Constants;

namespace EVotingSystem.Validation
{
    public class GenderValidation : ValidationAttribute, IClientModelValidator
    {
        /// <summary>
        /// Validates the Gender if it contains the accurate gender values
        /// </summary>
        /// <param name="value">The value here represents the gender</param>
        /// <returns>True if the gender is valid otherwise false if the gender is invalid</returns>
        public override bool IsValid(object value)
        {
            string Gender = (string)value;

            if (Gender == null || Gender.Equals(Config.Male) || Gender.Equals(Config.Female))
            {
                return true;
            }

            return false;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-GenderValidation", ErrorMessage);
        }
    }
}
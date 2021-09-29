using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace EVotingSystem.Validation
{
    public class NameValidation : ValidationAttribute, IClientModelValidator
    {
        /// <summary>
        /// Validates the name if its empty or contains non alphabetical letters
        /// </summary>
        /// <param name="value">The value here represents the name</param>
        /// <returns>True if the name is valid otherwise false if the name is invalid</returns>
        public override bool IsValid(object value)
        {
            string Name = (string)value;

            if (Name == null || Name.Length == 0)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < Name.Length; i++)
                {
                    if (IsLetter(Name[i]) == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsLetter(char c)
        {
            if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
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
            context.Attributes.Add("data-val-NameValidation", ErrorMessage);
        }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EVotingSystem.Validation
{
    public class NationalIdValidation : ValidationAttribute, IClientModelValidator
    {
        /// <summary>
        /// Validates the national id if its empty or contains non digit letters
        /// </summary>
        /// <param name="value">The value here represents the nationalId</param>
        /// <returns>True if the national id is valid otherwise false if the national id is invalid</returns>
        public override bool IsValid(object value)
        {
            string NationalId = (string)value;

            if (NationalId == null || NationalId.Length != 18 || NationalId.Contains("-") == false)
            {
                return false;
            }
            else
            {
                string[] Digits = NationalId.Split("-");
                if (Digits.Length != 4)
                {
                    return false;
                }
                else
                {
                    if (Digits[0].Length != 3 || Digits[1].Length != 4 || Digits[2].Length != 7 || Digits[3].Length != 1)
                    {
                        return false;
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            for (int j = 0; j < Digits[i].Length; j++)
                            {
                                if (IsDigit(Digits[i][j]) == false)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        private bool IsDigit(char ch)
        {
            return (ch >= '0' && ch <= '9');
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-NationalIdValidation", ErrorMessage);
        }
    }
}
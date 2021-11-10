using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace EVotingSystem.Validation
{
    public class StudentIdValidation : ValidationAttribute, IClientModelValidator
    {
        /// <summary>
        /// Validates the StudentId if its empty or does not contain the correct format.
        /// </summary>
        /// <param name="value">The value here represents the StudentId</param>
        /// <returns>True if the StudentId is valid otherwise false if the email is StudentId</returns>
        public bool IsValid(object value)
        {
            string StudentId = (string)value;

            if (StudentId == null || StudentId.Length != 9)
            {
                return false;
            }
            else
            {
                if ((StudentId[0] == 'H') || (StudentId[0] == 'h'))
                {
                    if (StudentId[1] != '0' || StudentId[2] != '0')
                    {
                        return false;
                    }
                    else
                    {
                        for (int i = 1; i < StudentId.Length; i++)
                        {
                            if (IsDigit(StudentId[i]) == false)
                            {
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        private bool IsDigit(char c)
        {
            if (c >= '0' && c <= '9')
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
            context.Attributes.Add("data-val-StudentIdValidation", ErrorMessage);
        }
    }
}
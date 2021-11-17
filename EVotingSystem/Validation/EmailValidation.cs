using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace EVotingSystem.Validation
{
    public class EmailValidation : ValidationAttribute, IClientModelValidator
    {
        /// <summary>
        /// Validates the email if its empty or does not contain the domain hct.ac.ae
        /// </summary>
        /// <param name="value">The value here represents the email</param>
        /// <returns>True if the email is valid otherwise false if the email is invalid</returns>
        public override bool IsValid(object value)
        {
            string Email = (string)value;

            const string Domain = "hct.ac.ae";
            const int IdLength = 9;
            const int EmailLength = 19;

            if (Email == null || Email.Contains("@") == false || Email.Length != EmailLength)
            {
                return false;
            }
            else
            {
                string[] Parts = Email.Split('@');
                if (Parts.Length != 2)
                {
                    return false;
                }
                else
                {
                    string CurrentId = Parts[0];
                    string CurrentDomain = Parts[1].ToLower();

                    if (CurrentDomain != Domain || CurrentId.Length != IdLength)
                    {
                        return false;
                    }
                    else
                    {
                        if (CurrentId[0] != 'H' && CurrentId[0] != 'h')
                        {
                            return false;
                        }
                        else if (CurrentId[1] != '0' || CurrentId[2] != '0')
                        {
                            return false;
                        }
                        else
                        {
                            for (int i = 3; i < CurrentId.Length; i++)
                            {
                                if (IsDigit(CurrentId[i]) == false)
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
            context.Attributes.Add("data-val-EmailValidation", ErrorMessage);
        }
    }
}
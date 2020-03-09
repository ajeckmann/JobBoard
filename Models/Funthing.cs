using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamCSharp.Models
{
    public class Funthing
    {
        [Key]

        
        public int FunthingId{get;set;}

        [Required(ErrorMessage="You Must include Funthing Title!")]
        [MinLength(2,ErrorMessage="The title must be at least 2 characters!!")]
        public string Title{get;set;}
        [Required(ErrorMessage="Need a date")]
        [FutureDate]
       
        public DateTime FunthingDate{get;set;}



        [Required(ErrorMessage="You Must include Funthing Description")]
        [MinLength(2,ErrorMessage="The Description must be at least 2 characters!!")]
        public string Description{get;set;}
        public DateTime CreatedAt{get;set;}=DateTime.Now;

        public DateTime UpdatedAT{get;set;}=DateTime.Now;
        
        public int UserID {get;set;}
        public User Creator {get;set;}
        //above is the nav toolbar for the one-to-many relationship

        public string Duration{get;set;}

        public List<Response> Participants{get;set;}
        //a funthing has a listof participants, compiled from REsponses. A Response contains  participant from the class user and a "participating" from class Funthing.
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
        DateTime check;
            if (value is DateTime)
            {
                check=(DateTime) value;
                if(check <DateTime.Now)
                {
                    return new ValidationResult("Enter a date after today for your Funthing!");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return new ValidationResult("Real date needed");
            }
        }
    }

}
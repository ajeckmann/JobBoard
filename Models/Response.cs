using System.ComponentModel.DataAnnotations;
namespace ExamCSharp.Models
{
    public class Response
    {
        [Key]
        public int ResponseId{get;set;}
        public int UserId{get;set;}
        public int FunthingId{get;set;}
        public User Participant{get;set;}
        public Funthing Participating{get;set;}


    }
}
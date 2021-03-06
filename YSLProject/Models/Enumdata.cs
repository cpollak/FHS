
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YSLProject.Models
{
    public class Enumdata
    {
        public enum UserType : int
        {
            Admin = 1,
            User = 2
        }

        public enum Gender : int
        {
            Male = 1,
            FeMale = 2
        }

        //public enum Language : int
        //{
        //    Hindi = 1,
        //    English = 2
        //}

        public enum RecertMonth
        {
            JAN, FEB, MAR, APR, MAY, JUN, JUL, AUG, SEP, OCT, NOV, DEC
        }
        public enum MembershipStatus : int
        {
            //Certified = 1,
            //NonCertified = 2,
            Active = 3,
            NonCovered = 4,
            Discharged = 5
        }

        public enum MemberStatus : int
        {
            New_Admissions = 1,
            Recertification = 2,
            Noncovered = 3,
            Discharge = 4
        }

        public enum FollowupStatus : int
        {
            Recertification_Initiated = 1,
            Follow_up = 2,
            Packet_in_Review = 3,
            Follow_up_with_Medicaid = 4,
            Requires_CPHL_Assistance = 5,
            Follow_up_Through_Edits = 6,
            Call_Member = 7,
            Refer_to_CPHL = 8,
            Call_Back = 9,
            Review_Recert = 10,            
            Call_Medicaid = 11,                
            Medicaid_Approved = 12,
            Medicaid_Rejected = 13,
            Recertification_Deferred = 14,
            Recert_in_Review = 15,
            Recertification_Approved = 16,
            Recertification_Denied = 17,
            Recertification_New = 18,
            Submit_Through_Edits = 19,
            Call_Medicaid_Hotline = 20,
            Call_Family = 21,
            NonCovered=22,
            Notify_CPHL = 23,
            To_be_discharged=24,
        }

        

        public enum FollowupFilterStatus : int
        {
            Recertification_Initiated = 1,
            Follow_up = 2,
            Packet_in_Review = 3,
            Follow_up_with_Medicaid = 4,
            Requires_CPHL_Assistance = 5,
            Follow_up_Through_Edits = 6,
            Recert_in_Review = 15,
            Recertification_Approved = 16,
            Recertification_Denied = 17,
            NonCovered=22,

        }
    }


}

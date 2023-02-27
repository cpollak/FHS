using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YSLProject.Table;

namespace YSLProject.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {

        }

        public DbSet<UserMaster> UserMaster { get; set; }
        public DbSet<MemberMaster> MemberMaster { get; set; }
        public DbSet<MemberCount> MemberCount { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<Spousal> Spousal { get; set; }        
        public DbSet<Logs> Logs { get; set; }
        public DbSet<PIT> PIT { get; set; }
        public DbSet<PITUploads> PITUploads { get; set; }
        public DbSet<PermissionMaster> PermissionMaster { get; set; }
        public DbSet<LanguageMaster> LanguageMaster { get; set; }
        public DbSet<Recertification_Follow_Up> Recertification_Follow_Up { get; set; }
        public DbSet<GeneralNotes> GeneralNotes { get; set; }
        
        public DbSet<GeneralNotes_Old> GeneralNotes_Old { get; set; }
        public DbSet<MemberMasterSP> memberMasterSPs { get; set; }
        public DbSet<GetAssignedMemberSP> GetAssignedMemberSP { get; set; }
        public DbSet<GetAssignMasterSP> GetAssignMasterSP { get; set; }

        public DbSet<CenterReportModel> CenterReportModelSPs { get; set; }
        public DbSet<CenterReportModelNon> CenterReportModelNonSPs { get; set; }
        public DbSet<ReportExcelExportModel> ReportExcelExportModelSps { get; set; }
        public DbSet<ReportNonCExcelExportModel> ReportNonCExcelExportModelSps { get; set; }
        public DbSet<Relationships> Relationships { get; set; }

        public DbSet<AuditTrail> AuditTrail { get; set; }
        public DbSet<FollowupStatusMaster> FollowupStatusMaster { get; set; }

        public DbSet<OutcomeMaster> OutcomeMaster { get; set; }
        public DbSet<CurrentStatusMaster> CurrentStatusMaster { get; set; }
    }
}

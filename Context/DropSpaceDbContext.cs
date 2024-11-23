using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DropSpace.Data;
using DropSpace.Data.Entity;
using DropSpace.Data.Entity.MasterData;
using DropSpace.Data.Entity.Droper;
using DropSpace.Data.Entity.LogInfo;
using DropSpace.Data.Entity.MasterData.PublicMapping;
using DropSpace.Data.Entity.MasterData.PoliceMapping;

namespace DropSpace.Context
{
    public class DropSpaceDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string?>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DropSpaceDbContext(DbContextOptions<DropSpaceDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        #region Master Data
        #region Police Map
        public DbSet<RangeMetro> rangeMetros { get; set; }
        public DbSet<DivisionDistrict> divisionDistricts { get; set; }
        public DbSet<ZoneCircle> zoneCircles { get; set; }
        public DbSet<PoliceThana> policeThanas { get; set; }
        #endregion
        #region Public Map
        public DbSet<Country> countries { get; set; }
        public DbSet<District> districts { get; set; }
        public DbSet<Division> divisions { get; set; }
        public DbSet<Thana> thanas { get; set; }
        public DbSet<UnionWard> unionWards { get; set; }
        public DbSet<Village> villages { get; set; }
        #endregion
        public DbSet<CrimeInfo> crimeInfos { get; set; }
        public DbSet<UserType> userTypes { get; set; }
        #endregion
        #region Dropers
        public DbSet<PersonsData> personalDatas { get; set; }
        public DbSet<UploadedFiles> uploadedFiles { get; set; }
        #endregion
        #region Log
        public DbSet<UserLogHistory> UserLogHistories { get; set; }
        public DbSet<OTPLogs> oTPLogs { get; set; }
        #endregion
        #region Settings Configs
        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            AddTimestamps();
            return await base.SaveChangesAsync();
        }

        private void AddTimestamps()
        {

            var entities = ChangeTracker.Entries().Where(x => x.Entity is Base && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var currentUsername = !string.IsNullOrEmpty(_httpContextAccessor?.HttpContext?.User?.Identity?.Name)
                ? _httpContextAccessor.HttpContext.User.Identity.Name
                : "Anonymous";

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((Base)entity.Entity).createdAt = DateTime.Now;
                    ((Base)entity.Entity).createdBy = currentUsername;
                }
                else
                {
                    entity.Property("createdAt").IsModified = false;
                    entity.Property("createdBy").IsModified = false;
                    ((Base)entity.Entity).updatedAt = DateTime.Now;
                    ((Base)entity.Entity).updatedBy = currentUsername;
                }

                #region changelog
                //int sessionId = 0;
                //DateTime myDate1 = new DateTime(1970, 1, 9, 0, 0, 00);
                //DateTime myDate2 = DateTime.Now;
                //TimeSpan myDateResult;
                //myDateResult = myDate2 - myDate1;
                //double seconds = myDateResult.TotalSeconds;
                //sessionId = Convert.ToInt32(seconds);

                //string? entityName = entity.Entity.GetType().Name;
                //string? entityState = entity.State.Tostring?();
                //if (entityName != "UserLogHistory")
                //{

                //    var builder = new ConfigurationBuilder()
                //                    .SetBasePath(Directory.GetCurrentDirectory())
                //                    .AddJsonFile("appsettings.json");

                //    var configuration = builder.Build();

                //    using (var db = new SqlConnection(configuration.GetConnectionstring?("AlphaConnection")))
                //    {
                //        db.Open();

                //        var entityMember = entity.Members;
                //        var value = entity.Members.Count();
                //        var entityinfo = entity.Entity.GetType();
                //        var entityVal = entity.Entity;
                //        string? customAttributeName = string?.Empty;
                //        var fieldName = entityinfo.GetProperties();
                //        for (int i = 0; i < fieldName.Count(); i++)
                //        {
                //            var columnName = fieldName[i].Name;
                //            string? colType = fieldName[i].PropertyType.Tostring?();
                //            var custmAttribute = fieldName[i].GetCustomAttributesData();
                //            if (custmAttribute.Count() >= 1)
                //                customAttributeName = custmAttribute.FirstOrDefault().AttributeType.Name;

                //            if (colType.Contains("AlphaManagement") || customAttributeName == "NotMappedAttribute")
                //            {

                //            }
                //            else
                //            {

                //                var valueName = entity?.Property(columnName)?.CurrentValue?.Tostring?();
                //                valueName = valueName?.Replace("'", "''");
                //                string? Tmp1 = $"INSERT INTO DbChangeHistories (entityName,fieldName,fieldValue,entityState,sessionId,createdBy,createdAt) VALUES('{entityName}','{columnName}','{valueName}','{entityState}','{sessionId}','{currentUsername}','{DateTime.Now}');";
                //                SqlCommand cmd1 = new SqlCommand(Tmp1, db);
                //                cmd1.ExecuteScalar();
                //            }

                //        }
                //        db.Close();
                //    }

                //}

                #endregion

            }
        }
        #endregion

    }
}

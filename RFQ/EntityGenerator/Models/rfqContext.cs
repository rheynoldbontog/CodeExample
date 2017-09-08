using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using EntityGenerator.Models.Mapping;

namespace EntityGenerator.Models
{
    public partial class rfqContext : DbContext
    {
        static rfqContext()
        {
            Database.SetInitializer<rfqContext>(null);
        }

        public rfqContext()
            : base("Name=rfqContext")
        {
        }

        public DbSet<activitylog> activitylogs { get; set; }
        public DbSet<activitylogtype> activitylogtypes { get; set; }
        public DbSet<address> addresses { get; set; }
        public DbSet<campaign> campaigns { get; set; }
        public DbSet<country> countries { get; set; }
        public DbSet<currency> currencies { get; set; }
        public DbSet<department> departments { get; set; }
        public DbSet<download> downloads { get; set; }
        public DbSet<emailaccount> emailaccounts { get; set; }
        public DbSet<equipmentcalibrationtype> equipmentcalibrationtypes { get; set; }
        public DbSet<equipmentpurchasetype> equipmentpurchasetypes { get; set; }
        public DbSet<externalauthenticationrecord> externalauthenticationrecords { get; set; }
        public DbSet<forumsforum> forumsforums { get; set; }
        public DbSet<forumsgroup> forumsgroups { get; set; }
        public DbSet<forumspost> forumsposts { get; set; }
        public DbSet<forumsprivatemessage> forumsprivatemessages { get; set; }
        public DbSet<forumssubscription> forumssubscriptions { get; set; }
        public DbSet<forumstopic> forumstopics { get; set; }
        public DbSet<genericattribute> genericattributes { get; set; }
        public DbSet<language> languages { get; set; }
        public DbSet<localestringresource> localestringresources { get; set; }
        public DbSet<localizedproperty> localizedproperties { get; set; }
        public DbSet<log> logs { get; set; }
        public DbSet<measuredimension> measuredimensions { get; set; }
        public DbSet<measureweight> measureweights { get; set; }
        public DbSet<messagetemplate> messagetemplates { get; set; }
        public DbSet<news> news { get; set; }
        public DbSet<newscomment> newscomments { get; set; }
        public DbSet<newslettersubscription> newslettersubscriptions { get; set; }
        public DbSet<permissionrecord> permissionrecords { get; set; }
        public DbSet<picture> pictures { get; set; }
        public DbSet<poll> polls { get; set; }
        public DbSet<pollanswer> pollanswers { get; set; }
        public DbSet<queuedemail> queuedemails { get; set; }
        public DbSet<quotation> quotations { get; set; }
        public DbSet<rfq> rfqs { get; set; }
        public DbSet<rfqline> rfqlines { get; set; }
        public DbSet<rfqlineformtype> rfqlineformtypes { get; set; }
        public DbSet<rfqlinetype> rfqlinetypes { get; set; }
        public DbSet<rfqstatu> rfqstatus { get; set; }
        public DbSet<rfqstatushistory> rfqstatushistories { get; set; }
        public DbSet<scheduletask> scheduletasks { get; set; }
        public DbSet<setting> settings { get; set; }
        public DbSet<stateprovince> stateprovinces { get; set; }
        public DbSet<supportcode> supportcodes { get; set; }
        public DbSet<test> tests { get; set; }
        public DbSet<topic> topics { get; set; }
        public DbSet<uom> uoms { get; set; }
        public DbSet<user> users { get; set; }
        public DbSet<usercontent> usercontents { get; set; }
        public DbSet<userrole> userroles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new activitylogMap());
            modelBuilder.Configurations.Add(new activitylogtypeMap());
            modelBuilder.Configurations.Add(new addressMap());
            modelBuilder.Configurations.Add(new campaignMap());
            modelBuilder.Configurations.Add(new countryMap());
            modelBuilder.Configurations.Add(new currencyMap());
            modelBuilder.Configurations.Add(new departmentMap());
            modelBuilder.Configurations.Add(new downloadMap());
            modelBuilder.Configurations.Add(new emailaccountMap());
            modelBuilder.Configurations.Add(new equipmentcalibrationtypeMap());
            modelBuilder.Configurations.Add(new equipmentpurchasetypeMap());
            modelBuilder.Configurations.Add(new externalauthenticationrecordMap());
            modelBuilder.Configurations.Add(new forumsforumMap());
            modelBuilder.Configurations.Add(new forumsgroupMap());
            modelBuilder.Configurations.Add(new forumspostMap());
            modelBuilder.Configurations.Add(new forumsprivatemessageMap());
            modelBuilder.Configurations.Add(new forumssubscriptionMap());
            modelBuilder.Configurations.Add(new forumstopicMap());
            modelBuilder.Configurations.Add(new genericattributeMap());
            modelBuilder.Configurations.Add(new languageMap());
            modelBuilder.Configurations.Add(new localestringresourceMap());
            modelBuilder.Configurations.Add(new localizedpropertyMap());
            modelBuilder.Configurations.Add(new logMap());
            modelBuilder.Configurations.Add(new measuredimensionMap());
            modelBuilder.Configurations.Add(new measureweightMap());
            modelBuilder.Configurations.Add(new messagetemplateMap());
            modelBuilder.Configurations.Add(new newsMap());
            modelBuilder.Configurations.Add(new newscommentMap());
            modelBuilder.Configurations.Add(new newslettersubscriptionMap());
            modelBuilder.Configurations.Add(new permissionrecordMap());
            modelBuilder.Configurations.Add(new pictureMap());
            modelBuilder.Configurations.Add(new pollMap());
            modelBuilder.Configurations.Add(new pollanswerMap());
            modelBuilder.Configurations.Add(new queuedemailMap());
            modelBuilder.Configurations.Add(new quotationMap());
            modelBuilder.Configurations.Add(new rfqMap());
            modelBuilder.Configurations.Add(new rfqlineMap());
            modelBuilder.Configurations.Add(new rfqlineformtypeMap());
            modelBuilder.Configurations.Add(new rfqlinetypeMap());
            modelBuilder.Configurations.Add(new rfqstatuMap());
            modelBuilder.Configurations.Add(new rfqstatushistoryMap());
            modelBuilder.Configurations.Add(new scheduletaskMap());
            modelBuilder.Configurations.Add(new settingMap());
            modelBuilder.Configurations.Add(new stateprovinceMap());
            modelBuilder.Configurations.Add(new supportcodeMap());
            modelBuilder.Configurations.Add(new testMap());
            modelBuilder.Configurations.Add(new topicMap());
            modelBuilder.Configurations.Add(new uomMap());
            modelBuilder.Configurations.Add(new userMap());
            modelBuilder.Configurations.Add(new usercontentMap());
            modelBuilder.Configurations.Add(new userroleMap());
        }
    }
}

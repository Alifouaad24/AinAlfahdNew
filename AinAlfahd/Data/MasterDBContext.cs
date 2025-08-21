using System;
using System.Collections.Generic;
using AinAlfahd.Models;
using AinAlfahd.Models_New;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace AinAlfahd.Data;

public partial class MasterDBContext : IdentityDbContext<IdentityUser>
{

    public MasterDBContext(DbContextOptions<MasterDBContext> options)
        : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
    }


    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountBox> AccountBoxes { get; set; }
    public virtual DbSet<Platform> Platforms { get; set; }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<Aliexbatch> Aliexbatches { get; set; }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<Batch> Batches { get; set; }

    public virtual DbSet<BatchStatus> BatchStatuses { get; set; }
    public virtual DbSet<ItemImages> ItemImages { get; set; }

    public virtual DbSet<Business> Businesses { get; set; }
    public virtual DbSet<ItemCondition> ItemConditions { get; set; }
    public virtual DbSet<Inventory> Inventory { get; set; }

    public virtual DbSet<BusinessArea> BusinessAreas { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

    public virtual DbSet<Exchange> Exchanges { get; set; }

    public virtual DbSet<IncomeBox> IncomeBoxes { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemStatus> ItemStatuses { get; set; }

    public virtual DbSet<ListPrice> ListPrices { get; set; }

    public virtual DbSet<Merchant> Merchants { get; set; }

    public virtual DbSet<NeighberLab> NeighberLabs { get; set; }

    public virtual DbSet<Offlinetran> Offlinetrans { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderCase> OrderCases { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Preorder> Preorders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<RegistrationLog> RegistrationLogs { get; set; }

    public virtual DbSet<SiteRolesnames> SiteRolesnames { get; set; }

    public virtual DbSet<SampleStatus> SampleStatuses { get; set; }

    public virtual DbSet<SerialNumber> SerialNumbers { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ShippingCompany> ShippingCompanies { get; set; }

    public virtual DbSet<Source> Sources { get; set; }

    public virtual DbSet<Specialty> Specialties { get; set; }

    public virtual DbSet<SpendingList> SpendingLists { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<StoreSize> StoreSizes { get; set; }

    public virtual DbSet<Models.TaskStatus> TaskStatuses { get; set; }

    public virtual DbSet<TblConfig> TblConfigs { get; set; }

    public virtual DbSet<TblLog> TblLogs { get; set; }

    public virtual DbSet<TblSize> TblSizes { get; set; }

    public virtual DbSet<Tbllat> Tbllats { get; set; }

    public virtual DbSet<Tblsku> Tblskus { get; set; }

    public virtual DbSet<TestDetail> TestDetails { get; set; }

    public virtual DbSet<TransactionType> TransactionTypes { get; set; }

    public virtual DbSet<UsCustomer> UsCustomers { get; set; }

    public virtual DbSet<UsCustomerReview> UsCustomerReviews { get; set; }

    public virtual DbSet<Uscity> Uscities { get; set; }

    public virtual DbSet<Siteusers> Siteusers { get; set; }

    public virtual DbSet<UserMerchant> UserMerchants { get; set; }

    public virtual DbSet<VwMostwanted> VwMostwanteds { get; set; }

    // Add CustomerService Table
    public DbSet<CustomerService> CustomerServices { get; set; }
    public DbSet<CustomerShipping> CustomerShipping { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Reciept> Reciepts { get; set; }
    public DbSet<Make> Makes { get; set; }

    public DbSet<Systemm> Systems { get; set; }
    public DbSet<ShippingBatch> ShippingBatchs { get; set; }
    public DbSet<ShippingTypes> ShippingTypes { get; set; }
    public DbSet<Amendment_Log> Amendment_Logs { get; set; }
    public DbSet<Amendment> Amendments { get; set; }

    //////////////////////////////////
    public DbSet<shipping_pricerole> shipping_pricerole { get; set; }
    public DbSet<trade_types> trade_types { get; set; }
    public DbSet<Currency> Currency { get; set; }
    public DbSet<weight_cat> weight_cat { get; set; }
    public DbSet<weight_types> weight_types { get; set; }
    public DbSet<Package> Packages { get; set; }
    public DbSet<RequestNote> RequestNotes { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);
        // /////////////////////////////////////////////////////////////////////////////////
        modelBuilder.Entity<CustomerService>()
            .HasKey(cs => new { cs.CustomerId, cs.ServiceId });

        modelBuilder.Entity<CustomerService>()
            .HasOne(cs => cs.Customer)
            .WithMany(c => c.CustomerServices)
            .HasForeignKey(cs => cs.CustomerId);

        modelBuilder.Entity<CustomerService>()
            .HasOne(cs => cs.Service)
            .WithMany(s => s.CustomerServices)
            .HasForeignKey(cs => cs.ServiceId);

        modelBuilder.Entity<ShippingBatch>()
            .Property(e => e.SellingIQ)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<ShippingBatch>()
            .Property(e => e.batchCostUS)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<OrderDetail>()
    .Property(e => e.Weight)
    .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Reciept>()
            .Property(e => e.Cost)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Reciept>()
            .Property(e => e.CostIQ)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Reciept>()
            .Property(e => e.DisCount)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Reciept>()
            .Property(e => e.SellingDisCount)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Reciept>()
            .Property(e => e.SellingPrice)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Reciept>()
            .Property(e => e.SellingUSD)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Reciept>()
            .Property(e => e.TotalPriceFromCust)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<OrderDetail>()
            .Property(e => e.Weight)
            .HasColumnType("decimal(18,2)");


        modelBuilder.Entity<Reciept>()
            .Property(e => e.Weight)
            .HasColumnType("decimal(18,2)");


        modelBuilder.Entity<TblConfig>()
            .HasData(new TblConfig
            {
                Id = 5,
                Db_env = "Dev",
            });




        modelBuilder.Entity<Customer>()
        .HasIndex(c => c.CustMob)
        .IsUnique();


        modelBuilder.Entity<Address>(entity =>
        {
            entity.Property(c => c.IsDeleted)
                .HasDefaultValue(false);
        });
        
        modelBuilder.Entity<Reciept>(entity =>
        {
            entity.Property(c => c.InsertDate)
                .HasDefaultValueSql("GETDATE()");
        });




        /////////////////////////////////////////////////////////////

        modelBuilder.UseCollation("Arabic_CI_AS");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("accounts");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountName)
                .HasMaxLength(50)
                .HasColumnName("accountName");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
        });

        modelBuilder.Entity<AccountBox>(entity =>
        {
            entity.ToTable("AccountBox");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.BatchId).HasColumnName("batchID");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.NotCollected).HasColumnName("not_collected");
            entity.Property(e => e.Notes)
                .HasMaxLength(500)
                .HasColumnName("notes");
            entity.Property(e => e.OrderNo).HasColumnName("order_no");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.SpentCategory).HasColumnName("spent_category");
            entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");
            entity.Property(e => e.TransactionType).HasColumnName("transaction_type");
        });

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.ActivityId).HasName("PK_test_activities");

            entity.Property(e => e.ActivityId).HasColumnName("activityID");
            entity.Property(e => e.ActivityName)
                .HasMaxLength(50)
                .HasColumnName("activityName");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
        });

        modelBuilder.Entity<Aliexbatch>(entity =>
        {
            entity.ToTable("aliexbatch");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BatchStatus).HasColumnName("batch_status");
            entity.Property(e => e.InsertDt)
                .HasColumnType("smalldatetime")
                .HasColumnName("insert_dt");
            entity.Property(e => e.MerchantId).HasColumnName("merchant_id");
        });

        modelBuilder.Entity<Area>(entity =>
        {
            entity.ToTable("areas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
            entity.Property(e => e.Sector).HasColumnName("sector");
            entity.Property(e => e.Spec).HasColumnName("spec");
            entity.Property(e => e.Zone).HasColumnName("zone");
        });

        modelBuilder.Entity<Batch>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adjusted).HasColumnName("adjusted");
            entity.Property(e => e.BatchName).HasMaxLength(50);
            entity.Property(e => e.BatchStatus).HasColumnName("batch_status");
            entity.Property(e => e.BatchUsvalue).HasColumnName("batchUSValue");
            entity.Property(e => e.Cbm).HasColumnName("CBM");
            entity.Property(e => e.CityId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("city_id");
            entity.Property(e => e.FinanceDt).HasColumnName("finance_dt");
            entity.Property(e => e.Financed).HasColumnName("financed");
            entity.Property(e => e.InsertDt).HasColumnName("insert_dt");
            entity.Property(e => e.MerchantId).HasColumnName("merchant_id");
            entity.Property(e => e.ShippingCompanyId).HasColumnName("ShippingCompanyID");
        });

        modelBuilder.Entity<BatchStatus>(entity =>
        {
            entity.ToTable("batch_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .HasColumnName("description");
        });

        modelBuilder.Entity<Business>(entity =>
        {
            entity.ToTable("business");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActivityId).HasColumnName("activity_id");
            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .HasColumnName("address");
            entity.Property(e => e.Address2)
                .HasMaxLength(50)
                .HasColumnName("address2");
            entity.Property(e => e.BusinessName)
                .HasMaxLength(50)
                .HasColumnName("Business_name");
            entity.Property(e => e.BusinessPhone)
                .HasMaxLength(50)
                .HasColumnName("business_phone");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.CustId).HasColumnName("cust_id");
            entity.Property(e => e.StateCode).HasColumnName("state_code");
            entity.Property(e => e.Zip).HasColumnName("zip");
        });

        modelBuilder.Entity<BusinessArea>(entity =>
        {
            entity.ToTable("business_areas");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BusinessId).HasColumnName("business_id");
            entity.Property(e => e.CityId).HasColumnName("city_id");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.CategoryName).HasMaxLength(50);

        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("cities");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
            entity.Property(e => e.IsNorth).HasColumnName("isNorth");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("customers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CustArea).HasColumnName("cust_area");
            entity.Property(e => e.CustCity).HasColumnName("cust_city");
            entity.Property(e => e.CustLandmark)
                .HasMaxLength(150)
                .HasColumnName("cust_landmark");
            entity.Property(e => e.CustMob)
                .HasMaxLength(15)
                .HasColumnName("cust_mob");
            entity.Property(e => e.CustMob2)
                .HasMaxLength(15)
                .HasColumnName("cust_mob2");
            entity.Property(e => e.CustName)
                .HasMaxLength(150)
                .HasColumnName("cust_name");
            entity.Property(e => e.CustProfile)
                .HasMaxLength(300)
                .HasColumnName("cust_profile");
            entity.Property(e => e.CustStatus).HasColumnName("cust_status");
            entity.Property(e => e.Fbid)
                .HasMaxLength(50)
                .HasColumnName("fbid");
            entity.Property(e => e.FullPackage).HasColumnName("full_package");
            entity.Property(e => e.Gisurl)
                .HasMaxLength(50)
                .HasColumnName("gisurl");
            entity.Property(e => e.Hexcode)
                .HasMaxLength(50)
                .HasColumnName("hexcode");
            entity.Property(e => e.InsertDt).HasColumnName("insert_dt");
            entity.Property(e => e.Lat)
                .HasMaxLength(150)
                .HasColumnName("lat");
            entity.Property(e => e.Lon).HasColumnName("lon");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.ToTable("Doctor");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.ToTable("error_log");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripton).HasColumnName("descripton");
            entity.Property(e => e.InsertDate).HasColumnName("insert_date");
            entity.Property(e => e.Sqlqry).HasColumnName("sqlqry");
        });

        modelBuilder.Entity<Exchange>(entity =>
        {
            entity.ToTable("exchange");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BankRate)
                .HasMaxLength(50)
                .HasColumnName("bank_rate");
            entity.Property(e => e.ExchangeRate)
                .HasMaxLength(50)
                .HasColumnName("exchange_rate");
            entity.Property(e => e.Managerno)
                .HasMaxLength(50)
                .HasColumnName("managerno");
            entity.Property(e => e.OldRate)
                .HasMaxLength(50)
                .HasColumnName("old_rate");
        });

        modelBuilder.Entity<IncomeBox>(entity =>
        {
            entity.ToTable("income_box");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Cleared).HasColumnName("cleared");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
            entity.Property(e => e.InsertDt).HasColumnName("insert_dt");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.MerchantId).HasColumnName("merchant_id");
            entity.Property(e => e.OrderNo).HasColumnName("order_no");
            entity.Property(e => e.TransactionType).HasColumnName("transaction_type");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("items");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ArDesc)
                .HasMaxLength(150)
                .HasColumnName("AR_DESC");
            entity.Property(e => e.Assymbly).HasColumnName("assymbly");
            entity.Property(e => e.Comb).HasColumnName("comb");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.EngName)
                .HasMaxLength(50)
                .HasColumnName("eng_name");
            entity.Property(e => e.Fr)
                .HasMaxLength(50)
                .HasColumnName("FR");
            entity.Property(e => e.H).HasColumnName("h");
            entity.Property(e => e.ImgUrl)
                .HasMaxLength(250)
                .HasColumnName("img_url");
            entity.Property(e => e.L).HasColumnName("l");
            entity.Property(e => e.PCode)
                .HasMaxLength(50)
                .HasColumnName("p_code");
            entity.Property(e => e.SitePrice)
                .HasColumnType("money")
                .HasColumnName("site_price");
            entity.Property(e => e.SubId).HasColumnName("sub_id");
            entity.Property(e => e.W).HasColumnName("w");
            entity.Property(e => e.WebUrl)
                .HasMaxLength(250)
                .HasColumnName("web_url");
            entity.Property(e => e.Weight).HasColumnName("weight");
        });

        modelBuilder.Entity<ItemStatus>(entity =>
        {
            entity.ToTable("item_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
        });

        modelBuilder.Entity<ListPrice>(entity =>
        {
            entity.ToTable("list_price");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LabId).HasColumnName("lab_id");
            entity.Property(e => e.Pricing).HasColumnName("pricing");
            entity.Property(e => e.SampleId).HasColumnName("sample_id");
        });

        modelBuilder.Entity<Merchant>(entity =>
        {
            entity.Property(e => e.Bgw).HasColumnName("bgw");
            entity.Property(e => e.Branch).HasColumnName("branch");
            entity.Property(e => e.Bypass).HasColumnName("bypass");
            entity.Property(e => e.Cities).HasColumnName("cities");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.Credit).HasColumnName("credit");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Hidden).HasColumnName("hidden");
            entity.Property(e => e.IsPublic).HasColumnName("isPublic");
            entity.Property(e => e.MerchantMob)
                .HasMaxLength(50)
                .HasColumnName("merchant_mob");
            entity.Property(e => e.MerchantName)
                .HasMaxLength(50)
                .HasColumnName("Merchant_name");
            entity.Property(e => e.Outskirts)
                .HasColumnType("money")
                .HasColumnName("outskirts");
            entity.Property(e => e.SmsAlert).HasColumnName("sms_alert");
            entity.Property(e => e.Sorting).HasColumnName("sorting");
            entity.Property(e => e.UsDelivery).HasColumnName("US_delivery");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<NeighberLab>(entity =>
        {
            entity.ToTable("neighberLabs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LabId).HasColumnName("labID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Offlinetran>(entity =>
        {
            entity.ToTable("offlinetrans");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SqlQry)
                .HasMaxLength(900)
                .HasColumnName("sql_qry");
            entity.Property(e => e.TranDate).HasColumnName("tran_date");
            entity.Property(e => e.TransferDate).HasColumnName("transfer_date");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("orders");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Acknoledged).HasColumnName("acknoledged");
            entity.Property(e => e.ActualDeliveryDt)
                .HasColumnType("smalldatetime")
                .HasColumnName("actual_delivery_dt");
            entity.Property(e => e.AdminAdj).HasColumnName("admin_adj");
            entity.Property(e => e.AdminBonus).HasColumnName("admin_bonus");
            entity.Property(e => e.AdvOrder).HasColumnName("adv_order");
            entity.Property(e => e.AdvancePayment).HasColumnName("advance_payment");
            entity.Property(e => e.AgentUserId).HasColumnName("agent_user_id");
            entity.Property(e => e.Aliexbatchid).HasColumnName("aliexbatchid");
            entity.Property(e => e.AssymblyCharges).HasColumnName("assymbly_charges");
            entity.Property(e => e.AssymblyCleared).HasColumnName("assymblyCleared");
            entity.Property(e => e.AssymblyCollectedBy).HasColumnName("assymbly_collected_by");
            entity.Property(e => e.BatchId).HasColumnName("BatchID");
            entity.Property(e => e.BonusDeduct).HasColumnName("bonus_deduct");
            entity.Property(e => e.BonusIq).HasColumnName("bonusIQ");
            entity.Property(e => e.BookingDt)
                .HasColumnType("smalldatetime")
                .HasColumnName("booking_dt");
            entity.Property(e => e.BookingSource)
                .HasColumnType("money")
                .HasColumnName("booking_source");
            entity.Property(e => e.BranchId).HasColumnName("branch_id");
            entity.Property(e => e.CenrtralBankPrice).HasColumnName("cenrtral_bank_price");
            entity.Property(e => e.CityReceipt).HasColumnName("city_receipt");
            entity.Property(e => e.CloseAcknoledgeDt)
                .HasColumnType("smalldatetime")
                .HasColumnName("close_acknoledge_dt");
            entity.Property(e => e.Closed).HasColumnName("closed");
            entity.Property(e => e.Collected).HasColumnName("collected");
            entity.Property(e => e.CollectedByOwner).HasColumnName("collected_by_owner");
            entity.Property(e => e.CollectionAdjustmentDt).HasColumnName("collection_adjustment_dt");
            entity.Property(e => e.CollectionDt)
                .HasColumnType("smalldatetime")
                .HasColumnName("collection_dt");
            entity.Property(e => e.CompDeliveryAdj).HasColumnName("comp_delivery_adj");
            entity.Property(e => e.CompSortAdj).HasColumnName("comp_sort_adj");
            entity.Property(e => e.DeductProgressed).HasColumnName("deduct_progressed");
            entity.Property(e => e.DeliveryCharges).HasColumnName("delivery_charges");
            entity.Property(e => e.DeliveryCleared).HasColumnName("deliveryCleared");
            entity.Property(e => e.DeliveryCollectedBy).HasColumnName("deliveryCollectedBy");
            entity.Property(e => e.DeliveryDt)
                .HasColumnType("smalldatetime")
                .HasColumnName("delivery_dt");
            entity.Property(e => e.DeliveryNotes)
                .HasMaxLength(250)
                .HasColumnName("delivery_notes");
            entity.Property(e => e.FinancAdj).HasColumnName("financ_adj");
            entity.Property(e => e.FinanceStatus).HasColumnName("finance_status");
            entity.Property(e => e.FullyPackage).HasColumnName("fully_package");
            entity.Property(e => e.Hxcode)
                .HasMaxLength(50)
                .HasColumnName("hxcode");
            entity.Property(e => e.IsCredit).HasColumnName("is_credit");
            entity.Property(e => e.LastBeforeStatusId).HasColumnName("last_before_status_id");
            entity.Property(e => e.LinkedAdvNorderNo).HasColumnName("linked_adv_norder_no");
            entity.Property(e => e.LocalDeliveryAdj).HasColumnName("local_delivery_adj");
            entity.Property(e => e.MerchantId).HasColumnName("merchant_id");
            entity.Property(e => e.Netrevenue).HasColumnName("netrevenue");
            entity.Property(e => e.Notes)
                .HasMaxLength(650)
                .HasColumnName("notes");
            entity.Property(e => e.OnlineOrder).HasColumnName("online_order");
            entity.Property(e => e.OrderCaseId).HasColumnName("order_case_id");
            entity.Property(e => e.OrderDt).HasColumnName("order_dt");
            entity.Property(e => e.OrderOwner).HasColumnName("order_owner");
            entity.Property(e => e.OrderStatus).HasColumnName("order_status");
            entity.Property(e => e.PageReturnArrange).HasColumnName("page_return_arrange");
            entity.Property(e => e.PickupDt)
                .HasColumnType("smalldatetime")
                .HasColumnName("pickup_dt");
            entity.Property(e => e.PreClose).HasColumnName("pre_close");
            entity.Property(e => e.PriceLog)
                .HasMaxLength(300)
                .HasColumnName("price_log");
            entity.Property(e => e.ReceiptDt).HasColumnName("receipt_dt");
            entity.Property(e => e.ReceiptNo)
                .HasMaxLength(50)
                .HasColumnName("receipt_no");
            entity.Property(e => e.ReturnDt).HasColumnName("return_dt");
            entity.Property(e => e.Returned).HasColumnName("returned");
            entity.Property(e => e.ServiceCharges).HasColumnName("service_charges");
            entity.Property(e => e.SmsnotificationDt)
                .HasColumnType("smalldatetime")
                .HasColumnName("smsnotification_dt");
            entity.Property(e => e.SortFeesAdj).HasColumnName("sort_fees_adj");
            entity.Property(e => e.Source).HasColumnName("source");
            entity.Property(e => e.TaskStatusId).HasColumnName("task_status_id");
            entity.Property(e => e.TaskWithAgentId).HasColumnName("task_with_agent_id");
            entity.Property(e => e.ToWarehouse).HasColumnName("to_warehouse");
            entity.Property(e => e.Totalqty).HasColumnName("totalqty");
            entity.Property(e => e.Transferred).HasColumnName("transferred");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.WithAgent).HasColumnName("with_agent");
            entity.Property(e => e.WithAgentId).HasColumnName("with_agent_id");
            entity.Property(e => e.WithDelivery).HasColumnName("with_delivery");
        });

        modelBuilder.Entity<OrderCase>(entity =>
        {
            entity.ToTable("order_cases");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .HasColumnName("description");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("order_details");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adjusted).HasColumnName("adjusted");
            entity.Property(e => e.BonusUs).HasColumnName("bonusUS");
            entity.Property(e => e.BookingDt)
                .HasColumnType("smalldatetime")
                .HasColumnName("booking_dt");
            entity.Property(e => e.CollectionFeesIq).HasColumnName("collection_feesIQ");
            entity.Property(e => e.DeliveryFeesIq).HasColumnName("Delivery_feesIQ");
            entity.Property(e => e.FinanceStatus).HasColumnName("finance_status");
            entity.Property(e => e.InsertDt)
                .HasColumnType("smalldatetime")
                .HasColumnName("insert_dt");
            entity.Property(e => e.ItemCode).HasColumnName("item_code");
            entity.Property(e => e.Missing).HasColumnName("missing");
            entity.Property(e => e.Notes)
                .HasMaxLength(50)
                .HasColumnName("notes");
            entity.Property(e => e.OnlineOrder).HasColumnName("online_order");
            entity.Property(e => e.OrderId)
                .HasMaxLength(50)
                .HasColumnName("OrderID");
            entity.Property(e => e.OrderNo).HasColumnName("order_no");
            entity.Property(e => e.OriginalAmount).HasColumnName("original_amount");
            entity.Property(e => e.PageReturn).HasColumnName("page_return");
            entity.Property(e => e.Preorder).HasColumnName("preorder");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Removed).HasColumnName("removed");
            entity.Property(e => e.Returned).HasColumnName("returned");
            entity.Property(e => e.Size).HasColumnName("size");
            entity.Property(e => e.Sorted).HasColumnName("sorted");
            entity.Property(e => e.SourcePrice).HasColumnName("source_price");
            entity.Property(e => e.TaskStatusId).HasColumnName("task_status_id");
            entity.Property(e => e.TempMissing).HasColumnName("temp_missing");
            entity.Property(e => e.TrackingNo)
                .HasMaxLength(50)
                .HasColumnName("tracking_no");
            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.ValueCleared).HasColumnName("valueCleared");
            entity.Property(e => e.ValueCollecedBy).HasColumnName("valueCollecedBy");
            entity.Property(e => e.WebsitePrice)
                .HasColumnType("money")
                .HasColumnName("website_price");
            entity.Property(e => e.Whs).HasColumnName("whs");
            entity.Property(e => e.WhsBatchId).HasColumnName("whs_batch_id");
            entity.Property(e => e.Whsid).HasColumnName("whsid");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.ToTable("order_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .HasColumnName("description");
            entity.Property(e => e.Role).HasColumnName("role");
        });

        modelBuilder.Entity<Preorder>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("preorders");

            entity.Property(e => e.CustId).HasColumnName("cust_id");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.InsertDate).HasColumnName("insert_date");
            entity.Property(e => e.OrderDate).HasColumnName("order_date");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Totalamount).HasColumnName("totalamount");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ProductName).HasMaxLength(150);
        });

        modelBuilder.Entity<RegistrationLog>(entity =>
        {
            entity.ToTable("registration_log");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.InsertDate).HasColumnName("insert_date");
            entity.Property(e => e.LabId).HasColumnName("lab_id");
            entity.Property(e => e.RegistrationType).HasColumnName("registration_type");
            entity.Property(e => e.RepresentitiveId).HasColumnName("representitive_id");
        });

        modelBuilder.Entity<SiteRolesnames>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<SampleStatus>(entity =>
        {
            entity.ToTable("sample_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
        });

        modelBuilder.Entity<SerialNumber>(entity =>
        {
            entity.HasKey(e => e.Sno).HasName("PK__SerialNu__CA1EE04C72F5487E");

            entity.ToTable("SerialNumber");

            entity.Property(e => e.Sno)
                .ValueGeneratedNever()
                .HasColumnName("SNo");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.ToTable("services");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .HasColumnName("description");
        });

        modelBuilder.Entity<ShippingCompany>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasMaxLength(50);
        });

        modelBuilder.Entity<Source>(entity =>
        {
            entity.ToTable("sources");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
        });

        modelBuilder.Entity<Specialty>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<SpendingList>(entity =>
        {
            entity.ToTable("spending_list");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Staetag)
                .HasMaxLength(50)
                .HasColumnName("staetag");
        });

        modelBuilder.Entity<StoreSize>(entity =>
        {
            entity.ToTable("store_size");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ItemCode).HasColumnName("item_code");
            entity.Property(e => e.SizeId).HasColumnName("size_id");
        });

        modelBuilder.Entity<Models.TaskStatus>(entity =>
        {
            entity.ToTable("task_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
        });

        modelBuilder.Entity<TblConfig>(entity =>
        {
            entity.ToTable("tbl_config");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MissingFav).HasColumnName("missing_fav");
        });

        modelBuilder.Entity<TblLog>(entity =>
        {
            entity.ToTable("tbl_log");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Pvalue).HasColumnName("pvalue");
        });

        modelBuilder.Entity<TblSize>(entity =>
        {
            entity.ToTable("tbl_size");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
            entity.Property(e => e.GroupIndex).HasColumnName("group_index");
            entity.Property(e => e.OrderIndex).HasColumnName("order_index");
        });

        modelBuilder.Entity<Tbllat>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tbllat");

            entity.Property(e => e.Str)
                .HasMaxLength(50)
                .HasColumnName("str");
        });

        modelBuilder.Entity<Tblsku>(entity =>
        {
            entity.ToTable("tblsku");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Skuurl).HasColumnName("skuurl");
        });

        modelBuilder.Entity<TestDetail>(entity =>
        {
            entity.ToTable("test_details");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TestId).HasColumnName("test_id");
            entity.Property(e => e.TestTypeId).HasColumnName("test_type_id");
        });

        modelBuilder.Entity<TransactionType>(entity =>
        {
            entity.ToTable("transaction_types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
        });

        modelBuilder.Entity<UsCustomer>(entity =>
        {
            entity.ToTable("usCustomer");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CustCityId).HasColumnName("cust_city_id");
            entity.Property(e => e.CustName)
                .HasMaxLength(50)
                .HasColumnName("cust_name");
            entity.Property(e => e.CustPhone)
                .HasMaxLength(50)
                .HasColumnName("cust_phone");
            entity.Property(e => e.CustPwd)
                .HasMaxLength(50)
                .HasColumnName("cust_pwd");
            entity.Property(e => e.CustStateId).HasColumnName("cust_state_id");
        });

        modelBuilder.Entity<UsCustomerReview>(entity =>
        {
            entity.ToTable("usCustomer_review");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BusinessId).HasColumnName("business_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.ReviewScore).HasColumnName("review_score");
        });

        modelBuilder.Entity<Uscity>(entity =>
        {
            entity.ToTable("UScities");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
            entity.Property(e => e.StateId).HasColumnName("StateID");
        });

        modelBuilder.Entity<Siteusers>(entity =>
        {
            entity.ToTable("Siteusers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AreaId).HasColumnName("area_id");
            entity.Property(e => e.BasicSalary).HasColumnName("basic_salary");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.EmploymentDate).HasColumnName("employment_date");
            entity.Property(e => e.FullPackage).HasColumnName("full_package");
            entity.Property(e => e.GenderId).HasColumnName("gender_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.LabId).HasColumnName("lab_id");
            entity.Property(e => e.LastLogin).HasColumnName("last_login");
            entity.Property(e => e.Loginid)
                .HasMaxLength(50)
                .HasColumnName("loginid");
            entity.Property(e => e.MerchantId).HasColumnName("merchant_id");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.Transportation).HasColumnName("transportation");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
            entity.Property(e => e.Userpwd)
                .HasMaxLength(50)
                .HasColumnName("userpwd");
        });

        modelBuilder.Entity<UserMerchant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tracking");

            entity.ToTable("user_merchant");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MerchantId).HasColumnName("merchant_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<VwMostwanted>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_mostwanted");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ImgUrl)
                .HasMaxLength(250)
                .HasColumnName("img_url");
            entity.Property(e => e.PCode)
                .HasMaxLength(50)
                .HasColumnName("p_code");
            entity.Property(e => e.SitePrice)
                .HasColumnType("money")
                .HasColumnName("site_price");
            entity.Property(e => e.Totalorders).HasColumnName("totalorders");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

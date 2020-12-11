using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace stockr.mssql
{
    public partial class StockrContext : DbContext
    {
        readonly IConfiguration _conf;

        public StockrContext()
        {
            _conf = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("app.secret.json", optional: true, reloadOnChange: true)
                .Build();
        }

        public StockrContext(DbContextOptions<StockrContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Conf> Conf { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<MarketOpen> MarketOpen { get; set; }
        public virtual DbSet<Quote> Quote { get; set; }
        public virtual DbSet<QuoteStg> QuoteStg { get; set; }
        public virtual DbSet<QuoteStgStg> QuoteStgStg { get; set; }
        public virtual DbSet<SqlData> SqlData { get; set; }
        public virtual DbSet<Symbol> Symbol { get; set; }
        public virtual DbSet<SymbolStg> SymbolStg { get; set; }
        public virtual DbSet<SymbolsBak> SymbolsBak { get; set; }
        public virtual DbSet<VHawt> VHawt { get; set; }
        public virtual DbSet<VQuoteLatest> VQuoteLatest { get; set; }
        public virtual DbSet<VSymbols> VSymbols { get; set; }
        public virtual DbSet<VSymbolsExtended> VSymbolsExtended { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_conf["DB:ConnectionString"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Conf>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.SystemTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Msg).HasMaxLength(2500);
                entity.Property(e => e.Catg).HasMaxLength(50);
                entity.Property(e => e.Src).HasMaxLength(50);
            });

            modelBuilder.Entity<MarketOpen>(entity =>
            {
                entity.HasIndex(e => new { e.DtOpen, e.DtClose })
                    .HasName("IX_MarketOpen_Dates");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtClose).HasColumnType("datetime");

                entity.Property(e => e.DtOpen).HasColumnType("datetime");
            });

            modelBuilder.Entity<Quote>(entity =>
            {
                entity.HasIndex(e => new { e.Symbol, e.LatestUpdate })
                    .HasName("idx_Quote_symbol_latestUpdate");

                entity.Property(e => e.QuoteId).HasColumnName("QuoteID");

                entity.Property(e => e.AvgTotalVolume).HasColumnName("avgTotalVolume");

                entity.Property(e => e.CalculationPrice)
                    .HasColumnName("calculationPrice")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Change)
                    .HasColumnName("change")
                    .HasColumnType("decimal(10, 7)");

                entity.Property(e => e.ChangePercent)
                    .HasColumnName("changePercent")
                    .HasColumnType("decimal(10, 7)");

                entity.Property(e => e.Close)
                    .HasColumnName("close")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.CloseSource)
                    .HasColumnName("closeSource")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CloseTime).HasColumnName("closeTime");

                entity.Property(e => e.CompanyName)
                    .HasColumnName("companyName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DelayedPrice)
                    .HasColumnName("delayedPrice")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.DelayedPriceTime).HasColumnName("delayedPriceTime");

                entity.Property(e => e.ExtendedChange)
                    .HasColumnName("extendedChange")
                    .HasColumnType("decimal(6, 3)");

                entity.Property(e => e.ExtendedChangePercent)
                    .HasColumnName("extendedChangePercent")
                    .HasColumnType("decimal(6, 3)");

                entity.Property(e => e.ExtendedPrice)
                    .HasColumnName("extendedPrice")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.ExtendedPriceTime).HasColumnName("extendedPriceTime");

                entity.Property(e => e.High)
                    .HasColumnName("high")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.HighSource)
                    .HasColumnName("highSource")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HighTime).HasColumnName("highTime");

                entity.Property(e => e.LastTradeTime).HasColumnName("lastTradeTime");

                entity.Property(e => e.LatestPrice)
                    .HasColumnName("latestPrice")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.LatestSource)
                    .HasColumnName("latestSource")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LatestTime)
                    .HasColumnName("latestTime")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LatestUpdate).HasColumnName("latestUpdate");

                entity.Property(e => e.LatestVolume).HasColumnName("latestVolume");

                entity.Property(e => e.Low)
                    .HasColumnName("low")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.LowSource)
                    .HasColumnName("lowSource")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LowTime).HasColumnName("lowTime");

                entity.Property(e => e.MarketCap).HasColumnName("marketCap");

                entity.Property(e => e.OddLotDelayedPrice)
                    .HasColumnName("oddLotDelayedPrice")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.OddLotDelayedPriceTime).HasColumnName("oddLotDelayedPriceTime");

                entity.Property(e => e.Open)
                    .HasColumnName("open")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.OpenSource)
                    .HasColumnName("openSource")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OpenTime).HasColumnName("openTime");

                entity.Property(e => e.PeRatio)
                    .HasColumnName("peRatio")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.PreviousClose)
                    .HasColumnName("previousClose")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.PreviousVolume).HasColumnName("previousVolume");

                entity.Property(e => e.PrimaryExchange)
                    .HasColumnName("primaryExchange")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol)
                    .HasColumnName("symbol")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Volume).HasColumnName("volume");

                entity.Property(e => e.Week52High)
                    .HasColumnName("week52High")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.Week52Low)
                    .HasColumnName("week52Low")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.YtdChange)
                    .HasColumnName("ytdChange")
                    .HasColumnType("decimal(10, 7)");
            });

            modelBuilder.Entity<QuoteStg>(entity =>
            {
                entity.HasKey(e => e.QuoteId);

                entity.ToTable("Quote_Stg");

                entity.Property(e => e.QuoteId).HasColumnName("QuoteID");

                entity.Property(e => e.AvgTotalVolume).HasColumnName("avgTotalVolume");

                entity.Property(e => e.CalculationPrice)
                    .HasColumnName("calculationPrice")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Change)
                    .HasColumnName("change")
                    .HasColumnType("decimal(10, 7)");

                entity.Property(e => e.ChangePercent)
                    .HasColumnName("changePercent")
                    .HasColumnType("decimal(10, 7)");

                entity.Property(e => e.Close)
                    .HasColumnName("close")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.CloseSource)
                    .HasColumnName("closeSource")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CloseTime).HasColumnName("closeTime");

                entity.Property(e => e.CompanyName)
                    .HasColumnName("companyName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DelayedPrice)
                    .HasColumnName("delayedPrice")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.DelayedPriceTime).HasColumnName("delayedPriceTime");

                entity.Property(e => e.ExtendedChange)
                    .HasColumnName("extendedChange")
                    .HasColumnType("decimal(6, 3)");

                entity.Property(e => e.ExtendedChangePercent)
                    .HasColumnName("extendedChangePercent")
                    .HasColumnType("decimal(6, 3)");

                entity.Property(e => e.ExtendedPrice)
                    .HasColumnName("extendedPrice")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.ExtendedPriceTime).HasColumnName("extendedPriceTime");

                entity.Property(e => e.High)
                    .HasColumnName("high")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.HighSource)
                    .HasColumnName("highSource")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HighTime).HasColumnName("highTime");

                entity.Property(e => e.LastTradeTime).HasColumnName("lastTradeTime");

                entity.Property(e => e.LatestPrice)
                    .HasColumnName("latestPrice")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.LatestSource)
                    .HasColumnName("latestSource")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LatestTime)
                    .HasColumnName("latestTime")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LatestUpdate).HasColumnName("latestUpdate");

                entity.Property(e => e.LatestVolume).HasColumnName("latestVolume");

                entity.Property(e => e.Low)
                    .HasColumnName("low")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.LowSource)
                    .HasColumnName("lowSource")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LowTime).HasColumnName("lowTime");

                entity.Property(e => e.MarketCap).HasColumnName("marketCap");

                entity.Property(e => e.OddLotDelayedPrice)
                    .HasColumnName("oddLotDelayedPrice")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.OddLotDelayedPriceTime).HasColumnName("oddLotDelayedPriceTime");

                entity.Property(e => e.Open)
                    .HasColumnName("open")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.OpenSource)
                    .HasColumnName("openSource")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OpenTime).HasColumnName("openTime");

                entity.Property(e => e.PeRatio)
                    .HasColumnName("peRatio")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.PreviousClose)
                    .HasColumnName("previousClose")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.PreviousVolume).HasColumnName("previousVolume");

                entity.Property(e => e.PrimaryExchange)
                    .HasColumnName("primaryExchange")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol)
                    .HasColumnName("symbol")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Volume).HasColumnName("volume");

                entity.Property(e => e.Week52High)
                    .HasColumnName("week52High")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.Week52Low)
                    .HasColumnName("week52Low")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.YtdChange)
                    .HasColumnName("ytdChange")
                    .HasColumnType("decimal(10, 7)");
            });

            modelBuilder.Entity<QuoteStgStg>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Quote_StgStg");

                entity.Property(e => e.AvgTotalVolume).HasColumnName("avgTotalVolume");

                entity.Property(e => e.CalculationPrice)
                    .HasColumnName("calculationPrice")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Change)
                    .HasColumnName("change")
                    .HasColumnType("decimal(10, 7)");

                entity.Property(e => e.ChangePercent)
                    .HasColumnName("changePercent")
                    .HasColumnType("decimal(10, 7)");

                entity.Property(e => e.Close)
                    .HasColumnName("close")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.CloseSource)
                    .HasColumnName("closeSource")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CloseTime).HasColumnName("closeTime");

                entity.Property(e => e.CompanyName)
                    .HasColumnName("companyName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DelayedPrice)
                    .HasColumnName("delayedPrice")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.DelayedPriceTime).HasColumnName("delayedPriceTime");

                entity.Property(e => e.ExtendedChange)
                    .HasColumnName("extendedChange")
                    .HasColumnType("decimal(6, 3)");

                entity.Property(e => e.ExtendedChangePercent)
                    .HasColumnName("extendedChangePercent")
                    .HasColumnType("decimal(6, 3)");

                entity.Property(e => e.ExtendedPrice)
                    .HasColumnName("extendedPrice")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.ExtendedPriceTime).HasColumnName("extendedPriceTime");

                entity.Property(e => e.High)
                    .HasColumnName("high")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.HighSource)
                    .HasColumnName("highSource")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HighTime).HasColumnName("highTime");

                entity.Property(e => e.LastTradeTime).HasColumnName("lastTradeTime");

                entity.Property(e => e.LatestPrice)
                    .HasColumnName("latestPrice")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.LatestSource)
                    .HasColumnName("latestSource")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LatestTime)
                    .HasColumnName("latestTime")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LatestUpdate).HasColumnName("latestUpdate");

                entity.Property(e => e.LatestVolume).HasColumnName("latestVolume");

                entity.Property(e => e.Low)
                    .HasColumnName("low")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.LowSource)
                    .HasColumnName("lowSource")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LowTime).HasColumnName("lowTime");

                entity.Property(e => e.MarketCap).HasColumnName("marketCap");

                entity.Property(e => e.OddLotDelayedPrice)
                    .HasColumnName("oddLotDelayedPrice")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.OddLotDelayedPriceTime).HasColumnName("oddLotDelayedPriceTime");

                entity.Property(e => e.Open)
                    .HasColumnName("open")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.OpenSource)
                    .HasColumnName("openSource")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OpenTime).HasColumnName("openTime");

                entity.Property(e => e.PeRatio)
                    .HasColumnName("peRatio")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.PreviousClose)
                    .HasColumnName("previousClose")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.PreviousVolume).HasColumnName("previousVolume");

                entity.Property(e => e.PrimaryExchange)
                    .HasColumnName("primaryExchange")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol)
                    .HasColumnName("symbol")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Volume).HasColumnName("volume");

                entity.Property(e => e.Week52High)
                    .HasColumnName("week52High")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.Week52Low)
                    .HasColumnName("week52Low")
                    .HasColumnType("decimal(11, 4)");

                entity.Property(e => e.YtdChange)
                    .HasColumnName("ytdChange")
                    .HasColumnType("decimal(10, 7)");
            });

            modelBuilder.Entity<SqlData>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ki)
                    .HasColumnName("ki")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Valstr)
                    .HasColumnName("valstr")
                    .HasMaxLength(4000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Symbol>(entity =>
            {
                entity.Property(e => e.SymbolId).HasColumnName("SymbolID");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Cik)
                    .HasColumnName("cik")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Createdate)
                    .HasColumnName("createdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Currency)
                    .HasColumnName("currency")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.Exchange)
                    .HasColumnName("exchange")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Figi)
                    .HasColumnName("figi")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IexId)
                    .HasColumnName("iexId")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsEnabled).HasColumnName("isEnabled");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Region)
                    .HasColumnName("region")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol1)
                    .HasColumnName("symbol")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SymbolStg>(entity =>
            {
                entity.HasKey(e => e.SymbolId);

                entity.ToTable("Symbol_Stg");

                entity.Property(e => e.SymbolId).HasColumnName("SymbolID");

                entity.Property(e => e.Cik)
                    .HasColumnName("cik")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Currency)
                    .HasColumnName("currency")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.Exchange)
                    .HasColumnName("exchange")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Figi)
                    .HasColumnName("figi")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IexId)
                    .HasColumnName("iexId")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsEnabled).HasColumnName("isEnabled");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Region)
                    .HasColumnName("region")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol)
                    .HasColumnName("symbol")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SymbolsBak>(entity =>
            {
                entity.ToTable("Symbols_bak");

                entity.HasIndex(e => new { e.DateModified, e.Symbol })
                    .HasName("IX_Symbols_Symbol");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.IexId).HasColumnName("iexId");

                entity.Property(e => e.IsEnabled).HasColumnName("isEnabled");

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SymbolName)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.SymbolType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VHawt>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vHawt");

                entity.Property(e => e.Change)
                    .HasColumnName("change")
                    .HasColumnType("decimal(38, 6)");

                entity.Property(e => e.LatestVolume).HasColumnName("latestVolume");

                entity.Property(e => e.Symbol)
                    .HasColumnName("symbol")
                    .HasMaxLength(500);

                entity.Property(e => e._0)
                    .HasColumnName("$0")
                    .HasColumnType("decimal(32, 5)");

                entity.Property(e => e._1)
                    .HasColumnName("$1")
                    .HasColumnType("decimal(32, 5)");

                entity.Property(e => e._2)
                    .HasColumnName("$2")
                    .HasColumnType("decimal(32, 5)");

                entity.Property(e => e._3)
                    .HasColumnName("$3")
                    .HasColumnType("decimal(32, 5)");

                entity.Property(e => e._4)
                    .HasColumnName("$4")
                    .HasColumnType("decimal(32, 5)");

                entity.Property(e => e._5)
                    .HasColumnName("$5")
                    .HasColumnType("decimal(32, 5)");
            });

            modelBuilder.Entity<VQuoteLatest>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vQuoteLatest");

                entity.Property(e => e.Change)
                    .HasColumnName("change")
                    .HasColumnType("decimal(32, 5)");

                entity.Property(e => e.ChangePercent)
                    .HasColumnName("changePercent")
                    .HasColumnType("decimal(32, 5)");

                entity.Property(e => e.LatestPrice)
                    .HasColumnName("latestPrice")
                    .HasColumnType("decimal(32, 5)");

                entity.Property(e => e.LatestUpdate).HasColumnName("latestUpdate");

                entity.Property(e => e.LatestVolume).HasColumnName("latestVolume");

                entity.Property(e => e.Rw).HasColumnName("rw");

                entity.Property(e => e.Symbol)
                    .HasColumnName("symbol")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<VSymbols>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vSymbols");

                entity.Property(e => e.Createdate)
                    .HasColumnName("createdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.IexId)
                    .HasColumnName("iexId")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsEnabled).HasColumnName("isEnabled");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol)
                    .HasColumnName("symbol")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SymbolId)
                    .HasColumnName("SymbolID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VSymbolsExtended>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vSymbolsExtended");

                entity.Property(e => e.CompanyName)
                    .HasColumnName("companyName")
                    .HasMaxLength(500);

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.PrimaryExchange)
                    .HasColumnName("primaryExchange")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Sector)
                    .HasColumnName("sector")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol)
                    .HasColumnName("symbol")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

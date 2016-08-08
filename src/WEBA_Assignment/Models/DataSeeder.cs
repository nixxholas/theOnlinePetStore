using WEBA_ASSIGNMENT.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
//Reference:
//http://stackoverflow.com/questions/34536021/seed-initial-data-in-entity-framework-7-rc-1-and-asp-net-mvc-6
namespace WEBA_ASSIGNMENT.Models
{
    public static class DataSeeder
    {
        public static async void SeedData(this IApplicationBuilder app)
        {
            var db = app.ApplicationServices.GetService<ApplicationDbContext>();

            //This is to make sure that the migration, seeding is done only when there are no students found in User table.
            //If there are student records found in User table, I will assume that, better don't do seeding.
            //The if statement block is to check whether there are records in the User table.
            //The db.Users will know. You call the Any() method to check
            //The purpose of having this if block here (which covers amost the entire SeedData() method)

            //Caution: Clear all the tables in the database first.
            db.Database.Migrate();


            //RoleStore needs using Microsoft.AspNet.Identity.EntityFramework;
            var identityRoleStore = new RoleStore<IdentityRole>(db);
            var identityRoleManager = new RoleManager<IdentityRole>(identityRoleStore, null, null, null, null, null);

            var superAdminRole = new IdentityRole { Name = "SUPER ADMIN" };
            await identityRoleManager.CreateAsync(superAdminRole);

            var adminRole = new IdentityRole { Name = "ADMIN" };
            await identityRoleManager.CreateAsync(adminRole);

            var officerRole = new IdentityRole { Name = "OFFICER" };
            await identityRoleManager.CreateAsync(officerRole);

            var customerRole = new IdentityRole { Name = "USER" };
            await identityRoleManager.CreateAsync(customerRole);

            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore, null, null, null, null, null, null, null, null);


            var danielUser = new ApplicationUser { UserName = "88881", Email = "DANIEL@EMU.COM", FullName = "DANIEL" };
            PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
            danielUser.PasswordHash = ph.HashPassword(danielUser, "P@ssw0rd"); //More complex password
            await userManager.CreateAsync(danielUser);

            await userManager.AddToRoleAsync(danielUser, "SUPER ADMIN");



            var susanUser = new ApplicationUser { UserName = "88882", Email = "SUSAN@EMU.COM", FullName = "SUSAN" };
            susanUser.PasswordHash = ph.HashPassword(susanUser, "P@ssw0rd"); //More complex password
            await userManager.CreateAsync(susanUser);

            await userManager.AddToRoleAsync(susanUser, "ADMIN");



            var randyUser = new ApplicationUser { UserName = "88883", Email = "RANDY@EMU.COM", FullName = "RANDY" };
            randyUser.PasswordHash = ph.HashPassword(randyUser, "P@ssw0rd"); //More complex password
            await userManager.CreateAsync(randyUser);

            await userManager.AddToRoleAsync(randyUser, "OFFICER");


            var thomasUser = new ApplicationUser { UserName = "88884", Email = "THOMAS@EMU.COM", FullName = "THOMAS" };
            thomasUser.PasswordHash = ph.HashPassword(thomasUser, "P@ssw0rd"); //More complex password

            await userManager.CreateAsync(thomasUser);
            await userManager.AddToRoleAsync(thomasUser, "OFFICER");

            var benUser = new ApplicationUser { UserName = "88885", Email = "BEN@EMU.COM", FullName = "BEN" };
            benUser.PasswordHash = ph.HashPassword(benUser, "P@ssw0rd"); //More complex password

            await userManager.CreateAsync(benUser);

            await userManager.AddToRoleAsync(benUser, "OFFICER");

            var gabrielUser = new ApplicationUser { UserName = "88886", Email = "GABRIEL@EMU.COM", FullName = "GABRIEL" };
            gabrielUser.PasswordHash = ph.HashPassword(gabrielUser, "P@ssw0rd"); //More complex password

            //Use the UserManager class instance, userManager
            //which manages the many-to-many AspNetUserRoles table
            //to create a user, Gabriel.
            await userManager.CreateAsync(gabrielUser); //Add the user
                                                        //Use the UserManager class instance, userManager
                                                        //which also manages the many-to-many AspNetUserRoles table
                                                        //to create a relationship describing that, Gabriel user is an OFFICER role user
            await userManager.AddToRoleAsync(gabrielUser, "OFFICER");

            var fredUser = new ApplicationUser { UserName = "88887", Email = "FRED@EMU.COM", FullName = "FRED" };
            fredUser.PasswordHash = ph.HashPassword(fredUser, "P@ssw0rd"); //More complex password

            //Use the UserManager class instance, userManager
            //which manages the many-to-many AspNetUserRoles table
            //to create a user, Fred.
            await userManager.CreateAsync(fredUser); //Add the user
                                                     //Use the UserManager class instance, userManager
                                                     //which also manages the many-to-many AspNetUserRoles table
                                                     //to create a relationship describing that, Fred user is an OFFICER role user
            await userManager.AddToRoleAsync(fredUser, "OFFICER");

            /**
             * DataSeeder for the Status Table
             * 
             * Stores all the allowed Metric Statuses
             * 
             * **/

            Status Available, Unavailable, OutOfStock;

            Available = new Status()
            {
                StatusName = "Available"
            };
            db.Statuses.Add(Available);
            
            Unavailable = new Status()
            {
                StatusName = "Unavailable"
            };
            db.Statuses.Add(Unavailable);
            
            OutOfStock = new Status()
            {
                StatusName = "Out Of Stock"
            };
            db.Statuses.Add(OutOfStock);

            /**
             * DataSeeder for the PresetMetric Table
             * 
             * Current types of Standard Metrics
             * based on the International System of Units
             * a.k.a SI Units
             * 
             * Length, Mass, Volumes & Generics (eg. Sizes)
             * 
             * **/

            PresetMetric Millimetre, Centimetre, Metre, Kilometre,
                Pounds, Tonnes, Milligrams, Grams, Kilograms, Millilitres,
                Litres, Small, Medium, Large, XLarge, XXLarge, 
                XXXLarge, XXXXLarge, Unit;

            Millimetre = new PresetMetric()
            {
                MetricType = "Length",
                MetricSubType = "Millimetre",
                MetricCharacter = "mm"
            };
            db.PresetMetrics.Add(Millimetre);

            Centimetre = new PresetMetric()
            {
                MetricType = "Length",
                MetricSubType = "Centimetre",
                MetricCharacter = "cm"
            };
            db.PresetMetrics.Add(Centimetre);

            Metre = new PresetMetric()
            {
                MetricType = "Length",
                MetricSubType = "Metres",
                MetricCharacter = "m"
            };
            db.PresetMetrics.Add(Metre);

            //Kilometre = new PresetMetric()
            //{
            //    MetricType = "Length",
            //    MetricSubType = "Kilometres",
            //    MetricCharacter = "km"
            //};
            //db.PresetMetrics.Add(Kilometre);

            Pounds = new PresetMetric()
            {
                MetricType = "Mass",
                MetricSubType = "Pounds",
                MetricCharacter = "lbs"
            };
            db.PresetMetrics.Add(Pounds);

            //Tonnes = new PresetMetric()
            //{
            //    MetricType = "Length",
            //    MetricSubType = "Tonnes",
            //    MetricCharacter = "ton"
            //};
            //db.PresetMetrics.Add(Tonnes);

            Milligrams = new PresetMetric()
            {
                MetricType = "Mass",
                MetricSubType = "Milligrams",
                MetricCharacter = "mg"
            };
            db.PresetMetrics.Add(Milligrams);

            Grams = new PresetMetric()
            {
                MetricType = "Mass",
                MetricSubType = "Grams",
                MetricCharacter = "g"
            };
            db.PresetMetrics.Add(Grams);

            Kilograms = new PresetMetric()
            {
                MetricType = "Mass",
                MetricSubType = "Kilograms",
                MetricCharacter = "kg"
            };
            db.PresetMetrics.Add(Kilograms);

            Millilitres = new PresetMetric()
            {
                MetricType = "Volume",
                MetricSubType = "Millilitres",
                MetricCharacter = "ml"
            };
            db.PresetMetrics.Add(Millilitres);

            Litres = new PresetMetric()
            {
                MetricType = "Volume",
                MetricSubType = "Litres",
                MetricCharacter = "l"
            };
            db.PresetMetrics.Add(Litres);

            Unit = new PresetMetric()
            {
                MetricType = "Generic",
                MetricSubType = "Unit",
                MetricCharacter = "Unit"
            };
            db.PresetMetrics.Add(Unit);

            Small = new PresetMetric()
            {
                MetricType = "Generic",
                MetricSubType = "Small",
                MetricCharacter = "S"
            };
            db.PresetMetrics.Add(Small);

            Medium = new PresetMetric()
            {
                MetricType = "Generic",
                MetricSubType = "Medium",
                MetricCharacter = "M"
            };
            db.PresetMetrics.Add(Medium);

            Large = new PresetMetric()
            {
                MetricType = "Generic",
                MetricSubType = "Large",
                MetricCharacter = "L"
            };
            db.PresetMetrics.Add(Large);

            XLarge = new PresetMetric()
            {
                MetricType = "Generic",
                MetricSubType = "XLarge",
                MetricCharacter = "XL"
            };
            db.PresetMetrics.Add(Large);

            XXLarge = new PresetMetric()
            {
                MetricType = "Generic",
                MetricSubType = "XXLarge",
                MetricCharacter = "XXL"
            };
            db.PresetMetrics.Add(XXLarge);

            XXXLarge = new PresetMetric()
            {
                MetricType = "Generic",
                MetricSubType = "XXXLarge",
                MetricCharacter = "XXXL"
            };
            db.PresetMetrics.Add(XXXLarge);

            XXXXLarge = new PresetMetric()
            {
                MetricType = "Generic",
                MetricSubType = "XXXXLarge",
                MetricCharacter = "XXXXL"
            };
            db.PresetMetrics.Add(XXXXLarge);

            // DataSeeder for the Visibility Table

            Visibility Visible, VisibleIgnored, Hidden;

            VisibleIgnored = new Visibility()
            {
                VisibilityName = "Visible (Ignoring Start Date and End Date)"
            };
            db.Visibilities.Add(VisibleIgnored);

            Visible = new Visibility()
            {
                VisibilityName = "Visible (with Start Date and End Date)"
            };
            db.Visibilities.Add(Visible);

            Hidden = new Visibility()
            {
                VisibilityName = "Hidden"
            };
            db.Visibilities.Add(Hidden);

            // DataSeeder for the Category Table

            Category Clearance, Donate, DogFood, CatFood, SmallAnimal, Treats,
                Shampoo, Grooming, DentalCare, Supplements, Toys, Accessories,
                ToiletryNeeds, TrainingAids, FleaNTickControl, GiftVouchers, PwP;

            Clearance = new Category()
            {
                CatName = "CLEARANCE SALE",
                VisibilityId = VisibleIgnored.VisibilityId,
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Categories.Add(Clearance);

            Donate = new Category()
            {
                CatName = "DONATE",
                VisibilityId = VisibleIgnored.VisibilityId,
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Categories.Add(Donate);

            DogFood = new Category()
            {
                CatName = "DOG FOOD",
                VisibilityId = VisibleIgnored.VisibilityId,
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Categories.Add(DogFood);

            CatFood = new Category()
            {
                CatName = "CAT FOOD",
                VisibilityId = VisibleIgnored.VisibilityId,
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Categories.Add(CatFood);

            SmallAnimal = new Category()
            {
                CatName = "SMALL ANIMAL",
                VisibilityId = VisibleIgnored.VisibilityId,
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Categories.Add(SmallAnimal);

            Treats = new Category()
            {
                CatName = "TREATS",
                VisibilityId = VisibleIgnored.VisibilityId,
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Categories.Add(Treats);

            Shampoo = new Category()
            {
                CatName = "SHAMPOO",
                VisibilityId = VisibleIgnored.VisibilityId,
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Categories.Add(Shampoo);

            Grooming = new Category()
            {
                CatName = "GROOMING",
                VisibilityId = VisibleIgnored.VisibilityId,
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Categories.Add(Grooming);

            DentalCare = new Category()
            {
                CatName = "DENTAL CARE",
                VisibilityId = VisibleIgnored.VisibilityId,
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Categories.Add(DentalCare);

            Supplements = new Category()
            {
                CatName = "SUPPLEMENTS",
                VisibilityId = VisibleIgnored.VisibilityId,
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Categories.Add(Supplements);

            Toys = new Category()
            {
                CatName = "TOYS",
                VisibilityId = VisibleIgnored.VisibilityId,
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Categories.Add(Toys);

            Accessories = new Category()
            {
                CatName = "ACCESSORIES",
                VisibilityId = VisibleIgnored.VisibilityId,
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Categories.Add(Accessories);

            ToiletryNeeds = new Category()
            {
                CatName = "TOILETRY NEEDS",
                VisibilityId = VisibleIgnored.VisibilityId,
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Categories.Add(ToiletryNeeds);

            TrainingAids = new Category()
            {
                CatName = "TRAINING AIDS",
                VisibilityId = VisibleIgnored.VisibilityId,
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Categories.Add(TrainingAids);

            FleaNTickControl = new Category()
            {
                CatName = "FLEA & TICK CONTROL",
                VisibilityId = VisibleIgnored.VisibilityId,
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Categories.Add(FleaNTickControl);

            GiftVouchers = new Category()
            {
                CatName = "GIFT VOUCHERS",
                VisibilityId = VisibleIgnored.VisibilityId,
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Categories.Add(GiftVouchers);

            PwP = new Category()
            {
                CatName = "PURCHASE WITH PURCHASE",
                VisibilityId = VisibleIgnored.VisibilityId,
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Categories.Add(PwP);

            // End Of Data Seeding for the Category Table

            // DataSeeder for the BrandPhoto Table
            // Seed in a dummy image for Brands that have no preincluded image
            BrandPhoto Dummy;

            Dummy = new BrandPhoto()
            {
                Format = "jpg",
                Height = 120,
                ImageSize = 1692,
                PublicCloudinaryId = "Brands/u0ofsgsn9b1q6tlwrkxg",
                SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466794773/Brands/u0ofsgsn9b1q6tlwrkxg.jpg",
                Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466794773/Brands/u0ofsgsn9b1q6tlwrkxg.jpg",
                Version = 1466794773,
                Width = 171,
                CreatedById = benUser.Id
            };

            // DataSeeder for the Brands Table
            Brands AddMate, Addiction, Advocate, Aeolus, AlfalfaKing, AmericanPetDiner,
                 Angels, AngelsEye, APT1022, AvoDerm, Azmira, B2K, Bark2Basics, BasicInstinct,
                 Beaphar, Bosch, BreederCelect, BritCare, BudleBudle, Burgess, ByNature,
                 CanadaLitter, CanineTribute, CanzRealMeat, CatsBest, CatanDogs, CatIt,
                 Chitocure, CloudStar, CoatHandler, DailyDelight, DancingPaws, DermCare,
                 DogIt, EaglePack, EaglePro, EarthBath, EcoBiopet, Espree, EVO, EyeEnvy,
                 FelineTribute, Ferplast, Fish4Pets, Forbis, Frontline, FussieCat, Gex,
                 GreedyDog, Hagen, HappiDoggy, HiTekNaturals, HolisticBites, HolisticSelect,
                 IDPets, JML, JustFish, JW, Kaning, KarenPryor, KitCat, KittyLitty, KMR,
                 Life4K9, Loveem, Marukan, MediterraneanNatural, Merrick, Mikki, MyPottyPad,
                 MysticSea, NaturalBalance, NaturesHarvest, NaturesVariety, NinaOttosson,
                 Nootie, NutraGold, NutriVet, Nutripe, OldMotherHubbard, OvenBakedTradition,
                 Oxbow, Pampets, Parallel, PeeWee, Percell, PetBotanics, PetLink, PetAlive,
                 Petcare, Petmate, Pinnacle, Pompey, Primal, ProDenPlaqueOff,
                 ProfessionalPetProduct, Revolution, RichardsOrganics, Sanabelle, Sanxia,
                 Schesir, Sentry, SmallPetSelect, SolidGold, SPALavish, Spotty, SynergyLabs,
                 TasteOfTheWild, TheNaturalPetTreatCompany, TikiCat, TimberWolfOrganics, TomNPus,
                 TopLife, TripleCrown, TropiClean, Virbac, Vitakraft, VivaLaSpa, Wellness,
                 WestPawDesign, WildSanko, Wishbone, Zignature, Zukes;

            /**
             * Brand IDs are automatically created and are in a numeric increment format.
             * 
             */
            AddMate = new Brands()
            {
                BrandName = "Add Mate",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 96,
                    ImageSize = 18363,
                    PublicCloudinaryId = "Brands/tit6egwg7gbqc6pgemmx",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466703383/Brands/tit6egwg7gbqc6pgemmx.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466703383/Brands/tit6egwg7gbqc6pgemmx.jpg",
                    Version = 1466703383,
                    Width = 380,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(AddMate);

            Addiction = new Brands()
            {
                BrandName = "Addiction",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 27080,
                    PublicCloudinaryId = "Brands/f4gr1p4fwl5oqszf4rlb",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466734352/Brands/f4gr1p4fwl5oqszf4rlb.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466734352/Brands/f4gr1p4fwl5oqszf4rlb.jpg",
                    Version = 1466734352,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Addiction);

            Advocate = new Brands()
            {
                BrandName = "Advocate",
                BrandPhoto = new BrandPhoto()
                {
                    // Imaginery BrandId auto-magically created.
                    // ValueGeneratedOnAdd() on ApplicationDbContext
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 26351,
                    PublicCloudinaryId = "Brands/vbap8qirkbg3puiri0da",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466740096/Brands/vbap8qirkbg3puiri0da.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466740096/Brands/vbap8qirkbg3puiri0da.jpg",
                    Version = 1466740096,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Advocate);

            Aeolus = new Brands()
            {
                BrandName = "Aeolus Dog Dryer",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 23113,
                    PublicCloudinaryId = "Brands/ndjfdkxd8pgawdszpxnt",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466740233/Brands/ndjfdkxd8pgawdszpxnt.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466740233/Brands/ndjfdkxd8pgawdszpxnt.jpg",
                    Version = 1466740233,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Aeolus);

            AlfalfaKing = new Brands()
            {
                BrandName = "Alfalfa King",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 45606,
                    PublicCloudinaryId = "Brands/mkkg3jfvlrkomp5s3hnx",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466740327/Brands/mkkg3jfvlrkomp5s3hnx.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466740327/Brands/mkkg3jfvlrkomp5s3hnx.jpg",
                    Version = 1466740327,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(AlfalfaKing);

            AmericanPetDiner = new Brands()
            {
                BrandName = "American Pet Diner",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 251,
                    ImageSize = 7955,
                    PublicCloudinaryId = "Brands/hfvgs7v6zbkowlvvtp8l",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466066280/Brands/hfvgs7v6zbkowlvvtp8l.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466066280/Brands/hfvgs7v6zbkowlvvtp8l.jpg",
                    Version = 1466066280,
                    Width = 201,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(AmericanPetDiner);

            Angels = new Brands()
            {
                BrandName = "Angel's",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 251,
                    ImageSize = 6036,
                    PublicCloudinaryId = "Brands/uzhh398ujqis7wqdqjjx",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466743616/Brands/uzhh398ujqis7wqdqjjx.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466743616/Brands/uzhh398ujqis7wqdqjjx.jpg",
                    Version = 1466743616,
                    Width = 150,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Angels);

            AngelsEye = new Brands()
            {
                BrandName = "Angel's Eye",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 33646,
                    PublicCloudinaryId = "Brands/idznhlnfjzx7h4zwsn6k",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466747277/Brands/idznhlnfjzx7h4zwsn6k.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466747277/Brands/idznhlnfjzx7h4zwsn6k.jpg",
                    Version = 1466747277,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(AngelsEye);

            APT1022 = new Brands()
            {
                BrandName = "APT 1022",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 182,
                    ImageSize = 9385,
                    PublicCloudinaryId = "Brands/uywfq0isejzowlbcncco",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466747523/Brands/uywfq0isejzowlbcncco.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466747523/Brands/uywfq0isejzowlbcncco.jpg",
                    Version = 1466747523,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(APT1022);

            AvoDerm = new Brands()
            {
                BrandName = "AvoDerm",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 24788,
                    PublicCloudinaryId = "Brands/zscuuofknz7ttyh0kqzj",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466747612/Brands/zscuuofknz7ttyh0kqzj.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466747612/Brands/zscuuofknz7ttyh0kqzj.jpg",
                    Version = 1466747612,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(AvoDerm);

            Azmira = new Brands()
            {
                BrandName = "Azmira",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 484,
                    ImageSize = 86406,
                    PublicCloudinaryId = "Brands/ynickkk94i1nekjbza1w",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466750904/Brands/ynickkk94i1nekjbza1w.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466750904/Brands/ynickkk94i1nekjbza1w.jpg",
                    Version = 1466750904,
                    Width = 500,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Azmira);

            B2K = new Brands()
            {
                BrandName = "B2K",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 350,
                    ImageSize = 72120,
                    PublicCloudinaryId = "Brands/towdmksonwd8adlbhplc",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466751162/Brands/towdmksonwd8adlbhplc.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466751162/Brands/towdmksonwd8adlbhplc.jpg",
                    Version = 1466751162,
                    Width = 752,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(B2K);

            Bark2Basics = new Brands()
            {
                BrandName = "Bark 2 Basics",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 37715,
                    PublicCloudinaryId = "Brands/ow8rcpkh0x2yqh97pk02",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466751231/Brands/ow8rcpkh0x2yqh97pk02.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466751231/Brands/ow8rcpkh0x2yqh97pk02.jpg",
                    Version = 1466751231,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Bark2Basics);

            BasicInstinct = new Brands()
            {
                BrandName = "Basic Instinct",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 136,
                    ImageSize = 26655,
                    PublicCloudinaryId = "Brands/h42w6s5w8cqlotkoiyi7",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466751422/Brands/h42w6s5w8cqlotkoiyi7.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466751422/Brands/h42w6s5w8cqlotkoiyi7.jpg",
                    Version = 1466751422,
                    Width = 220,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(BasicInstinct);

            Beaphar = new Brands()
            {
                BrandName = "Beaphar",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 800,
                    ImageSize = 165308,
                    PublicCloudinaryId = "Brands/ak8blj54xjqou1wyh184",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466751518/Brands/ak8blj54xjqou1wyh184.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466751518/Brands/ak8blj54xjqou1wyh184.jpg",
                    Version = 1466751518,
                    Width = 1300,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Beaphar);

            Bosch = new Brands()
            {
                BrandName = "Bosch",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 50562,
                    PublicCloudinaryId = "Brands/qfsndfihcpvwybgdmbgc",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466751586/Brands/qfsndfihcpvwybgdmbgc.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466751586/Brands/qfsndfihcpvwybgdmbgc.jpg",
                    Version = 1466751586,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Bosch);

            BreederCelect = new Brands()
            {
                BrandName = "Breeder Celect",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 300,
                    ImageSize = 9257,
                    PublicCloudinaryId = "Brands/wimeemxthe5fqrfsvjikc",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466751678/Brands/wimeemxthe5fqrfsvjik.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466751678/Brands/wimeemxthe5fqrfsvjik.jpg",
                    Version = 1466751678,
                    Width = 300,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(BreederCelect);

            BritCare = new Brands()
            {
                BrandName = "Brit Care",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 340,
                    ImageSize = 42153,
                    PublicCloudinaryId = "Brands/xplwqusk9dksdi30rwv6",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466751806/Brands/xplwqusk9dksdi30rwv6.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466751806/Brands/xplwqusk9dksdi30rwv6.jpg",
                    Version = 1466751806,
                    Width = 366,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(BritCare);

            BudleBudle = new Brands()
            {
                BrandName = "Budle'Budle",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 170,
                    ImageSize = 36955,
                    PublicCloudinaryId = "Brands/pstmlpe0bvrj6yspt8oe",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466751918/Brands/pstmlpe0bvrj6yspt8oe.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466751918/Brands/pstmlpe0bvrj6yspt8oe.jpg",
                    Version = 1466751918,
                    Width = 350,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(BudleBudle);

            Burgess = new Brands()
            {
                BrandName = "Burgess",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 32296,
                    PublicCloudinaryId = "Brands/squ2ywclp04aaktlk6fr",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466753177/Brands/squ2ywclp04aaktlk6fr.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466753177/Brands/squ2ywclp04aaktlk6fr.jpg",
                    Version = 1466753177,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Burgess);

            ByNature = new Brands()
            {
                BrandName = "By Nature",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 106,
                    ImageSize = 9971,
                    PublicCloudinaryId = "Brands/z7trhwxwt4ekyji3nw4m",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466753483/Brands/z7trhwxwt4ekyji3nw4m.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466753483/Brands/z7trhwxwt4ekyji3nw4m.jpg",
                    Version = 1466753483,
                    Width = 289,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(ByNature);

            CanadaLitter = new Brands()
            {
                BrandName = "Canada Litter",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 302,
                    ImageSize = 107824,
                    PublicCloudinaryId = "Brands/xd6seuazssacj9hjv9cx",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466753863/Brands/xd6seuazssacj9hjv9cx.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466753863/Brands/xd6seuazssacj9hjv9cx.jpg",
                    Version = 1466753863,
                    Width = 640,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(CanadaLitter);

            CanineTribute = new Brands()
            {
                BrandName = "Canine Tribute",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 27984,
                    PublicCloudinaryId = "Brands/bprxjqelcrrfusmk8qla",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466755982/Brands/bprxjqelcrrfusmk8qla.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466755982/Brands/bprxjqelcrrfusmk8qla.jpg",
                    Version = 1466755982,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(CanineTribute);

            CanzRealMeat = new Brands()
            {
                BrandName = "Canz Real Meat",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 30489,
                    PublicCloudinaryId = "Brands/jdjgqtfudzbc93q9cqkb",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466756050/Brands/jdjgqtfudzbc93q9cqkb.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466756050/Brands/jdjgqtfudzbc93q9cqkb.jpg",
                    Version = 1466756050,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(CanzRealMeat);

            CatsBest = new Brands()
            {
                BrandName = "Cat's Best",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 300,
                    ImageSize = 9257,
                    PublicCloudinaryId = "Brands/wimeemxthe5fqrfsvjikc",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466751678/Brands/wimeemxthe5fqrfsvjik.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466751678/Brands/wimeemxthe5fqrfsvjik.jpg",
                    Version = 1466751678,
                    Width = 300,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(CatsBest);

            CatanDogs = new Brands()
            {
                BrandName = "CatanDog's",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 84,
                    ImageSize = 16598,
                    PublicCloudinaryId = "Brands/da9ix14ckujkywexvwmo",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466756145/Brands/da9ix14ckujkywexvwmo.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466756145/Brands/da9ix14ckujkywexvwmo.jpg",
                    Version = 1466756145,
                    Width = 180,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(CatanDogs);

            CatIt = new Brands()
            {
                BrandName = "CatIt",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 28074,
                    PublicCloudinaryId = "Brands/soax69czeighglbncjiz",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466756199/Brands/soax69czeighglbncjiz.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466756199/Brands/soax69czeighglbncjiz.jpg",
                    Version = 1466756199,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(CatIt);

            Chitocure = new Brands()
            {
                BrandName = "Chitocure",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 33511,
                    PublicCloudinaryId = "Brands/cjpqaktzqn2xyzt6yzcs",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466756258/Brands/cjpqaktzqn2xyzt6yzcs.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466756258/Brands/cjpqaktzqn2xyzt6yzcs.jpg",
                    Version = 1466756258,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Chitocure);

            CloudStar = new Brands()
            {
                BrandName = "Cloud Star",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 40846,
                    PublicCloudinaryId = "Brands/jwufpgyc3ncvzmsc0qrk",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466264399/Brands/jwufpgyc3ncvzmsc0qrk.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466264399/Brands/jwufpgyc3ncvzmsc0qrk.jpg",
                    Version = 1466264399,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(CloudStar);

            CoatHandler = new Brands()
            {
                BrandName = "Coat Handler",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 38016,
                    PublicCloudinaryId = "Brands/py1x45ks49cx1mupkoas",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466756377/Brands/py1x45ks49cx1mupkoas.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466756377/Brands/py1x45ks49cx1mupkoas.jpg",
                    Version = 1466756377,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(CoatHandler);

            DailyDelight = new Brands()
            {
                BrandName = "Daily Delight",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 34676,
                    PublicCloudinaryId = "Brands/ycstvr1fkicrlbzyo8e9",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466756449/Brands/ycstvr1fkicrlbzyo8e9.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466756449/Brands/ycstvr1fkicrlbzyo8e9.jpg",
                    Version = 1466756449,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(DailyDelight);

            DancingPaws = new Brands()
            {
                BrandName = "Dancing Paws",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 278,
                    ImageSize = 54203,
                    PublicCloudinaryId = "Brands/fe3xkezop49tgn8byvaz",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466756540/Brands/fe3xkezop49tgn8byvaz.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466756540/Brands/fe3xkezop49tgn8byvaz.jpg",
                    Version = 1466756540,
                    Width = 500,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(DancingPaws);

            DermCare = new Brands()
            {
                BrandName = "Derm Care",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 33033,
                    PublicCloudinaryId = "Brands/xgbtldplf6ldedrjydft",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466262801/Brands/xgbtldplf6ldedrjydft.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466262801/Brands/xgbtldplf6ldedrjydft.jpg",
                    Version = 1466262801,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(DermCare);

            DogIt = new Brands()
            {
                BrandName = "DogIt",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 24489,
                    PublicCloudinaryId = "Brands/dne1hajhoyxej5xvorxe",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466756705/Brands/dne1hajhoyxej5xvorxe.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466756705/Brands/dne1hajhoyxej5xvorxe.jpg",
                    Version = 1466756705,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(DogIt);

            EaglePack = new Brands()
            {
                BrandName = "Eagle Pack",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 39278,
                    PublicCloudinaryId = "Brands/x6iatnduivdgljx8gvwc",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466756785/Brands/x6iatnduivdgljx8gvwc.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466756785/Brands/x6iatnduivdgljx8gvwc.jpg",
                    Version = 1466756785,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(EaglePack);

            EaglePro = new Brands()
            {
                BrandName = "Eagle Pro Holistic",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 68,
                    ImageSize = 27836,
                    PublicCloudinaryId = "Brands/bki98o0irbeec3g43tw0",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466756926/Brands/bki98o0irbeec3g43tw0.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466756926/Brands/bki98o0irbeec3g43tw0.jpg",
                    Version = 1466756926,
                    Width = 169,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(EaglePro);

            EarthBath = new Brands()
            {
                BrandName = "EarthBath",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 30363,
                    PublicCloudinaryId = "Brands/d1dilcatll5ktesh01ii",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466756987/Brands/d1dilcatll5ktesh01ii.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466756987/Brands/d1dilcatll5ktesh01ii.jpg",
                    Version = 1466756987,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(EarthBath);

            //Undone
            EcoBiopet = new Brands()
            {
                BrandName = "Eco-Biopet",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 33033,
                    PublicCloudinaryId = "Brands/xgbtldplf6ldedrjydft",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466262801/Brands/xgbtldplf6ldedrjydft.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466262801/Brands/xgbtldplf6ldedrjydft.jpg",
                    Version = 1466262801,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(EcoBiopet);

            Espree = new Brands()
            {
                BrandName = "Espree",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 1392,
                    ImageSize = 1866086,
                    PublicCloudinaryId = "Brands/jgvqk3ljtjsd0m86asui",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466757634/Brands/jgvqk3ljtjsd0m86asui.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466757634/Brands/jgvqk3ljtjsd0m86asui.jpg",
                    Version = 1466757634,
                    Width = 3198,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Espree);

            EVO = new Brands()
            {
                BrandName = "EVO",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 41551,
                    PublicCloudinaryId = "Brands/vxso2u9bjhbt9kcxzcy3",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466757696/Brands/vxso2u9bjhbt9kcxzcy3.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466757696/Brands/vxso2u9bjhbt9kcxzcy3.jpg",
                    Version = 1466757696,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(EVO);

            EyeEnvy = new Brands()
            {
                BrandName = "Eye Envy",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 30897,
                    PublicCloudinaryId = "Brands/quryogltxqeujt2hppmk",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466757887/Brands/quryogltxqeujt2hppmk.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466757887/Brands/quryogltxqeujt2hppmk.jpg",
                    Version = 1466757887,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(EyeEnvy);

            FelineTribute = new Brands()
            {
                BrandName = "Feline Tribute",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 26738,
                    PublicCloudinaryId = "Brands/bcsd9drkatxjwnmzddqp",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466757938/Brands/bcsd9drkatxjwnmzddqp.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466757938/Brands/bcsd9drkatxjwnmzddqp.jpg",
                    Version = 1466757938,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(FelineTribute);

            Ferplast = new Brands()
            {
                BrandName = "Ferplast",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 23836,
                    PublicCloudinaryId = "Brands/ntpyhbft5bt9dnsjrjsf",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466758045/Brands/ntpyhbft5bt9dnsjrjsf.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466758045/Brands/ntpyhbft5bt9dnsjrjsf.jpg",
                    Version = 1466758045,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Ferplast);

            Fish4Pets = new Brands()
            {
                BrandName = "Fish4Pets",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 30638,
                    PublicCloudinaryId = "Brands/jp9fxtum9tyyvfydmuf6",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466758148/Brands/jp9fxtum9tyyvfydmuf6.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466758148/Brands/jp9fxtum9tyyvfydmuf6.jpg",
                    Version = 1466758148,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Fish4Pets);

            Forbis = new Brands()
            {
                BrandName = "Forbis",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 162,
                    ImageSize = 13675,
                    PublicCloudinaryId = "Brands/mhzvzex1amdlhmkx21zs",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466758221/Brands/mhzvzex1amdlhmkx21zs.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466758221/Brands/mhzvzex1amdlhmkx21zs.jpg",
                    Version = 1466758221,
                    Width = 250,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Forbis);

            Frontline = new Brands()
            {
                BrandName = "Frontline",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 200,
                    ImageSize = 23389,
                    PublicCloudinaryId = "Brands/gdfyteyab1z0bmwe0tpt",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466758323/Brands/gdfyteyab1z0bmwe0tpt.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466758323/Brands/gdfyteyab1z0bmwe0tpt.jpg",
                    Version = 1466758323,
                    Width = 600,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Frontline);

            FussieCat = new Brands()
            {
                BrandName = "Fussie Cat",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 29737,
                    PublicCloudinaryId = "Brands/hrnlojqbf6ys7estxblk",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466758402/Brands/hrnlojqbf6ys7estxblk.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466758402/Brands/hrnlojqbf6ys7estxblk.jpg",
                    Version = 1466758402,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(FussieCat);

            Gex = new Brands()
            {
                BrandName = "Gex",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 300,
                    ImageSize = 6782,
                    PublicCloudinaryId = "Brands/pihcxnqur1k9a5lbz10o",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466758473/Brands/pihcxnqur1k9a5lbz10o.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466758473/Brands/pihcxnqur1k9a5lbz10o.jpg",
                    Version = 1466758473,
                    Width = 300,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Gex);

            GreedyDog = new Brands()
            {
                BrandName = "Greedy Dog",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 1737,
                    ImageSize = 105937,
                    PublicCloudinaryId = "Brands/wbxa34qe4pl1dfjbvx7u",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466758554/Brands/wbxa34qe4pl1dfjbvx7u.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466758554/Brands/wbxa34qe4pl1dfjbvx7u.jpg",
                    Version = 1466758554,
                    Width = 801,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(GreedyDog);

            Hagen = new Brands()
            {
                BrandName = "Hagen",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 1200,
                    ImageSize = 115990,
                    PublicCloudinaryId = "Brands/aepza2fmbbahf0iixvao",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466758686/Brands/aepza2fmbbahf0iixvao.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466758686/Brands/aepza2fmbbahf0iixvao.jpg",
                    Version = 1466758686,
                    Width = 2927,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Hagen);

            HappiDoggy = new Brands()
            {
                BrandName = "HappiDoggy",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 49280,
                    PublicCloudinaryId = "Brands/iv1zj0kzkyrqmolswbfv",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466758742/Brands/iv1zj0kzkyrqmolswbfv.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466758742/Brands/iv1zj0kzkyrqmolswbfv.jpg",
                    Version = 1466758742,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(HappiDoggy);

            HiTekNaturals = new Brands()
            {
                BrandName = "Hi-Tek Naturals Grain Free",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 51561,
                    PublicCloudinaryId = "Brands/znlywbrrcsn79kqqrkhh",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466758802/Brands/znlywbrrcsn79kqqrkhh.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466758802/Brands/znlywbrrcsn79kqqrkhh.jpg",
                    Version = 1466758802,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(HiTekNaturals);

            // This is brand is blank/empty
            //HolisticBites = new Brands()
            //{
            //    BrandName = "Holistic Bites",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 179,
            //        ImageSize = 33033,
            //        PublicCloudinaryId = "Brands/xgbtldplf6ldedrjydft",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466262801/Brands/xgbtldplf6ldedrjydft.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466262801/Brands/xgbtldplf6ldedrjydft.jpg",
            //        Version = 1466262801,
            //        Width = 202
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(HolisticBites);

            HolisticSelect = new Brands()
            {
                BrandName = "Holistic Select",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 54360,
                    PublicCloudinaryId = "Brands/ghc2w8rursq3g5li1jm6",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466759009/Brands/ghc2w8rursq3g5li1jm6.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466759009/Brands/ghc2w8rursq3g5li1jm6.jpg",
                    Version = 1466759009,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(HolisticSelect);

            IDPets = new Brands()
            {
                BrandName = "ID-Pets",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 265,
                    ImageSize = 51840,
                    PublicCloudinaryId = "Brands/j6umxs3xvhbe4ppudmux",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466759051/Brands/j6umxs3xvhbe4ppudmux.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466759051/Brands/j6umxs3xvhbe4ppudmux.jpg",
                    Version = 1466759051,
                    Width = 265,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(IDPets);

            JML = new Brands()
            {
                BrandName = "JML",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 630,
                    ImageSize = 65576,
                    PublicCloudinaryId = "Brands/mjp0sdmwkvg14hidk7ez",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466759240/Brands/mjp0sdmwkvg14hidk7ez.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466759240/Brands/mjp0sdmwkvg14hidk7ez.jpg",
                    Version = 1466759240,
                    Width = 1200,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(JML);

            JustFish = new Brands()
            {
                BrandName = "Just Fish",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 24595,
                    PublicCloudinaryId = "Brands/tbgjadtm2s645hwrkjjg",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466759285/Brands/tbgjadtm2s645hwrkjjg.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466759285/Brands/tbgjadtm2s645hwrkjjg.jpg",
                    Version = 1466759285,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(JustFish);

            JW = new Brands()
            {
                BrandName = "JW",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 23910,
                    PublicCloudinaryId = "Brands/kfnlizwypvvqdmmdrkyo",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466759326/Brands/kfnlizwypvvqdmmdrkyo.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466759326/Brands/kfnlizwypvvqdmmdrkyo.jpg",
                    Version = 1466759326,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(JW);

            //Undone
            Kaning = new Brands()
            {
                BrandName = "Kaning",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 33033,
                    PublicCloudinaryId = "Brands/xgbtldplf6ldedrjydft",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466262801/Brands/xgbtldplf6ldedrjydft.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466262801/Brands/xgbtldplf6ldedrjydft.jpg",
                    Version = 1466262801,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Kaning);

            KarenPryor = new Brands()
            {
                BrandName = "Karen Pryor",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 26399,
                    PublicCloudinaryId = "Brands/jmptoxpdqtdghc4wqidt",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466759578/Brands/jmptoxpdqtdghc4wqidt.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466759578/Brands/jmptoxpdqtdghc4wqidt.jpg",
                    Version = 1466759578,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(KarenPryor);

            KitCat = new Brands()
            {
                BrandName = "Kit Cat",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 285,
                    ImageSize = 20752,
                    PublicCloudinaryId = "Brands/becgkvedm5wepvplkyqy",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466702593/Brands/becgkvedm5wepvplkyqy.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466702593/Brands/becgkvedm5wepvplkyqy.jpg",
                    Version = 1466702593,
                    Width = 300,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(KitCat);

            //Undone
            KittyLitty = new Brands()
            {
                BrandName = "Kitty Litty",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 285,
                    ImageSize = 20752,
                    PublicCloudinaryId = "Brands/becgkvedm5wepvplkyqy",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466702593/Brands/becgkvedm5wepvplkyqy.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466702593/Brands/becgkvedm5wepvplkyqy.jpg",
                    Version = 1466702593,
                    Width = 300,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(KittyLitty);

            KMR = new Brands()
            {
                BrandName = "KMR",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 129,
                    ImageSize = 14055,
                    PublicCloudinaryId = "Brands/cljuaq2lt9gaxtauzraj",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466759844/Brands/cljuaq2lt9gaxtauzraj.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466759844/Brands/cljuaq2lt9gaxtauzraj.jpg",
                    Version = 1466759844,
                    Width = 200,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(KMR);

            Life4K9 = new Brands()
            {
                BrandName = "Life 4K9",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 320,
                    ImageSize = 15367,
                    PublicCloudinaryId = "Brands/homrqnob6yb8pfxwzqzp",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466759961/Brands/homrqnob6yb8pfxwzqzp.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466759961/Brands/homrqnob6yb8pfxwzqzp.jpg",
                    Version = 1466759961,
                    Width = 880,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Life4K9);

            Loveem = new Brands()
            {
                BrandName = "Love'em",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 485,
                    ImageSize = 98010,
                    PublicCloudinaryId = "Brands/xzke0lpvyevbcokzasa8",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466760065/Brands/xzke0lpvyevbcokzasa8.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466760065/Brands/xzke0lpvyevbcokzasa8.jpg",
                    Version = 1466760065,
                    Width = 677,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Loveem);

            Marukan = new Brands()
            {
                BrandName = "Marukan",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 27631,
                    PublicCloudinaryId = "Brands/uxaxsqad0hnbljbdtf0r",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466760157/Brands/uxaxsqad0hnbljbdtf0r.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466760157/Brands/uxaxsqad0hnbljbdtf0r.jpg",
                    Version = 1466760157,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Marukan);

            MediterraneanNatural = new Brands()
            {
                BrandName = "Mediterranean Natural",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 186,
                    ImageSize = 6646,
                    PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
                    Version = 1466139306,
                    Width = 270,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(MediterraneanNatural);

            Merrick = new Brands()
            {
                BrandName = "Merrick",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 546,
                    ImageSize = 207138,
                    PublicCloudinaryId = "Brands/tjv3tjg5ipzcr5lxkgmn",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466760331/Brands/tjv3tjg5ipzcr5lxkgmn.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466760331/Brands/tjv3tjg5ipzcr5lxkgmn.jpg",
                    Version = 1466760331,
                    Width = 900,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(Merrick);

            //Mikki = new Brands()
            //{
            //    BrandName = "Mikki",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Mikki);

            //MyPottyPad = new Brands()
            //{
            //    BrandName = "My Potty Pad",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(MyPottyPad);

            //MysticSea = new Brands()
            //{
            //    BrandName = "Mystic Sea",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(MysticSea);

            //NaturalBalance = new Brands()
            //{
            //    BrandName = "Natural Balance",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(NaturalBalance);

            //NaturesHarvest = new Brands()
            //{
            //    BrandName = "Natures Harvest",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(NaturesHarvest);

            //NaturesVariety = new Brands()
            //{
            //    BrandName = "Natures Variety",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(NaturesVariety);

            //NinaOttosson = new Brands()
            //{
            //    BrandName = "Nina Ottosson",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(NinaOttosson);

            //Nootie = new Brands()
            //{
            //    BrandName = "Nootie",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Nootie);

            //NutraGold = new Brands()
            //{
            //    BrandName = "NutraGold",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(NutraGold);

            //NutriVet = new Brands()
            //{
            //    BrandName = "Nutri-Vet",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(NutriVet);

            //Nutripe = new Brands()
            //{
            //    BrandName = "Nutripe",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Nutripe);

            //OldMotherHubbard = new Brands()
            //{
            //    BrandName = "Old Mother Hubbard",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(OldMotherHubbard);

            //OvenBakedTradition = new Brands()
            //{
            //    BrandName = "Oven-Baked Tradition",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(OvenBakedTradition);

            //Oxbow = new Brands()
            //{
            //    BrandName = "Oxbow",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Oxbow);

            //Pampets = new Brands()
            //{
            //    BrandName = "Pampets",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Pampets);

            //Parallel = new Brands()
            //{
            //    BrandName = "Parallel",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Parallel);

            //PeeWee = new Brands()
            //{
            //    BrandName = "Pee Wee",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(PeeWee);

            //Percell = new Brands()
            //{
            //    BrandName = "Percell",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Percell);

            //PetBotanics = new Brands()
            //{
            //    BrandName = "Pet Botanics",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(PetBotanics);

            //PetLink = new Brands()
            //{
            //    BrandName = "Pet Link",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(PetLink);

            //PetAlive = new Brands()
            //{
            //    BrandName = "PetAlive",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(PetAlive);

            //Petcare = new Brands()
            //{
            //    BrandName = "Petcare",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Petcare);

            //Petmate = new Brands()
            //{
            //    BrandName = "Petmate",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Petmate);

            //Pinnacle = new Brands()
            //{
            //    BrandName = "Pinnacle",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Pinnacle);

            //Pompey = new Brands()
            //{
            //    BrandName = "Pompey",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Pompey);

            //Primal = new Brands()
            //{
            //    BrandName = "Primal",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Primal);

            //ProDenPlaqueOff = new Brands()
            //{
            //    BrandName = "ProDen Plaque Off",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(ProDenPlaqueOff);

            //ProfessionalPetProduct = new Brands()
            //{
            //    BrandName = "Professional Pet Product (PPP)",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(ProfessionalPetProduct);

            //Revolution = new Brands()
            //{
            //    BrandName = "Revolution",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Revolution);

            //RichardsOrganics = new Brands()
            //{
            //    BrandName = "Richards Organics",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(RichardsOrganics);

            //Sanabelle = new Brands()
            //{
            //    BrandName = "Sanabelle",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Sanabelle);

            //Sanxia = new Brands()
            //{
            //    BrandName = "Sanxia",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Sanxia);

            //Schesir = new Brands()
            //{
            //    BrandName = "Schesir",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Schesir);

            //Sentry = new Brands()
            //{
            //    BrandName = "Sentry",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Sentry);

            //SmallPetSelect = new Brands()
            //{
            //    BrandName = "Small Pet Select",
            //    BrandPhoto = new BrandPhoto()
            //    {
            //        Format = "jpg",
            //        Height = 186,
            //        ImageSize = 6646,
            //        PublicCloudinaryId = "Brands/mwd14fzvsacnqpeo7wka",
            //        SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466139306/Brands/mwd14fzvsacnqpeo7wka.jpg",
            //        Version = 1466139306,
            //        Width = 270
            //    },
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(SmallPetSelect);

            SolidGold = new Brands()
            {
                BrandName = "Solid Gold",
                BrandPhoto = new BrandPhoto()
                {
                    Format = "jpg",
                    Height = 179,
                    ImageSize = 36195,
                    PublicCloudinaryId = "Brands/mzhtv4hg4eaw1xnc8yv1",
                    SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1466264430/Brands/mzhtv4hg4eaw1xnc8yv1.jpg",
                    Url = "http://res.cloudinary.com/nixxholas/image/upload/v1466264430/Brands/mzhtv4hg4eaw1xnc8yv1.jpg",
                    Version = 1466264430,
                    Width = 202,
                    CreatedById = benUser.Id
                },
                BrandCategory = new List<BrandCategory>(),
                CreatedById = randyUser.Id,
                UpdatedById = thomasUser.Id
            };
            db.Brands.Add(SolidGold);

            //SPALavish = new Brands()
            //{
            //    BrandName = "Spa Lavish",
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(SPALavish);

            //Spotty = new Brands()
            //{
            //    BrandName = "Spotty",                
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Spotty);

            //SynergyLabs = new Brands()
            //{
            //    BrandName = "Synergy Labs",
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(SynergyLabs);

            //TasteOfTheWild = new Brands()
            //{
            //    BrandName = "Taste Of The Wild",
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(TasteOfTheWild);

            //TheNaturalPetTreatCompany = new Brands()
            //{
            //    BrandName = "The Natural Pet Treat Company",
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(TheNaturalPetTreatCompany);

            //TikiCat = new Brands()
            //{
            //    BrandName = "Tiki Cat",
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(TikiCat);

            //TimberWolfOrganics = new Brands()
            //{
            //    BrandName = "Timberwolf Organics",
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(TimberWolfOrganics);

            //TomNPus = new Brands()
            //{
            //    BrandName = "Tom & Pus",
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(TomNPus);

            //TopLife = new Brands()
            //{
            //    BrandName = "TopLife",
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(TopLife);

            //TripleCrown = new Brands()
            //{
            //    BrandName = "Triple Crown",
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(TripleCrown);

            //TropiClean = new Brands()
            //{
            //    BrandName = "TropiClean",
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(TropiClean);

            //Virbac = new Brands()
            //{
            //    BrandName = "Virbac",                
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Virbac);

            //Vitakraft = new Brands()
            //{
            //    BrandName = "Vitakraft",
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Vitakraft);

            //VivaLaSpa = new Brands()
            //{
            //    BrandName = "Viva La Spa",
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(VivaLaSpa);

            //Wellness = new Brands()
            //{
            //    BrandName = "Wellness",
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Wellness);

            //WestPawDesign = new Brands()
            //{
            //    BrandName = "West Paw Design",
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(WestPawDesign);

            //WildSanko = new Brands()
            //{
            //    BrandName = "Wild Sanko",
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(WildSanko);

            //Wishbone = new Brands()
            //{
            //    BrandName = "Wishbone",
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Wishbone);

            //Zignature = new Brands()
            //{
            //    BrandName = "Zignature",
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Zignature);

            //Zukes = new Brands()
            //{
            //    BrandName = "Zuke's",
            //    BrandCategory = new List<BrandCategory>()
            //};
            //db.Brands.Add(Zukes);

            // End of Data Seeding for the Brands Table

            // DataSeeder for the BrandCategory Table

            // Skip Clearance Sale..
            // Allow the user to assign their own brands for clearance sales

            // Donate Brand Categories
            // Divine Choice Brand Missing

            BrandCategory AvoDermDonate = new BrandCategory();
            AvoDermDonate.BrandId = AvoDerm.BrandId;
            AvoDermDonate.CatId = Donate.CatId;
            db.BrandCategory.Add(AvoDermDonate);

            BrandCategory FerplastDonate = new BrandCategory();
            FerplastDonate.BrandId = Ferplast.BrandId;
            FerplastDonate.CatId = Donate.CatId;
            db.BrandCategory.Add(FerplastDonate);

            BrandCategory HiTekNaturalsDonate = new BrandCategory();
            HiTekNaturalsDonate.BrandId = HiTekNaturals.BrandId;
            HiTekNaturalsDonate.CatId = Donate.CatId;
            db.BrandCategory.Add(HiTekNaturalsDonate);

            // End of Donate Brand Categories

            // Dog Food Brand Categories
            BrandCategory AddictionDogFood = new BrandCategory();
            AddictionDogFood.BrandId = Addiction.BrandId;
            AddictionDogFood.CatId = DogFood.CatId;
            db.BrandCategory.Add(AddictionDogFood);

            BrandCategory BoschDogFood = new BrandCategory();
            BoschDogFood.BrandId = Bosch.BrandId;
            BoschDogFood.CatId = DogFood.CatId;
            db.BrandCategory.Add(BoschDogFood);

            BrandCategory CanineTributeDogFood = new BrandCategory();
            CanineTributeDogFood.BrandId = CanineTribute.BrandId;
            CanineTributeDogFood.CatId = DogFood.CatId;
            db.BrandCategory.Add(CanineTributeDogFood);

            BrandCategory EaglePackDogFood = new BrandCategory();
            EaglePackDogFood.BrandId = EaglePack.BrandId;
            EaglePackDogFood.CatId = DogFood.CatId;
            db.BrandCategory.Add(EaglePackDogFood);

            BrandCategory EagleProDogFood = new BrandCategory();
            EagleProDogFood.BrandId = EaglePro.BrandId;
            EagleProDogFood.CatId = DogFood.CatId;
            db.BrandCategory.Add(EagleProDogFood);

            BrandCategory EVODogFood = new BrandCategory();
            EVODogFood.BrandId = EVO.BrandId;
            EVODogFood.CatId = DogFood.CatId;
            db.BrandCategory.Add(EVODogFood);

            BrandCategory HiTekNaturalsDogFood = new BrandCategory();
            HiTekNaturalsDogFood.BrandId = HiTekNaturals.BrandId;
            HiTekNaturalsDogFood.CatId = DogFood.CatId;
            db.BrandCategory.Add(HiTekNaturalsDogFood);

            BrandCategory HolisticSelectDogFood = new BrandCategory();
            HolisticSelectDogFood.BrandId = HolisticSelect.BrandId;
            HolisticSelectDogFood.CatId = DogFood.CatId;
            db.BrandCategory.Add(HolisticSelectDogFood);

            //BrandCategory NutraGoldDogFood = new BrandCategory();
            //NutraGoldDogFood.BrandId = NutraGold.BrandId;
            //NutraGoldDogFood.CatId = DogFood.CatId;
            //db.BrandCategory.Add(NutraGoldDogFood);

            //BrandCategory NutripeDogFood = new BrandCategory();
            //NutripeDogFood.BrandId = Nutripe.BrandId;
            //NutripeDogFood.CatId = DogFood.CatId;
            //db.BrandCategory.Add(NutripeDogFood);

            //BrandCategory OvenBakedTraditionDogFood = new BrandCategory();
            //OvenBakedTraditionDogFood.BrandId = OvenBakedTradition.BrandId;
            //OvenBakedTraditionDogFood.CatId = DogFood.CatId;
            //db.BrandCategory.Add(OvenBakedTraditionDogFood);

            //BrandCategory PinnacleDogFood = new BrandCategory();
            //PinnacleDogFood.BrandId = Pinnacle.BrandId;
            //PinnacleDogFood.CatId = DogFood.CatId;
            //db.BrandCategory.Add(PinnacleDogFood);

            //BrandCategory SchesirDogFood = new BrandCategory();
            //SchesirDogFood.BrandId = Schesir.BrandId;
            //SchesirDogFood.CatId = DogFood.CatId;
            //db.BrandCategory.Add(SchesirDogFood);

            BrandCategory SolidGoldDogFood = new BrandCategory();
            SolidGoldDogFood.BrandId = SolidGold.BrandId;
            SolidGoldDogFood.CatId = DogFood.CatId;
            db.BrandCategory.Add(SolidGoldDogFood);

            //BrandCategory TasteOfTheWildDogFood = new BrandCategory();
            //TasteOfTheWildDogFood.BrandId = TasteOfTheWild.BrandId;
            //TasteOfTheWildDogFood.CatId = DogFood.CatId;
            //db.BrandCategory.Add(TasteOfTheWildDogFood);

            //BrandCategory TimberWolfOrganicsDogFood = new BrandCategory();
            //TimberWolfOrganicsDogFood.BrandId = TimberWolfOrganics.BrandId;
            //TimberWolfOrganicsDogFood.CatId = DogFood.CatId;
            //db.BrandCategory.Add(TimberWolfOrganicsDogFood);

            //BrandCategory WellnessDogFood = new BrandCategory();
            //WellnessDogFood.BrandId = Wellness.BrandId;
            //WellnessDogFood.CatId = DogFood.CatId;
            //db.BrandCategory.Add(WellnessDogFood);

            //BrandCategory WishboneDogFood = new BrandCategory();
            //WishboneDogFood.BrandId = Wishbone.BrandId;
            //WishboneDogFood.CatId = DogFood.CatId;
            //db.BrandCategory.Add(WishboneDogFood);

            //BrandCategory ZignatureDogFood = new BrandCategory();
            //ZignatureDogFood.BrandId = Zignature.BrandId;
            //ZignatureDogFood.CatId = DogFood.CatId;
            //db.BrandCategory.Add(ZignatureDogFood);

            // End of Dog Food Brand Categories

            // Cat Food Brand Categories

            BrandCategory AdditionCatFood = new BrandCategory();
            AdditionCatFood.BrandId = Addiction.BrandId;
            AdditionCatFood.CatId = CatFood.CatId;
            db.BrandCategory.Add(AdditionCatFood);

            BrandCategory AvoDermCatFood = new BrandCategory();
            AvoDermCatFood.BrandId = AvoDerm.BrandId;
            AvoDermCatFood.CatId = CatFood.CatId;
            db.BrandCategory.Add(AvoDermCatFood);

            BrandCategory DailyDelightCatFood = new BrandCategory();
            DailyDelightCatFood.BrandId = DailyDelight.BrandId;
            DailyDelightCatFood.CatId = CatFood.CatId;
            db.BrandCategory.Add(DailyDelightCatFood);

            BrandCategory EaglePackCatFood = new BrandCategory();
            EaglePackCatFood.BrandId = EaglePack.BrandId;
            EaglePackCatFood.CatId = CatFood.CatId;
            db.BrandCategory.Add(EaglePackCatFood);

            BrandCategory EVOCatFood = new BrandCategory();
            EVOCatFood.BrandId = EVO.BrandId;
            EVOCatFood.CatId = CatFood.CatId;
            db.BrandCategory.Add(EVOCatFood);

            BrandCategory FelineTributeCatFood = new BrandCategory();
            FelineTributeCatFood.BrandId = FelineTribute.BrandId;
            FelineTributeCatFood.CatId = CatFood.CatId;
            db.BrandCategory.Add(FelineTributeCatFood);

            BrandCategory FussieCatCatFood = new BrandCategory();
            FussieCatCatFood.BrandId = FussieCat.BrandId;
            FussieCatCatFood.CatId = CatFood.CatId;
            db.BrandCategory.Add(FussieCatCatFood);

            BrandCategory HolisticSelectCatFood = new BrandCategory();
            HolisticSelectCatFood.BrandId = HolisticSelect.BrandId;
            HolisticSelectCatFood.CatId = CatFood.CatId;
            db.BrandCategory.Add(HolisticSelectCatFood);

            //BrandCategory NutripeCatFood = new BrandCategory();
            //NutripeCatFood.BrandId = Nutripe.BrandId;
            //NutripeCatFood.CatId = CatFood.CatId;
            //db.BrandCategory.Add(NutripeCatFood);

            //BrandCategory SanabelleCatFood = new BrandCategory();
            //SanabelleCatFood.BrandId = Sanabelle.BrandId;
            //SanabelleCatFood.CatId = CatFood.CatId;
            //db.BrandCategory.Add(SanabelleCatFood);

            //BrandCategory SchesirCatFood = new BrandCategory();
            //SchesirCatFood.BrandId = Schesir.BrandId;
            //SchesirCatFood.CatId = CatFood.CatId;
            //db.BrandCategory.Add(SchesirCatFood);

            BrandCategory SolidGoldCatFood = new BrandCategory();
            SolidGoldCatFood.BrandId = SolidGold.BrandId;
            SolidGoldCatFood.CatId = CatFood.CatId;
            db.BrandCategory.Add(SolidGoldCatFood);

            //BrandCategory TasteOfTheWildCatFood = new BrandCategory();
            //TasteOfTheWildCatFood.BrandId = TasteOfTheWild.BrandId;
            //TasteOfTheWildCatFood.CatId = CatFood.CatId;
            //db.BrandCategory.Add(TasteOfTheWildCatFood);

            //BrandCategory WellnessCatFood = new BrandCategory();
            //WellnessCatFood.BrandId = Wellness.BrandId;
            //WellnessCatFood.CatId = CatFood.CatId;
            //db.BrandCategory.Add(WellnessCatFood);

            // End of Cat Food Brand Category

            // Small Animal Brand Category

            BrandCategory AlfalfaKingSmallAnimal = new BrandCategory();
            AlfalfaKingSmallAnimal.BrandId = AlfalfaKing.BrandId;
            AlfalfaKingSmallAnimal.CatId = SmallAnimal.CatId;
            db.BrandCategory.Add(AlfalfaKingSmallAnimal);

            BrandCategory AmericanPetDinerSmallAnimal = new BrandCategory();
            AmericanPetDinerSmallAnimal.BrandId = AmericanPetDiner.BrandId;
            AmericanPetDinerSmallAnimal.CatId = SmallAnimal.CatId;
            db.BrandCategory.Add(AmericanPetDinerSmallAnimal);

            BrandCategory BurgessSmallAnimal = new BrandCategory();
            BurgessSmallAnimal.BrandId = Burgess.BrandId;
            BurgessSmallAnimal.CatId = SmallAnimal.CatId;
            db.BrandCategory.Add(BurgessSmallAnimal);

            //BrandCategory OxbowSmallAnimal = new BrandCategory();
            //OxbowSmallAnimal.BrandId = Oxbow.BrandId;
            //OxbowSmallAnimal.CatId = SmallAnimal.CatId;
            //db.BrandCategory.Add(OxbowSmallAnimal);

            //BrandCategory SmallPetSelectSmallAnimal = new BrandCategory();
            //SmallPetSelectSmallAnimal.BrandId = SmallPetSelect.BrandId;
            //SmallPetSelectSmallAnimal.CatId = SmallAnimal.CatId;
            //db.BrandCategory.Add(SmallPetSelectSmallAnimal);

            //BrandCategory WildSankoSmallAnimal = new BrandCategory();
            //WildSankoSmallAnimal.BrandId = WildSanko.BrandId;
            //WildSankoSmallAnimal.CatId = SmallAnimal.CatId;
            //db.BrandCategory.Add(WildSankoSmallAnimal);

            // End of Small Animal Brand Categories

            // Treats Brand Categories
            // Small Animal Treats Sub Category is here

            BrandCategory AddictionTreats = new BrandCategory();
            AddictionTreats.BrandId = Addiction.BrandId;
            AddictionTreats.CatId = Treats.CatId;
            db.BrandCategory.Add(AddictionTreats);

            BrandCategory AngelsTreats = new BrandCategory();
            AngelsTreats.BrandId = Angels.BrandId;
            AngelsTreats.CatId = Treats.CatId;
            db.BrandCategory.Add(AngelsTreats);

            BrandCategory BasicInstinctTreats = new BrandCategory();
            BasicInstinctTreats.BrandId = BasicInstinct.BrandId;
            BasicInstinctTreats.CatId = Treats.CatId;
            db.BrandCategory.Add(BasicInstinctTreats);

            BrandCategory CanzRealMeatTreats = new BrandCategory();
            CanzRealMeatTreats.BrandId = CanzRealMeat.BrandId;
            CanzRealMeatTreats.CatId = Treats.CatId;
            db.BrandCategory.Add(CanzRealMeatTreats);

            BrandCategory Fish4PetsTreats = new BrandCategory();
            Fish4PetsTreats.BrandId = Fish4Pets.BrandId;
            Fish4PetsTreats.CatId = Treats.CatId;
            db.BrandCategory.Add(Fish4PetsTreats);

            BrandCategory JustFishTreats = new BrandCategory();
            JustFishTreats.BrandId = JustFish.BrandId;
            JustFishTreats.CatId = Treats.CatId;
            db.BrandCategory.Add(JustFishTreats);

            BrandCategory LoveemTreats = new BrandCategory();
            LoveemTreats.BrandId = Loveem.BrandId;
            LoveemTreats.CatId = Treats.CatId;
            db.BrandCategory.Add(LoveemTreats);

            //BrandCategory OldMotherHubbardTreats = new BrandCategory();
            //OldMotherHubbardTreats.BrandId = OldMotherHubbard.BrandId;
            //OldMotherHubbardTreats.CatId = Treats.CatId;
            //db.BrandCategory.Add(OldMotherHubbardTreats);

            //BrandCategory PetBotanicsTreats = new BrandCategory();
            //PetBotanicsTreats.BrandId = PetBotanics.BrandId;
            //PetBotanicsTreats.CatId = Treats.CatId;
            //db.BrandCategory.Add(PetBotanicsTreats);

            BrandCategory SolidGoldTreats = new BrandCategory();
            SolidGoldTreats.BrandId = SolidGold.BrandId;
            SolidGoldTreats.CatId = Treats.CatId;
            db.BrandCategory.Add(SolidGoldTreats);

            //BrandCategory WellnessTreats = new BrandCategory();
            //WellnessTreats.BrandId = Wellness.BrandId;
            //WellnessTreats.CatId = Treats.CatId;
            //db.BrandCategory.Add(WellnessTreats);

            //BrandCategory ZukesTreats = new BrandCategory();
            //ZukesTreats.BrandId = Zukes.BrandId;
            //ZukesTreats.CatId = Treats.CatId;
            //db.BrandCategory.Add(ZukesTreats);

            // End of Treats Brand Category

            // Shampoo Brand Category

            BrandCategory APT1022Shampoo = new BrandCategory();
            APT1022Shampoo.BrandId = APT1022.BrandId;
            APT1022Shampoo.CatId = Shampoo.CatId;
            db.BrandCategory.Add(APT1022Shampoo);

            BrandCategory Bark2BasicsShampoo = new BrandCategory();
            Bark2BasicsShampoo.BrandId = Bark2Basics.BrandId;
            Bark2BasicsShampoo.CatId = Shampoo.CatId;
            db.BrandCategory.Add(Bark2BasicsShampoo);

            BrandCategory ChitocureShampoo = new BrandCategory();
            ChitocureShampoo.BrandId = Chitocure.BrandId;
            ChitocureShampoo.CatId = Shampoo.CatId;
            db.BrandCategory.Add(ChitocureShampoo);

            BrandCategory CoatHandlerShampoo = new BrandCategory();
            CoatHandlerShampoo.BrandId = CoatHandler.BrandId;
            CoatHandlerShampoo.CatId = Shampoo.CatId;
            db.BrandCategory.Add(CoatHandlerShampoo);

            BrandCategory DermCareShampoo = new BrandCategory();
            DermCareShampoo.BrandId = DermCare.BrandId;
            DermCareShampoo.CatId = Shampoo.CatId;
            db.BrandCategory.Add(DermCareShampoo);

            BrandCategory EarthBathShampoo = new BrandCategory();
            EarthBathShampoo.BrandId = EarthBath.BrandId;
            EarthBathShampoo.CatId = Shampoo.CatId;
            db.BrandCategory.Add(EarthBathShampoo);

            BrandCategory MarukanShampoo = new BrandCategory();
            MarukanShampoo.BrandId = Marukan.BrandId;
            MarukanShampoo.CatId = Shampoo.CatId;
            db.BrandCategory.Add(MarukanShampoo);

            //BrandCategory NootieShampoo = new BrandCategory();
            //NootieShampoo.BrandId = Nootie.BrandId;
            //NootieShampoo.CatId = Shampoo.CatId;
            //db.BrandCategory.Add(NootieShampoo);

            //BrandCategory ProfessionalPetProductShampoo = new BrandCategory();
            //ProfessionalPetProductShampoo.BrandId = ProfessionalPetProduct.BrandId;
            //ProfessionalPetProductShampoo.CatId = Shampoo.CatId;
            //db.BrandCategory.Add(ProfessionalPetProductShampoo);

            //BrandCategory SentryShampoo = new BrandCategory();
            //SentryShampoo.BrandId = Sentry.BrandId;
            //SentryShampoo.CatId = Shampoo.CatId;
            //db.BrandCategory.Add(SentryShampoo);

            //BrandCategory SPALavishShampoo = new BrandCategory();
            //SPALavishShampoo.BrandId = SPALavish.BrandId;
            //SPALavishShampoo.CatId = Shampoo.CatId;
            //db.BrandCategory.Add(SPALavishShampoo);

            //BrandCategory SynergyLabsShampoo = new BrandCategory();
            //SynergyLabsShampoo.BrandId = SynergyLabs.BrandId;
            //SynergyLabsShampoo.CatId = Shampoo.CatId;
            //db.BrandCategory.Add(SynergyLabsShampoo);

            //BrandCategory TropiCleanShampoo = new BrandCategory();
            //TropiCleanShampoo.BrandId = TropiClean.BrandId;
            //TropiCleanShampoo.CatId = Shampoo.CatId;
            //db.BrandCategory.Add(TropiCleanShampoo);

            // End of Shampoo Brand Category

            // Grooming Brand Category

            BrandCategory AeolusGrooming = new BrandCategory();
            AeolusGrooming.BrandId = Aeolus.BrandId;
            AeolusGrooming.CatId = Grooming.CatId;
            db.BrandCategory.Add(AeolusGrooming);

            BrandCategory EyeEnvyGrooming = new BrandCategory();
            EyeEnvyGrooming.BrandId = EyeEnvy.BrandId;
            EyeEnvyGrooming.CatId = Grooming.CatId;
            db.BrandCategory.Add(EyeEnvyGrooming);

            BrandCategory FerplastGrooming = new BrandCategory();
            FerplastGrooming.BrandId = Ferplast.BrandId;
            FerplastGrooming.CatId = Grooming.CatId;
            db.BrandCategory.Add(FerplastGrooming);

            BrandCategory MarukanGrooming = new BrandCategory();
            MarukanGrooming.BrandId = Marukan.BrandId;
            MarukanGrooming.CatId = Grooming.CatId;
            db.BrandCategory.Add(MarukanGrooming);

            //BrandCategory MikkiGrooming = new BrandCategory();
            //MikkiGrooming.BrandId = Mikki.BrandId;
            //MikkiGrooming.CatId = Grooming.CatId;
            //db.BrandCategory.Add(MikkiGrooming);

            //BrandCategory ProfessionalPetProductGrooming = new BrandCategory();
            //ProfessionalPetProductGrooming.BrandId = ProfessionalPetProduct.BrandId;
            //ProfessionalPetProductGrooming.CatId = Grooming.CatId;
            //db.BrandCategory.Add(ProfessionalPetProductGrooming);

            //BrandCategory VirbacGrooming = new BrandCategory();
            //VirbacGrooming.BrandId = Virbac.BrandId;
            //VirbacGrooming.CatId = Grooming.CatId;
            //db.BrandCategory.Add(VirbacGrooming);

            // End of Grooming Brand Category

            // Dental Care Brand Category

            BrandCategory HappiDoggyDentalCare = new BrandCategory();
            HappiDoggyDentalCare.BrandId = HappiDoggy.BrandId;
            HappiDoggyDentalCare.CatId = DentalCare.CatId;
            db.BrandCategory.Add(HappiDoggyDentalCare);

            //BrandCategory NutriVetDentalCare = new BrandCategory();
            //NutriVetDentalCare.BrandId = NutriVet.BrandId;
            //NutriVetDentalCare.CatId = DentalCare.CatId;
            //db.BrandCategory.Add(NutriVetDentalCare);

            //BrandCategory TropiCleanDentalCare = new BrandCategory();
            //TropiCleanDentalCare.BrandId = TropiClean.BrandId;
            //TropiCleanDentalCare.CatId = DentalCare.CatId;
            //db.BrandCategory.Add(TropiCleanDentalCare);

            // End of Dental Care Brand Category

            // Supplements Brand Category

            BrandCategory AngelsSupplements = new BrandCategory();
            AngelsSupplements.BrandId = Angels.BrandId;
            AngelsSupplements.CatId = Supplements.CatId;
            db.BrandCategory.Add(AngelsSupplements);

            // Pet Ag
            BrandCategory KMRSupplements = new BrandCategory();
            KMRSupplements.BrandId = KMR.BrandId;
            KMRSupplements.CatId = Supplements.CatId;
            db.BrandCategory.Add(KMRSupplements);

            BrandCategory SolidGoldSupplements = new BrandCategory();
            SolidGoldSupplements.BrandId = SolidGold.BrandId;
            SolidGoldSupplements.CatId = Supplements.CatId;
            db.BrandCategory.Add(SolidGoldSupplements);

            // End of Supplements Brand Category

            // Toys Brand Category
            // Small Animal Toys Sub Category is here

            BrandCategory CatItToys = new BrandCategory();
            CatItToys.BrandId = CatIt.BrandId;
            CatItToys.CatId = Toys.CatId;
            db.BrandCategory.Add(CatItToys);

            BrandCategory DogItToys = new BrandCategory();
            DogItToys.BrandId = DogIt.BrandId;
            DogItToys.CatId = Toys.CatId;
            db.BrandCategory.Add(DogItToys);

            BrandCategory FerplastToys = new BrandCategory();
            FerplastToys.BrandId = Ferplast.BrandId;
            FerplastToys.CatId = Toys.CatId;
            db.BrandCategory.Add(FerplastToys);

            BrandCategory JWToys = new BrandCategory();
            JWToys.BrandId = JW.BrandId;
            JWToys.CatId = Toys.CatId;
            db.BrandCategory.Add(JWToys);

            //BrandCategory NinaOttossonToys = new BrandCategory();
            //NinaOttossonToys.BrandId = NinaOttosson.BrandId;
            //NinaOttossonToys.CatId = Toys.CatId;
            //db.BrandCategory.Add(NinaOttossonToys);

            //BrandCategory PercellToys = new BrandCategory();
            //PercellToys.BrandId = Percell.BrandId;
            //PercellToys.CatId = Toys.CatId;
            //db.BrandCategory.Add(PercellToys);

            //BrandCategory WestPawDesignToys = new BrandCategory();
            //WestPawDesignToys.BrandId = WestPawDesign.BrandId;
            //WestPawDesignToys.CatId = Toys.CatId;
            //db.BrandCategory.Add(WestPawDesignToys);

            // End of Toys Brand Category

            // Accessories Brand Category

            // Sub categories
            // Small Animals Accessories
            // Small Animal Cages/Enclosures
            // Small Animal Drinkers/Feeders
            // Products with missing brands
            // Alex Smart Bottle 500ml.. What's the brand?
            // Jolly Hay Rack & Feeding Bowl
            // Living World Water Bottle 400ml
            // Small Animal Houses
            // Products with missing brands
            // Natural Grass House
            // Straw Mats/Houses
            // ID-Pets Tags
            // Apparels & Shoes
            // Bags, Carriers & Crates
            // Products with missing brands
            // Airline Approved Carriers With Wheels
            // Butter Nautical Pet Bag
            // Butter Sleek Pet Bag
            // Deluxe Airline Approved Carrier
            // Beds
            // Snooza Pet Futon (It's a hidden brand lol)
            // Bowls & Drinking Fountains
            // Cages & Enclosures
            // Cages Sub-sub category
            // Products with missing brands
            // 2 Tier Cat Cage
            // 2.5FT Cage With Wheels
            // 2.5FT Foldable Cage
            // 2.5FT Japanese Style 3 Tier Cat Cage
            // 3 Tier Cat Cage
            // 3FT Japanase Style 3 Tier Cat Cage
            // 5 Tier Cat Cage
            // 5ft Powder Coated Cast Iron Cage
            // Cat Scratch Post & Towers
            // Products with missing brands
            // 2-Level Square Scratcher
            // Barcelona
            // Blue Cone Scratcher
            // Cat Mat Scratcher
            // Cat Scratch Post
            // Cat Scratch Stand
            // Cat Scratch Stand (Mouse)
            // Cat Scratcher With Tunnel
            // Corner Cat Scratch Post
            // Hanging Scratcher
            // Heart Cat Scratch Post
            // Long Branch Monaco
            // Multi Level Cat Playground (Brown)
            // Paw Base Scratcher
            // Paw Base Scratching Post
            // Plush Furniture Scratcher
            // Prague Scratcher
            // Red Cone Scratcher
            // Star Cat Scratcher Post
            // Stella Paw Scratcher
            // Odor Control & Animal Repellent
            // Bio-X 3 in 1 Spray

            BrandCategory AddMateAccessories = new BrandCategory();
            AddMateAccessories.BrandId = AddMate.BrandId;
            AddMateAccessories.CatId = Accessories.CatId;
            db.BrandCategory.Add(AddMateAccessories);

            BrandCategory CatItAcessories = new BrandCategory();
            CatItAcessories.BrandId = CatIt.BrandId;
            CatItAcessories.CatId = Accessories.CatId;
            db.BrandCategory.Add(CatItAcessories);

            BrandCategory DogItAccessories = new BrandCategory();
            DogItAccessories.BrandId = DogIt.BrandId;
            DogItAccessories.CatId = Accessories.CatId;
            db.BrandCategory.Add(DogItAccessories);

            BrandCategory FerplastAccessories = new BrandCategory();
            FerplastAccessories.BrandId = Ferplast.BrandId;
            FerplastAccessories.CatId = Accessories.CatId;
            db.BrandCategory.Add(FerplastAccessories);

            BrandCategory HagenAccessories = new BrandCategory();
            HagenAccessories.BrandId = Hagen.BrandId;
            HagenAccessories.CatId = Accessories.CatId;
            db.BrandCategory.Add(HagenAccessories);

            BrandCategory IDPetsAccessories = new BrandCategory();
            IDPetsAccessories.BrandId = IDPets.BrandId;
            IDPetsAccessories.CatId = Accessories.CatId;
            db.BrandCategory.Add(IDPetsAccessories);

            BrandCategory JMLAccessories = new BrandCategory();
            JMLAccessories.BrandId = JML.BrandId;
            JMLAccessories.CatId = Accessories.CatId;
            db.BrandCategory.Add(JMLAccessories);

            BrandCategory KaningAccessories = new BrandCategory();
            KaningAccessories.BrandId = Kaning.BrandId;
            KaningAccessories.CatId = Accessories.CatId;
            db.BrandCategory.Add(KaningAccessories);

            BrandCategory MarukanAccessories = new BrandCategory();
            MarukanAccessories.BrandId = Marukan.BrandId;
            MarukanAccessories.CatId = Accessories.CatId;
            db.BrandCategory.Add(MarukanAccessories);

            //BrandCategory OxbowAccessories = new BrandCategory();
            //OxbowAccessories.BrandId = Oxbow.BrandId;
            //OxbowAccessories.CatId = Accessories.CatId;
            //db.BrandCategory.Add(OxbowAccessories);

            //BrandCategory SanxiaAccessories = new BrandCategory();
            //SanxiaAccessories.BrandId = Sanxia.BrandId;
            //SanxiaAccessories.CatId = Accessories.CatId;
            //db.BrandCategory.Add(SanxiaAccessories);

            //BrandCategory WildSankoAcessories = new BrandCategory();
            //WildSankoAcessories.BrandId = WildSanko.BrandId;
            //WildSankoAcessories.CatId = Accessories.CatId;
            //db.BrandCategory.Add(WildSankoAcessories);

            // End of Accessories Brand Category

            // Toiletry Needs Brand Category

            // Sub categories & Products with missing brands
            // Small Animal Tolietry Needs
            // Cat Litter & Animal Bedding
            // Sub sub categories
            // Clumping
            // Crystals & Corbs
            // Others
            // Pine
            // OKO Plus Cat Litter
            // Recycled Paper                        
            // Back-2-Nature Animal Bredding
            // Carefresh Ultra Pet Bedding
            // Litter Box, Tray, Liners
            // Kitty Litty
            // Litter Scoop
            // OKO Plus Cat Scoop
            // Pee Pad / Diapers
            // Bow Pad Pet Sheet
            // BUY 1 GET 1 FREE KingBorn Pet Sheets
            // Drypets Pee Pad
            // KingBorn Pet Sheets
            // My Potty Pad

            BrandCategory BreederCelectToiletryNeeds = new BrandCategory();
            BreederCelectToiletryNeeds.BrandId = BreederCelect.BrandId;
            BreederCelectToiletryNeeds.CatId = ToiletryNeeds.CatId;
            db.BrandCategory.Add(BreederCelectToiletryNeeds);

            BrandCategory BurgessToiletryNeeds = new BrandCategory();
            BurgessToiletryNeeds.BrandId = Burgess.BrandId;
            BurgessToiletryNeeds.CatId = ToiletryNeeds.CatId;
            db.BrandCategory.Add(BurgessToiletryNeeds);

            BrandCategory CanadaLitterToiletryNeeds = new BrandCategory();
            CanadaLitterToiletryNeeds.BrandId = CanadaLitter.BrandId;
            CanadaLitterToiletryNeeds.CatId = ToiletryNeeds.CatId;
            db.BrandCategory.Add(CanadaLitterToiletryNeeds);

            BrandCategory CatItToiletryNeeds = new BrandCategory();
            CatItToiletryNeeds.BrandId = CatIt.BrandId;
            CatItToiletryNeeds.CatId = ToiletryNeeds.CatId;
            db.BrandCategory.Add(CatItToiletryNeeds);

            BrandCategory FerplastToiletryNeeds = new BrandCategory();
            FerplastToiletryNeeds.BrandId = Ferplast.BrandId;
            FerplastToiletryNeeds.CatId = ToiletryNeeds.CatId;
            db.BrandCategory.Add(FerplastToiletryNeeds);

            BrandCategory FussieCatToiletryNeeds = new BrandCategory();
            FussieCatToiletryNeeds.BrandId = FussieCat.BrandId;
            FussieCatToiletryNeeds.CatId = ToiletryNeeds.CatId;
            db.BrandCategory.Add(FussieCatToiletryNeeds);

            BrandCategory GexToiletryNeeds = new BrandCategory();
            GexToiletryNeeds.BrandId = Gex.BrandId;
            GexToiletryNeeds.CatId = ToiletryNeeds.CatId;
            db.BrandCategory.Add(GexToiletryNeeds);

            BrandCategory MarukanToiletryNeeds = new BrandCategory();
            MarukanToiletryNeeds.BrandId = Marukan.BrandId;
            MarukanToiletryNeeds.CatId = ToiletryNeeds.CatId;
            db.BrandCategory.Add(MarukanToiletryNeeds);

            //BrandCategory OxbowToiletryNeeds = new BrandCategory();
            //OxbowToiletryNeeds.BrandId = Oxbow.BrandId;
            //OxbowToiletryNeeds.CatId = ToiletryNeeds.CatId;
            //db.BrandCategory.Add(OxbowToiletryNeeds);

            //BrandCategory PampetsToiletryNeeds = new BrandCategory();
            //PampetsToiletryNeeds.BrandId = Pampets.BrandId;
            //PampetsToiletryNeeds.CatId = ToiletryNeeds.CatId;
            //db.BrandCategory.Add(PampetsToiletryNeeds);

            //BrandCategory PeeWeeToiletryNeeds = new BrandCategory();
            //PeeWeeToiletryNeeds.BrandId = PeeWee.BrandId;
            //PeeWeeToiletryNeeds.CatId = ToiletryNeeds.CatId;
            //db.BrandCategory.Add(PeeWeeToiletryNeeds);

            //BrandCategory PercellToiletryNeeds = new BrandCategory();
            //PercellToiletryNeeds.BrandId = Percell.BrandId;
            //PercellToiletryNeeds.CatId = ToiletryNeeds.CatId;
            //db.BrandCategory.Add(PercellToiletryNeeds);

            //BrandCategory PetmateToiletryNeeds = new BrandCategory();
            //PetmateToiletryNeeds.BrandId = Petmate.BrandId;
            //PetmateToiletryNeeds.CatId = ToiletryNeeds.CatId;
            //db.BrandCategory.Add(PetmateToiletryNeeds);

            //BrandCategory TomNPusToiletryNeeds = new BrandCategory();
            //TomNPusToiletryNeeds.BrandId = TomNPus.BrandId;
            //TomNPusToiletryNeeds.CatId = ToiletryNeeds.CatId;
            //db.BrandCategory.Add(TomNPusToiletryNeeds);

            //BrandCategory WildSankoToiletryNeeds = new BrandCategory();
            //WildSankoToiletryNeeds.BrandId = WildSanko.BrandId;
            //WildSankoToiletryNeeds.CatId = ToiletryNeeds.CatId;
            //db.BrandCategory.Add(WildSankoToiletryNeeds);

            // End of Toiletry Needs Brand Category

            // Training Aids Brand Category

            // Sub categories
            // Karen Pryor Clicker Training
            // Triple Crown

            BrandCategory KarenPryorTrainingAids = new BrandCategory();
            KarenPryorTrainingAids.BrandId = KarenPryor.BrandId;
            KarenPryorTrainingAids.CatId = TrainingAids.CatId;
            db.BrandCategory.Add(KarenPryorTrainingAids);

            //BrandCategory TripleCrownTrainingAids = new BrandCategory();
            //TripleCrownTrainingAids.BrandId = TripleCrown.BrandId;
            //TripleCrownTrainingAids.CatId = TrainingAids.CatId;
            //db.BrandCategory.Add(TripleCrownTrainingAids);

            // End of Training Aids Brand Category

            // Flea & Tick Control Brand Category

            // Sub Categories
            // Bayer Advocate
            // CatanDog's 
            // Frontline
            // Professional Pet Product
            // Revolution

            BrandCategory AdvocateFleaNTick = new BrandCategory();
            AdvocateFleaNTick.BrandId = Advocate.BrandId; // Advocate (Bayer)
            AdvocateFleaNTick.CatId = FleaNTickControl.CatId;
            db.BrandCategory.Add(AdvocateFleaNTick);

            BrandCategory CatanDogsFleaNTick = new BrandCategory();
            CatanDogsFleaNTick.BrandId = CatanDogs.BrandId;
            CatanDogsFleaNTick.CatId = FleaNTickControl.CatId;
            db.BrandCategory.Add(CatanDogsFleaNTick);

            BrandCategory FrontlineFleaNTick = new BrandCategory();
            FrontlineFleaNTick.BrandId = Frontline.BrandId;
            FrontlineFleaNTick.CatId = FleaNTickControl.CatId;
            db.BrandCategory.Add(FrontlineFleaNTick);

            //BrandCategory ProfessionalPetProductFleaNTick = new BrandCategory();
            //ProfessionalPetProductFleaNTick.BrandId = ProfessionalPetProduct.BrandId;
            //ProfessionalPetProductFleaNTick.CatId = FleaNTickControl.CatId;
            //db.BrandCategory.Add(ProfessionalPetProductFleaNTick);

            //BrandCategory RevolutionFleaNTick = new BrandCategory();
            //RevolutionFleaNTick.BrandId = Revolution.BrandId;
            //RevolutionFleaNTick.CatId = FleaNTickControl.CatId;
            //db.BrandCategory.Add(RevolutionFleaNTick);

            // End of Flea & Tick Control Brand Category

            // Gift Vouchers Brand Category

            // Products with missing brands
            // Pro Plan Indoor Cat
            // Pro Plan Shredded Blend Lamb And Rice 15kg
            // ProPlan Kitten
            // SGD $100 Gift Voucher
            // SGD $200 Gift Voucher
            // SGD $25 Gift Voucher
            // SGD $50 Gift Voucher
            // Shipping Charge

            // End of Gift Vouchers Brand Category

            // Purchase With Purchase Brand Category

            // PWP: Catch Scratch Pole, Missing Brand Link

            //BrandCategory RichardsOrganicsPwP = new BrandCategory();
            //RichardsOrganicsPwP.BrandId = RichardsOrganics.BrandId;
            //RichardsOrganicsPwP.CatId = PwP.CatId;
            //db.BrandCategory.Add(RichardsOrganicsPwP);

            // End of Purchase With Purchase Brand Category

            // ----------------------- PRODUCTS SEEDING -------------------------------- //
            Product AEOLUSTD901HOSEDDRYER;

            AEOLUSTD901HOSEDDRYER = new Product()
            {
                ProdName = "AEOLUS TD901 HOSED DRYER",
                SavingsOverview = "",
                Description = "<p><strong>DESCRIPTION</strong></p>" +
                "< hr />" +
                "< p > &bull; Features & nbsp;: Variable wind speed control with heating option </ p >" +
                    "< p > &bull; Air speed : 38m / s - 48m / s </ p >" +
                       "< p > &bull; Power: 1600W </ p >" +
                         "< p > &bull; Heat: approx 45 & deg; C - 65 & deg; C </ p >" +
                                 "< p > &bull; Powerful, high velocity dryer delivers a high volume of warm air to blast water from coats </ p >" +
                    "< p > &bull; Two - stage filtration and solid state variable speed controls </ p >" +
                         "< p > &bull; Weighing just approximately 5 kg </ p >" +
                            "< p > &bull; Puncture - resistant, triple reinforced flexible hose with option nozzle </ p >" +
                                 "< p > &bull; Constructed of rugged, durable steel with non-skid tabs </ p >" +
                                    "< p > &bull; Dual mounted legs allow both vertical and horizontal use </ p >" +
                                       "< p > &bull; Washable and easy replace filter </ p > ",
                Brand = Aeolus,
                ProductCategory = new List<ProductCategory>(),
                Quantity = 50,
                ThresholdInvertoryQuantity = 25,
                Published = 1,
                Specials = new List<ProductSpecials>(),
                isConsumable = 0,
                Metrics = new List<Metrics>(),
                ProductPhotos = new List<ProductPhoto>(),
                CreatedById = randyUser.Id,
                UpdatedById = randyUser.Id
            };
            db.Products.Add(AEOLUSTD901HOSEDDRYER);

            // ---------------------- END OF PRODUCTS SEEDING -------------------------- //

            // ---------------------- PRODUCT CATEGORY SEEDING ------------------------- //
            ProductCategory AEOLUSHOSEDDRYERTOGROOMING;

            AEOLUSHOSEDDRYERTOGROOMING = new ProductCategory()
            {
                ProdId = AEOLUSTD901HOSEDDRYER.ProdId,
                CatId = Grooming.CatId
            };
            db.ProductCategory.Add(AEOLUSHOSEDDRYERTOGROOMING);

            // ---------------------- END OF PRODUCT CATEGORY SEEDING ------------------ //

            // -------------------------- METRICS SEEDING ----------------------------- //

            Metrics AEOLUSHOSEDDRYERMETRIC1;

            AEOLUSHOSEDDRYERMETRIC1 = new Metrics()
            {
                MetricAmount = 1,
                MetricType = Unit.MetricSubType,
                PMetricId = Unit.PMetricId,
                Quantity = 50,
                ProdId = AEOLUSTD901HOSEDDRYER.ProdId,
                StatusId = Available.StatusId,
                Status = Available,
                Prices = new List<Price>(),
                CreatedById = randyUser.Id
            };
            db.Metrics.Add(AEOLUSHOSEDDRYERMETRIC1);

            // ------------------------- END OF METRICS SEEDING ----------------------- //

            // -------------------------- PRICE SEEDING ----------------------------- //

            Price AEOLUSHOSEDDRYERPRICE1;

            AEOLUSHOSEDDRYERPRICE1 = new Price()
            {
                MetricId = AEOLUSHOSEDDRYERMETRIC1.MetricId,
                RRP = Convert.ToDecimal(238.00),
                Value = Convert.ToDecimal(238.00),
                CreatedById = randyUser.Id
            };
            db.Prices.Add(AEOLUSHOSEDDRYERPRICE1);

            // ------------------------- END OF PRICE SEEDING ----------------------- //

            // ---------------------- PRODUCT PHOTOS SEEING ---------------------------- //

            ProductPhoto AEOLUSHOSEDDRYERPHOTO1;

            AEOLUSHOSEDDRYERPHOTO1 = new ProductPhoto()
            {
                ProdId = AEOLUSTD901HOSEDDRYER.ProdId,
                PublicCloudinaryId = "Products/lpqew6uabr3axhxkjrvp",
                Format = "jpg",
                Height = 490,
                ImageSize = 45260,
                SecureUrl = "https://res.cloudinary.com/nixxholas/image/upload/v1470649649/Products/lpqew6uabr3axhxkjrvp.jpg",
                Url = "http://res.cloudinary.com/nixxholas/image/upload/v1470649649/Products/lpqew6uabr3axhxkjrvp.jpg",
                Version = 1470649649,
                Width = 490,
                isPrimaryPhoto = 1,
                CreatedById = randyUser.Id
            };
            db.ProductPhotos.Add(AEOLUSHOSEDDRYERPHOTO1);

            // ---------------------- END OF PRODUCT PHOTOS SEEDING -------------------- //

            db.SaveChanges();


            return;
        }//End of SeedData() method





    }
}





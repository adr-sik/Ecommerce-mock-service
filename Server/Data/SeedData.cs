using Bogus;
using Shared.Models;
using Server.Models.ProductComponents;
using Server.Models.ProductTypes;
using Shared.Models.Enums;
using Server.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Server.Data
{
    public class SeedData
    {
        private static string GenerateRandomImageUrl(string prompt)
        {
            //return $"https://placehold.co/{width}x{height}/e0e0e0/2e2e2e?text={encodedPrompt}";
            return "";
        }
        public static void Initialize(EcommerceContext context)
        {

            //Seeding ProductComponents
            var cameras = new Faker<Camera>()
                .RuleFor(c => c.Megapixels, f => f.Random.Int(8, 108))
                .Generate(10);
            context.Cameras.AddRange(cameras);

            var allowedBrands = new List<CpuBrand>
                {
                    CpuBrand.Intel,
                    CpuBrand.AMD
                };
            var laptopCpus = new Faker<Cpu>()
                .RuleFor(c => c.Brand, f => f.PickRandom(allowedBrands))
                .RuleFor(c => c.Model, f => f.Commerce.ProductName())
                .RuleFor(c => c.NumberOfCores, f => f.Random.Int(4, 10))
                .Generate(15);
            context.Cpus.AddRange(laptopCpus);

            allowedBrands = new List<CpuBrand>
                {
                    CpuBrand.Apple,
                    CpuBrand.Qualcomm,
                    CpuBrand.MediaTek,
                    CpuBrand.Google
                };
            var phoneCpus = new Faker<Cpu>()
                .RuleFor(c => c.Brand, f => f.PickRandom(allowedBrands))
                .RuleFor(c => c.Model, f => f.Commerce.ProductName())
                .RuleFor(c => c.NumberOfCores, f => f.Random.Int(2, 24))
                .Generate(15);
            context.Cpus.AddRange(phoneCpus);

            var phoneDisplays = new Faker<Display>()
                .RuleFor(d => d.ScreenSizeInches, f => Math.Round(f.Random.Double(5.5, 7.0), 1))
                .RuleFor(d => d.Resolution, f => f.PickRandom("720x1280", "1080x2400", "1440x3200"))
                .RuleFor(d => d.RefreshRateHz, f => f.PickRandom(60, 90, 120, 144))
                .Generate(10);
            context.Displays.AddRange(phoneDisplays);

            var laptopDisplays = new Faker<Display>()
                .RuleFor(d => d.ScreenSizeInches, f => Math.Round(f.Random.Double(13.0, 32.0), 1))
                .RuleFor(d => d.Resolution, f => f.PickRandom("1920x1080", "2560x1440", "3840x2160"))
                .RuleFor(d => d.RefreshRateHz, f => f.PickRandom(60, 120, 144, 240, 360))
                .Generate(30);
            context.Displays.AddRange(laptopDisplays);

            var gpus = new Faker<Gpu>()
                .RuleFor(g => g.Brand, f => f.PickRandom("Nvidia", "AMD", "Radeon"))
                .RuleFor(g => g.Model, f => f.Commerce.ProductName())
                .RuleFor(g => g.VRAM, f => f.Random.Int(2, 24))
                .Generate(30);
            context.Gpus.AddRange(gpus);

            var rams = new Faker<Ram>()
                .RuleFor(r => r.Capacity, f => f.PickRandom(8, 16, 32))
                .Generate(10);
            context.Rams.AddRange(rams);

            //Seeding ProductTypes
            var phoneBrands = new[] { "Apple", "Samsung", "Sony", "Google", "Xiaomi", "OnePlus" };
            var laptopBrands = new[] { "Dell", "HP", "Lenovo", "Apple", "Asus", "Acer" };
            var headphonesBrands = new[] { "Sony", "Bose", "Sennheiser", "Apple", "Samsung", "JBL" };

            var headphones = new Faker<Headphones>()
                .RuleFor(h => h.Brand, f => f.PickRandom(headphonesBrands))
                .RuleFor(h => h.Model, (f, p) =>
                {
                    return p.Brand switch
                    {
                        "Sony" => f.PickRandom("WH-1000XM5", "WF-1000XM5", "LinkBuds S"),
                        "Bose" => f.PickRandom("QuietComfort Ultra", "QuietComfort 45", "SoundLink Micro"),
                        "Sennheiser" => f.PickRandom("Momentum 4 Wireless", "HD 660S2", "Accentum Wireless"),
                        "Apple" => f.PickRandom("AirPods Max", "AirPods Pro 2", "AirPods 3rd Gen"),
                        "Samsung" => f.PickRandom("Galaxy Buds2 Pro", "Galaxy Buds FE", "Galaxy Buds3"),
                        "JBL" => f.PickRandom("Live 770NC", "Tune 720BT", "Vibe Beam"),
                        _ => f.Commerce.ProductName()
                    };
                })
                .RuleFor(h => h.Price, f => f.Random.Decimal(50.00m, 500.00m))
                .RuleFor(h => h.Description, f => f.Lorem.Sentence())
                .RuleFor(h => h.Stock, f => f.Random.Int(0, 100))
                .RuleFor(h => h.Sale, f => f.Random.Bool(0.8f) ? null : f.Random.Int(10, 50))
                .RuleFor(h => h.HeadphoneType, f => f.PickRandom<HeadphoneType>())
                .RuleFor(h => h.BatteryLife, f => f.Random.Int(10, 40))
                .RuleFor(h => h.ChargingAccessory, f => f.Random.Bool(0.3f) ? null : new ChargingAccessory
                {                   
                        Name = f.Commerce.ProductName(),
                        BatteryCapacity = f.Random.Int(1000, 5000),
                        IsFastCharging = f.Random.Bool(0.5f),
                        Port = f.PickRandom("USB-C", "Lightning", "3.5mm")
                })
                .Generate(100);
            context.Headphones.AddRange(headphones);


            var laptops = new Faker<Laptop>()
                .RuleFor(l => l.Brand, f => f.PickRandom(laptopBrands))
                .RuleFor(p => p.Model, (f, p) =>
                {
                    return p.Brand switch
                    {
                        "Dell" => f.PickRandom("XPS 15", "Latitude 7440", "Inspiron 16"),
                        "HP" => f.PickRandom("Spectre x360", "EliteBook 840 G10", "Pavilion 15"),
                        "Lenovo" => f.PickRandom("ThinkPad X1 Carbon", "Yoga 9i", "IdeaPad Flex 5"),
                        "Apple" => f.PickRandom("MacBook Air M3", "MacBook Pro 14 M3", "MacBook Pro 16 M3"),
                        "Asus" => f.PickRandom("ROG Zephyrus G14", "ZenBook 14 OLED", "Vivobook 15"),
                        "Acer" => f.PickRandom("Swift 5", "Predator Helios 16", "Aspire 3"),
                        _ => f.Commerce.ProductName()
                    };
                })
                .RuleFor(l => l.Price, f => f.Random.Decimal(1000.00m, 3500.00m))
                .RuleFor(l => l.Description, f => f.Lorem.Sentence())
                .RuleFor(l => l.Stock, f => f.Random.Int(0, 100))
                .RuleFor(l => l.Sale, f => f.Random.Bool(0.8f) ? null : f.Random.Int(10, 50))
                .RuleFor(l => l.Display, f => f.PickRandom(laptopDisplays))
                .RuleFor(l => l.Cpu, f => f.PickRandom(laptopCpus))
                .RuleFor(l => l.Gpu, f => f.PickRandom(gpus))
                .RuleFor(l => l.Ram, f => f.PickRandom(rams))
                .Generate(100);
            context.Laptops.AddRange(laptops);

            var phones = new Faker<Phone>()
                .RuleFor(p => p.Brand, f => f.PickRandom(phoneBrands))
                .RuleFor(p => p.Model, (f, p) =>
                {
                    return p.Brand switch
                    {
                        "Apple" => f.PickRandom("iPhone 15 Pro", "iPhone 15", "iPhone SE"),
                        "Samsung" => f.PickRandom("Galaxy S24 Ultra", "Galaxy Z Flip5", "Galaxy A55"),
                        "Sony" => f.PickRandom("Xperia 1 V", "Xperia 5 V", "Xperia 10 V"),
                        "Google" => f.PickRandom("Pixel 8 Pro", "Pixel 8", "Pixel 8a"),
                        "Xiaomi" => f.PickRandom("Xiaomi 14 Ultra", "Redmi Note 13 Pro", "Xiaomi 13T"),
                        "OnePlus" => f.PickRandom("OnePlus 12", "OnePlus 11R", "OnePlus Nord 3"),
                        _ => f.Commerce.ProductName()
                    };
                })
                .RuleFor(p => p.Price, f => f.Random.Decimal(300.00m, 1500.00m))
                .RuleFor(p => p.Description, f => f.Lorem.Sentence())
                .RuleFor(p => p.Stock, f => f.Random.Int(0, 100))
                .RuleFor(p => p.Sale, f => f.Random.Bool(0.8f) ? null : f.Random.Int(10, 50))
                .RuleFor(p => p.Color, f => f.PickRandom("Black", "White", "Pink", "Red", "Green", "Gold"))
                .RuleFor(p => p.Camera, f => f.PickRandom(cameras))
                .RuleFor(p => p.Display, f => f.PickRandom(phoneDisplays))
                .RuleFor(p => p.Cpu, f => f.PickRandom(phoneCpus))
                .Generate(100);
            context.Phones.AddRange(phones);
            context.SaveChanges();

            //Seeding images
            var thumbnailImages = new List<Image>();
            var products = new List<Product>();
            products.AddRange(laptops);
            products.AddRange(phones);
            products.AddRange(headphones);
            
            foreach(Product product in products)
            {
                var image = new Faker<Image>()
                    .RuleFor(i => i.Path, f => $"https://placehold.co/400x400/{new Faker().Commerce.Color().Replace(" ","")}/{new Faker().Commerce.Color().Replace(" ", "")}/?text={product.Model}")
                    .RuleFor(i => i.ProductId, f => product.Id)
                    .Generate();
                thumbnailImages.Add(image);
            }

            context.Images.AddRange(thumbnailImages);
            context.SaveChanges();
        }
    }
}

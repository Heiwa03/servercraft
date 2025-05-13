using System.Linq;
using servercraft.Models;
using servercraft.Models.Domain;

namespace servercraft
{
    public class FullSpecsSeeder
    {
        public static void Seed()
        {
            using (var db = new ServerMarketContext())
            {
                // Delete unwanted ServerFullSpecs by processor name
                var specsToDelete = db.Set<ServerFullSpecs>()
                    .Where(f =>
                        f.Processor == "Intel Xeon Gold 6248R 3.0GHz, 24C/48T" ||
                        f.Processor == "Intel Xeon Platinum 8380 2.3GHz, 40C/80T"
                    ).ToList();
                foreach (var spec in specsToDelete)
                {
                    db.ServerFullSpecs.Remove(spec);
                }
                db.SaveChanges();

                // Dell PowerEdge T40
                var t40 = db.Servers.FirstOrDefault(s => s.Name == "Dell PowerEdge T40");
                if (t40 != null)
                {
                    t40.FullSpecs = new ServerFullSpecs
                    {
                        Processor = "Intel Xeon E-2224G 3.5GHz, 4C/4T",
                        Memory = "8GB DDR4 ECC UDIMM",
                        Storage = "1TB 7200RPM SATA HDD",
                        Network = "1x Gigabit Ethernet",
                        Power = "300W Power Supply",
                        FormFactor = "Mini Tower",
                        ServerId = t40.Id
                    };
                }

                // HP ProLiant ML30
                var ml30 = db.Servers.FirstOrDefault(s => s.Name == "HP ProLiant ML30");
                if (ml30 != null)
                {
                    ml30.FullSpecs = new ServerFullSpecs
                    {
                        Processor = "Intel Xeon E-2224 3.4GHz, 4C/4T",
                        Memory = "16GB DDR4 ECC UDIMM",
                        Storage = "2TB 7200RPM SATA HDD",
                        Network = "2x Gigabit Ethernet",
                        Power = "350W Power Supply",
                        FormFactor = "Tower",
                        ServerId = ml30.Id
                    };
                }

                // StorageBox 8TB
                var storageBox = db.Servers.FirstOrDefault(s => s.Name == "StorageBox 8TB");
                if (storageBox != null)
                {
                    storageBox.FullSpecs = new ServerFullSpecs
                    {
                        Processor = "Intel Atom C3338 2.2GHz, 2C/4T",
                        Memory = "8GB DDR4 ECC",
                        Storage = "8TB SATA HDD",
                        Network = "1x Gigabit Ethernet",
                        Power = "150W Power Supply",
                        FormFactor = "1U Rack",
                        ServerId = storageBox.Id
                    };
                }

                // Lenovo ThinkSystem SR650
                var sr650 = db.Servers.FirstOrDefault(s => s.Name == "Lenovo ThinkSystem SR650");
                if (sr650 != null)
                {
                    sr650.FullSpecs = new ServerFullSpecs
                    {
                        Processor = "Intel Xeon Platinum 8280 2.7GHz, 28C/56T",
                        Memory = "64GB DDR4 ECC Registered",
                        Storage = "2TB NVMe SSD",
                        Network = "4x 10GbE SFP+",
                        Power = "2x 750W Redundant Power Supplies",
                        FormFactor = "2U Rack",
                        ServerId = sr650.Id
                    };
                }

                // Fujitsu PRIMERGY TX1330
                var tx1330 = db.Servers.FirstOrDefault(s => s.Name == "Fujitsu PRIMERGY TX1330");
                if (tx1330 != null)
                {
                    tx1330.FullSpecs = new ServerFullSpecs
                    {
                        Processor = "Intel Xeon E-2136 3.3GHz, 6C/12T",
                        Memory = "16GB DDR4 ECC UDIMM",
                        Storage = "2TB SATA HDD",
                        Network = "2x Gigabit Ethernet",
                        Power = "450W Power Supply",
                        FormFactor = "Tower",
                        ServerId = tx1330.Id
                    };
                }

                // CloudX Mini
                var cloudX = db.Servers.FirstOrDefault(s => s.Name == "CloudX Mini");
                if (cloudX != null)
                {
                    cloudX.FullSpecs = new ServerFullSpecs
                    {
                        Processor = "AMD Ryzen 5 3600 3.6GHz, 6C/12T",
                        Memory = "32GB DDR4",
                        Storage = "512GB NVMe SSD",
                        Network = "1x 2.5GbE",
                        Power = "250W Power Supply",
                        FormFactor = "Mini ITX",
                        ServerId = cloudX.Id
                    };
                }

                // Lenovo ThinkSystem ST50
                var st50 = db.Servers.FirstOrDefault(s => s.Name == "Lenovo ThinkSystem ST50");
                if (st50 != null)
                {
                    st50.FullSpecs = new ServerFullSpecs
                    {
                        Processor = "Intel Xeon E-2224G 3.5GHz, 4C/4T",
                        Memory = "8GB DDR4 ECC UDIMM",
                        Storage = "1TB SATA HDD",
                        Network = "1x Gigabit Ethernet",
                        Power = "250W Power Supply",
                        FormFactor = "Tower",
                        ServerId = st50.Id
                    };
                }

                // SuperMicro E302-9D
                var e302 = db.Servers.FirstOrDefault(s => s.Name == "SuperMicro E302-9D");
                if (e302 != null)
                {
                    e302.FullSpecs = new ServerFullSpecs
                    {
                        Processor = "Intel Xeon D-2146NT 2.3GHz, 8C/16T",
                        Memory = "32GB DDR4 ECC SODIMM",
                        Storage = "1TB NVMe SSD",
                        Network = "2x 10GbE SFP+",
                        Power = "200W Power Supply",
                        FormFactor = "Mini PC",
                        ServerId = e302.Id
                    };
                }

                // Dell PowerEdge R740
                var r740 = db.Servers.FirstOrDefault(s => s.Name == "Dell PowerEdge R740");
                if (r740 != null)
                {
                    r740.FullSpecs = new ServerFullSpecs
                    {
                        Processor = "Intel Xeon Gold 6248R 3.0GHz, 24C/48T",
                        Memory = "64GB DDR4-2933MHz ECC Registered Memory",
                        Storage = "2TB NVMe SSD + 4TB 7.2K RPM SATA HDD",
                        Network = "Dual 10GbE SFP+ + Dual 1GbE RJ45",
                        Power = "Dual 750W Redundant Power Supplies",
                        FormFactor = "2U Rack",
                        ServerId = r740.Id
                    };
                }

                db.SaveChanges();
            }
        }
    }
} 
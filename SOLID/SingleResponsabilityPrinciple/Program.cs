namespace SingleResponsabilityPrinciple {
    using System;
    using System.IO;
    using Newtonsoft.Json;
    using static System.Console;
    using Newtonsoft.Json.Converters;
    using Microsoft.Extensions.Configuration;

    class Program {
        private static IConfigurationRoot configuration;

        static void Main(string[] args) {
            var builder = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json");

            configuration = builder?.Build();

            try {
                var rating = new RatingEngine(configuration);
                var rate = rating.Rate();

                WriteLine($"{rate.Rating} - {rate.Message}");
            } catch (Exception e) {
                WriteLine($"{e.Message} - {e.StackTrace}");
            }

            ReadLine();
        }
    }

    public class RatingEngine {
        private IConfigurationRoot configuration;

        public RatingEngine (IConfigurationRoot configuration) {
            this.configuration = configuration;
        }

        public PolicyReturned Rate() {
            var response = new PolicyReturned();
            
            SaveLog($"Start consuming the method: {DateTime.Now}");

            var policy = ReadObject<Policy>(configuration["policyFile"]);

            if (policy == null) {
                response.Message = "The archive is empty or dosen't have correct values";

                return response;
            }

            switch (policy.Type) {
                case PolicyType.Auto:
                    if (string.IsNullOrEmpty(policy.Make)) {
                        response.Message = "Auto policy must specify Make";

                        return response;
                    }

                    if (policy.Make == "BMW") {
                        if (policy.Deductible < 500) {
                            response.Rating = 1000m;
                        }

                        response.Rating = 900m;
                    }

                    break;
                case PolicyType.Land:
                    if (policy.BondAmount == 0 || policy.Valuation == 0) {
                        response.Message = "Land policy must specify Bond Amount and Valuation.";

                        return response;
                    }

                    if (policy.BondAmount < 0.8m * policy.Valuation) {
                        response.Message = "Insufficient bond amount.";

                        return response;
                    }

                    response.Rating = policy.BondAmount * 0.05m;

                    break;
                case PolicyType.Life:
                    response = PoliciyLife(policy);

                    break;
                default:
                    response.Message = "Unknown policy type";
                    break;
            }

            PersistenceData(response, configuration["persitenceFile"]);

            SaveLog($"Rating completed: {DateTime.Now}");

            return response;
        }

        private void SaveLog(string message) {
            WriteLine(message);
        }

        private T ReadObject<T>(string filePath) {
            var information = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(information, new StringEnumConverter());
        }

        private PolicyReturned PoliciyLife(Policy policy) {
            var response = new PolicyReturned();

            if (policy.DateOfBirth == DateTime.MinValue) {
                response.Message = "Life policy must include Date of Birth.";
                return response;
            }

            if (policy.DateOfBirth < DateTime.Today.AddYears(-100)) {
                response.Message = "Centenarians are not eligible for coverage.";
                return response;
            }

            if (policy.Amount == 0) {
                response.Message = "Life policy must include an Amount.";
                return response;
            }

            var age = DateTime.Today.Year - policy.DateOfBirth.Year;

            if (policy.DateOfBirth.Month == DateTime.Today.Month && DateTime.Today.Day < policy.DateOfBirth.Day || DateTime.Today.Month < policy.DateOfBirth.Month) {
                age--;
            }

            var baseRate = policy.Amount * age / 200;

            if (policy.IsSmoker) {
                response.Rating = baseRate * 2;
                return response;
            }

            response.Rating = baseRate;

            return response;
        }

        private void PersistenceData<T>(T data, string filePath) {
            using(var writetext = new StreamWriter(filePath)) {
                writetext.WriteLine(JsonConvert.SerializeObject(data));
            }
        }
    }

    public class Policy {
        public PolicyType Type { get; set; }

        #region Life Insurance

        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsSmoker { get; set; }
        public decimal Amount { get; set; }

        #endregion

        #region Land

        public string Address { get; set; }
        public decimal Size { get; set; }
        public decimal Valuation { get; set; }
        public decimal BondAmount { get; set; }

        #endregion

        #region Auto

        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Miles { get; set; }
        public decimal Deductible { get; set; }

        #endregion
    }

    public class PolicyReturned {
        public string Message { get; set; } = string.Empty;
        public decimal Rating { get; set; } = 0m;
    }

    public enum PolicyType {
        Life = 0,
        Land = 1,
        Auto = 2
    }
}

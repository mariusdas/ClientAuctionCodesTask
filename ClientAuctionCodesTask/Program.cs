using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClientAuctionCodesTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var Client = new Client();

            List<Product> products = new List<Product>();
            ReadProducts(products);

            List<CardNumbers> cardNumbers = new List<CardNumbers>();
            ReadCardNumbers(cardNumbers);

            List<AuctionCodes> auctionCodes = new List<AuctionCodes>();
            ReadAuctionCodes(auctionCodes);

            var programLoop = true;
            while (programLoop)
            {
                Console.WriteLine("Paspaudus mygtuka 1 --- Sugeneruos nauja 'AKCIJOS' koda.");
                Console.WriteLine("Paspaudus mygtuka 2 --- Rodyti visus produktus.");
                Console.WriteLine("Paspaudus mygtuka 3 --- Rodyti akcijos produktus.");
                Console.WriteLine("Paspaudus mygtuka 4 --- Rodyti be akcijos produktus.");
                Console.WriteLine("Paspaudus mygtuka 5 --- Prisijungti klientui su korteles numeriu.");
                Console.WriteLine("Paspaudus mygtuka 6 --- Iseiti.");

                var input = Console.ReadLine();

                if (input == "1")
                {
                    Generate(auctionCodes);
                }
                else if (input == "2")
                {
                    Console.WriteLine("Produkto kodas || Akcija ( true - taip, false - ne )");

                    foreach (var product in products)
                    {
                        Console.WriteLine(product.ProductCode + " || " + product.IsAuction);
                    }
                }
                else if (input == "3")
                {
                    Console.WriteLine("Produkto kodas || Akcija ( true - taip, false - ne )");

                    foreach (var product in products.Where(x => x.IsAuction))
                    {
                        Console.WriteLine(product.ProductCode + " || " + product.IsAuction);
                    }
                }
                else if (input == "4")
                {
                    Console.WriteLine("Produkto kodas || Akcija ( true - taip, false - ne )");

                    foreach (var product in products.Where(x => x.IsAuction == false))
                    {
                        Console.WriteLine(product.ProductCode + " || " + product.IsAuction);
                    }
                }
                else if (input == "5")
                {
                    var checkCode = true;
                    while (checkCode)
                    {
                        Console.WriteLine("Iveskite kliento koreteles numeri. O atsaukti paspauskite 'x'. ");
                        var number = Console.ReadLine();

                        if (cardNumbers.Where(x => x.CardNumberCode == number).Count() == 1)
                        {
                            Console.WriteLine("Sveiki prisijunge!");
                            Client.CardNumber = number;
                            Client.ProdcutCodes = new List<string>();

                            var checkProductCodes = true;
                            while (checkProductCodes)
                            {
                                Console.WriteLine("Paspaudus mygtuka 1 --- Pasirinkite preke, ivede prekes koda.");
                                Console.WriteLine("Paspaudus mygtuka 2 --- Ivede akcijos koda gaunate visas akcijines prekes.");
                                Console.WriteLine("Paspaudus mygtuka 3 --- Rodyti kliento perkancias prekes, jos gali kartotis, nes gali buti, kad perkama daugiau nei viena karta.");
                                Console.WriteLine("Paspaudus mygtuka 4 --- Atsijungti.");

                                var choise = Console.ReadLine();
                                if (choise == "1")
                                {
                                    Console.WriteLine("Produkto kodas || Akcija ( true - taip, false - ne )");

                                    foreach (var product in products)
                                    {
                                        Console.WriteLine(product.ProductCode + " || " + product.IsAuction);
                                    }

                                    Console.WriteLine("Pasirinkite ir iveskite prekes koda. Jei norite iseiti paspauskite 'x'.");
                                    var productCodeInput = Console.ReadLine();

                                    if (products.Where(x => x.ProductCode == productCodeInput).Count() == 1)
                                    {
                                        Client.ProdcutCodes.Add(productCodeInput);
                                    }
                                }
                                else if (choise == "2")
                                {
                                    Console.WriteLine("Iveskite akcijos koda.");
                                    var auctionNumber = Console.ReadLine();
                                    if (auctionCodes.Where(x => x.AuctionCode == auctionNumber).Count() == 1)
                                    {
                                        foreach (var product in products.Where(x => x.IsAuction))
                                        {
                                            Client.ProdcutCodes.Add(product.ProductCode);
                                        }
                                    }
                                }
                                else if (choise == "3")
                                {
                                    if (Client.ProdcutCodes.Count() == 0)
                                    {
                                        Console.WriteLine("Nera pasirenktu prekiu");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Produkto kodas");
                                        foreach (var product in Client.ProdcutCodes)
                                        {
                                            Console.WriteLine(product);
                                        }
                                    }
                                }
                                else if (choise == "4")
                                {
                                    checkProductCodes = false;
                                    checkCode = false;
                                }
                            }

                        }
                        else
                        {
                            checkCode = false;
                        }
                    }
                }
                else if (input == "6")
                {
                    programLoop = false;
                    System.Environment.Exit(1);
                }
            }
        }

        private static void Generate(List<AuctionCodes> auctionCodes)
        {
            var checkCode = true;
            while (checkCode)
            {
                var randomCode = RandomString(8);
                if (auctionCodes.Where(x => x.AuctionCode == randomCode).Count() == 0)
                {
                    var auctionCode = new AuctionCodes();
                    auctionCode.AuctionCode = randomCode;
                    auctionCodes.Add(auctionCode);

                    using (StreamWriter sw = File.AppendText(@"AuctionCodes.txt"))
                    {
                        sw.WriteLine('\n' + randomCode);
                    }

                    Console.WriteLine("Sugeneruotas akcijos kodas: " + randomCode);
                    checkCode = false;
                }
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static void ReadAuctionCodes(List<AuctionCodes> auctionCodes)
        {
            string[] auctionCodesLines = System.IO.File.ReadAllLines(@"AuctionCodes.txt");
            foreach (var auctionCodesLine in auctionCodesLines)
            {
                var auctionCode = new AuctionCodes();
                auctionCode.AuctionCode = auctionCodesLine;
                auctionCodes.Add(auctionCode);
            }
        }

        private static void ReadCardNumbers(List<CardNumbers> cardNumbers)
        {
            string[] cardNumbersLines = System.IO.File.ReadAllLines(@"CardNumbers.txt");
            foreach (var cardNumbersLine in cardNumbersLines)
            {
                var cardNumber = new CardNumbers();
                cardNumber.CardNumberCode = cardNumbersLine;
                cardNumbers.Add(cardNumber);
            }
        }

        private static void ReadProducts(List<Product> products)
        {
            string[] productCodeLines = System.IO.File.ReadAllLines(@"ProductCodes.txt");
            foreach (var productCodeLine in productCodeLines)
            {
                var splitProduct = productCodeLine.Split(' ');
                var product = new Product();
                product.ProductCode = splitProduct[0];
                product.IsAuction = Convert.ToBoolean(Convert.ToInt16(splitProduct[1]));
                products.Add(product);
            }
        }
    }
}

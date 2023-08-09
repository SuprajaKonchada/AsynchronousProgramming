using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XMLProcessing
{
    class Program
    {
        static async Task Main()
        {
            try
            {
                string xmlFilePath = "C:\\Users\\Supraja Konchada\\source\\repos\\XMLProcessing\\XMLProcessing\\Input.xml";
                XDocument xmlDoc = XDocument.Load(xmlFilePath);
                //stopwatch to calculate time for total response
                var totalResponsetime = new Stopwatch();
                totalResponsetime.Start();
                //stopwatch to calculate time for each responses
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                string orderId = GetOrderId(xmlDoc);
                PrintElapsedTime("Order ID", stopwatch);

                DateTime requestedShipDate = GetRequestedShipDate(xmlDoc);
                PrintElapsedTime("Order Request Date", stopwatch);

                string createdBy = GetCreatedBy(xmlDoc);
                PrintElapsedTime("CreatedBy", stopwatch);

                double totalLoad = GetTotalLoad(xmlDoc);
                PrintElapsedTime("Load of the order", stopwatch);

                int totalQuantity = GetTotalQuantity(xmlDoc);
                PrintElapsedTime("Quantities of the Order", stopwatch);

                double totalPrice = GetTotalPrice(xmlDoc);
                PrintElapsedTime("Price", stopwatch);

                int maxLeadTime = GetMaxLeadTime(xmlDoc);
                PrintElapsedTime("Manufacturing Days", stopwatch);

                double availableTruck = GetAvailableTruck(maxLeadTime, totalLoad);
                PrintElapsedTime("Getting Available Truck", stopwatch);

                var customerNames = GetCustomerNames(xmlDoc);
                var customerAddresses = GetCustomerAddresses(xmlDoc);
                var customerPhones = GetCustomerPhones(xmlDoc);
                var customerEmails = GetCustomerEmails(xmlDoc);

                stopwatch.Stop();
                totalResponsetime.Stop();
                Console.WriteLine($"Total Response Time {totalResponsetime.ElapsedMilliseconds} ms");

                DisplayOutput(orderId, requestedShipDate, createdBy, totalLoad, totalQuantity, totalPrice, maxLeadTime, availableTruck, customerNames, customerAddresses, customerPhones, customerEmails);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        /// <summary>
        /// Gets the ID of Order
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        static string GetOrderId(XDocument xmlDoc)
        {
            return xmlDoc.Descendants("ID").FirstOrDefault()?.Value;
        }
        /// <summary>
        /// Gets the requested ship date
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>

        static DateTime GetRequestedShipDate(XDocument xmlDoc)
        {
            return DateTime.Parse(xmlDoc.Descendants("RequestedShipDate").FirstOrDefault()?.Value);
        }
        /// <summary>
        /// gets the created by
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        static string GetCreatedBy(XDocument xmlDoc)
        {
            return xmlDoc.Descendants("CreatedBy").FirstOrDefault()?.Value;
        }
        /// <summary>
        /// gets the total number of load factors
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        static double GetTotalLoad(XDocument xmlDoc)
        {
            return xmlDoc.Descendants("LOADFACTOR").Select(e => double.Parse(e.Value)).Sum();
        }
        /// <summary>
        /// gets the total number of quantities
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        static int GetTotalQuantity(XDocument xmlDoc)
        {
            return xmlDoc.Descendants("Order_Quantity").Select(e => int.Parse(e.Value)).Sum();
        }
        /// <summary>
        /// gets the total price according to the number of items 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        static double GetTotalPrice(XDocument xmlDoc)
        {
            var prices = xmlDoc.Descendants("USPrice").Select(e => double.Parse(e.Value)).ToList();
            var quantities = xmlDoc.Descendants("Order_Quantity").Select(e => int.Parse(e.Value)).ToList();

            double totalPrice = 0;
            for (int i = 0; i < prices.Count; i++)
            {
                totalPrice += prices[i] * quantities[i];
            }
            return totalPrice;

        }
        /// <summary>
        /// gets the maximum lead time
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        static int GetMaxLeadTime(XDocument xmlDoc)
        {
            return xmlDoc.Descendants("LeadTime").Select(e => int.Parse(e.Value)).Max();
        }
        /// <summary>
        /// gets the available trucks
        /// </summary>
        /// <param name="manufacturingDays"></param>
        /// <param name="load"></param>
        /// <returns></returns>
        static double GetAvailableTruck(int manufacturingDays, double load)
        {
            return manufacturingDays * load;
        }
        /// <summary>
        /// gets the names of customers in a list
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        static List<string> GetCustomerNames(XDocument xmlDoc)
        {
            return xmlDoc.Descendants("Name").Select(e => e.Value).ToList();
        }
        /// <summary>
        /// gets the addresses of every customer
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>

        static List<string> GetCustomerAddresses(XDocument xmlDoc)
        {
            return xmlDoc.Descendants("Address1").Select(e => e.Value).ToList();
        }
        /// <summary>
        /// gets the phonenumber of every customer
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        static List<string> GetCustomerPhones(XDocument xmlDoc)
        {
            return xmlDoc.Descendants("Phone").Select(e => e.Value).ToList();
        }
        /// <summary>
        /// gets the email ID of every customer
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        static List<string> GetCustomerEmails(XDocument xmlDoc)
        {
            return xmlDoc.Descendants("DeliveryReceiptEmail").Select(e => e.Value).ToList();
        }
        /// <summary>
        /// Print statement to display as output
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="requestedShipDate"></param>
        /// <param name="createdBy"></param>
        /// <param name="totalLoad"></param>
        /// <param name="totalQuantity"></param>
        /// <param name="totalPrice"></param>
        /// <param name="maxLeadTime"></param>
        /// <param name="availableTruck"></param>
        /// <param name="customerNames"></param>
        /// <param name="customerAddresses"></param>
        /// <param name="customerPhones"></param>
        /// <param name="customerEmails"></param>
        static void DisplayOutput(string orderId, DateTime requestedShipDate, string createdBy, double totalLoad, int totalQuantity, double totalPrice, int maxLeadTime, double availableTruck, List<string> customerNames, List<string> customerAddresses, List<string> customerPhones, List<string> customerEmails)
        {
            Console.WriteLine("\n--- Generated Output ---");
            Console.WriteLine($"Order ID: {orderId}");
            Console.WriteLine($"Order Request Date: {requestedShipDate}");
            Console.WriteLine($"CreatedBy: {createdBy}");
            Console.WriteLine($"Load of the order: {totalLoad}");
            Console.WriteLine($"Quantities of the Order: {totalQuantity}");
            Console.WriteLine($"Price: {totalPrice}");
            Console.WriteLine($"Manufacturing Days: {maxLeadTime}");
            Console.WriteLine($"Available Truck: {availableTruck}");
            for (int i = 0; i < customerNames.Count; i++)
            {
                Console.WriteLine($"\nCustomer{i + 1} Name: {customerNames[i]}");
                Console.WriteLine($"Customer{i + 1} Address: {customerAddresses[i]}");
                Console.WriteLine($"Customer{i + 1} Phone: {customerPhones[i]}");
                Console.WriteLine($"Customer{i + 1} Email: {customerEmails[i]}");
            }
        }
        /// <summary>
        /// calculating time for the property and then restarts the stopwatch
        /// </summary>
        /// <param name="property"></param>
        /// <param name="stopwatch"></param>
        static void PrintElapsedTime(string property, Stopwatch stopwatch)
        {
            Console.WriteLine($"{property} : {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Restart();
        }
    }
}

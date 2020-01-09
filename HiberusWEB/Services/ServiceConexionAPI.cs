using HiberusWEB.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HiberusWEB.Services
{
    public static class ServiceConexionAPI
    {
        static List<RateEnt> ratesList = new List<RateEnt>();
        static List<Transaction> transactionList = new List<Transaction>();

        /// <summary>
        /// Método asíncrono de acceso al API de rates
        /// </summary>
        /// <returns>Ienumerable(RateEnt)</returns>
        async private static Task GetRatesAPI()
        {

            ratesList = new List<RateEnt>();

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:51164/api/rates");
            HttpContent content = response.Content;

            string data = await content.ReadAsStringAsync();

            ratesList = JsonConvert.DeserializeObject<List<RateEnt>>(data);

        }

        /// <summary>
        /// Método asíncrono de acceso al API de transactions
        /// </summary>
        /// <returns>Ienumerable(Transaction)</returns>
        async private static Task GetTransactionsAPI()
        {
            transactionList = new List<Transaction>();

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:51164/api/transactions");
            HttpContent content = response.Content;

            string data = await content.ReadAsStringAsync();

            transactionList = JsonConvert.DeserializeObject<List<Transaction>>(data);

        }

        /// <summary>
        /// Función estática que devuelve la lista con los
        /// resultados de la API Rates
        /// </summary>
        /// <returns>Ienumerable(RateEnt)</returns>
        public static IEnumerable<RateEnt> GetRates()
        {
            //Esperamos a la ejecución
            GetRatesAPI().Wait();

            //Devolvemos la lista rellena por el método asincrono
            return ratesList;
        }

        /// <summary>
        /// Función estática que devuelve la lista con los
        /// resultados de la API Rates
        /// </summary>
        /// <returns>Ienumerable(Transaction)</returns>
        public static IEnumerable<Transaction> GetTransactions()
        {
            //Esperamos a la ejecución
            GetTransactionsAPI().Wait();

            //Devolvemos la lista rellena por el método asincrono
            return transactionList;
        }
    }
}

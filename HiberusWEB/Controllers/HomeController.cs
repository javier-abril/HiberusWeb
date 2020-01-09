using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HiberusWEB.Models;
using HiberusWEB.Negocio;

namespace HiberusWEB.Controllers
{
    public class HomeController : Controller
    {
        LogicaNegocio negocio = new LogicaNegocio();

        // GET: Home
        public IActionResult Index()
        {

            return View();
        }

        // GET: Home/Rates
        public IActionResult Rates()
        {
            try
            {

                List<RateEnt> listRates = Services.ServiceConexionAPI.GetRates().ToList();
                return View(listRates);

            }catch (Exception ex)
            {
                return StatusCode(503,"Se ha producido un error conectando a la API");
            }
        }

        // GET: Home/Transactions
        public IActionResult Transactions()
        {
            try
            {
                List<Transaction> listTransactions = Services.ServiceConexionAPI.GetTransactions().ToList();
                return View(listTransactions);
            }
            catch (Exception ex)
            {
                return StatusCode(503, "Se ha producido un error conectando a la API");
            }
        }

        // GET: Home/Sku/Id
        public IActionResult Sku(string Id)
        {
            try
            {
                string skuObtenido = Id;
                List<Transaction> listTransactionsEUR;
                List<Transaction> listTransactionsFiltrada;
                decimal sumaTotal=0;
                
                //Filtramos la lista con el SKU recibido con LINQ
                listTransactionsFiltrada = Services.ServiceConexionAPI.GetTransactions()
                                                    .Where(tr => tr.Sku.ToUpper().Equals(skuObtenido.ToUpper().Trim())).ToList();

                //Realizamos el calculo en euros
                listTransactionsEUR = negocio.CalculoEnEuros(listTransactionsFiltrada).ToList();

                //Sacamos el total para devolverlo en el viewbag
                foreach (Transaction tr in listTransactionsEUR)
                {
                    sumaTotal = sumaTotal + tr.Amount;
                }

                //Usamos math.round que por default hace redondeo bancario
                ViewBag.SumaTotal = Math.Round(sumaTotal,2);

                ViewBag.SkuSeleccionado = Id;

                return View(listTransactionsEUR);
            }
            catch (Exception ex)
            {
                return StatusCode(503, "Se ha producido un error conectando a la API");
            }
        }


        // GET: Home/Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

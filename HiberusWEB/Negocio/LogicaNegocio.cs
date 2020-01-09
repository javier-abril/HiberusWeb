using HiberusWEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiberusWEB.Negocio
{
    public class LogicaNegocio
    {
        public IEnumerable<Transaction> CalculoEnEuros (IEnumerable<Transaction> listaEntrada)
        {
            List<RateEnt> ratesList = Services.ServiceConexionAPI.GetRates().ToList();
            List<Transaction> listaSalida = new List<Transaction>();

            foreach (Transaction tr in listaEntrada)
            {
                //Si viene a Euros directamente la insertamos
                if (tr.Currency.Equals("EUR"))
                {
                    listaSalida.Add(tr);
                }
                else //Sino buscamos la conversion
                {
                    //Buscamos primero conversion directa
                    RateEnt rate = ratesList.Where(rt => rt.From.Equals(tr.Currency) && rt.To.Equals("EUR")).FirstOrDefault();

                    //Si lo encuentra a la primera calculamos el Amount a EUR y lo añadimos
                    if (rate != null)
                    {
                        Transaction trTemp = tr;

                        trTemp.Currency = "EUR";
                        trTemp.Amount = tr.Amount * rate.Rate;

                        listaSalida.Add(trTemp);
                    }
                    else
                    {
                        RateEnt primerRate=new RateEnt();
                        RateEnt rateIntermedio=new RateEnt();
                        Transaction trTemp = tr;

                        //Cogemos todos los rates que tienen from = al de entrada
                        List<RateEnt> ratesFrom = ratesList.Where(rt => rt.From.Equals(tr.Currency)).ToList();
                        
                        //Por cada rate from(secundario) buscamos si tiene conversion a Euros
                        foreach (RateEnt rateTo in ratesFrom)
                        {
                            RateEnt rateToEUR = ratesList.Where(rt => rt.From.Equals(rateTo.To) && rt.To.Equals("EUR")).FirstOrDefault();

                            //Si encuentra el secundario asigna los rates para los calculos posteriores
                            if (rateToEUR != null)
                            {
                                primerRate = rateTo;
                                rateIntermedio = rateToEUR;
                            }
                        }

                        trTemp.Currency = "EUR";
                        trTemp.Amount = (tr.Amount * primerRate.Rate) / rateIntermedio.Rate;

                        listaSalida.Add(trTemp);

                    }
                }
            }

            return listaEntrada;
        }
    }
}

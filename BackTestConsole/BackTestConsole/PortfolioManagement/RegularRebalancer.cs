using PricingLibrary.RebalancingOracleDescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackTestConsole.PortfolioManagement
{
    class RegularRebalancer
    {
        
        public int Period;

        public RegularRebalancer(IRebalancingOracleDescription description)
        {
            switch (description.Type)
            {
                case RebalancingOracleType.Regular:
                    RegularOracleDescription? regularOracleDescription = description as RegularOracleDescription;
                    Period = regularOracleDescription.Period;
                    break;

                case RebalancingOracleType.Weekly:
                    // Traitement pour le rééquilibrage hebdomadaire
                    Console.WriteLine("Weekly rebalancing is selected. Not implemented yet");

                    break;

                default:
                    // Traitement pour les valeurs inconnues
                    Console.WriteLine("Unknown rebalancing type.");
                    break;
            }
        }
    }
}
namespace IRPF.Console;

using System;
using System.Globalization;

/// <summary>
/// Main program class for the IRPF (Income Tax) calculator application.
/// </summary>
public class Program
{
    /// <summary>
    /// Entry point of the application.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    public static void Main(string[] args)
    {
        var taxCalculationService = new TaxCalculationService();
        var userInterface = new ConsoleUserInterface();
        
        int numberOfTaxpayers = userInterface.GetNumberOfTaxpayers();
        
        for (int i = 0; i < numberOfTaxpayers; i++)
        {
            // Get taxpayer information
            string taxpayerName = userInterface.GetTaxpayerName();
            decimal grossSalary = userInterface.GetGrossSalary();
            
            // Calculate taxes
            var taxResult = taxCalculationService.CalculateTaxes(grossSalary);
            
            // Display results
            userInterface.DisplayTaxResults(taxpayerName, grossSalary, taxResult);
        }
    }
}

/// <summary>
/// Represents the result of tax calculations.
/// </summary>
public class TaxResult
{
    /// <summary>
    /// Gets or sets the INSS (social security) deduction amount.
    /// </summary>
    public decimal InssDeduction { get; set; }
    
    /// <summary>
    /// Gets or sets the income tax deduction amount.
    /// </summary>
    public decimal IncomeTaxDeduction { get; set; }
    
    /// <summary>
    /// Gets or sets the net salary after all deductions.
    /// </summary>
    public decimal NetSalary { get; set; }
}

/// <summary>
/// Service responsible for calculating taxes.
/// </summary>
public class TaxCalculationService
{
    /// <summary>
    /// Calculates all applicable taxes for a given gross salary.
    /// </summary>
    /// <param name="grossSalary">The gross salary amount.</param>
    /// <returns>A TaxResult object containing all tax calculations.</returns>
    public TaxResult CalculateTaxes(decimal grossSalary)
    {
        decimal inssDeduction = CalculateInssDeduction(grossSalary);
        decimal incomeTaxBase = grossSalary - inssDeduction;
        decimal incomeTaxDeduction = CalculateIncomeTaxDeduction(incomeTaxBase);
        decimal netSalary = grossSalary - inssDeduction - incomeTaxDeduction;
        
        return new TaxResult
        {
            InssDeduction = inssDeduction,
            IncomeTaxDeduction = incomeTaxDeduction,
            NetSalary = netSalary
        };
    }
    
    /// <summary>
    /// Calculates the INSS (social security) deduction based on the gross salary.
    /// </summary>
    /// <param name="grossSalary">The gross salary amount.</param>
    /// <returns>The INSS deduction amount.</returns>
    private decimal CalculateInssDeduction(decimal grossSalary)
    {
        // INSS tax brackets for 2024
        const decimal BRACKET_1_LIMIT = 1412.00m;
        const decimal BRACKET_2_LIMIT = 2666.68m;
        const decimal BRACKET_3_LIMIT = 4000.03m;
        const decimal BRACKET_4_LIMIT = 7786.02m;
        const decimal INSS_CEILING = 908.85m; // Maximum INSS contribution
        
        // INSS tax rates and adjustments
        const decimal RATE_1 = 0.075m;
        const decimal RATE_2 = 0.09m;
        const decimal RATE_3 = 0.12m;
        const decimal RATE_4 = 0.14m;
        const decimal ADJUSTMENT_2 = 21.18m;
        const decimal ADJUSTMENT_3 = 101.18m;
        const decimal ADJUSTMENT_4 = 181.18m;
        
        if (grossSalary <= BRACKET_1_LIMIT)
            return grossSalary * RATE_1;
        else if (grossSalary <= BRACKET_2_LIMIT)
            return grossSalary * RATE_2 - ADJUSTMENT_2;
        else if (grossSalary <= BRACKET_3_LIMIT)
            return grossSalary * RATE_3 - ADJUSTMENT_3;
        else if (grossSalary <= BRACKET_4_LIMIT)
            return grossSalary * RATE_4 - ADJUSTMENT_4;
        else
            return INSS_CEILING;
    }
    
    /// <summary>
    /// Calculates the income tax deduction based on the taxable income.
    /// </summary>
    /// <param name="taxableIncome">The taxable income amount after INSS deduction.</param>
    /// <returns>The income tax deduction amount.</returns>
    private decimal CalculateIncomeTaxDeduction(decimal taxableIncome)
    {
        // Income tax brackets for 2024
        const decimal BRACKET_1_LOWER = 1903.99m;
        const decimal BRACKET_1_UPPER = 2826.65m;
        const decimal BRACKET_2_LOWER = 2826.66m;
        const decimal BRACKET_2_UPPER = 3751.05m;
        const decimal BRACKET_3_LOWER = 3751.06m;
        const decimal BRACKET_3_UPPER = 4664.68m;
        const decimal BRACKET_4_LOWER = 4664.68m;
        
        // Income tax rates and deductions
        const decimal RATE_1 = 0.075m;
        const decimal RATE_2 = 0.15m;
        const decimal RATE_3 = 0.225m;
        const decimal RATE_4 = 0.275m;
        const decimal DEDUCTION_1 = 142.80m;
        const decimal DEDUCTION_2 = 354.80m;
        const decimal DEDUCTION_3 = 636.13m;
        const decimal DEDUCTION_4 = 869.36m;
        
        if (taxableIncome < BRACKET_1_LOWER)
            return 0;
        else if (taxableIncome <= BRACKET_1_UPPER)
            return taxableIncome * RATE_1 - DEDUCTION_1;
        else if (taxableIncome <= BRACKET_2_UPPER)
            return taxableIncome * RATE_2 - DEDUCTION_2;
        else if (taxableIncome <= BRACKET_3_UPPER)
            return taxableIncome * RATE_3 - DEDUCTION_3;
        else
            return taxableIncome * RATE_4 - DEDUCTION_4;
    }
}

/// <summary>
/// Handles user interaction through the console.
/// </summary>
public class ConsoleUserInterface
{
    private readonly CultureInfo _brazilianCulture = new CultureInfo("pt-BR");
    
    /// <summary>
    /// Gets the number of taxpayers to process from the user.
    /// </summary>
    /// <returns>The number of taxpayers.</returns>
    public int GetNumberOfTaxpayers()
    {
        int numberOfTaxpayers;
        bool isValidInput;
        
        do
        {
            Console.WriteLine("Informe o número de contribuintes a calcular:");
            isValidInput = int.TryParse(Console.ReadLine(), out numberOfTaxpayers) && numberOfTaxpayers > 0;
            
            if (!isValidInput)
            {
                Console.WriteLine("Por favor informe um valor válido para o número de contribuintes.");
            }
        } while (!isValidInput);
        
        return numberOfTaxpayers;
    }
    
    /// <summary>
    /// Gets the taxpayer's name from the user.
    /// </summary>
    /// <returns>The taxpayer's name.</returns>
    public string GetTaxpayerName()
    {
        string taxpayerName;
        bool isValidInput;
        
        do
        {
            Console.WriteLine("Informe o nome do contribuinte:");
            taxpayerName = Console.ReadLine() ?? string.Empty;
            isValidInput = !string.IsNullOrWhiteSpace(taxpayerName);
            
            if (!isValidInput)
            {
                Console.WriteLine("Por favor, informe um valor válido para o contribuinte.");
            }
        } while (!isValidInput);
        
        return taxpayerName;
    }
    
    /// <summary>
    /// Gets the gross salary amount from the user.
    /// </summary>
    /// <returns>The gross salary amount.</returns>
    public decimal GetGrossSalary()
    {
        decimal grossSalary;
        bool isValidInput;
        
        do
        {
            Console.WriteLine("Informe um valor válido para o salário:");
            isValidInput = decimal.TryParse(Console.ReadLine(), out grossSalary) && grossSalary > 0;
            
            if (!isValidInput)
            {
                Console.WriteLine("Por favor informe um valor válido para o salário.");
            }
        } while (!isValidInput);
        
        return grossSalary;
    }
    
    /// <summary>
    /// Displays the tax calculation results for a taxpayer.
    /// </summary>
    /// <param name="taxpayerName">The name of the taxpayer.</param>
    /// <param name="grossSalary">The gross salary amount.</param>
    /// <param name="taxResult">The tax calculation results.</param>
    public void DisplayTaxResults(string taxpayerName, decimal grossSalary, TaxResult taxResult)
    {
        Console.WriteLine();
        Console.WriteLine($"Nome do Contribuinte: {taxpayerName}");
        Console.WriteLine($"Salário Bruto: {grossSalary.ToString("C", _brazilianCulture)}");
        Console.WriteLine($"Desconto INSS: {taxResult.InssDeduction.ToString("C", _brazilianCulture)}");
        Console.WriteLine($"Desconto IR: {taxResult.IncomeTaxDeduction.ToString("C", _brazilianCulture)}");
        Console.WriteLine($"Salário Líquido: {taxResult.NetSalary.ToString("C", _brazilianCulture)}");
        Console.WriteLine();
    }
}

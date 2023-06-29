namespace IRPF.Console;
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Informe o número de contribuintes a calcular:");
        int numeroContribuintes = 0;
        if (int.TryParse(Console.ReadLine(), out int resultNumeroContribuintes))
            numeroContribuintes = resultNumeroContribuintes;
        else
        {
            Console.WriteLine("Por favor informe um valor válido para o número de contribuintes.");
            Main(args);
        }

        int contador = 0;
        while (contador < numeroContribuintes)
        {
            string? contribuinte = ObterContribuinte();
            decimal salarioBruto = ObterSalarioBruto();

            decimal desconto = 0;
            if (salarioBruto >= 1903.99m && salarioBruto <= 2826.65m)
                desconto = salarioBruto * 0.075m - 142.80m;
            else if (salarioBruto >= 2826.66m && salarioBruto <= 3751.05m)
                desconto = salarioBruto * 0.15m - 354.80m;
            else if (salarioBruto >= 3751.06m && salarioBruto <= 4664.68m)
                desconto = salarioBruto * 0.225m - 636.13m;
            else if (salarioBruto > 4664.68m)
                desconto = salarioBruto * 0.275m - 869.36m;

            decimal salarioLiquido = salarioBruto - desconto;

            Console.WriteLine($"Nome do Contribuinte: {contribuinte}");
            Console.WriteLine($"Salário Bruto: {salarioBruto,2:c}");
            Console.WriteLine($"Desconto: {desconto,2:c}");
            Console.WriteLine($"Salário Liquido: {salarioLiquido,2:c}");

            contador++;
        }
    }

    private static decimal ObterSalarioBruto()
    {
        Console.WriteLine("Informe um valor válido para o salário:");
        decimal salarioBruto;
        if (decimal.TryParse(Console.ReadLine(), out decimal resultSalarioBruto))
            salarioBruto = resultSalarioBruto;
        else
        {
            Console.WriteLine("Por favor informe um valor válido para o salário.");
            salarioBruto = ObterSalarioBruto();
        }
        return salarioBruto;
    }

    private static string? ObterContribuinte()
    {
        Console.WriteLine("Informe o nome do contribuinte:");
        string? contribuinte = Console.ReadLine();        
        if (string.IsNullOrWhiteSpace(contribuinte))
        {
            Console.WriteLine("Por favor, informe um valor válido para o contribuinte.");
            contribuinte = ObterContribuinte();
        }
        return contribuinte;
    }
}

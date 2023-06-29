namespace IRPF.Console;
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Informe o nome do contribuinte:");
        string? contribuinte = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(contribuinte))
        {
            Console.WriteLine("Por favor, informe um valor válido para o contribuinte.");
            Main(args);
        }

        Console.WriteLine("Informe um valor válido para o salário:");
        decimal salarioBruto = 0;
        if (decimal.TryParse(Console.ReadLine(), out decimal result))
            salarioBruto = result;
        else
        {
            Console.WriteLine("Por favor informe um valor válido para o salário.");
            Main(args);
        }

        decimal desconto = 0;

        if (salarioBruto >= 1903.99m && salarioBruto <= 2826.65m)
            desconto = salarioBruto * 0.075m - 142.80m;
        else if(salarioBruto >= 2826.66m && salarioBruto <= 3751.05m)
            desconto = salarioBruto * 0.15m - 142.80m;
        else if (salarioBruto >= 3751.06m && salarioBruto <= 4664.68m)
            desconto = salarioBruto * 0.225m - 142.80m;
        else if (salarioBruto > 4664.68m)
            desconto = salarioBruto * 0.275m - 142.80m;

        decimal salarioLiquido = salarioBruto - desconto;

        Console.WriteLine($"Nome do Contribuinte: {contribuinte}");
        Console.WriteLine($"Salário Bruto: {salarioBruto,2:c}");
        Console.WriteLine($"Desconto: {desconto,2:c}");
        Console.WriteLine($"Salário Liquido: {salarioLiquido,2:c}");
    }
}

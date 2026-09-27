using System;

namespace Ej1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const decimal iva = 0.21m;
            const decimal envioGratis = 50m;
            const decimal costeEnvio = 4.95m;

            Console.WriteLine("Introduce el nombre del producto:");
            string nombreIntroducido = Console.ReadLine();
            string nombre = string.IsNullOrWhiteSpace(nombreIntroducido) ? "Producto sin nombre" : nombreIntroducido;

            Console.WriteLine("Introduce el precio unitario:");
            if (!decimal.TryParse(Console.ReadLine(), out decimal precioProd) || precioProd <= 0)
            {
                Console.WriteLine("Precio no válido.");
                return;
            }

            Console.WriteLine("Introduce las unidades del producto:");
            if (!int.TryParse(Console.ReadLine(), out int unidades) || unidades <= 0)
            {
                Console.WriteLine("Unidades no válidas.");
                return;
            }

            Console.WriteLine("Introduce el cupón de descuento (Enter si no tienes):");
            string cupon = Console.ReadLine();
          
            decimal descuento;
            bool cuponInvalido = false;

            switch (cupon?.ToUpper())
            {
                case "DAM10":
                    descuento = 0.10m;
                    break;
                case "DAM20":
                    descuento = 0.20m;
                    break;
                default:
                    descuento = 0m;
                    if (cupon != null)
                        cuponInvalido = true;
                    cupon = "No hay cupon";
                    break;
            }

            decimal subtotal = unidades * precioProd;
            decimal descuentoImporte = subtotal * descuento;
            decimal baseImponible = subtotal - descuentoImporte;
            decimal ivaImporte = baseImponible * iva;
            decimal totalProductos = baseImponible + ivaImporte;
            decimal envio = totalProductos >= envioGratis ? 0 : costeEnvio;
            decimal totalAPagar = totalProductos + envio;

            Console.WriteLine("========== TICKET ==========");
            Console.WriteLine($"{nombre}: {unidades} {(unidades > 1 ? "unidades" : "unidad")} x {precioProd:F2} €");
            Console.WriteLine($"Subtotal: {subtotal:F2} €");
            Console.WriteLine($"Cupón: {cupon ?? "(ninguno)"} (-{descuentoImporte:F2} €)");
            Console.WriteLine($"Base imponible: {baseImponible:F2} €");
            Console.WriteLine($"IVA ({iva:P0}): {ivaImporte:F2} €");
            Console.WriteLine($"Total productos: {totalProductos:F2} €");
            Console.WriteLine($"Envío: {(envio>0 ? "4.95€" : "gratis")} ");
            Console.WriteLine($"TOTAL A PAGAR: {totalAPagar:F2} €");
            
            if (cuponInvalido)
            {
                Console.WriteLine("Aviso: el cupón introducido no es válido, no se aplicó ningún descuento.");
            }
        }
    }
}

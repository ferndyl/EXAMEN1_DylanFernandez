using System;

// Variables globales
string nombre = string.Empty, id = string.Empty, cargo = string.Empty;
float horasNormales = 0f, horasExtras = 0f, horasTrabajadasTotales = 0f;
float tarifaBase = 4000.0f, tarifaExtra = 6000.0f;
float salarioBruto = 0f, deducciones = 0f, salarioNeto = 0f;
int diasTrabajados = 0, diasConPenalizacion = 0;
bool asistenciaPerfecta = true;

float ConvertirHora12a24(int hora, string periodo)
{
    if (periodo == "PM" && hora != 12) return hora + 12;
    if (periodo == "AM" && hora == 12) return 0;
    return hora;
}

string ValidarPeriodo()
{
    string periodo;
    do
    {
        Console.Write("Ingrese AM o PM: ");
        periodo = Console.ReadLine().ToUpper();
        if (periodo != "AM" && periodo != "PM")
            Console.WriteLine("Error: Debe ingresar AM o PM correctamente.");
    } while (periodo != "AM" && periodo != "PM");
    return periodo;
}

void SolicitudDatos()
{
    Console.Clear();
    Console.Write("Ingrese el ID del empleado: ");
    id = Console.ReadLine();
    while (string.IsNullOrEmpty(id))
    {
        Console.Write("Error: El ID no puede estar vacío. Ingrese nuevamente: ");
        id = Console.ReadLine();
    }

    Console.Write("Ingrese el nombre del empleado: ");
    nombre = Console.ReadLine();
    while (string.IsNullOrEmpty(nombre))
    {
        Console.Write("Error: El nombre no puede estar vacío. Ingrese nuevamente: ");
        nombre = Console.ReadLine();
    }

    Console.Write("Ingrese el cargo del empleado: ");
    cargo = Console.ReadLine();
    while (string.IsNullOrEmpty(cargo))
    {
        Console.Write("Error: El cargo no puede estar vacío. Ingrese nuevamente: ");
        cargo = Console.ReadLine();
    }

    Console.Write("Ingrese los días trabajados en la semana (mínimo 2, máximo 7): ");
    while (!int.TryParse(Console.ReadLine(), out diasTrabajados) || diasTrabajados < 2 || diasTrabajados > 7)
    {
        Console.Write("Error: Los días deben estar entre 2 y 7. Ingrese nuevamente: ");
    }

    horasNormales = 0;
    horasExtras = 0;
    horasTrabajadasTotales = 0;
    diasConPenalizacion = 0;

    for (int i = 1; i <= diasTrabajados; i++)
    {
        int horaEntrada, horaSalida;
        string periodoEntrada, periodoSalida;
        float horasDia;

        Console.Write($"Ingrese la hora de entrada (1-12) para el día {i}: ");
        while (!int.TryParse(Console.ReadLine(), out horaEntrada) || horaEntrada < 1 || horaEntrada > 12)
        {
            Console.Write("Error: La hora debe estar entre 1 y 12. Ingrese nuevamente: ");
        }
        periodoEntrada = ValidarPeriodo();
        horaEntrada = (int)ConvertirHora12a24(horaEntrada, periodoEntrada);

        Console.Write($"Ingrese la hora de salida (1-12) para el día {i}: ");
        while (!int.TryParse(Console.ReadLine(), out horaSalida) || horaSalida < 1 || horaSalida > 12)
        {
            Console.Write("Error: La hora debe estar entre 1 y 12. Ingrese nuevamente: ");
        }
        periodoSalida = ValidarPeriodo();
        horaSalida = (int)ConvertirHora12a24(horaSalida, periodoSalida);

        if (horaSalida <= horaEntrada)
        {
            Console.WriteLine("Error: La hora de salida debe ser mayor que la de entrada.");
            asistenciaPerfecta = false;
            continue;
        }

        horasDia = horaSalida - horaEntrada;
        if (horasDia < 4)
        {
            Console.WriteLine("Error: No puede trabajar menos de 4 horas al día. Se anulará el registro de este día.");
            asistenciaPerfecta = false;
            diasConPenalizacion++;
            continue;
        }

        if (horaEntrada > 9)
        {
            Console.WriteLine("Penalización: Llegó tarde este día.");
            diasConPenalizacion++;
        }

        if (horasDia > 8)
        {
            horasExtras += horasDia - 8;
            horasNormales += 8;
        }
        else
        {
            horasNormales += horasDia;
        }

        horasTrabajadasTotales += horasDia;
    }

    if (horasTrabajadasTotales > 48)
    {
        horasExtras += horasTrabajadasTotales - 48;
        horasNormales = 48;
    }

    Console.WriteLine("Datos ingresados correctamente.");
    Console.ReadKey();
}

void CalcularSalario()
{
    salarioBruto = (horasNormales * tarifaBase) + (horasExtras * tarifaExtra);
    if (asistenciaPerfecta) salarioBruto += 50000;
    float penalizacion = salarioBruto * 0.05f * diasConPenalizacion;
    deducciones = (salarioBruto * 0.09f) + penalizacion;
    salarioNeto = salarioBruto - deducciones;

    Console.WriteLine("Salario calculado correctamente.");
    Console.ReadKey();
}

void MostrarReporte()
{
    Console.Clear();
    Console.WriteLine("--- Reporte de Empleado ---");
    Console.WriteLine($"ID: {id}");
    Console.WriteLine($"Nombre: {nombre}");
    Console.WriteLine($"Cargo: {cargo}");
    Console.WriteLine($"Horas Normales: {horasNormales}");
    Console.WriteLine($"Horas Extras: {horasExtras}");
    Console.WriteLine($"Días Trabajados: {diasTrabajados}");
    Console.WriteLine($"Días con Penalización: {diasConPenalizacion}");
    Console.WriteLine($"Horas Totales: {horasTrabajadasTotales}");
    Console.WriteLine($"Salario Bruto: ¢{salarioBruto}");
    Console.WriteLine($"Deducciones (9% + Penalización): ¢{deducciones}");
    Console.WriteLine($"Salario Neto: ¢{salarioNeto}");
    Console.ReadKey();
}

void Menu()
{
    byte opcion;
    do
    {
        Console.Clear();
        Console.WriteLine("1- Ingresar Datos");
        Console.WriteLine("2- Calcular Salario");
        Console.WriteLine("3- Mostrar Reporte");
        Console.WriteLine("4- Salir");
        Console.Write("Seleccione una opción: ");
        opcion = Convert.ToByte(Console.ReadLine());

        switch (opcion)
        {
            case 1: SolicitudDatos(); break;
            case 2: CalcularSalario(); break;
            case 3: MostrarReporte(); break;
            case 4: Console.WriteLine("Saliendo..."); break;
            default: Console.WriteLine("Opción inválida"); break;
        }
    } while (opcion != 4);
}

Menu();

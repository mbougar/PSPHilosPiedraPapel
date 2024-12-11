using System;
using System.Collections.Generic;
using System.Threading;

namespace PSPHilosPiedraPapel
{
    internal static class Program
    {
        private static readonly object Locker = new object();
        private static readonly string[] Opciones = { "Piedra", "Papel", "Tijera" };

        private static List<string> _nombresHilos = new List<string>
        {
            "Jugador 1", "Jugador 2", "Jugador 3", "Jugador 4",
            "Jugador 5", "Jugador 6", "Jugador 7", "Jugador 8",
            "Jugador 9", "Jugador 10", "Jugador 11", "Jugador 12",
            "Jugador 13", "Jugador 14", "Jugador 15", "Jugador 16"
        };

        private static List<Thread> _hilos = new List<Thread> { };
        private static List<List<string>> _resultados = new List<List<string>>();

        static void Main()
        {
            CrearJugadores();
            Competir();
        }

        static void CrearJugadores()
        {
            Console.WriteLine("Jugadores: ");
            
            foreach (var nombre in _nombresHilos)
            {
                var hilo = new Thread(() =>
                {
                    lock (Locker)
                    {
                        Console.WriteLine($" - {nombre}");
                    }
                });
                _hilos.Add(hilo);
                hilo.Start();
                hilo.Join();
            }
        }

        static void Competir()
        {
            // Creamos una nueva lista de hilos del tamaño de _nombresHilos, los hacemos competir recorriendo la lista de dos a dos,
            // eliminamos a los perdedores de _nombresHilos y llamamos recursivamente a la funcion (creando una nueva lista de hilos de mitad de tamaño)
            // hasta que solo quede un nombre. Posible Problema: son hilos nuevos cada ietracion no un único hilo que sobrevive todas las iteraciones.
            
            // Para solucionarlo podemos usar funciones recursivas para que cada hilo vaya escribiendo en cola en una lista de resultados e ir eliminando a los hilos cunado no aparecen
            // en la lista de ganadores. Para ello necesitariamos crear un diccionario con los nombres de cada hilo y sus resultados y una lista de nombres de la cual los
            // hilos obtengan su nombre al principio del programa.
            
        }
    }
}
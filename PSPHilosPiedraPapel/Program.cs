using System;
using System.Collections.Generic;
using System.Threading;

namespace PSPHilosPiedraPapel
{
    internal static class Program
    {
        private static readonly object Locker = new object();
        private static readonly string[] Opciones = { "Piedra", "Papel", "Tijera" };
        private static int _ronda = 1;
        
        private static List<string> _jugadores =
        [
            "Jugador 1", "Jugador 2", "Jugador 3", "Jugador 4",
            "Jugador 5", "Jugador 6", "Jugador 7", "Jugador 8",
            "Jugador 9", "Jugador 10", "Jugador 11", "Jugador 12",
            "Jugador 13", "Jugador 14", "Jugador 15", "Jugador 16"
        ];
        
        private static List<string>[] _resultados = new List<string>[16];
        private static List<bool> _terminados = [];

        private static void Main()
        {
            CrearJugadores();
        }

        private static void CrearJugadores()
        {
            Console.WriteLine("Jugadores: ");
            
            foreach (var nombre in _jugadores)
            {
                var hilo = new Thread(() => Jugar(nombre));
                hilo.Start();
            }
            
            GestionarTorneo();
        }

        private static void GestionarTorneo()
        {
            if (_ronda == 5)
            {
                Console.WriteLine($"Ganador: {_jugadores}");
                return;
            }
            
            while (_terminados.Count != _jugadores.Count)
            {
                
            }

            for (var i = 0; i < _terminados.Count; i += 2)
            {
                var resultadosJugador1 = _resultados[i];
                var resultadosJugador2 = _resultados[i + 1];
                
                var ganador = CompararResultados(resultadosJugador1, resultadosJugador2);
                _jugadores.RemoveAt(i + 2 - ganador);
            }
            
            _resultados = new List<string>[_resultados.Length / 2];
            _terminados = [];
            _ronda++;
            
            GestionarTorneo();
        }
        
        static void Jugar(string nombre)
        {
            if (!_jugadores.Contains(nombre) || _jugadores.Count == 1)
            {
                return;
            }
            
            int index;
            
            lock (Locker)
            {
                index = _jugadores.IndexOf(nombre);
                var random = new Random();
                _resultados[index] =
                [
                    Opciones[random.Next(Opciones.Length)],
                    Opciones[random.Next(Opciones.Length)],
                    Opciones[random.Next(Opciones.Length)]
                ];
                _terminados.Add(true);
            }
            
            Jugar(nombre);
        }

        private static int CompararResultados(List<string> resultadosJugador1, List<string> resultadosJugador2)
        {
            var resultado = resultadosJugador1.Select((t, i) => (t, resultadosJugador2[i]) switch
                {
                    ("Piedra", "Tijera") => 1,
                    ("Tijera", "Papel") => 1,
                    ("Papel", "Piedra") => 1,
                    ("Tijera", "Piedra") => -1,
                    ("Papel", "Tijera") => -1,
                    ("Piedra", "Papel") => -1,
                    _ => 0
                })
                .Sum();

            return resultado > 0 ? 1 : 2;
        }
    }
}

    // Creamos una nueva lista de hilos del tamaño de _nombresHilos, los hacemos competir recorriendo la lista de dos a dos,
    // eliminamos a los perdedores de _nombresHilos y llamamos recursivamente a la funcion (creando una nueva lista de hilos de mitad de tamaño)
    // hasta que solo quede un nombre. Posible Problema: son hilos nuevos cada ietracion no un único hilo que sobrevive todas las iteraciones.
            
    // Para solucionarlo podemos usar funciones recursivas para que cada hilo vaya escribiendo en cola en una lista de resultados e ir eliminando a los hilos cunado no aparecen
    // en la lista de ganadores. Para ello necesitariamos crear un diccionario con los nombres de cada hilo y sus resultados y una lista de nombres de la cual los
    // hilos obtengan su nombre al principio del programa.
            
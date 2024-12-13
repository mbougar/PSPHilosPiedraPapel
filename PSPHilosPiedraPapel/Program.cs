
namespace PSPHilosPiedraPapel
{
    internal static class Program
    {
        private static readonly object Locker = new();
        private static readonly string[] Opciones = ["Piedra", "Papel", "Tijera"];
        private static int _ronda = 1;

        private static List<string> _jugadores =
        [
            "Jugador 1", "Jugador 2", "Jugador 3", "Jugador 4",
            "Jugador 5", "Jugador 6", "Jugador 7", "Jugador 8",
            "Jugador 9", "Jugador 10", "Jugador 11", "Jugador 12",
            "Jugador 13", "Jugador 14", "Jugador 15", "Jugador 16"
        ];

        private static Dictionary<string, List<string>> _resultados = new Dictionary<string, List<string>>();
        private static Dictionary<string, bool> _terminados = new Dictionary<string, bool>();

        private static void Main()
        {
            Console.WriteLine("Iniciando torneo de Piedra, Papel o Tijera...");
            GestionarTorneo();
        }

        private static void GestionarTorneo()
        {
            foreach (var jugador in _jugadores)
            {
                _resultados[jugador] = [];
                var hilo = new Thread(() => Jugar(jugador));
                hilo.Start();
            }
            
            while (_jugadores.Count > 1)
            {
                while (_terminados.ContainsValue(false))
                {
                    Thread.Sleep(500);
                }
                
                Console.WriteLine($"\n--- Ronda {_ronda} ---");
                
                var ganadores = new List<string>();

                for (var i = 0; i < _jugadores.Count; i += 2)
                {
                    var ganadorIndex = CompararResultados(_resultados[_jugadores[i]], _resultados[_jugadores[i + 1]]) - 1;
                    ganadores.Add(_jugadores[i + ganadorIndex]);
                    Console.WriteLine($"Partida: {_jugadores[i]} vs {_jugadores[i + 1]} - Ganador: {_jugadores[i + ganadorIndex]}");
                    _terminados[_jugadores[i]] = false;
                    _terminados[_jugadores[i + 1]] = false;
                }
                
                _jugadores = ganadores;
                _ronda++;
            }

            Console.WriteLine($"\nGanador del torneo: {_jugadores[0]}\n");
            _jugadores = [];
        }
        
        private static void Jugar(string name)
        {
            lock (Locker)
            {
                _terminados[name] = false;
            }
            
            while (_jugadores.Contains(name))
            {
                var random = new Random();
                var jugada = new List<string>
                {
                    Opciones[random.Next(Opciones.Length)],
                    Opciones[random.Next(Opciones.Length)],
                    Opciones[random.Next(Opciones.Length)]
                };
                
                _resultados[name] = jugada;
                _terminados[name] = true;
                Console.WriteLine($"{name} jugó: {string.Join(", ", jugada)}");
                
                while (_terminados[name])
                {
                    Thread.Sleep(100);
                }
            }
            
            _terminados[name] = true;
        }

        private static int CompararResultados(List<string> resultadosJugador1, List<string> resultadosJugador2)
        {
            var puntosJugador1 = 0;
            var puntosJugador2 = 0;

            for (var i = 0; i < 3; i++)
            {
                switch ((resultadosJugador1[i], resultadosJugador2[i]))
                {
                    case ("Piedra", "Tijera"):
                    case ("Tijera", "Papel"):
                    case ("Papel", "Piedra"):
                        puntosJugador1++;
                        break;
                    case ("Tijera", "Piedra"):
                    case ("Papel", "Tijera"):
                    case ("Piedra", "Papel"):
                        puntosJugador2++;
                        break;
                }
            }

            return puntosJugador1 > puntosJugador2 ? 1 : 2;
        }
    }
}
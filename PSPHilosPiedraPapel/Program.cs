namespace PSPHilosPiedraPapel
{
    internal static class Program
    {
        private static readonly string[] Opciones = ["Piedra", "Papel", "Tijera"];
        private static int _ronda = 1;

        private static List<string> _jugadores =
        [
            "Jugador 1", "Jugador 2", "Jugador 3", "Jugador 4",
            "Jugador 5", "Jugador 6", "Jugador 7", "Jugador 8",
            "Jugador 9", "Jugador 10", "Jugador 11", "Jugador 12",
            "Jugador 13", "Jugador 14", "Jugador 15", "Jugador 16"
        ];

        private static Dictionary<string, string> _resultados = new Dictionary<string, string>();
        private static Dictionary<string, bool> _enPausa = new Dictionary<string, bool>();

        private static void Main()
        {
            Console.WriteLine("Iniciando torneo de Piedra, Papel o Tijera...");
            GestionarTorneo();
        }

        private static void GestionarTorneo()
        {
            foreach (var jugador in _jugadores)
            {
                _resultados[jugador] = "";
                _enPausa[jugador] = true;
                var hilo = new Thread(() => Jugar(jugador));
                hilo.Start();
            }

            while (_jugadores.Count > 1)
            {
                Console.WriteLine($"\n--- Ronda {_ronda} ---\n");
                var ganadores = new List<string>();
                var perdedores = new List<string>();

                for (var i = 0; i < _jugadores.Count; i += 2)
                {
                    Console.WriteLine($"Partida: {_jugadores[i]} vs {_jugadores[i + 1]}\n");
                    var puntuacion = 0;
                    var tirada = 0;
                    var jugador1 = _jugadores[i];
                    var jugador2 = _jugadores[i + 1];

                    while ((tirada > 2 && puntuacion == 0) || (tirada <= 2 && puntuacion != 2 && puntuacion != -2))
                    {
                        _enPausa[jugador1] = false;
                        _enPausa[jugador2] = false;
                        
                        // Ponemos main en pausa mientras jugador1 o 2 estan fuera de pausa
                        while (!_enPausa[jugador1] || !_enPausa[jugador2])
                        {
                            Thread.Sleep(10);
                        }

                        puntuacion += CompararResultados(_resultados[jugador1], _resultados[jugador2]);
                        Console.WriteLine($"   - Puntuacion: {puntuacion}");
                        tirada++;
                    }

                    switch (puntuacion)
                    {
                        case > 0:
                            ganadores.Add(jugador1);
                            perdedores.Add(jugador2);
                            Console.WriteLine($"\nGanador: {_jugadores[i]}\n");
                            break;

                        case < 0:
                            ganadores.Add(jugador2);
                            perdedores.Add(jugador1);
                            Console.WriteLine($"\nGanador: {_jugadores[i + 1]}\n");
                            break;
                    }
                }

                _jugadores = ganadores;
                _ronda++;
            }

            Console.WriteLine($"\nGanador del torneo: {_jugadores[0]}\n");
            
            // Eliminamos el hilo del ganador
            var ganador = _jugadores[0];
            _jugadores = new List<string>();
            _enPausa[ganador] = true;
        }

        private static void Jugar(string name)
        {
            while (_jugadores.Contains(name))
            {
                while (_enPausa[name])
                {
                    // Eliminamos el hilo si ya no esta dentro de la lista de jugadores
                    if (!_jugadores.Contains(name))
                    {
                        return;
                    }
                    Thread.Sleep(10);
                }

                var random = new Random();
                var jugada = Opciones[random.Next(Opciones.Length)];
                _resultados[name] = jugada;
                Console.WriteLine($"{name} jugó: {jugada}");

                _enPausa[name] = true;
            }
        }

        private static int CompararResultados(string resultadosJugador1, string resultadosJugador2)
        {
            var puntuacion = (resultadosJugador1, resultadosJugador2) switch
            {
                ("Piedra", "Tijera") or ("Tijera", "Papel") or ("Papel", "Piedra") => 1,
                ("Tijera", "Piedra") or ("Papel", "Tijera") or ("Piedra", "Papel") => -1,
                _ => 0
            };

            return puntuacion;
        }
    }
}

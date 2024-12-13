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

        // Diccionario que representa el ultimo movimiento de cada jugador
        private static readonly Dictionary<string, string> Resultados = new();
        private static readonly Dictionary<string, bool> EnPausa = new();

        private static void Main()
        {
            Console.WriteLine("Iniciando torneo de Piedra, Papel o Tijera...");
            GestionarTorneo();
        }

        private static void GestionarTorneo()
        {
            foreach (var jugador in _jugadores)
            {
                Resultados[jugador] = "";
                EnPausa[jugador] = true;
                var hilo = new Thread(() => Jugar(jugador));
                hilo.Start();
            }

            while (_jugadores.Count > 1)
            {
                Console.WriteLine($"\n--- Ronda {_ronda} ---\n");
                var ganadores = new List<string>();

                for (var i = 0; i < _jugadores.Count; i += 2)
                {
                    Console.WriteLine($"Partida: {_jugadores[i]} vs {_jugadores[i + 1]}\n");

                    var puntuacion1 = 0;
                    var puntuacion2 = 0;

                    var tirada = 0;
                    
                    var jugador1 = _jugadores[i];
                    var jugador2 = _jugadores[i + 1];

                    while ((puntuacion1 != 2) && (puntuacion2 != 2) || ((puntuacion1 - puntuacion2 == 0) && tirada > 2))
                    {
                        EnPausa[jugador1] = false;
                        EnPausa[jugador2] = false;
                        
                        // Ponemos main en pausa mientras jugador1 o 2 estan fuera de pausa
                        while (!EnPausa[jugador1] || !EnPausa[jugador2])
                        {
                            Thread.Sleep(10);
                        }

                        var resultadoMano = CompararResultados(Resultados[jugador1], Resultados[jugador2]);
                        switch (resultadoMano)
                        {
                            case > 0:
                                puntuacion1++;
                                break;
                            case < 0:
                                puntuacion2++;
                                break;
                        }
                        Console.WriteLine($"   - Puntuacion: {resultadoMano}");
                        tirada++;
                    }

                    switch (puntuacion1 - puntuacion2)
                    {
                        case > 0:
                            ganadores.Add(jugador1);
                            Console.WriteLine($"\nGanador: {_jugadores[i]}\n");
                            break;

                        case < 0:
                            ganadores.Add(jugador2);
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
            EnPausa[ganador] = true;
        }

        private static void Jugar(string name)
        {
            while (_jugadores.Contains(name))
            {
                while (EnPausa[name])
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
                Resultados[name] = jugada;
                Console.WriteLine($"{name} jugó: {jugada}");

                EnPausa[name] = true;
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

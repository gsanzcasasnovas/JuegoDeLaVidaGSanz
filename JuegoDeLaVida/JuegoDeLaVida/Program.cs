using System;
using System.Text;
using System.Threading;

namespace JuegoDeLaVida
{
    class Program
    {


        static void Main(string[] args)
        {
            int filas = 0, columnas = 0; 

           if(args.Length != 0)
            {
                filas = int.Parse(args[0]);
                columnas = int.Parse(args[1]); 
            }
        

            JuegoDeLaVida juego = new JuegoDeLaVida(filas, columnas);
            juego.InsertarCelulasAleatorias();

            while (true)
            {
                juego.CrearNuevaGeneracion();
                Thread.Sleep(1500);
                Console.Clear();
                Console.WriteLine(juego.DibujarGeneracion());
            }
        }
    }

    public class JuegoDeLaVida
    {
        bool[,] generacion;
        Random random = new Random();

        public JuegoDeLaVida(int numFilas, int numColumnas)
        {
            CrearPrimeraGeneracion(numFilas, numColumnas);
        }

        private void CrearPrimeraGeneracion(int filas, int columnas)
        {
            if (filas <= 0 || columnas <= 0)
            {
                filas = 40;
                columnas = 80;
            }
            generacion = new bool[filas, columnas];
        }

        public void CrearNuevaGeneracion()
        {
            bool[,] nuevaGeneracion = new bool[generacion.GetLength(0), generacion.GetLength(1)];
            Iterador((fila, columna) =>
            {
                if (generacion[fila, columna] )
                {
                    if(ContarCelulasVecinas(fila,columna) < 2)
                    {
                        nuevaGeneracion[fila, columna] = !generacion[fila, columna];
                    }
                    else if(ContarCelulasVecinas(fila, columna) == 2 || ContarCelulasVecinas(fila, columna) == 3)
                    {
                        generacion[fila, columna] = generacion[fila, columna];
                    }
                    else if (ContarCelulasVecinas(fila, columna) > 3)
                    {
                        nuevaGeneracion[fila, columna] = !generacion[fila, columna];
                    }
                }
                else if (ContarCelulasVecinas(fila, columna) == 3)
                {
                    nuevaGeneracion[fila, columna] = !generacion[fila, columna];
                }
                
            });

            generacion = (bool[,]) nuevaGeneracion.Clone();
                
        }

        public string DibujarGeneracion()
        {
            StringBuilder builder = new StringBuilder();

            Iterador((fila, columna) =>
           {
               builder.Append(generacion[fila, columna] ? "@" : "-");
           },
           () =>
           {
               builder.AppendLine();
           });

            return builder.ToString();

        }

        public void InsertarCelulasAleatorias()
        {
            Iterador((filas, columnas) => { generacion[filas, columnas] = random.Next(0, 2) == 1; });
        }

        public int ContarCelulasVecinas(int fila, int columna)
        {
            int count = 0;

            count += fila + 1 < generacion.GetLength(0) && generacion[fila + 1, columna] ? 1 : 0;
            count += columna + 1 < generacion.GetLength(1) && generacion[fila, columna + 1] ? 1 : 0;
            count += fila - 1 >= generacion.GetLowerBound(0) && generacion[fila - 1, columna] ? 1 : 0;
            count += columna - 1 >= generacion.GetLowerBound(1) && generacion[fila, columna - 1] ? 1 : 0;
            count += fila + 1 < generacion.GetLength(0) && columna - 1 >= generacion.GetLowerBound(1) && generacion[fila + 1, columna - 1] ? 1 : 0;
            count += fila - 1 >= generacion.GetLowerBound(0) && columna - 1 >= generacion.GetLowerBound(1) && generacion[fila - 1, columna - 1] ? 1 : 0;
            count += fila + 1 < generacion.GetLength(0) && columna + 1 < generacion.GetLength(1) && generacion[fila + 1, columna + 1] ? 1 : 0;
            count += fila - 1 >= generacion.GetLowerBound(0) && columna + 1 < generacion.GetLowerBound(1) && generacion[fila - 1, columna + 1] ? 1 : 0;

            return count;
        }



        private void Iterador(Action<int, int> accion1, Action accion2 = null)
        {
            for (var fila = 0; fila < generacion.GetLength(0); fila++)
            {
                for (var columna = 0; columna < generacion.GetLength(1); columna++)
                {
                    accion1.Invoke(fila, columna);
                }
                
                    accion2?.Invoke();
                
            }
        }






    }

}

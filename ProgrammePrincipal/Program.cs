using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammePrincipal
{
    class Program
    {
        static public void AfficherGrille(char[,] grille)
        {
            Console.WriteLine("Grille de jeu");
            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    Console.Write(grille[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        static public char[,] InitialiserGrille()
        {
            Console.WriteLine("Combien de lignes souhaitez-vous ?");
            int nbL = int.Parse(Console.ReadLine());
            Console.WriteLine("Combien de colonnes souhaitez-vous ?");
            int nbC = int.Parse(Console.ReadLine());
            char[,] grille = new char[nbL, nbC];
            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    grille[i, j] = 'X';
                }
            }
            return grille;
        }
        public static char[,] GenererGrille(char[,] grille, int[] coord_init)
        {
            int nblignes_grille = grille.GetLength(0);
            int nbcolonnes_grille = grille.GetLength(1);

            Random rnd = new Random();
            int nbmines = rnd.Next(nblignes_grille / 2, nblignes_grille * nbcolonnes_grille / 2 + 1);
            int nbtresors = rnd.Next(1, 4);

            int i;

            int coord_ligne = 0;
            int coord_colonne = 0;

            //génération des mines

            for (i = 0; i < nbmines; i++)
            {
                coord_ligne = rnd.Next(0, nblignes_grille);
                coord_colonne = rnd.Next(0, nbcolonnes_grille);

                //vérification : on ne doit pas être sur la case initiale
                while ((coord_ligne == coord_init[0] && coord_colonne == coord_init[1]) || grille[coord_ligne, coord_colonne] == 'M')
                {
                    coord_ligne = rnd.Next(0, nblignes_grille);
                    coord_colonne = rnd.Next(0, nbcolonnes_grille);
                }
                grille[coord_ligne, coord_colonne] = 'M';
            }

            //génération des trésors

            for (i = 0; i < nbtresors; i++)
            {
                coord_ligne = rnd.Next(0, nblignes_grille);
                coord_colonne = rnd.Next(0, nbcolonnes_grille);

                //vérification : on ne doit pas être sur la case initiale, ni sur une mine
                while ((coord_ligne == coord_init[0] && coord_colonne == coord_init[1]) || (grille[coord_ligne, coord_colonne] == 'M') || (grille[coord_ligne, coord_colonne] == 'T'))
                {
                    coord_ligne = rnd.Next(0, nblignes_grille);
                    coord_colonne = rnd.Next(0, nbcolonnes_grille);
                }
                grille[coord_ligne, coord_colonne] = 'T';
            }
            return grille;
        }


        static public int[] ChoisirCase(char[,] grille)
        {
            int nblignes_grille = grille.GetLength(0);
            int nbcolonnes_grille = grille.GetLength(1);

            Console.WriteLine("Quelle case souhaitez-vous découvrir ? (Entrez le numéro de ligne)");
            int li = int.Parse(Console.ReadLine());
            while (li > nblignes_grille || li < 1)
            {
                Console.WriteLine("Attention ! Le numéro de ligne doit être compris entre 1 et {0} !", nblignes_grille);
                Console.WriteLine("Quelle case souhaitez-vous découvrir ? (Entrez le numéro de ligne)");
                li = int.Parse(Console.ReadLine());
            }
            Console.WriteLine("Quelle case souhaitez-vous découvrir ? (Entrez le numéro de colonne)");
            int col = int.Parse(Console.ReadLine());
            while (col > nbcolonnes_grille || col < 1)
            {
                Console.WriteLine("Attention ! Le numéro de colonne doit être compris entre 1 et {0} !", nbcolonnes_grille);
                Console.WriteLine("Quelle case souhaitez-vous découvrir ? (Entrez le numéro de ligne)");
                col = int.Parse(Console.ReadLine());
            }

            return (new int[] { li - 1, col - 1 });
        }

        int[,] DeterminerVoisins(char[,] grille, int[] coord_init)
        {
            int[,] voisins;
            //Vérification : si on est sur la première ligne
            if(coord_init[0] == 0)
            {
                //Vérification : si on est sur la première colonne
                if(coord_init[1] == 0)
                {
                    voisins = new int[3, 2];
                    voisins[0, 0] = 0;
                    voisins[0, 1] = 1;
                    voisins[1, 0] = 1;
                    voisins[1, 1] = 1;
                    voisins[2, 0] = 1;
                    voisins[2, 1] = 0;
                    return voisins;
                }
                //Vérification : si on est sur la dernière colonne
                else if(coord_init[1] == grille.GetLength(1) - 1)
                {
                    voisins = new int[3, 2];
                    voisins[0, 0] = 0;
                    voisins[0, 1] = grille.GetLength(1) - 2;
                    voisins[1, 0] = 1;
                    voisins[1, 1] = grille.GetLength(1) - 2;
                    voisins[2, 0] = 1;
                    voisins[2, 1] = grille.GetLength(1) - 1;
                    return voisins;
                }
                else
                {
                    voisins = new int[5, 2];
                    int a = 0;
                    int b = 0;
                    for(int i=coord_init[0]; i<= coord_init[0] + 1; i++)
                    {
                        for(int j= coord_init[1] - 1; j <= coord_init[0] + 1; j++)
                        {
                            if(i!= coord_init[0] && j!= coord_init[1])
                            {
                                voisins[a, b] = i;
                                b++;
                                voisins[a, b] = j;
                                a++;
                            }
                        }
                    }
                    return voisins;
                }
            }
            //Vérification : si on est sur la dernière ligne
            if (coord_init[0] == grille.GetLength(0) - 1)
            {
                //Vérification : si on est sur la première colonne
                if (coord_init[1] == 0)
                {
                    voisins = new int[3, 2];
                    voisins[0, 0] = grille.GetLength(0) - 2;
                    voisins[0, 1] = 0;
                    voisins[1, 0] = grille.GetLength(0) - 2;
                    voisins[1, 1] = 1;
                    voisins[2, 0] = grille.GetLength(0) - 1;
                    voisins[2, 1] = 1;
                    return voisins;
                }
                //Vérification : si on est sur la dernière colonne
                else if (coord_init[1] == grille.GetLength(1) - 1)
                {
                    voisins = new int[3, 2];
                    voisins[0, 0] = grille.GetLength(0) - 2;
                    voisins[0, 1] = grille.GetLength(1) - 2;
                    voisins[1, 0] = grille.GetLength(0) - 2;
                    voisins[1, 1] = grille.GetLength(1) - 2;
                    voisins[2, 0] = grille.GetLength(0) - 1;
                    voisins[2, 1] = grille.GetLength(1) - 1;
                    return voisins;
                }
                else
                {
                    voisins = new int[5, 2];
                    int a = 0;
                    int b = 0;
                    for (int i = coord_init[0] - 1; i <= coord_init[0]; i++)
                    {
                        for (int j = coord_init[1] - 1; j <= coord_init[0] + 1; j++)
                        {
                            if (i != coord_init[0] && j != coord_init[1])
                            {
                                voisins[a, b] = i;
                                b++;
                                voisins[a, b] = j;
                                a++;
                            }
                        }
                    }
                    return voisins;
                }
            }
            voisins = new int[8, 2];
            int a = 0;
            int b = 0;
            for (int i = coord_init[0] - 1; i <= coord_init[0] + 1; i++)
            {
                for (int j = coord_init[1] - 1; j <= coord_init[0] + 1; j++)
                {
                    if (i != coord_init[0] && j != coord_init[1])
                    {
                        voisins[a, b] = i;
                        b++;
                        voisins[a, b] = j;
                        a++;
                    }
                }
            }
            return voisins;
        }

        static void Main(string[] args)
        {
            char[,] grilleJeu = InitialiserGrille();
            AfficherGrille(grilleJeu);

            int[] cases_choisies = ChoisirCase(grilleJeu);

            char[,] grilleCache = GenererGrille(grilleJeu, cases_choisies);
            AfficherGrille(grilleCache);

            Console.ReadLine();
        }
    }
}

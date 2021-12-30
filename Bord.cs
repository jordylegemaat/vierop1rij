// <copyright file="Bord.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Vierop1Rij.Domein
{
    using System;

    /// <summary>
    /// met de Class Bord kunnen wij een bord object maken.
    /// </summary>
    public class Bord
    {
        private readonly SpeelKleur[,] speelVeld;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bord"/> class.
        /// Constructor om een bord object te maken.
        /// </summary>
        /// <param name="speelVeld">Speelveld is een 2d array die de dimensies van het bord zijn.</param>
        public Bord()
        {
            this.AantalKolommen = 7;
            this.AantalRijen = 6;
            this.speelVeld = new SpeelKleur[this.AantalKolommen, this.AantalRijen];
            this.CreeerBord();
        }

        public int AantalKolommen { get; private set; }

        public int AantalRijen { get; private set; }

        /// <summary>
        /// Voegt schijf kleur toe aan kolom van het bord. De schijf valt door tot de laagste open rij.
        /// </summary>
        /// <param name="kolom">0 based index van de kolom.</param>
        /// <param name="speelKleur">Kleur van de schijf.</param>
        public void VoegSchijfToe(int kolom, SpeelKleur speelKleur)
        {
            // Als de speler de kleur NietGezet wilt zetten gooi een exceptie.
            if (speelKleur == SpeelKleur.NietGezet)
            {
                throw new ArgumentNullException(nameof(speelKleur));
            }

            // Als de speler een schijf op een niet geldige kolom wilt zetten gooi een exceptie.
            if (!this.IsValidKolom(kolom))
            {
                throw new ArgumentOutOfRangeException(nameof(kolom));
            }

            // Gooi een exceptie als de cel al een kleur heeft.
            if (this.IsKolomVol(kolom))
            {
                throw new ArgumentException(nameof(kolom));
            }

            // Geef mij de laagste open rij, loop door de rij van onder naar boven. Krijg de kleur via de GetKleur() method
            // Check of de kleur op NietGezet staat zet de kleur op die rij en kolom en break uit de loop.
            for (int rij = this.speelVeld.GetLength(1) - 1; rij >= 0; rij--)
            {
                if (this.CellKleur(kolom, rij) == SpeelKleur.NietGezet)
                {
                    // Op de gevonden rij en kolom in het speelveld de speelkleur aanpassen.
                    this.speelVeld[kolom, rij] = speelKleur;
                    break;
                }
            }
        }

        public SpeelKleur WelkKleurHeeft4op1Rij()
        {
            SpeelKleur kleurMet4Op1Rij = SpeelKleur.NietGezet;

            // loopp door de kolommen van linke naar rechts, zolang de kleur niet gezet is.
            for (int kolom = 0; kleurMet4Op1Rij == SpeelKleur.NietGezet && kolom < this.speelVeld.GetLength(1); kolom++)
            {
                // loop door de rijen van boven naar beneden, zolang de kleur bniet gezet is.
                for (int rij = this.speelVeld.GetUpperBound(1); kleurMet4Op1Rij == SpeelKleur.NietGezet && rij >= 0; rij--)
                {
                    kleurMet4Op1Rij = this.HeeftCell4Op1Rij(kolom, rij);
                }
            }

            return kleurMet4Op1Rij;
        }

        internal bool IsVol()
        {
            throw new NotImplementedException();
        }

        private SpeelKleur HeeftCell4Op1Rij(int kolom, int rij)
        {
            // check kolom rij?
            SpeelKleur matchingKleur = this.CellKleur(kolom, rij);

            if (matchingKleur != SpeelKleur.NietGezet &&
                (this.HeeftCell4Op1RijVerticaalInDeMatchingKleur(kolom, rij, matchingKleur) ||
                this.HeeftCell4Op1RijHorizontaalInDeMatchingKleur(kolom, rij, matchingKleur) ||
                this.HeeftCell4Op1RijDiagonaalInDeMatchingKleur(kolom, rij, matchingKleur)))
            {
                return matchingKleur;
            }

            return SpeelKleur.NietGezet;
        }

        private bool HeeftCell4Op1RijVerticaalInDeMatchingKleur(int kolom, int rij, SpeelKleur matchingKleur)
        {
            // als de kolom  < 3 dan gaat de eerste if als fout en ga je dus niet outer bounds van de array
            return (rij > 2 && /* boven */
                    matchingKleur == this.speelVeld[kolom, rij] &&
                    matchingKleur == this.speelVeld[kolom, --rij] &&
                    matchingKleur == this.speelVeld[kolom, --rij] &&
                    matchingKleur == this.speelVeld[kolom, --rij]) ||
                    (rij < this.speelVeld.GetUpperBound(1) - 2 && /*beneden*/
                    matchingKleur == this.speelVeld[kolom, rij] &&
                    matchingKleur == this.speelVeld[kolom, ++rij] &&
                    matchingKleur == this.speelVeld[kolom, ++rij] &&
                    matchingKleur == this.speelVeld[kolom, ++rij]);
        }

        private bool HeeftCell4Op1RijHorizontaalInDeMatchingKleur(int kolom, int rij, SpeelKleur matchingKleur)
        {
            // als de kolom  < 3 dan gaat de eerste if als fout en ga je dus niet outer bounds van de array
            return (kolom > 2 && /*links*/
                    matchingKleur == this.speelVeld[kolom, rij] &&
                    matchingKleur == this.speelVeld[--kolom, rij] &&
                    matchingKleur == this.speelVeld[--kolom, rij] &&
                    matchingKleur == this.speelVeld[--kolom, rij]) ||
                    (kolom < this.speelVeld.GetUpperBound(1) - 2 && /*rechts*/
                    matchingKleur == this.speelVeld[kolom, rij] &&
                    matchingKleur == this.speelVeld[++kolom, rij] &&
                    matchingKleur == this.speelVeld[++kolom, rij] &&
                    matchingKleur == this.speelVeld[++kolom, rij]);
        }

        private bool HeeftCell4Op1RijDiagonaalInDeMatchingKleur(int kolom, int rij, SpeelKleur matchingKleur)
        {
            // als de kolom  < 3 dan gaat de eerste if als fout en ga je dus niet outer bounds van de array
            // eerst naar rechtboven
            return (kolom < this.speelVeld.GetUpperBound(0) - 2 && /*rechts*/
                    rij > 2 && /* boven */
                    matchingKleur == this.speelVeld[kolom, rij] &&
                    matchingKleur == this.speelVeld[++kolom, --rij] &&
                    matchingKleur == this.speelVeld[++kolom, --rij] &&
                    matchingKleur == this.speelVeld[++kolom, --rij])
                    ||
                    (kolom < this.speelVeld.GetUpperBound(0) - 2 && /*rechts*/
                    rij < this.speelVeld.GetUpperBound(1) - 2 &&  /*beneden*/
                    matchingKleur == this.speelVeld[kolom, rij] &&
                    matchingKleur == this.speelVeld[++kolom, ++rij] &&
                    matchingKleur == this.speelVeld[++kolom, ++rij] &&
                    matchingKleur == this.speelVeld[++kolom, ++rij])
                    ||
                    (kolom > 2 && /*links*/
                    rij > 2 && /* boven */
                    matchingKleur == this.speelVeld[kolom, rij] &&
                    matchingKleur == this.speelVeld[--kolom, --rij] &&
                    matchingKleur == this.speelVeld[--kolom, --rij] &&
                    matchingKleur == this.speelVeld[--kolom, --rij])
                    ||
                    (kolom > 2 && /*links*/
                    rij < this.speelVeld.GetUpperBound(1) - 2 && /*beneden*/
                    matchingKleur == this.speelVeld[kolom, rij] &&
                    matchingKleur == this.speelVeld[--kolom, ++rij] &&
                    matchingKleur == this.speelVeld[--kolom, ++rij] &&
                    matchingKleur == this.speelVeld[--kolom, ++rij]);
        }

        /// <summary>
        /// Deze functie geeft true terug als de kolom vol is.
        /// </summary>
        /// <param name="kolom">De controleren kolom.</param>
        /// <returns>True als die vol is, false als die niet vol is.</returns>
        private bool IsKolomVol(int kolom)
        {
            return this.CellKleur(kolom, 0) != SpeelKleur.NietGezet;
        }

        /// <summary>
        /// Deze functie kijkt of de meegegeven kolom wel mogelijk is.
        /// </summary>
        /// <param name="kolom">De controleren kolom.</param>
        /// <returns>true als de kolom valid is, false als die niet valid is.</returns>
        private bool IsValidKolom(int kolom)
        {
            return kolom < this.speelVeld.GetLength(0) && kolom >= 0;
        }

        /// <summary>
        /// Geeft de kleur op de positie rij en kolom.
        /// </summary>
        /// <param name="kolom">Rij van speelveld.</param>
        /// <param name="rij">Kolom van speelveld.</param>
        /// <returns>Kleur van de meegekregen rij en kolom.</returns>
        public SpeelKleur CellKleur(int kolom, int rij)
        {
            return this.speelVeld[kolom, rij];
        }

        /// <summary>
        /// Creeërt nieuw bord met NietGezetteKleuren.
        /// </summary>
        private void CreeerBord()
        {
            //this.SpeelVeld = new SpeelKleur[7, 6];

            // Loop door alle elementen van de 2D array en zet de speelkleur naar NietGezet.
            for (int kolom = 0; kolom < this.speelVeld.GetLength(0); kolom++)
            {
                for (int rij = 0; rij < this.speelVeld.GetLength(1); rij++)
                {
                    this.speelVeld[kolom, rij] = SpeelKleur.NietGezet;
                }
            }
        }

        /// <summary>
        /// Checkt of er een valid cel is die de speler meegeeft.
        /// </summary>
        /// <param name="kolom">kolom van de cel die gecheckt moet worden.</param>
        /// <param name="rij">Rij van de cel die gecheckt moet worden.</param>
        /// <returns>true als het een geldige kolom is, false als die niet geldig is.</returns>
        private bool IsValidCel(int kolom, int rij)
        {
            return (kolom <= this.speelVeld.GetLength(0) && kolom >= 0) && (rij <= this.speelVeld.GetLength(1) && rij >= 0);
        }
    }
}
